using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;
using System;
using TagTool.Tags.Definitions;
using TagTool.Geometry;
using TagTool.Cache.HaloOnline;
using TagTool.Shaders;
using System.Diagnostics;

namespace TagTool.Commands.Shaders
{
    // TODO: Support Reach gpix
    public class DumpDisassembledShadersCommand : Command
    {
        GameCache Cache;
        RenderMethodTemplate CurrentRmt2;
        RenderMethodDefinition CurrentRmdf;
        List<byte> CurrentOptionIndices;
        int CurrentEntryPointIndex;
        bool IsXbox;

        public DumpDisassembledShadersCommand(GameCache cache) : base(false, "DumpDisassembledShaders", "Dump disassembled shaders", "DumpDisassembledShaders", "")
        {
            Cache = cache;
            CurrentRmt2 = null;
            CurrentRmdf = null;
            CurrentOptionIndices = new List<byte>();
            CurrentEntryPointIndex = -1;
            IsXbox = false;
        }

        public override object Execute(List<string> args)
        {
            if (Cache.GetType() == typeof(GameCacheGen3) && UseXSDCommand.XSDFileInfo == null)
                return new TagToolError(CommandError.CustomError, "You must use the \"UseXSD\" command first!");

            if (args.Count > 0)
            {
                DirectoryInfo cacheDirectory = new DirectoryInfo(args[0]);
                if (!cacheDirectory.Exists)
                    return new TagToolError(CommandError.ArgInvalid, "Invalid cache directory.");

                foreach (var cacheFile in cacheDirectory.GetFiles("*.map"))
                {
                    if (cacheFile.Name == "shared.map" || cacheFile.Name == "campaign.map")
                        continue;

                    Console.WriteLine($"Disassembling shaders in \"{cacheFile.Name}\"");
                    DisassembleCacheShaders(GameCache.Open(cacheFile));
                }
            }
            else
            {
                DisassembleCacheShaders(Cache);
            }

            return true;
        }

        private void DisassembleCacheShaders(GameCache cache)
        {
            IsXbox = CacheVersionDetection.GetGeneration(cache.Version) != CacheGeneration.HaloOnline;

            Type entryPointEnum = typeof(EntryPoint);
            if (cache.Version >= CacheVersion.HaloReach)
                entryPointEnum = typeof(EntryPointReach);
            else if (cache.Version >= CacheVersion.HaloOnline301003 && cache.Version <= CacheVersion.HaloOnline700123)
                entryPointEnum = typeof(EntryPointMs30);

            using (var stream = cache.OpenCacheRead())
            {
                GlobalCacheFilePixelShaders gpix = null;
                if (cache.Version >= CacheVersion.HaloReach)
                    gpix = cache.Deserialize<GlobalCacheFilePixelShaders>(stream, cache.TagCache.FindFirstInGroup("gpix"));

                Dictionary<string, CachedTag> shaderTypes = new Dictionary<string, CachedTag>();

                foreach (var tag in cache.TagCache.NonNull())
                {
                    if (tag.Name == null || tag.Name == "" || tag.Group.Tag != "rmdf")
                        continue;
                    if (cache.Version == CacheVersion.HaloOnlineED && tag.Name.StartsWith("ms30\\"))
                        continue; // ignore ms30 in ms23, disassemble from ms30 directly instead
                    var pieces = tag.Name.Split('\\');

                    shaderTypes[pieces[pieces.Length - 1]] = tag;
                }

                foreach (var shaderType in shaderTypes.Keys)
                {
                    CurrentRmdf = cache.Deserialize<RenderMethodDefinition>(stream, shaderTypes[shaderType]);

                    CachedTag glvsTag = CurrentRmdf.GlobalVertexShader;
                    CachedTag glpsTag = CurrentRmdf.GlobalPixelShader;

                    if (glvsTag == null || glpsTag == null)
                    {
                        Console.WriteLine($"WARNING: Cache \"{cache.DisplayName}\" has invalid shader type \"{shaderType}\"");
                        continue;
                    }

                    var glvsTagName = glvsTag.Name;
                    var glpsTagName = glpsTag.Name;

                    var glvs = cache.Deserialize<GlobalVertexShader>(stream, glvsTag);
                    var glps = cache.Deserialize<GlobalPixelShader>(stream, glpsTag);

                    foreach (var tag in cache.TagCache.NonNull())
                    {
                        if (tag.IsInGroup("rmt2") && tag.Name.StartsWith($"shaders\\{shaderType}_templates"))
                        {
                            // disassemble specified shaders related to rmt2
                            var tagName = tag.Name;
                            var rmt2Tag = cache.TagCache.GetTag(tagName, "rmt2");

                            CurrentRmt2 = cache.Deserialize<RenderMethodTemplate>(stream, rmt2Tag);

                            if (CurrentRmt2.PixelShader == null)
                            {
                                new TagToolError(CommandError.CustomError, "Template pixel shader was null");
                                CurrentRmt2 = null;
                                continue;
                            }

                            foreach (var index in rmt2Tag.Name.Split('\\')[2].Remove(0, 1).Split('_'))
                                CurrentOptionIndices.Add(Convert.ToByte(index));

                            var pixl = cache.Deserialize<PixelShader>(stream, CurrentRmt2.PixelShader);

                            Directory.CreateDirectory(cache.Version.ToString() + "\\" + tagName);

                            foreach (var entry in Enum.GetValues(entryPointEnum))
                            {
                                CurrentEntryPointIndex = GetEntryPointIndex(entry, cache.Version);

                                if (CurrentEntryPointIndex < pixl.EntryPointShaders.Count)
                                {
                                    var entryShader = pixl.EntryPointShaders[CurrentEntryPointIndex].Offset;

                                    if (pixl.EntryPointShaders[CurrentEntryPointIndex].Count != 0)
                                    {
                                        string entryName = entry.ToString().ToLower() + ".pixel_shader";
                                        string pixelShaderFilename = Path.Combine(tagName, entryName);

                                        DisassembleShader(pixl, entryShader, pixelShaderFilename, cache, stream, pixl.Shaders[entryShader].GlobalCachePixelShaderIndex != -1 ? gpix : null);
                                    }
                                }
                            }

                            CurrentEntryPointIndex = -1;
                            CurrentOptionIndices.Clear();
                            CurrentRmt2 = null;
                        }
                    }

                    // glps
                    Directory.CreateDirectory(cache.Version.ToString() + "\\" + glpsTagName);
                    foreach (var entry in Enum.GetValues(entryPointEnum))
                    {
                        CurrentEntryPointIndex = GetEntryPointIndex(entry, cache.Version);

                        if (CurrentEntryPointIndex < glps.EntryPoints.Count)
                        {
                            var entryShader = glps.EntryPoints[CurrentEntryPointIndex].ShaderIndex;
                            if (entryShader != -1)
                            {
                                string entryName = entry.ToString().ToLower() + ".shared_pixel_shader";
                                string pixelShaderFilename = Path.Combine(glpsTagName, entryName);

                                DisassembleShader(glps, entryShader, pixelShaderFilename, cache, stream, glps.Shaders[entryShader].GlobalCachePixelShaderIndex != -1 ? gpix : null);
                            }
                            else if (glps.EntryPoints[CurrentEntryPointIndex].Option.Count > 0)
                            {
                                foreach (var option in glps.EntryPoints[CurrentEntryPointIndex].Option)
                                {
                                    var methodIndex = option.RenderMethodOptionIndex;
                                    for (int i = 0; i < option.OptionMethodShaderIndices.Count; i++)
                                    {
                                        var optionIndex = i;
                                        string glpsFilename = entry.ToString().ToLower() + $"_{methodIndex}_{optionIndex}" + ".shared_pixel_shader";
                                        glpsFilename = Path.Combine(glpsTagName, glpsFilename);
                                        DisassembleShader(glps, option.OptionMethodShaderIndices[i], glpsFilename, cache, stream, glps.Shaders[option.OptionMethodShaderIndices[i]].GlobalCachePixelShaderIndex != -1 ? gpix : null);
                                    }
                                }
                            }
                        }
                    }

                    CurrentEntryPointIndex = -1;

                    // glvs
                    foreach (VertexType vert in Enum.GetValues(typeof(VertexType)))
                    {
                        if ((int)vert < glvs.VertexTypes.Count)
                        {
                            var vertexFormat = glvs.VertexTypes[(int)vert];
                            var dirName = Path.Combine(glvsTagName, vert.ToString().ToLower());
                            foreach (var entry in Enum.GetValues(entryPointEnum))
                            {
                                CurrentEntryPointIndex = GetEntryPointIndex(entry, cache.Version);

                                if (CurrentEntryPointIndex < vertexFormat.DrawModes.Count)
                                {
                                    var entryShader = vertexFormat.DrawModes[CurrentEntryPointIndex].ShaderIndex;
                                    if (entryShader != -1)
                                    {
                                        Directory.CreateDirectory(cache.Version.ToString() + "\\" + dirName);
                                        string entryName = entry.ToString().ToLower() + ".shared_vertex_shader";
                                        string vertexShaderFileName = Path.Combine(dirName, entryName);

                                        DisassembleShader(glvs, entryShader, vertexShaderFileName, cache, stream, null);
                                    }
                                }
                            }
                        }
                    }

                    CurrentEntryPointIndex = -1;
                    CurrentRmdf = null;
                }

                CachedTag rasgTag = cache.TagCache.FindFirstInGroup("rasg");
                if (rasgTag != null)
                {
                    var rasg = cache.Deserialize<RasterizerGlobals>(stream, rasgTag);

                    for (int i = 0; i < rasg.DefaultShaders.Count; i++)
                    {
                        var explicitShader = rasg.DefaultShaders[i];

                        if (explicitShader.PixelShader == null)
                        {
                            Console.WriteLine($"Invalid explicit pixel shader {i}");
                        }
                        else
                        {
                            string shaderName = explicitShader.PixelShader.Name.Split('\\')[2];
                            Directory.CreateDirectory(cache.Version.ToString() + "\\explicit\\" + shaderName);

                            var pixl = cache.Deserialize<PixelShader>(stream, explicitShader.PixelShader);
                            foreach (var entry in Enum.GetValues(entryPointEnum))
                            {
                                CurrentEntryPointIndex = GetEntryPointIndex(entry, cache.Version);

                                if (pixl.EntryPointShaders.Count <= CurrentEntryPointIndex)
                                    break;

                                for (int j = 0; j < pixl.EntryPointShaders[CurrentEntryPointIndex].Count; j++)
                                {
                                    int shaderIndex = pixl.EntryPointShaders[CurrentEntryPointIndex].Offset + j;
                                    string entryName = shaderIndex + "_" + entry.ToString().ToLower() + ".pixel_shader";
                                    string pixelShaderFilename = Path.Combine("explicit\\" + shaderName, entryName);

                                    DisassembleShader(pixl, shaderIndex, pixelShaderFilename, cache, stream, pixl.Shaders[shaderIndex].GlobalCachePixelShaderIndex != -1 ? gpix : null);
                                }
                            }

                            CurrentEntryPointIndex = -1;
                        }

                        if (explicitShader.VertexShader == null)
                        {
                            Console.WriteLine($"Invalid explicit vertex shader {i}");
                        }
                        else
                        {
                            string shaderName = explicitShader.VertexShader.Name.Split('\\')[2];

                            var vtsh = cache.Deserialize<VertexShader>(stream, explicitShader.VertexShader);
                            foreach (var entry in Enum.GetValues(entryPointEnum))
                            {
                                CurrentEntryPointIndex = GetEntryPointIndex(entry, cache.Version);

                                if (vtsh.EntryPoints.Count <= CurrentEntryPointIndex)
                                    break;

                                for (int j = 0; j < vtsh.EntryPoints[CurrentEntryPointIndex].SupportedVertexTypes.Count; j++)
                                {
                                    for (int k = 0; k < vtsh.EntryPoints[CurrentEntryPointIndex].SupportedVertexTypes[j].Count; k++)
                                    {
                                        int shaderIndex = vtsh.EntryPoints[CurrentEntryPointIndex].SupportedVertexTypes[j].Offset + k;

                                        var dirName = Path.Combine("explicit\\" + shaderName + "\\", ((VertexType)k).ToString().ToLower() + "\\");
                                        Directory.CreateDirectory(cache.Version.ToString() + "\\" + dirName);

                                        string entryName = shaderIndex + "_" + entry.ToString().ToLower() + ".vertex_shader";
                                        string vertexShaderFilename = Path.Combine(dirName, entryName);

                                        DisassembleShader(vtsh, shaderIndex, vertexShaderFilename, cache, stream, null);
                                    }
                                }
                            }

                            CurrentEntryPointIndex = -1;
                        }
                    }
                }

                CachedTag chgdTag = cache.TagCache.FindFirstInGroup("chgd");
                if (chgdTag != null)
                {
                    var chgd = cache.Deserialize<ChudGlobalsDefinition>(stream, chgdTag);

                    for (int i = 0; i < chgd.HudShaders.Count; i++)
                    {
                        var hudShader = chgd.HudShaders[i];

                        if (hudShader.PixelShader == null)
                        {
                            Console.WriteLine($"Invalid chud pixel shader {i}");
                        }
                        else
                        {
                            string shaderName = hudShader.PixelShader.Name.Split('\\')[2];
                            Directory.CreateDirectory(cache.Version.ToString() + "\\chud\\" + shaderName);

                            var pixl = cache.Deserialize<PixelShader>(stream, hudShader.PixelShader);
                            foreach (var entry in Enum.GetValues(entryPointEnum))
                            {
                                CurrentEntryPointIndex = GetEntryPointIndex(entry, cache.Version);

                                if (pixl.EntryPointShaders.Count <= CurrentEntryPointIndex)
                                    break;

                                for (int j = 0; j < pixl.EntryPointShaders[CurrentEntryPointIndex].Count; j++)
                                {
                                    int shaderIndex = pixl.EntryPointShaders[CurrentEntryPointIndex].Offset + j;
                                    string entryName = shaderIndex + "_" + entry.ToString().ToLower() + ".pixel_shader";
                                    string pixelShaderFilename = Path.Combine("chud\\" + shaderName, entryName);

                                    DisassembleShader(pixl, shaderIndex, pixelShaderFilename, cache, stream, pixl.Shaders[shaderIndex].GlobalCachePixelShaderIndex != -1 ? gpix : null);
                                }
                            }

                            CurrentEntryPointIndex = -1;
                        }

                        if (hudShader.VertexShader == null)
                        {
                            Console.WriteLine($"Invalid chud vertex shader {i}");
                        }
                        else
                        {
                            string shaderName = hudShader.VertexShader.Name.Split('\\')[2];

                            var vtsh = cache.Deserialize<VertexShader>(stream, hudShader.VertexShader);
                            foreach (var entry in Enum.GetValues(entryPointEnum))
                            {
                                CurrentEntryPointIndex = GetEntryPointIndex(entry, cache.Version);

                                if (vtsh.EntryPoints.Count <= CurrentEntryPointIndex)
                                    break;

                                for (int j = 0; j < vtsh.EntryPoints[CurrentEntryPointIndex].SupportedVertexTypes.Count; j++)
                                {
                                    for (int k = 0; k < vtsh.EntryPoints[CurrentEntryPointIndex].SupportedVertexTypes[j].Count; k++)
                                    {
                                        int shaderIndex = vtsh.EntryPoints[CurrentEntryPointIndex].SupportedVertexTypes[j].Offset + k;

                                        var dirName = Path.Combine("chud\\" + shaderName + "\\", ((VertexType)k).ToString().ToLower() + "\\");
                                        Directory.CreateDirectory(cache.Version.ToString() + "\\" + dirName);

                                        string entryName = shaderIndex + "_" + entry.ToString().ToLower() + ".vertex_shader";
                                        string vertexShaderFilename = Path.Combine(dirName, entryName);

                                        DisassembleShader(vtsh, shaderIndex, vertexShaderFilename, cache, stream, null);
                                    }
                                }
                            }

                            CurrentEntryPointIndex = -1;
                        }
                    }
                }
            }
        }

        private string DisassembleShader(object definition, int shaderIndex, string filename, GameCache cache, Stream stream, GlobalCacheFilePixelShaders gpix)
        {
            string path = $"{cache.Version}\\{filename}";

            if (IsXbox)
            {
                switch (definition)
                {
                    case PixelShader pixl:
                        if (pixl.Shaders[shaderIndex].XboxShaderReference != null ||
                            (cache.Version >= CacheVersion.HaloReach && pixl.Shaders[shaderIndex].GlobalCachePixelShaderIndex != -1))
                            break;
                        return "NO DATA";
                    case VertexShader vtsh:
                        if (vtsh.Shaders[shaderIndex].XboxShaderReference != null)
                            break;
                        return "NO DATA";
                    case GlobalPixelShader glps:
                        if (glps.Shaders[shaderIndex].XboxShaderReference != null ||
                            (cache.Version >= CacheVersion.HaloReach && glps.Shaders[shaderIndex].GlobalCachePixelShaderIndex != -1))
                            break;
                        return "NO DATA";
                    case GlobalVertexShader glvs:
                        if (glvs.Shaders[shaderIndex].XboxShaderReference != null)
                            break;
                        return "NO DATA";
                }

                return DisassembleGen3Shader(definition, shaderIndex, cache, stream, path, gpix);
            }
            else
            {
                return DisassembleHaloOnlineShader(definition, shaderIndex, path);
            }

            return null;
        }

        private string DisassembleGen3Shader(object definition, int shaderIndex, GameCache cache, Stream stream, string filename, GlobalCacheFilePixelShaders gpix)
        {
            var file = UseXSDCommand.XSDFileInfo;
            if (file == null)
            {
                Console.WriteLine("xsd.exe not found! use command usexsd <directory> to load xsd.exe");
                return null;
            }

            var tempFile = Path.GetTempFileName();
            string disassembly = "";
            string xsdArguments = " \"" + tempFile + "\"";
            List<int> disassemblyConstants = new List<int>();

            //
            // Set the arguments for xsd.exe according to the XDK documentation
            //
            try
            {
                CompiledShaderInfo shaderInfo = GetCompiledShaderInfo(definition, shaderIndex, gpix, IsXbox);

                File.WriteAllBytes(tempFile, shaderInfo.DebugData);
                File.WriteAllBytes(tempFile, shaderInfo.ConstantData);
                File.WriteAllBytes(tempFile, shaderInfo.ShaderData);

                xsdArguments = (shaderInfo.PixelShader ? "/rawps" : "/rawvs") + xsdArguments;

                ProcessStartInfo info = new ProcessStartInfo(UseXSDCommand.XSDFileInfo.FullName)
                {
                    Arguments = xsdArguments,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = false
                };
                Process xsd = Process.Start(info);

                // read disassembly + get all constants used in the shader
                // this is a bit hacky, however is the only way it can be done atm

                while (!xsd.StandardOutput.EndOfStream)
                {
                    string line = xsd.StandardOutput.ReadLine();
                    disassembly += line + "\n";

                    if (line.Length <= 4 || !line.Contains(" "))
                        continue;

                    var lineParts = line.Replace(",", "").Remove(0, 4).Split(' ');

                    // [0] is the instruction, skip it
                    for (int i = 1; i < lineParts.Length; i++)
                    {
                        if (lineParts[i].StartsWith("c") && char.IsDigit(lineParts[i][1]))
                        {
                            string registerString = lineParts[i];
                            registerString = registerString.Remove(0, 1);
                            registerString = registerString.Split('.')[0];
                            registerString = registerString.Replace("_abs", "");

                            int register = int.Parse(registerString);
                            if (!disassemblyConstants.Contains(register))
                                disassemblyConstants.Add(register);
                        }
                    }
                }

                xsd.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to disassemble shader: {e.ToString()}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            using (var writer = File.CreateText(filename))
            {
                GenerateGen3ShaderHeader(definition, shaderIndex, cache, stream, writer, gpix, disassemblyConstants);
                writer.WriteLine(disassembly);
            }

            return disassembly;
        }

        private CompiledShaderInfo GetCompiledShaderInfo(object definition, int shaderIndex, GlobalCacheFilePixelShaders gpix, bool isXbox)
        {
            switch (definition)
            {
                case PixelShader pixl:
                    if (gpix == null && shaderIndex < pixl.Shaders.Count)
                    {
                        PixelShaderBlock shaderBlock = pixl.Shaders[shaderIndex];
                        bool containsConstants = shaderBlock.PCConstantTable.Constants.Count > 0 || shaderBlock.XBoxConstantTable.Constants.Count > 0;
                        return new CompiledShaderInfo(true, containsConstants, shaderBlock.XboxShaderReference, isXbox ? shaderBlock.XBoxConstantTable.Constants : shaderBlock.PCConstantTable.Constants);
                    }
                    else if (gpix != null)
                    {
                        PixelShaderBlock shaderBlock = gpix.Shaders[pixl.Shaders[shaderIndex].GlobalCachePixelShaderIndex];
                        bool containsConstants = shaderBlock.PCConstantTable.Constants.Count > 0 || shaderBlock.XBoxConstantTable.Constants.Count > 0;
                        return new CompiledShaderInfo(true, containsConstants, shaderBlock.XboxShaderReference, isXbox ? shaderBlock.XBoxConstantTable.Constants : shaderBlock.PCConstantTable.Constants);
                    }
                    break;
                case GlobalPixelShader glps:
                    if (gpix == null && shaderIndex < glps.Shaders.Count)
                    {
                        PixelShaderBlock shaderBlock = glps.Shaders[shaderIndex];
                        bool containsConstants = shaderBlock.PCConstantTable.Constants.Count > 0 || shaderBlock.XBoxConstantTable.Constants.Count > 0;
                        return new CompiledShaderInfo(true, containsConstants, shaderBlock.XboxShaderReference, isXbox ? shaderBlock.XBoxConstantTable.Constants : shaderBlock.PCConstantTable.Constants);
                    }
                    else if (gpix != null)
                    {
                        PixelShaderBlock shaderBlock = gpix.Shaders[glps.Shaders[shaderIndex].GlobalCachePixelShaderIndex];
                        bool containsConstants = shaderBlock.PCConstantTable.Constants.Count > 0 || shaderBlock.XBoxConstantTable.Constants.Count > 0;
                        return new CompiledShaderInfo(true, containsConstants, shaderBlock.XboxShaderReference, isXbox ? shaderBlock.XBoxConstantTable.Constants : shaderBlock.PCConstantTable.Constants);
                    }
                    break;
                case VertexShader vtsh:
                    if (shaderIndex < vtsh.Shaders.Count)
                    {
                        VertexShaderBlock shaderBlock = vtsh.Shaders[shaderIndex];
                        bool containsConstants = shaderBlock.PCConstantTable.Constants.Count > 0 || shaderBlock.XBoxConstantTable.Constants.Count > 0;
                        return new CompiledShaderInfo(false, containsConstants, shaderBlock.XboxShaderReference, isXbox ? shaderBlock.XBoxConstantTable.Constants : shaderBlock.PCConstantTable.Constants);
                    }
                    break;
                case GlobalVertexShader glvs:
                    if (shaderIndex < glvs.Shaders.Count)
                    {
                        VertexShaderBlock shaderBlock = glvs.Shaders[shaderIndex];
                        bool containsConstants = shaderBlock.PCConstantTable.Constants.Count > 0 || shaderBlock.XBoxConstantTable.Constants.Count > 0;
                        return new CompiledShaderInfo(false, containsConstants, shaderBlock.XboxShaderReference, isXbox ? shaderBlock.XBoxConstantTable.Constants : shaderBlock.PCConstantTable.Constants);
                    }
                    break;
            }

            return null;
        }

        private void GenerateGen3ShaderHeader(object definition, int shaderIndex, GameCache cache, Stream stream, StreamWriter writer, GlobalCacheFilePixelShaders gpix, List<int> disassemblyConstants = null)
        {
            CompiledShaderInfo shaderInfo = GetCompiledShaderInfo(definition, shaderIndex, gpix, IsXbox);

            if (!shaderInfo.ValidConstantTable && CurrentRmt2 != null)
                shaderInfo.GenerateParametersFromTemplate(cache, stream, CurrentEntryPointIndex, CurrentRmt2, CurrentRmdf, CurrentOptionIndices);

            List<ShaderParameter> parameters = shaderInfo.Parameters;
            List<ShaderParameter> orderedParameters = shaderInfo.ReorderParameters();
            List<RealQuaternion> constants = shaderInfo.GetXboxShaderConstants();

            List<int> usedConstants = new List<int>();

            WriteHeaderLine(writer);
            WriteHeaderLine(writer, "Generated by TagTool and Xbox Shader Disassembler");
            WriteHeaderLine(writer);

            if (parameters.Count > 0)
            {
                WriteHeaderLine(writer, "Parameters:");
                WriteHeaderLine(writer);

                uint regNameLength = 1;

                for (int i = 0; i < parameters.Count; i++)
                {
                    var param = parameters[i];
                    string paramName = cache.StringTable.GetString(parameters[i].ParameterName);
                    WriteHeaderLine(writer, $"\t{GetTypeString(param.RegisterType, param.RegisterCount)} {paramName};");

                    if (paramName.Length > regNameLength)
                        regNameLength = (uint)paramName.Length;
                }

                WriteHeaderLine(writer);
                WriteHeaderLine(writer);
                WriteHeaderLine(writer, "Registers:");
                WriteHeaderLine(writer);
                WriteHeaderLine(writer, $"\t\t{"Name".ToLength(regNameLength)} {"Reg".ToLength(6)}{"Size".ToLength(4)}");
                WriteHeaderLine(writer, $"\t\t{"-".Repeat(regNameLength - 1)} ----- ----");

                for (int i = 0; i < orderedParameters.Count; i++)
                {
                    var param = orderedParameters[i];

                    if (param.RegisterType == ShaderParameter.RType.Vector)
                    {
                        // store used registers to sort literal constants
                        for (int j = 0; j < param.RegisterCount; j++)
                            usedConstants.Add(j + param.RegisterIndex);
                    }

                    string name = cache.StringTable.GetString(orderedParameters[i].ParameterName).ToLength(regNameLength);
                    string reg = GetRegisterString(param.RegisterType, param.RegisterIndex).ToLength(6);
                    string size = param.RegisterCount.ToString().ToLength(4);

                    WriteHeaderLine(writer, $"\t\t{name} {reg}{size}");
                }

                WriteHeaderLine(writer);
            }

            WriteHeaderLine(writer);
            WriteHeaderLine(writer, "Constants:");
            WriteHeaderLine(writer);

            int constantRegister = 255;
            List<int> validConstants = new List<int>();
            for (int i = 0; i < constants.Count; i++)
            {
                while (true)
                {
                    if (!usedConstants.Contains(constantRegister))
                    {
                        if (disassemblyConstants.Contains(constantRegister))
                            break;
                        validConstants.Add(constantRegister);
                    }

                    constantRegister--;
                    if (constantRegister < 0)
                    {
                        //Console.WriteLine("Constant register could not be matched, using highest available.");
                        constantRegister = validConstants[0];
                        validConstants.RemoveAt(0);
                        break;
                    }
                }

                WriteHeaderLine(writer, $"\t\tc{constantRegister} {constants[i]} ");
                constantRegister--;
            }

            WriteHeaderLine(writer);
        }

        private void WriteHeaderLine(StreamWriter writer, string entry = null)
        {
            string result = "// ";
            if (entry != null)
                result += entry;
            writer.WriteLine(result);
        }

        private string GetRegisterString(ShaderParameter.RType type, int registerIndex)
        {
            string result = "";
            switch (type)
            {
                case ShaderParameter.RType.Boolean:
                    result = "b";
                    break;
                case ShaderParameter.RType.Integer:
                    result = "i";
                    break;
                case ShaderParameter.RType.Vector:
                    result = "c";
                    break;
                case ShaderParameter.RType.Sampler:
                    result = "s";
                    break;
            }
            result += registerIndex.ToString();
            return result;
        }

        private string GetTypeString(ShaderParameter.RType type, int size)
        {
            string result = "";
            switch (type)
            {
                case ShaderParameter.RType.Boolean:
                    result = "bool";
                    break;
                case ShaderParameter.RType.Integer:
                    result = "integer";
                    break;
                case ShaderParameter.RType.Vector:
                    result = "float4";
                    if (size > 1)
                        result += $"x{size}";
                    break;
                case ShaderParameter.RType.Sampler:
                    result = "sampler2D";
                    break;
            }
            return result;
        }

        private string DisassembleHaloOnlineShader(object definition, int shaderIndex, string filename)
        {
            string disassembly = null;

            if (definition.GetType() == typeof(PixelShader) || definition.GetType() == typeof(GlobalPixelShader))
            {
                PixelShaderBlock shader_block = null;
                if (definition.GetType() == typeof(PixelShader))
                {
                    var _definition = definition as PixelShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                if (definition.GetType() == typeof(GlobalPixelShader))
                {
                    var _definition = definition as GlobalPixelShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                var pc_shader = shader_block.PCShaderBytecode;
                disassembly = D3DCompiler.Disassemble(pc_shader);
                if (pc_shader == null)
                    disassembly = null;
            }

            if (definition.GetType() == typeof(VertexShader) || definition.GetType() == typeof(GlobalVertexShader))
            {
                VertexShaderBlock shader_block = null;
                if (definition.GetType() == typeof(VertexShader))
                {
                    var _definition = definition as VertexShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                if (definition.GetType() == typeof(GlobalVertexShader))
                {
                    var _definition = definition as GlobalVertexShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                var pc_shader = shader_block.PCShaderBytecode;
                disassembly = D3DCompiler.Disassemble(pc_shader);
                if (pc_shader == null)
                    disassembly = null;
            }

            using (var writer = File.CreateText(filename))
            {
                writer.WriteLine(disassembly);
            }

            return disassembly;
        }

        private int GetEntryPointIndex(object input, CacheVersion version)
        {
            if (version >= CacheVersion.HaloReach)
                return (int)((EntryPointReach)input);
            else if (version >= CacheVersion.HaloOnline301003 && version <= CacheVersion.HaloOnline700123)
                return (int)((EntryPointMs30)input);
            else
                return (int)((EntryPoint)input);
        }
    }
}

