using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Shaders
{
    class RecompileShadersCommand : Command
    {
        GameCache Cache;

        public RecompileShadersCommand(GameCache cache) :
            base(true,

                "RecompileShaders",
                "Recompiles all shader templates",

                "RecompileShaders [shader type]",

                "Recompiles all shader templates\n" +
                "[shader type] - Specify shader type, EX. \"shader\" for \'rmsh\'.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            using (var stream = Cache.OpenCacheReadWrite())
            {
                if (args.Count > 0)
                {
                    string shaderType = args[0].ToLower();
                    return RecompileShaderType(stream, shaderType);
                }
                else
                {
                    foreach (string shaderType in Enum.GetNames(typeof(HaloShaderGenerator.Shared.ShaderType)))
                    {
                        RecompileShaderType(stream, shaderType.ToLower());
                    }
                }
            }
            return true;
        }

        private object RecompileShaderType(Stream stream, string shaderType)
        {
            // get relevant rmdf
            if (!Cache.TagCache.TryGetTag($"shaders\\{shaderType}.rmdf", out CachedTag rmdfTag))
            {
                // don't need actual options yet - we just need to initialize the generator (input option indices are verified using rmdf, which we don't have yet)
                List<byte> fakeOptions = new List<byte>();
                for (int i = 0; i < 16; i++)
                    fakeOptions.Add(0);

                rmdfTag = GenerateShaderCommand.GenerateRmdf(Cache, stream, shaderType, fakeOptions.ToArray());
                if (rmdfTag == null)
                    return new TagToolError(CommandError.TagInvalid, $"Could not find or generate rmdf tag for \"{shaderType}\"");
            }

            var rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);

            if (rmdf.GlobalVertexShader == null || rmdf.GlobalPixelShader == null)
                return new TagToolError(CommandError.TagInvalid, "A global shader was missing from rmdf");

            foreach (var tag in Cache.TagCache.NonNull())
            {
                if (tag.Group.Tag != "rmt2" ||
                    tag.Name.StartsWith("ms30") ||
                    !tag.Name.Split('\\')[1].StartsWith(shaderType + "_templates"))
                    continue;

                List<byte> options = new List<byte>();

                foreach (var option in tag.Name.Split('\\')[2].Remove(0, 1).Split('_'))
                    options.Add(byte.Parse(option));

                // make up options count, may not work very well
                while (options.Count != rmdf.Categories.Count)
                    options.Add(0);

                GenerateShaderCommand.GenerateRenderMethodTemplate(Cache, stream, shaderType, options.ToArray(), rmdf);
            }

            return true;
        }
    }
}
