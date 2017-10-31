using TagTool.Commands.Bitmaps;
using TagTool.Commands.Models;
using TagTool.Commands.RenderModels;
using TagTool.Commands.RenderMethods;
using TagTool.Commands.Scenarios;
using TagTool.Commands.Unicode;
using TagTool.Commands.Files;
using TagTool.Commands.Video;
using BlamCore.Serialization;
using BlamCore.TagDefinitions;
using TagTool.Commands.ModelAnimationGraphs;
using TagTool.Commands.ScenarioStructureBSPs;
using BlamCore.Cache;
using System.Reflection;
using System.IO;
using System.Xml;
using TagTool.Commands.Sounds;
using TagTool.Commands.PixelShaders;
using TagTool.Commands.VertexShaders;
using TagTool.Commands.ScenarioLightmaps;

namespace TagTool.Commands.Editing
{
    static class EditTagContextFactory
    {
        public static XmlDocument Documentation { get; } = new XmlDocument();

        public static CommandContext Create(CommandContextStack contextStack, GameCacheContext cacheContext, CachedTagInstance tag)
        {
            var documentationPath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName}\\BlamCore.xml";

            if (Documentation.ChildNodes.Count == 0 && File.Exists(documentationPath))
                Documentation.Load(documentationPath);

            var groupName = cacheContext.GetString(tag.Group.Name);

            var tagName = $"0x{tag.Index:X4}";

            if (cacheContext.TagNames.ContainsKey(tag.Index))
            {
                tagName = cacheContext.TagNames[tag.Index];
                tagName = $"(0x{tag.Index:X4}) {tagName.Substring(tagName.LastIndexOf('\\') + 1)}";
            }

            var commandContext = new CommandContext(contextStack.Context, string.Format("{0}.{1}", tagName, groupName));

            object definition = null;

            using (var stream = cacheContext.OpenTagCacheRead())
                definition = cacheContext.Deserializer.Deserialize(new TagSerializationContext(stream, cacheContext, tag), TagDefinition.Find(tag.Group.Tag));

            switch (tag.Group.Tag.ToString())
            {
                case "vfsl": // vfiles_list
                    VFilesContextFactory.Populate(commandContext, cacheContext, tag, (VFilesList)definition);
                    break;

                case "bink":
                    VideoContextFactory.Populate(commandContext, cacheContext, tag, (Bink)definition);
                    break;

                case "unic": // multilingual_unicode_string_list
                    UnicodeContextFactory.Populate(commandContext, cacheContext, tag, (MultilingualUnicodeStringList)definition);
                    break;

                case "bitm": // bitmap
                    BitmapContextFactory.Populate(commandContext, cacheContext, tag, (Bitmap)definition);
                    break;

                case "hlmt": // model
                    ModelContextFactory.Populate(commandContext, cacheContext, tag, (Model)definition);
                    break;

                case "mode": // render_model
                    RenderModelContextFactory.Populate(commandContext, cacheContext, tag, (RenderModel)definition);
                    break;

                case "pmdf":
                    ParticleModelContextFactory.Populate(commandContext, cacheContext, tag, (ParticleModel)definition);
                    break;

                case "jmad":
                    AnimationContextFactory.Populate(commandContext, cacheContext, tag, (ModelAnimationGraph)definition);
                    break;

                case "rm  ": // render_method
                case "rmsh": // shader
                case "rmd ": // shader_decal
                case "rmfl": // shader_foliage
                case "rmhg": // shader_halogram
                case "rmss": // shader_screen
                case "rmtr": // shader_terrain
                case "rmw ": // shader_water
                case "rmzo": // shader_zonly
                case "rmcs": // shader_custom
                    RenderMethodContextFactory.Populate(commandContext, cacheContext, tag, (RenderMethod)definition);
                    break;

                case "pixl":
                    PixelShaderContextFactory.Populate(commandContext, cacheContext, tag, (PixelShader)definition);
                    break;

                case "scnr":
                    ScnrContextFactory.Populate(commandContext, cacheContext, tag, (Scenario)definition);
                    break;

                case "sbsp":
                    BSPContextFactory.Populate(commandContext, cacheContext, tag, (ScenarioStructureBsp)definition);
                    break;
                case "Lbsp":
                    LightmapContextFactory.Populate(commandContext, cacheContext, tag, (ScenarioLightmapBspData)definition);
                    break;

                case "snd!":
                    SoundContextFactory.Populate(commandContext, cacheContext, tag, (Sound)definition);
                    break;

				case "vtsh":
					VertexShaderContextFactory.Populate(commandContext, cacheContext, tag, (VertexShader)definition);
					break;
			}

            var structure = new TagStructureInfo(TagDefinition.Find(tag.Group.Tag));

            commandContext.AddCommand(new ListFieldsCommand(cacheContext, structure, definition));
            commandContext.AddCommand(new SetFieldCommand(contextStack, cacheContext, tag, structure, definition));
            commandContext.AddCommand(new EditBlockCommand(contextStack, cacheContext, tag, definition));
            commandContext.AddCommand(new AddBlockElementsCommand(contextStack, cacheContext, tag, structure, definition));
            commandContext.AddCommand(new RemoveBlockElementsCommand(contextStack, cacheContext, tag, structure, definition));
            commandContext.AddCommand(new CopyBlockElementsCommand(contextStack, cacheContext, tag, structure, definition));
            commandContext.AddCommand(new PasteBlockElementsCommand(contextStack, cacheContext, tag, structure, definition));
            commandContext.AddCommand(new ForEachCommand(contextStack, cacheContext, tag, structure, definition));
            commandContext.AddCommand(new SaveTagChangesCommand(cacheContext, tag, definition));
            commandContext.AddCommand(new PokeTagChangesCommand(cacheContext, tag, definition));
            commandContext.AddCommand(new ExitToCommand(contextStack));

            return commandContext;
        }
    }
}
