using System.Collections.Generic;
using System.IO;
using System.Linq;

using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Decorators
{
    class ExtractModelCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private DecoratorSet Decorator { get; }

        public ExtractModelCommand(GameCache cacheContext, CachedTag tag, DecoratorSet decorator)
            : base(true,

                  name:
                  "ExtractModel",

                  description:
                  "Extracts the current decorator model as AMF, DAE or OBJ",

                  usage:
                  "ExtractModel <path> [variant] [dds]",

                  examples:
                  "extractmodel d:\\rock %rock_c\n" +
                  "extractmodel d:\\*\\model.amf ?\n" +
                  "foreach dctr extractmodel d:\\rips\\*.dae dds",

                  helpMessage:
                  "- [variant] Specify instance name, ? prompts with a list of names, default * extacts all.\n" +
                  "- [dds] Will extract used bitmaps in dds format to subfolders within <path> folder.\n" +
                  "- Format will be chosen based on extension given for output file, AMF if folder only.\n" +
                  "- All occurrence of * in <path>, will be replaced with the model name/index.\n" +
                  "- If <path> is a folder only, an auto-named AMF file will be extracted there.")
        {
            Cache = cacheContext;
            Tag = tag;
            Decorator = decorator;
        }

        public override object Execute(List<string> args)
        {
            if (Decorator.RenderModel is null)
                return new TagToolError(CommandError.CustomError, "The model does not have a render model associated with it.");

            // Deserialize the render model tag
            RenderModel renderModel;
            using (var cacheStream = Cache.OpenCacheRead())
                renderModel = Cache.Deserialize<RenderModel>(cacheStream, Decorator.RenderModel);
            
            if (renderModel.Geometry.Resource is null)
                return new TagToolError(CommandError.CustomError, "Render model does not have a resource associated with it");

            if (renderModel.InstancePlacements.Count == 0)
                return new TagToolError(CommandError.CustomError, "The model has no variants.");

            if (args.Count < 1 || args.Count > 3)
                return new TagToolError(CommandError.ArgCount);

            var variants = renderModel.InstancePlacements
                .Select(x => Cache.StringTable.GetString(x.Name)).Distinct().OrderBy(v => v).ToArray();

            var exportFilePath = args[0];
            var exportFileFormat = args[0].Contains(".") ? Path.GetExtension(args[0]).Replace(".", "") : "amf";
            var filterArg = args.Count > 1 && (variants.Contains(args[1].ToLower()) || args[1] == "?" || args[1] == "*") ? args[1] : "*";
            var extractBitmaps = (args.Count == 2 && filterArg != "dds" && args[1] == "dds") || (args.Count == 3 && args[2] == "dds");

            var modelName = Path.GetFileNameWithoutExtension(Tag.Name is null ? Tag.Index.ToString("X8") : Tag.Name);
            if (!exportFilePath.Contains(".")) exportFilePath = Path.Combine(exportFilePath, $"{modelName}.{exportFileFormat}");
            exportFilePath = exportFilePath.Replace("*", modelName);

            var exportBitmapsFolder = extractBitmaps ? Path.GetDirectoryName(exportFilePath) : null;


            // Prompt for variants if requested
            string[] modelVariant =
                (filterArg == "*") ? null :
                (filterArg == "?") ? new TagToolChoicePrompt.Multiple(variants).Prompt().Where(x => x.Value == true).Select(x => x.Key).ToArray() :
                new string[] { filterArg };

            // Extract model and bitmaps (if requested)
            var resource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(renderModel.Geometry.Resource);
            renderModel.Geometry.SetResourceBuffers(resource, false);
            ModelExtractor extractor = new ModelExtractor(Cache, renderModel, Decorator.Texture.Name);
            if (!extractor.Export(exportFileFormat, exportFilePath, exportBitmapsFolder, modelVariant))
                return new TagToolError(CommandError.OperationFailed, "Export failed");

            return true;
        }
    }
}
