using System.Collections.Generic;
using System.IO;
using System.Linq;

using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderModels
{
    class ExtractModelCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private RenderModel Definition { get; }

        public ExtractModelCommand(GameCache cacheContext, CachedTag tag, RenderModel model)
            : base(true,
                  
                  name:
                  "ExtractModel",
                  
                  description:
                  "Extracts the current render model definition as AMF, DAE or OBJ",

                  usage:
                  "ExtractModel <path> [variant] [dds]",

                  examples:
                  "extractmodel d:\\banshee major\n" +
                  "extractmodel d:\\*\\model.amf ?\n" +
                  "foreach mode has:smg extractmodel d:\\rips\\*.dae dds",

                  helpMessage:
                  "- [variant] Specify perm name, ? prompts with a list of names, default * extacts all.\n" +
                  "- [dds] Will extract used bitmaps in dds format to subfolders within <path> folder.\n" +
                  "- Format will be chosen based on extension given for output file, AMF if folder only.\n" +
                  "- All occurrence of * in <path>, will be replaced with the model name/index.\n" +
                  "- If <path> is a folder only, an auto-named AMF file will be extracted there.")
        {
            Cache = cacheContext;
            Tag = tag;
            Definition = model;
        }
        
        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 3)
                return new TagToolError(CommandError.ArgCount);

            var variants = Definition.Regions.SelectMany(
                r => r.Permutations.Select(p => Cache.StringTable.GetString(p.Name)) ).Distinct().OrderBy(v => v).ToList();

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
            var resource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(Definition.Geometry.Resource);
            Definition.Geometry.SetResourceBuffers(resource, false);
            ModelExtractor extractor = new ModelExtractor(Cache, Definition);
            if (!extractor.Export(exportFileFormat, exportFilePath, exportBitmapsFolder, modelVariant))
                return new TagToolError(CommandError.OperationFailed, "Export failed");

            return true;
        }
    }
}