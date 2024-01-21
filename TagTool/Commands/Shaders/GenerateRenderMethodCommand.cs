using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Shaders.ShaderMatching;
using TagTool.Shaders.ShaderGenerator;
using static TagTool.Tags.Definitions.RenderMethod.RenderMethodPostprocessBlock;

namespace TagTool.Commands.Shaders
{
    public class GenerateRenderMethodCommand : Command
    {
        GameCacheHaloOnlineBase Cache;

        public GenerateRenderMethodCommand(GameCacheHaloOnlineBase cache) :
            base(true,

                "GenerateRenderMethod",
                "Generates a render method tag from the specified template",

                "GenerateRenderMethod <tag name> <rmt2 name>",

                "Generates a render method tag from the specified template\n" +
                "The rm\'s parameters will use default values as specified in rmop")
        {
            Cache = cache;
        }

        static readonly Dictionary<string, string> ShaderTypeGroups = new Dictionary<string, string>
        {
            ["shader"] = "rmsh",
            ["decal"] = "rmd ",
            ["halogram"] = "rmhg",
            ["water"] = "rmw ",
            ["black"] = "rmbk",
            ["terrain"] = "rmtr",
            ["custom"] = "rmcs",
            ["foliage"] = "rmfl",
            ["screen"] = "rmss",
            ["cortana"] = "rmct",
            ["zonly"] = "rmzo",
        };

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            if (!Cache.TagCache.TryGetTag($"{args[1].Split('.')[0]}.rmt2", out var rmt2Tag))
                return new TagToolError(CommandError.TagInvalid, $"Could not find \"{args[1]}.rmt2\"");

            // easier to get the type, and cleaner to check if ms30
            ShaderMatcherNew.Rmt2Descriptor.TryParse(rmt2Tag.Name, out var rmt2Descriptor);

            // check if tag already exists, or allocate new one
            string rmGroup = ShaderTypeGroups[rmt2Descriptor.Type];
            if (!Cache.TagCache.TryGetTag($"{args[0]}.{rmGroup}", out var rmTag))
                rmTag = Cache.TagCache.AllocateTag(Cache.TagCache.TagDefinitions.GetTagGroupFromTag(rmGroup), args[0]);

            string prefix = rmt2Descriptor.IsMs30 ? "ms30\\" : "";
            string rmdfName = prefix + "shaders\\" + rmt2Descriptor.Type;
            if (!Cache.TagCache.TryGetTag($"{rmdfName}.rmdf", out var rmdfTag))
                return new TagToolError(CommandError.TagInvalid, $"Could not find \"{rmdfName}.rmdf\"");

            using (var stream = Cache.OpenCacheReadWrite())
            {
                var rmt2 = Cache.Deserialize<RenderMethodTemplate>(stream, rmt2Tag);
                var rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);

                // store rmop definitions for quick lookup
                List<RenderMethodOption> renderMethodOptions = new List<RenderMethodOption>();
                for (int i = 0; i < rmt2Descriptor.Options.Length; i++)
                {
                    var rmopTag = rmdf.Categories[i].ShaderOptions[rmt2Descriptor.Options[i]].Option;
                    if (rmopTag != null)
                        renderMethodOptions.Add(Cache.Deserialize<RenderMethodOption>(stream, rmopTag));
                }

                // create definition
                object definition = Activator.CreateInstance(Cache.TagCache.TagDefinitions.GetTagDefinitionType(rmGroup));
                // make changes as RenderMethod so the code can be reused for each rm type
                var rmDefinition = definition as RenderMethod;

                rmDefinition.BaseRenderMethod = rmdfTag;

                // initialize lists
                rmDefinition.Options = new List<RenderMethod.RenderMethodOptionIndex>();
                rmDefinition.ShaderProperties = new List<RenderMethod.RenderMethodPostprocessBlock>();

                foreach (var option in rmt2Descriptor.Options)
                    rmDefinition.Options.Add(new RenderMethod.RenderMethodOptionIndex { OptionIndex = option });
                rmDefinition.SortLayer = TagTool.Shaders.SortingLayerValue.Normal;
                rmDefinition.PredictionAtomIndex = -1;

                PopulateRenderMethodConstants populateConstants = new PopulateRenderMethodConstants();

                // setup shader property
                RenderMethod.RenderMethodPostprocessBlock shaderProperty = new RenderMethod.RenderMethodPostprocessBlock
                {
                    Template = rmt2Tag,
                    // setup constants
                    TextureConstants = populateConstants.SetupTextureConstants(rmt2, renderMethodOptions, Cache),
                    RealConstants = populateConstants.SetupRealConstants(rmt2, renderMethodOptions, Cache),
                    IntegerConstants = populateConstants.SetupIntegerConstants(rmt2, renderMethodOptions, Cache),
                    BooleanConstants = populateConstants.SetupBooleanConstants(rmt2, renderMethodOptions, Cache),
                    // get alpha blend mode
                    BlendMode = populateConstants.GetAlphaBlendMode(rmt2Descriptor, rmdf, Cache),
                    // TODO
                    QueryableProperties = new short[] { -1, -1, -1, -1, -1, -1, -1, -1 }
                };

                rmDefinition.ShaderProperties.Add(shaderProperty);

                Cache.Serialize(stream, rmTag, definition);
                (Cache as GameCacheHaloOnlineBase).SaveTagNames();
            }

            Console.WriteLine($"Generated {rmGroup} tag: {rmTag.Name}.{rmTag.Group}");
            return true;
        }
    }
}
