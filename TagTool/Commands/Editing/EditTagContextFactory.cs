using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using System.Reflection;
using System.IO;
using System.Xml;
using TagTool.Commands.Bitmaps;
using TagTool.Commands.Bipeds;
using TagTool.Commands.Video;
using TagTool.Commands.Unicode;
using TagTool.Commands.CollisionModels;
using TagTool.Commands.Forge;
using TagTool.Commands.Models;
using TagTool.Commands.RenderModels;
using TagTool.Commands.ModelAnimationGraphs;
using TagTool.Commands.Sounds;
using TagTool.Commands.RenderMethods;
using TagTool.Commands.Shaders;
using TagTool.Commands.ScenarioLightmaps;
using TagTool.Commands.Files;

namespace TagTool.Commands.Editing
{
    static class EditTagContextFactory
    {
        public static XmlDocument Documentation { get; } = new XmlDocument();

        public static CommandContext Create(CommandContextStack contextStack, GameCache cache, CachedTag tag, object definition)
        {
            var documentationPath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName}\\TagTool.xml";

            if (Documentation.ChildNodes.Count == 0 && File.Exists(documentationPath))
                Documentation.Load(documentationPath);

            var groupName = cache.StringTable.GetString(tag.Group.Name);
            var tagName = tag?.Name ?? $"0x{tag.Index:X4}";

            var commandContext = new CommandContext(contextStack.Context, string.Format("{0}.{1}", tagName, groupName));
            
            switch (tag.Group.Tag.ToString())
            {
                case "bitm":
                    BitmapContextFactory.Populate(commandContext, cache, tag, (Bitmap)definition);
                    break;
                
                case "bipd":
                    BipedContextFactory.Populate(commandContext, cache, tag, (Biped)definition);
                    break;
                
                case "bink":
                    VideoContextFactory.Populate(commandContext, cache, tag, (Bink)definition);
                    break;

                case "coll":
                    CollisionModelContextFactory.Populate(commandContext, cache, tag, (CollisionModel)definition);
                    break;

                case "forg":
                    ForgeContextFactory.Populate(commandContext, cache, tag, (ForgeGlobalsDefinition)definition);
                    break;
              
                case "hlmt":
                    ModelContextFactory.Populate(commandContext, cache, tag, (Model)definition);
                    break;

                case "jmad":
                    AnimationContextFactory.Populate(commandContext, cache, tag, (ModelAnimationGraph)definition);
                    break;

                case "pmdf":
                    ParticleModelContextFactory.Populate(commandContext, cache, tag, (ParticleModel)definition);
                    break;

                case "unic":
                    UnicodeContextFactory.Populate(commandContext, cache, tag, (MultilingualUnicodeStringList)definition);
                    break;

                case "snd!":
                    SoundContextFactory.Populate(commandContext, cache, tag, (Sound)definition);
                    break;

                case "rmt2":
                    RenderMethodTemplateContextFactory.Populate(commandContext, cache, tag, (RenderMethodTemplate)definition);
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
                    RenderMethodContextFactory.Populate(commandContext, cache, tag, (RenderMethod)definition);
                    break;

                case "pixl":
                    Shaders.ShaderContextFactory<PixelShader>.Populate(commandContext, cache, tag, (PixelShader)definition);
                    break;

                case "vtsh":
                    Shaders.ShaderContextFactory<VertexShader>.Populate(commandContext, cache, tag, (VertexShader)definition);
                    break;

                case "glps":
                    Shaders.ShaderContextFactory<GlobalPixelShader>.Populate(commandContext, cache, tag, (GlobalPixelShader)definition);
                    break;

                case "glvs":
                    Shaders.ShaderContextFactory<GlobalVertexShader>.Populate(commandContext, cache, tag, (GlobalVertexShader)definition);
                    break;

                case "Lbsp":
                    LightmapContextFactory.Populate(commandContext, cache, tag, (ScenarioLightmapBspData)definition);
                    break;


                case "vfsl":
                    VFilesContextFactory.Populate(commandContext, cache, tag, (VFilesList)definition);
                    break;

                    /*

                case "mode": 
                    RenderModelContextFactory.Populate(commandContext, cacheContext, tag, (RenderModel)definition);
                    break;

                case "sbsp":
                    BSPContextFactory.Populate(commandContext, cacheContext, tag, (ScenarioStructureBsp)definition);
                    break;

                case "scnr":
                    ScnrContextFactory.Populate(commandContext, cacheContext, tag, (Scenario)definition);
                    break;                        
                                    */
            }
            
            var structure = TagStructure.GetTagStructureInfo(TagDefinition.Find(tag.Group.Tag), cache.Version);

            commandContext.AddCommand(new ListFieldsCommand(cache, structure, definition));
            commandContext.AddCommand(new SetFieldCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new EditBlockCommand(contextStack, cache, tag, definition));
            commandContext.AddCommand(new AddBlockElementsCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new RemoveBlockElementsCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new CopyBlockElementsCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new PasteBlockElementsCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new SaveTagChangesCommand(cache, tag, definition));
            commandContext.AddCommand(new PokeTagChangesCommand(cache, tag, definition));
            commandContext.AddCommand(new ExitToCommand(contextStack));

            return commandContext;
        }
    }
}
