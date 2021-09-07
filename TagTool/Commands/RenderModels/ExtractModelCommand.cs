using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                  "Extracts the current render model definition to AMF, DAE or OBJ",

                  usage:
                  "ExtractModel [variant name] <extratPath> [dds]",

                  examples:
                  "ExtractModel d:\\elite_rip.amf dds\n" +
                  "ExtractModel major d:\\banshee dds",

                  helpMessage:
                  "- [dds] Will extract asspciated bitmaps in dds format to an auto-named folder.\n" +
                  "- If <extratPath> is a folder, an auto-named AMF file will be extrcted there.\n" +
                  "- Although dds will extract correctly with all formats, obj currently exports with an invalid mtl file and dae seems to export without materials referenced, so advised to use AMF for now if you want the texture pre-applied.\n" +
                  "- Variant selection not working with AMF, it just extracts all, i'll be back to fix soon.")
        {
            Cache = cacheContext;
            Tag = tag;
            Definition = model;
        }
        
        public override object Execute(List<string> args)
        {
            string variantName = "*";
            string modelFileName = "";
            bool extractBitmaps = false;
            
            switch (args.Count)
            {
                case (1):
                    modelFileName = args[0];
                    break;

                case (2):
                    if (args[1] == "dds")
                    {
                        modelFileName = args[0];
                        extractBitmaps = true;
                    }
                    else
                    {
                        variantName = args[0];
                        modelFileName = args[1];
                    }
                    break;
                
                case (3):
                    variantName = args[0];
                    modelFileName = args[1];
                    extractBitmaps = true;
                    break;

                default:
                    return new TagToolError(CommandError.ArgCount);
            }

            var fileType = Path.GetExtension(modelFileName).Replace(".", "");
            if (fileType == "") fileType = "amf";

            // If model file name has no extension, throw it away and replace with
            if (!modelFileName.Contains("."))
            {
                if (Tag.Name != null)
                {
                    var split = Tag.Name.Split('\\');
                    modelFileName += "\\" + split[split.Length - 1];
                }
                else
                    modelFileName += "\\" + Tag.Index.ToString("X8");

                modelFileName += "." + fileType;
            }

            if (Definition.Geometry.Resource == null)
            {
                new TagToolError(CommandError.CustomError, "Render model does not have a resource associated with it!");
                return true;
            }

            // Deserialize the resource definition

            var definition = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(Definition.Geometry.Resource);
            Definition.Geometry.SetResourceBuffers(definition);

            using (var resourceStream = new MemoryStream())
            {
                var modelFile = new FileInfo(modelFileName);

                if (!modelFile.Directory.Exists)
                    modelFile.Directory.Create();

                var extractBitmapsPath = "";
                if (extractBitmaps)
                    extractBitmapsPath = modelFile.FullName.Substring(0, modelFile.FullName.Length - modelFile.Extension.Length);

                ModelExtractor extractor = new ModelExtractor(Cache, Definition);
                extractor.ExtractRenderModel(variantName);

                switch (fileType)
                {
                    case "obj":
                        extractor.ExportObject(modelFile);
                        break;
                    case "dae":
                        extractor.ExportCollada(modelFile);
                        break;
                    case "amf":
                        extractor.ExportAMF(modelFile);
                        break;
                    default:
                        return new TagToolError(CommandError.ArgInvalid, $"Unsupported file type \"{fileType}\"");
                }

                if (extractBitmapsPath != "")
                    extractor.ExtractBitmaps(extractBitmapsPath, "dds");

                Console.WriteLine($"Model successfully extracted to \"{modelFileName}\"");
            }

            return true;
        }
    }
}