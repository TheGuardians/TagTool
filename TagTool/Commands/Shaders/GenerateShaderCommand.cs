using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Cache.HaloOnline;
using HaloShaderGenerator.Shader;

namespace TagTool.Commands.Shaders
{
    public class GenerateShaderCommand : Command
    {
        GameCache Cache;

        public GenerateShaderCommand(GameCache cache) :
            base(true,

                "GenerateShader",
                "Generates a shader template and a relevant \'rm  \' tag if specified",

                "GenerateShader [generate rm tag] <shader type> <options>",

                "Generates a shader template and a relevant \'rm  \' tag if specified\n" +
                "[generate rm tag y/n] - Specify \"y\" to create and populate a relevant \'rm  \' tag from the generated rmt2\n" +
                "<shader type> - Specify shader type, EX. \"shader\" for \'rmsh\'\n" +
                "<options> - Specify the template\'s options as either integers or by names")
        {
            Cache = cache;
        }

        static readonly List<string> SupportedShaderTypes = new List<string> { "shader", /*"black"*/ };

        public override object Execute(List<string> args)
        {
            if (args.Count < 2)
                return new TagToolError(CommandError.ArgCount);

            bool generateRenderMethod = false;
            if (args[0].ToLower() == "y")
            {
                // add support soon
                //generateRenderMethod = true;
                args.RemoveAt(0);
            }
            else if (args[0].ToLower() == "n")
                args.RemoveAt(0);

            string shaderType = args[0].ToLower();
            if (!SupportedShaderTypes.Contains(shaderType))
                return new TagToolError(CommandError.CustomMessage, $"Shader type \"{shaderType}\" is currently unsupported");

            args.RemoveAt(0); // we should only have options from this point

            // get relevant rmdf
            if (!Cache.TagCache.TryGetTag($"shaders\\{shaderType}.rmdf", out var rmdfTag))
                return new TagToolError(CommandError.TagInvalid, $"Could not find rmdf tag for \"{shaderType}\"");

            using (var stream = Cache.OpenCacheReadWrite())
            {
                var rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);

                if (rmdf.GlobalVertexShader == null || rmdf.GlobalPixelShader == null)
                    return new TagToolError(CommandError.TagInvalid, "A global shader was missing from rmdf");

                List<int> options = new List<int>();
                for (int i = 0; i < args.Count; i++)
                {
                    // parse options as int, if fails try finding in rmdf string
                    if (!int.TryParse(args[i].ToLower(), out int optionInteger))
                    {
                        bool found = false;

                        for (int j = 0; j < rmdf.Methods[i].ShaderOptions.Count; j++)
                        {
                            if (Cache.StringTable.GetString(rmdf.Methods[i].ShaderOptions[j].Type) == args[i].ToLower())
                            {
                                found = true;
                                options.Add(j);
                            }
                        }

                        if (!found)
                            return new TagToolError(CommandError.ArgInvalid, $"Shader option \"{args[i]}\" not found");
                    }

                    else
                    {
                        options.Add(optionInteger);
                    }
                }

                // make up options count, may not work very well
                while (options.Count != rmdf.Methods.Count)
                    options.Add(0);

                // build tagname
                string rmt2TagName = $"shaders\\{shaderType}_templates\\_{string.Join("_", options.ToArray())}";

                // generate shader
                switch (shaderType)
                {
                    case "shader": GenerateShader(stream, options, rmt2TagName, rmdf); break;
                    //case "black": GenerateShaderBlack(stream, rmt2TagName, rmdf); break;
                }

                Console.WriteLine($"Successfully generated shader template \"{rmt2TagName}\"");

                // shader generated, create rm if specified
                /*if (generateRenderMethod)
                {

                }*/
            }

            return true;
        }

        private void GenerateShader(Stream stream, List<int> options, string rmt2Name, RenderMethodDefinition rmdf)
        {
            Albedo albedo = (Albedo)options[0];
            Bump_Mapping bumpMapping = (Bump_Mapping)options[1];
            Alpha_Test alphaTest = (Alpha_Test)options[2];
            Specular_Mask specularMask = (Specular_Mask)options[3];
            Material_Model materialModel = (Material_Model)options[4];
            Environment_Mapping environmentMapping = (Environment_Mapping)options[5];
            Self_Illumination selfIllumination = (Self_Illumination)options[6];
            Blend_Mode blendMode = (Blend_Mode)options[7];
            Parallax parallax = (Parallax)options[8];
            Misc misc = (Misc)options[9];
            Distortion distortion = (Distortion)options[10];

            var generator = new ShaderGenerator(albedo, bumpMapping, alphaTest, specularMask, materialModel, environmentMapping, selfIllumination, blendMode, parallax, misc, distortion);

            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);

            var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);

            if (!Cache.TagCache.TryGetTag(rmt2Name + ".rmt2", out var rmt2Tag))
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);

            Cache.Serialize(stream, rmt2Tag, rmt2);
            Cache.SaveStrings();
            (Cache as GameCacheHaloOnline).SaveTagNames();
        }
    }
}
