using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using System.Reflection;
using System.IO;
using System.Xml;
using TagTool.Commands.Bitmaps;

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
                /*
                case "bipd":
                    BipedContextFactory.Populate(commandContext, cacheContext, tag, (Biped)definition);
                    break;

                case "bink":
                    VideoContextFactory.Populate(commandContext, cacheContext, tag, (Bink)definition);
                    break;

                case "coll":
                    CollisionModelContextFactory.Populate(commandContext, cacheContext, tag, (CollisionModel)definition);
                    break;

                case "forg":
                    ForgeContextFactory.Populate(commandContext, cacheContext, tag, (ForgeGlobalsDefinition)definition);
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
                */
            }
            
            var structure = TagStructure.GetTagStructureInfo(TagDefinition.Find(tag.Group.Tag), cache.Version);

            commandContext.AddCommand(new ListFieldsCommand(cache, structure, definition));
            commandContext.AddCommand(new SetFieldCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new ExitToCommand(contextStack));

            return commandContext;
        }
    }
}
