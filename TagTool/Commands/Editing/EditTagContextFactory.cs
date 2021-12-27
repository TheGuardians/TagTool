using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using System.Reflection;
using System.IO;
using System.Xml;
using TagTool.Commands.Bitmaps;
using TagTool.Commands.Bipeds;
using TagTool.Commands.Decorators;
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
using TagTool.Commands.ScenarioStructureBSPs;
using TagTool.Commands.Scenarios;
using TagTool.Cache.HaloOnline;
using DefinitionsGen2 = TagTool.Tags.Definitions.Gen2;
using CommandsGen2 = TagTool.Commands.Gen2;
using DefinitionsGen4 = TagTool.Tags.Definitions.Gen4;
using CommandsGen4 = TagTool.Commands.Gen4;
using TagTool.Commands.Common;

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

            var groupName = tag.Group.ToString();
            var tagName = tag?.Name ?? $"0x{tag.Index:X4}";

            var commandContext = new CommandContext(contextStack.Context, string.Format("{0}.{1}", tagName, groupName));
            commandContext.ScriptGlobals.Add(ExecuteCSharpCommand.GlobalTagKey, tag);
            commandContext.ScriptGlobals.Add(ExecuteCSharpCommand.GlobalDefinitionKey, definition);

            commandContext.AddCommand(new ExecuteCSharpCommand(contextStack));
            if (CacheVersionDetection.IsInGen(CacheGeneration.Third, cache.Version) || CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, cache.Version))
            {
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

                    case "dctr":
                        DecoratorSetContextFactory.Populate(commandContext, cache, tag, (DecoratorSet)definition);
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
                        ShaderContextFactory<PixelShader>.Populate(commandContext, cache, tag, (PixelShader)definition);
                        break;

                    case "vtsh":
                        ShaderContextFactory<VertexShader>.Populate(commandContext, cache, tag, (VertexShader)definition);
                        break;

                    case "glps":
                        ShaderContextFactory<GlobalPixelShader>.Populate(commandContext, cache, tag, (GlobalPixelShader)definition);
                        break;

                    case "glvs":
                        ShaderContextFactory<GlobalVertexShader>.Populate(commandContext, cache, tag, (GlobalVertexShader)definition);
                        break;

                    case "Lbsp":
                        LightmapContextFactory.Populate(commandContext, cache, tag, (ScenarioLightmapBspData)definition);
                        break;


                    case "vfsl":
                        VFilesContextFactory.Populate(commandContext, cache, tag, (VFilesList)definition);
                        break;

                    case "mode":
                        RenderModelContextFactory.Populate(commandContext, cache, tag, (RenderModel)definition);
                        break;

                    case "sbsp":
                        BSPContextFactory.Populate(commandContext, cache, tag, (ScenarioStructureBsp)definition);
                        break;

                    case "scnr":
                        ScnrContextFactory.Populate(commandContext, cache, tag, (Scenario)definition);
                        break;
                }
            }

            if (CacheVersionDetection.IsInGen(CacheGeneration.Second, cache.Version))
            {
                switch (tag.Group.Tag.ToString())
                {
                    case "sbsp":
                        CommandsGen2.ScenarioStructureBSPs.BSPContextFactory.Populate(commandContext, cache, tag, (DefinitionsGen2.ScenarioStructureBsp)definition);
                        break;
                    case "jmad":
                        CommandsGen2.ModelAnimationGraphs.AnimationContextFactory.Populate(commandContext, cache, tag, (DefinitionsGen2.ModelAnimationGraph)definition);
                        break;
                }
            }
            if (CacheVersionDetection.IsInGen(CacheGeneration.Fourth, cache.Version))
            {
                switch (tag.Group.Tag.ToString())
                {
                    case "jmad":
                        CommandsGen4.ModelAnimationGraphs.AnimationContextFactory.Populate(commandContext, cache, tag, (DefinitionsGen4.ModelAnimationGraph)definition);
                        break;
                }
            }

            var structure = TagStructure.GetTagStructureInfo(cache.TagCache.TagDefinitions.GetTagDefinitionType(tag.Group), cache.Version, cache.Platform);

            commandContext.AddCommand(new ListFieldsCommand(cache, structure, definition));
            commandContext.AddCommand(new SetFieldCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new EditBlockCommand(contextStack, cache, tag, definition));
            commandContext.AddCommand(new AddBlockElementsCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new RemoveBlockElementsCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new CopyBlockElementsCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new PasteBlockElementsCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new MoveBlockElementCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new SwapBlockElementsCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new ForEachCommand(contextStack, cache, tag, structure, definition));
            commandContext.AddCommand(new SaveTagChangesCommand(cache, tag, definition));
            commandContext.AddCommand(new ExportCommandsCommand(cache, definition as TagStructure));

            commandContext.AddCommand(new ExitToCommand(contextStack));

            if(CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, cache.Version))
            {
                commandContext.AddCommand(new PokeTagChangesCommand(cache as GameCacheHaloOnlineBase, tag as CachedTagHaloOnline, definition));
            }

            return commandContext;
        }
    }
}
