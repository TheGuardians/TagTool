using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Tags.Definitions;

namespace TagTool.Shaders
{
    class XboxShaderParser
    {
        public UPDBParser UPDB;
        public byte[] ShaderData => IsVertexShader ? (Block as VertexShaderBlock).XboxShaderReference?.ShaderData : (Block as PixelShaderBlock).XboxShaderReference?.ShaderData;
        public byte[] DebugData => IsVertexShader ? (Block as VertexShaderBlock).XboxShaderReference?.DebugData : (Block as PixelShaderBlock).XboxShaderReference?.DebugData;
        public byte[] ConstantData => IsVertexShader ? (Block as VertexShaderBlock).XboxShaderReference?.ConstantData : (Block as PixelShaderBlock).XboxShaderReference?.ConstantData;

        public bool IsVertexShader => Block?.GetType() == typeof(VertexShaderBlock);
        public bool IsPixelShader => Block?.GetType() == typeof(PixelShaderBlock);
        public bool IsValid => !(CacheContext == null || Tag == null || Block == null || ShaderData == null || ShaderData.Length == 0);

        private GameCacheContext CacheContext { get; }

        public object Block { get; set; }
        public object Tag { get; set; }

        public enum OutputFile
        {
            ShaderData,
            DebugData,
            ConstantData
        }

        public XboxShaderParser(VertexShader tag, VertexShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(PixelShader tag, PixelShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(GlobalVertexShader tag, VertexShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(GlobalPixelShader tag, PixelShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(object tag, object block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null)
        {
            CacheContext = gamecachecontext;
            Block = block;
            Tag = tag;
            if (!IsValid) return;
            WriteOutput(OutputFile.ShaderData);
            WriteOutput(OutputFile.ConstantData);
            WriteOutput(OutputFile.DebugData);


            //UPDB = updb_parser != null ? updb_parser : new UPDBParser(DebugData); //TODO: It appears that the current format is for Vertex uPDB's only...

            //if (typeof(T) == typeof(GlobalVertexShader)) ProcessShader(Tag as GlobalVertexShader, Block as VertexShaderBlock);
            //if (typeof(T) == typeof(VertexShader)) ProcessShader(Tag as VertexShader, Block as VertexShaderBlock);
            //if (typeof(T) == typeof(GlobalPixelShader)) ProcessShader(Tag as GlobalPixelShader, Block as PixelShaderBlock);
            //if (typeof(T) == typeof(PixelShader)) ProcessShader(Tag as PixelShader, Block as PixelShaderBlock);

        }

        private List<ShaderParameter> GetParameters()
        {
            if (Tag.GetType() == typeof(PixelShader)) return GetParameters(Tag as PixelShader);
            throw new NotImplementedException();
        }
        private List<ShaderParameter> GetParameters(PixelShader Tag)
        {
            var Block = this.Block as PixelShaderBlock;
            return Block.XboxParameters.ToList();
        }
        private string GetShaderParameterTypeString(ShaderParameter.RType type)
        {
            if (type == ShaderParameter.RType.Boolean) return "b";
            if (type == ShaderParameter.RType.Float) return "c";
            if (type == ShaderParameter.RType.Integer) return "i";
            if (type == ShaderParameter.RType.Sampler) return "s";
            throw new NotImplementedException();
        }
        private string GetShaderParameterNameString(ShaderParameter.RType type)
        {
            if (type == ShaderParameter.RType.Boolean) return "bool";
            if (type == ShaderParameter.RType.Float) return "float4";
            if (type == ShaderParameter.RType.Integer) return "integer";
            if (type == ShaderParameter.RType.Sampler) return "sampler";
            throw new NotImplementedException();
        }

        public string GetParametersBlock()
        {
            StringBuilder sb = new StringBuilder();
            var parameters = GetParameters();
            foreach (var parameter in parameters)
            {
                sb.AppendLine($"//   {GetShaderParameterNameString(parameter.RegisterType)} {CacheContext.GetString(parameter.ParameterName)};");
            }
            return sb.ToString();

        }

        private string FormatRegistersBlockEntry(string _name, string _reg, string _size, int max_name_length, int reg_length, int size_length)
        {
            string name_col = $"{_name}{new String(' ', max_name_length - _name.Length + 1)}";
            string reg_col = $"{_reg}{new String(' ', reg_length - _reg.Length + 1)}";
            string size_col = $"{new String(' ', size_length - _size.Length)}{_size}";
            return name_col + reg_col + size_col;
        }

        public string GetRegistersBlock()
        {
            int max_name_length = 14;
            int reg_length = 5;
            int size_length = 4;

            var registers = GetParameters();
            foreach (var register in registers)
            {
                var name = CacheContext.GetString(register.ParameterName);
                max_name_length = Math.Max(max_name_length, name.Length);
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"//   {FormatRegistersBlockEntry("Name", "Reg", "Size", max_name_length, reg_length, size_length)}");
            sb.AppendLine($"//   {FormatRegistersBlockEntry(new String('-', max_name_length), new String('-', reg_length), new String('-', size_length), max_name_length, reg_length, size_length)}");
            foreach (var register in registers)
            {
                string name = CacheContext.GetString(register.ParameterName);
                string reg = $"{GetShaderParameterTypeString(register.RegisterType)}{register.RegisterIndex}";
                string size = register.RegisterCount.ToString();
                sb.AppendLine($"//   {FormatRegistersBlockEntry(name, reg, size, max_name_length, reg_length, size_length)}");
            }
            return sb.ToString();
        }

        public string Disassemble(string _raw_shader_code = null)
        {
            var raw_shader_code = _raw_shader_code == null ? XSDDisassemble() : _raw_shader_code;
            var formatted_raw_block = CommentStringBlock(raw_shader_code, true);
            var converted_shader_code = ConvertXboxShader(raw_shader_code);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("//");
            sb.AppendLine($"// Generated by Tag Tool {Environment.Version}");
            sb.AppendLine("//");
            sb.AppendLine("// Parameters:");
            sb.Append(GetParametersBlock());
            sb.AppendLine("//");
            sb.AppendLine("//");
            sb.AppendLine("// Registers:");
            sb.AppendLine("//");
            sb.Append(GetRegistersBlock());
            sb.AppendLine("//");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("// Converted PC Shader Assembly");
            sb.AppendLine(converted_shader_code);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("// Raw Xbox 360 Shader Assembly");
            sb.AppendLine(formatted_raw_block);
            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }

        private string ConvertXboxShader(string raw_shader_code)
        {
            if (IsVertexShader) throw new NotImplementedException();

            var instructions = raw_shader_code.Split(new[] { "\r\n", "\r", "\n", Environment.NewLine }, StringSplitOptions.None).ToList();
            for (var instruction_index = instructions.Count - 1; instruction_index >= 0; instruction_index--)
            {
                var instruction = instructions[instruction_index];
                instruction = instruction.Replace(" + ", "");
                instruction = instruction.Trim();

                var original_instruction = instruction;

                // Take the left side of any comments
                instruction = instruction.Split(new[] { "//" }, StringSplitOptions.None)[0];

                if (String.IsNullOrWhiteSpace(instruction))
                {
                    instructions.RemoveAt(instruction_index);
                    continue;
                }

                var assembly_codes = instruction.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);

                var assembly_code = assembly_codes.Length > 0 ? assembly_codes[0] : null;
                if (assembly_code != null)
                {
                    if (
                        assembly_code == "exec" ||
                        assembly_code == "exece" ||
                        assembly_code == "alloc" ||
                        assembly_code == "kill_eq" ||
                        assembly_code == "kill_ge" ||
                        assembly_code == "kill_gt" ||
                        assembly_code == "kill_ne" ||
                        assembly_code == "sqrt"
                        )
                    {
                        instruction = "//" + instruction;

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }
                    if (assembly_code == "sqrt")
                    {
                        instruction = "";
                        instruction = $"rsq {assembly_codes[1]}, {assembly_codes[2]} // {original_instruction}";
                        instruction += $"\nrcp {assembly_codes[1]}, {assembly_codes[1]} // 1/(1/sqrt)";

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }
                    if (assembly_code == "movs")
                    {
                        instruction = $"mov {assembly_codes[1]}, {assembly_codes[2]} // {original_instruction}";

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }
                    if (assembly_code == "mulsc")
                    {
                        instruction = $"mul {assembly_codes[1]}, {assembly_codes[2]}, {assembly_codes[3]} // {original_instruction}";

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }
                    if (assembly_code == "addsc")
                    {
                        instruction = $"add {assembly_codes[1]}, {assembly_codes[2]}, {assembly_codes[3]} // {original_instruction}";

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }
                    if (assembly_code == "tfetch2D")
                    {
                        // TODO: Find the highest register number and allocate a new one for this fix instead of defaulting to 31
                        instruction = $"texld r31.xyzw, {assembly_codes[2]}, {assembly_codes[3].Replace("tf", "s")} // {original_instruction} (copy to register)";
                        instruction += "\n";

                        var destination = assembly_codes[1];
                        if (destination.Contains(".") && destination.Contains("_")) // Has a mask
                        {
                            var destinationA = assembly_codes[1].Split('.')[0];
                            var destinationB = assembly_codes[1].Split('.')[1];
                            const string coords = "xyzw";

                            //TODO: Bunching up these commands would speed it up a bit
                            for (var destination_index = 0; destination_index < 4; destination_index++)
                            {
                                var destination_coordinate_char = destinationB[destination_index];
                                if (destination_coordinate_char == '_') continue;
                                if (destination_coordinate_char == '1') throw new NotImplementedException();

                                var destination_coordinate = new String(destination_coordinate_char, 1);
                                var source_coordinate = new String(coords[destination_index], 1);

                                instruction += $"mov {destinationA}.{destination_coordinate}, r31.{source_coordinate} // {original_instruction} (masking {destination_coordinate})\n";
                            }





                        }
                        else instruction += $"mov {assembly_codes[1]}, r31.xyzw // {original_instruction} (default masking)";

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }

                }

                instructions[instruction_index] = instruction.Trim();
            }

            // Register Fixups
            for (var instruction_index = instructions.Count - 1; instruction_index >= 0; instruction_index--)
            {
                var instruction = instructions[instruction_index];
                instruction = instruction.Replace(" + ", "");
                instruction = instruction.Trim();

                var original_instruction = instruction;

                // Take the left side of any comments
                instruction = instruction.Split(new[] { "//" }, StringSplitOptions.None)[0];

                if (String.IsNullOrWhiteSpace(instruction))
                {
                    instructions.RemoveAt(instruction_index);
                    continue;
                }

                var assembly_codes = instruction.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);

                var assembly_code = assembly_codes.Length > 0 ? assembly_codes[0] : null;
                if (assembly_code != null)
                {

                    // : Anti Fuck Cheatcodes for 1337 kids :
                    if (IsPixelShader) // Pixel Shader 
                    {
                        // Fixup Output Colors
                        if (assembly_codes.Length >= 2)
                            for (var assembly_code_index = 1; assembly_code_index < assembly_codes.Length; assembly_code_index++)
                                if (assembly_codes[assembly_code_index][0] == 'o')
                                {
                                    var index_str = assembly_codes[assembly_code_index].Substring(1);
                                    var arr = index_str.Split('.');
                                    index_str = arr[0];
                                    var args_str = arr.Length > 1 ? arr[1] : null;

                                    if (Int32.TryParse(index_str, out Int32 index))
                                    {
                                        if (args_str == null) assembly_codes[assembly_code_index] = $"oC{index}";
                                        else assembly_codes[assembly_code_index] = $"oC{index}.{args_str}";
                                        var assembly_code_args = assembly_codes.Skip(1);
                                        instruction = $"{assembly_code} " + string.Join(", ", assembly_code_args);
                                    }
                                }

                    }

                    if (IsVertexShader) // Vertex Shader
                    {

                    }

                    // Fixup Register Differences Top Down
                    if (assembly_codes.Length >= 2)
                        for (var assembly_code_index = 1; assembly_code_index < assembly_codes.Length; assembly_code_index++)
                            if (assembly_codes[assembly_code_index][0] == 'c')
                            {
                                var index_str = assembly_codes[assembly_code_index].Substring(1);
                                var arr = index_str.Split('.');
                                index_str = arr[0];
                                var args_str = arr.Length > 1 ? arr[1] : null;

                                if (Int32.TryParse(index_str, out Int32 index) && index >= (255 - 32))
                                {
                                    if (args_str == null) assembly_codes[assembly_code_index] = $"c{index - 32}";
                                    else assembly_codes[assembly_code_index] = $"c{index - 32}.{args_str}";
                                    var assembly_code_args = assembly_codes.Skip(1);
                                    instruction = $"{assembly_code} " + string.Join(", ", assembly_code_args);
                                }
                            }

                }

                instructions[instruction_index] = instruction.Trim();
            }

            List<string> header = new List<string>();
            if (IsVertexShader) header.Insert(0, "vs_3_0");
            if (IsPixelShader) header.Insert(0, "ps_3_0");

            var registers = GetParameters();
            foreach (var register in registers)
            {
                if (register.RegisterType == ShaderParameter.RType.Sampler)
                {
                    header.Add($"dcl_2d {GetShaderParameterTypeString(register.RegisterType)}{register.RegisterIndex} // {CacheContext.GetString(register.ParameterName)}");
                }
            }

            instructions.InsertRange(0, header);

            return String.Join("\n", instructions);
        }

        private byte[] ProcessVertexShader(VertexShaderBlock Block)
        {
            throw new NotImplementedException();
        }

        private byte[] ProcessPixelShader(PixelShaderBlock Block)
        {
            var raw_shader_code = XSDDisassemble();
            var converted_shader_code = ConvertXboxShader(raw_shader_code);

            var shader_bytecode = Utilities.DirectXUtilities.AssembleShader(converted_shader_code);

            Console.WriteLine("Processed PixelShader");
            return shader_bytecode;
        }

        public byte[] ProcessShader()
        {
            if (IsVertexShader) return ProcessVertexShader(Block as VertexShaderBlock);
            if (IsPixelShader) return ProcessPixelShader(Block as PixelShaderBlock);
            return null;
        }

        private string CommentStringBlock(string input, bool trim)
        {
            var linesInText = input.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);

            StringBuilder stringWithRowNumbers = new StringBuilder();

            foreach (var line in linesInText)
            {
                var str = line;
                if (trim) str = line.Trim();

                stringWithRowNumbers.AppendLine($"// {str}");
            }
            string result = stringWithRowNumbers.ToString();
            return result;
        }

        public void WriteOutput(OutputFile file)
        {
            Directory.CreateDirectory(@"Temp");

            if (file == OutputFile.ShaderData)
            {
                WriteOutput(@"Temp\permutation.shader", ShaderData);
            }

            if (file == OutputFile.ConstantData)
            {
                WriteOutput(@"Temp\permutation.shader.updb", DebugData);
            }

            if (file == OutputFile.ShaderData)
            {
                WriteOutput(@"Temp\permutation.shader.cbin", ConstantData);
            }
        }

        private string XSDDisassemble()
        {
            if (!File.Exists(@"Tools\xsd.exe"))
            {
                Console.WriteLine("Missing tools, please install xsd.exe before porting shaders.");
                return null;
            }

            WriteOutput(OutputFile.ShaderData);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"Tools\xsd.exe",
                    Arguments = IsVertexShader ? "/rawvs permutation.shader" : "/rawps permutation.shader",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "temp")
                }
            };
            process.Start();

            string shader_code = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!String.IsNullOrWhiteSpace(err)) throw new Exception(err);

            return shader_code;
        }

        private void WriteOutput(string file, byte[] data)
        {
            if (File.Exists(file)) File.Delete(file);
            if (data.Length > 0)
                using (EndianWriter output = new EndianWriter(File.OpenWrite(file), EndianFormat.BigEndian))
                    output.WriteBlock(data);
        }
    }
}
