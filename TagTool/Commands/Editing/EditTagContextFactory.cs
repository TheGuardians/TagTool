using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using System.Reflection;
using System.IO;
using System.Xml;
using TagTool.Commands.Video;
using TagTool.Commands.Bitmaps;
using TagTool.Commands.CollisionModels;
using TagTool.Commands.Models;
using TagTool.Commands.ModelAnimationGraphs;
using TagTool.Commands.ScenarioLightmaps;
using TagTool.Commands.RenderModels;
using TagTool.Commands.RenderMethods;
using TagTool.Commands.ScenarioStructureBSPs;
using TagTool.Commands.Scenarios;
using TagTool.Commands.Sounds;
using TagTool.Commands.Unicode;
using TagTool.Commands.Files;
using TagTool.Tags;

namespace TagTool.Commands.Editing
{
    static class EditTagContextFactory
    {
        public static XmlDocument Documentation { get; } = new XmlDocument();

        public static CommandContext Create(CommandContextStack contextStack, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, object definition)
        {
            var documentationPath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName}\\TagTool.xml";

            if (Documentation.ChildNodes.Count == 0 && File.Exists(documentationPath))
                Documentation.Load(documentationPath);

            var groupName = cacheContext.GetString(tag.Group.Name);
            var tagName = tag?.Name ?? $"0x{tag.Index:X4}";

            var commandContext = new CommandContext(contextStack.Context, string.Format("{0}.{1}", tagName, groupName));

            switch (tag.Group.Tag.ToString())
            {
                case "bink":
                    VideoContextFactory.Populate(commandContext, cacheContext, tag, (Bink)definition);
                    break;

                case "bitm": // bitmap
                    BitmapContextFactory.Populate(commandContext, cacheContext, tag, (Bitmap)definition);
                    break;

                case "coll":
                    CollisionModelContextFactory.Populate(commandContext, cacheContext, tag, (CollisionModel)definition);
                    break;

                case "hlmt": // model
                    ModelContextFactory.Populate(commandContext, cacheContext, tag, (Model)definition);
                    break;

                case "jmad":
                    AnimationContextFactory.Populate(commandContext, cacheContext, tag, (ModelAnimationGraph)definition);
                    break;

                case "Lbsp":
                    LightmapContextFactory.Populate(commandContext, cacheContext, tag, (ScenarioLightmapBspData)definition);
                    break;

                case "mode": // render_model
                    RenderModelContextFactory.Populate(commandContext, cacheContext, tag, (RenderModel)definition);
                    break;

                case "pmdf":
                    ParticleModelContextFactory.Populate(commandContext, cacheContext, tag, (ParticleModel)definition);
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

                case "sbsp":
                    BSPContextFactory.Populate(commandContext, cacheContext, tag, (ScenarioStructureBsp)definition);
                    break;

                case "scnr":
                    ScnrContextFactory.Populate(commandContext, cacheContext, tag, (Scenario)definition);
                    break;

                case "snd!":
                    SoundContextFactory.Populate(commandContext, cacheContext, tag, (Sound)definition);
                    break;

                case "unic": // multilingual_unicode_string_list
                    UnicodeContextFactory.Populate(commandContext, cacheContext, tag, (MultilingualUnicodeStringList)definition);
                    break;

                case "vfsl": // vfiles_list
                    VFilesContextFactory.Populate(commandContext, cacheContext, tag, (VFilesList)definition);
                    break;

                case "pixl":
                    Shaders.ShaderContextFactory<PixelShader>.Populate(commandContext, cacheContext, tag, (PixelShader)definition);
                    break;

                case "vtsh":
                    Shaders.ShaderContextFactory<VertexShader>.Populate(commandContext, cacheContext, tag, (VertexShader)definition);
                    break;

                case "glps":
                    Shaders.ShaderContextFactory<GlobalPixelShader>.Populate(commandContext, cacheContext, tag, (GlobalPixelShader)definition);
                    break;

                case "glvs":
                    Shaders.ShaderContextFactory<GlobalVertexShader>.Populate(commandContext, cacheContext, tag, (GlobalVertexShader)definition);
                    break;

                case "rmt2":
                    Shaders.RenderMethodTemplateContextFactory.Populate(commandContext, cacheContext, tag, (RenderMethodTemplate)definition);
                    break;
            }

            var structure = ReflectionCache.GetTagStructureInfo(TagDefinition.Find(tag.Group.Tag));

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
