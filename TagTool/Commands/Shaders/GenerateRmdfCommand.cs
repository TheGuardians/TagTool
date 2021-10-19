using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Shaders;

namespace TagTool.Commands.Shaders
{
    class GenerateRmdfCommand : Command
    {
        GameCache Cache;

        public GenerateRmdfCommand(GameCache cache) :
            base(true,

                "GenerateRmdf",
                "Generates a render method definition tag according to the specified type.",

                "GenerateRmdf <type> [update]",
                "Generates a render method definition tag according to the specified type.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            if (args.Count > 1 && args[1] == "update")
            {
                using (var stream = Cache.OpenCacheReadWrite())
                {
                    TagTool.Shaders.ShaderGenerator.RenderMethodDefinitionGenerator.UpdateRenderMethodDefinition(Cache, stream, args[0]);
                }
                Console.WriteLine($"\"shaders\\{args[0]}\" updated successfully.");
                return true;
            }

            var generator = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GetGlobalShaderGenerator(args[0], true);

            if (generator == null)
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\" is not a supported or valid shader type.");

            string rmdfName = $"shaders\\{args[0]}";
            
            if (!Cache.TagCache.TryGetTag<RenderMethodDefinition>(rmdfName, out CachedTag rmdfTag))
            {
                rmdfTag = Cache.TagCache.AllocateTag<RenderMethodDefinition>(rmdfName);
            }

            using (var stream = Cache.OpenCacheReadWrite())
            {
                var rmdf = TagTool.Shaders.ShaderGenerator.RenderMethodDefinitionGenerator.GenerateRenderMethodDefinition(Cache, stream, generator, args[0], out _, out _);
                Cache.Serialize(stream, rmdfTag, rmdf);
            }

            (Cache as GameCacheHaloOnlineBase).SaveTagNames();

            Console.WriteLine($"\"{rmdfName}\" generated successfully.");
            return true;
        }
    }
}
