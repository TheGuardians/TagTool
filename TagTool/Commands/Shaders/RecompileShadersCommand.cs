using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using static TagTool.Commands.Shaders.GenerateShaderCommand;
using TagTool.Shaders.ShaderGenerator;
using TagTool.Cache.HaloOnline;

namespace TagTool.Commands.Shaders
{
    class RecompileShadersCommand : Command
    {
        GameCache Cache;

        public RecompileShadersCommand(GameCache cache) :
            base(true,

                "RecompileShaders",
                "Recompiles all shader templates",

                "RecompileShaders [shader type]",

                "Recompiles all shader templates\n" +
                "[shader type] - Specify shader type, EX. \"shader\" for \'rmsh\'.")
        {
            Cache = cache;
        }

        private static Dictionary<string, List<string>> ExplicitShaderGroups = new Dictionary<string, List<string>>
        {
            { "decorator", new List<string> { "decorator_default", "decorator_no_wind", "decorator_shaded", "decorator_static", "decorator_sun", "decorator_wavy" } },
            { "chud", new List<string> { "chud_cortana_camera", "chud_cortana_composite", "chud_cortana_offscreen", "chud_cortana_screen", "chud_cortana_screen_final",
                "chud_crosshair", "chud_directional_damage", "chud_directional_damage_apply", "chud_double_gradient", "chud_emblem", "chud_medal", "chud_meter",
                "chud_meter_chapter", "chud_meter_gradient", "chud_meter_shield", "chud_meter_single_color", "chud_navpoint", "chud_radial_gradient",
                "chud_really_simple", "chud_sensor", "chud_simple", "chud_solid", "chud_text_simple", "chud_texture_cam", "chud_turbulence" } },
            { "shadow_apply", new List<string> { "shadow_apply", "shadow_apply_bilinear", "shadow_apply_fancy", "shadow_apply_faster", "shadow_apply_point", } },
            { "final_composite", new List<string> { "final_composite", "final_composite_debug", "final_composite_dof", "final_composite_zoom" } },
            { "displacement", new List<string> { "displacement", "displacement_motion_blur" } },
        };

        public override object Execute(List<string> args)
        {
            using (var stream = Cache.OpenCacheReadWrite())
            {
                if (args.Count > 0)
                {
                    string shaderType = args[0].ToLower();

                    if (ExplicitShaderGroups.ContainsKey(shaderType))
                        return RecompileExplicitShaderGroup(stream, shaderType);

                    return RecompileShaderTypeAsync(stream, shaderType);
                }
                else
                {
                    foreach (string shaderType in Enum.GetNames(typeof(HaloShaderGenerator.Shared.ShaderType)))
                    {
                        RecompileShaderTypeAsync(stream, shaderType.ToLower());
                    }
                }
            }
            return true;
        }

        private object RecompileExplicitShaderGroup(Stream stream, string shaderType)
        {
            var shaderList = ExplicitShaderGroups[shaderType];

            List<SExplicitRecompileInfo> recompileInfo = new List<SExplicitRecompileInfo>();

            if (shaderType == "chud")
            {
                var chgd = Cache.Deserialize<ChudGlobalsDefinition>(stream, Cache.TagCache.FindFirstInGroup("chgd"));

                for (int i = 0; i < chgd.HudShaders.Count-1; i++) // skip last as it is HO only
                {
                    SExplicitRecompileInfo info = new SExplicitRecompileInfo
                    {
                        IsChud = true,
                        PixelTag = chgd.HudShaders[i].PixelShader,
                        VertexTag = chgd.HudShaders[i].VertexShader,
                        ExplicitName = chgd.HudShaders[i].PixelShader.Name.Split('\\').Last(),
                    };
                    recompileInfo.Add(info);
                }
            }
            else
            {
                foreach (var explicitShader in shaderList)
                {
                    SExplicitRecompileInfo info = new SExplicitRecompileInfo();

                    if (!Cache.TagCache.TryGetTag($"rasterizer\\shaders\\{explicitShader}.pixl", out info.PixelTag) ||
                        !Cache.TagCache.TryGetTag($"rasterizer\\shaders\\{explicitShader}.vtsh", out info.VertexTag))
                    {
                        new TagToolError(CommandError.CustomMessage, $"Explicit shader {explicitShader} could not be found (skipping)");
                    }
                    else
                    {
                        info.IsChud = false;
                        info.ExplicitName = explicitShader;
                        recompileInfo.Add(info);
                    }
                }
            }

            List<Task<SExplicitRecompileInfo>> tasks = new List<Task<SExplicitRecompileInfo>>();

            foreach (var info in recompileInfo)
            {
                Task<SExplicitRecompileInfo> generatorTask = Task.Run(() => {
                    return GenerateExplicitShaderAsync(Cache, info);
                });
                tasks.Add(generatorTask);
            }

            float percentageComplete = 0.00f;
            Console.Write($"\rRecompiling {shaderType} shaders... {string.Format("{0:0.00}", percentageComplete)}%");

            int completed = 0;
            while (completed != tasks.Count)
            {
                int count = tasks.FindAll(x => x.IsCompleted).Count;
                if (count > completed)
                {
                    completed = count;

                    percentageComplete = ((float)count / (float)tasks.Count) * 100.0f;
                    Console.Write($"\rRecompiling {shaderType} templates... {string.Format("{0:0.00}", percentageComplete)}%");
                }

                System.Threading.Thread.Sleep(100); // wait to prevent constant cli writes
            }

            Console.Write($"\rSuccessfully recompiled {tasks.Count} {shaderType} templates. Serializing...");

            // serialize
            foreach (var task in tasks)
            {
                Cache.Serialize(stream, task.Result.PixelTag, task.Result.PixelShader);
                Cache.Serialize(stream, task.Result.VertexTag, task.Result.VertexShader);
                (Cache as GameCacheHaloOnlineBase).SaveTagNames();
            }

            Console.Write($"\rSuccessfully recompiled {tasks.Count} {shaderType} shaders. Serializing... Done");
            Console.WriteLine();

            return true;
        }

        private object RecompileShaderTypeAsync(Stream stream, string shaderType)
        {
            if (!Cache.TagCache.TryGetTag($"shaders\\{shaderType}.rmdf", out CachedTag rmdfTag))
                return new TagToolError(CommandError.TagInvalid, $"Missing \"{shaderType}\" rmdf");

            var rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);

            if (rmdf.GlobalVertexShader == null || rmdf.GlobalPixelShader == null)
                return new TagToolError(CommandError.TagInvalid, "A global shader was missing from rmdf");

            var glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);
            var glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);

            List<CachedTag> regenTags = new List<CachedTag>();
            foreach (var tag in Cache.TagCache.NonNull())
            {
                if (tag.Group.Tag != "rmt2" ||
                    tag.Name.StartsWith("ms30") ||
                    !tag.Name.Split('\\')[1].StartsWith(shaderType + "_templates"))
                    continue;
                regenTags.Add(tag);
            }

            List<STemplateRecompileInfo> recompileInfo = new List<STemplateRecompileInfo>();

            foreach (var tag in regenTags)
            {
                List<byte> options = new List<byte>();
                foreach (var option in tag.Name.Split('\\')[2].Remove(0, 1).Split('_'))
                    options.Add(byte.Parse(option));
                while (options.Count < rmdf.Categories.Count)
                    options.Add(0);
                var aOptions = options.ToArray();

                STemplateRecompileInfo info = new STemplateRecompileInfo
                {
                    Name = $"shaders\\{shaderType}_templates\\_{string.Join("_", aOptions)}",
                    ShaderType = shaderType,
                    Options = aOptions,
                    Tag = tag,
                    Dependants = GetDependantsAsync(Cache, stream, shaderType, aOptions),
                    AllRmopParameters = ShaderGeneratorNew.GatherParameters(Cache, stream, rmdf, options)
                };

                recompileInfo.Add(info);
            }

            List<Task<STemplateRecompileInfo>> tasks = new List<Task<STemplateRecompileInfo>>();

            foreach (var info in recompileInfo)
            {
                Task<STemplateRecompileInfo> generatorTask = Task.Run(() => { 
                    return GenerateRenderMethodTemplateAsync(Cache, info, rmdf, glvs, glps); });
                tasks.Add(generatorTask);
            }

            float percentageComplete = 0.00f;
            Console.Write($"\rRecompiling {shaderType} templates... {string.Format("{0:0.00}", percentageComplete)}%");

            int completed = 0;
            while (completed != tasks.Count)
            {
                int count = tasks.FindAll(x => x.IsCompleted).Count;
                if (count > completed)
                {
                    completed = count;

                    percentageComplete = ((float)count / (float)tasks.Count) * 100.0f;
                    Console.Write($"\rRecompiling {shaderType} templates... {string.Format("{0:0.00}", percentageComplete)}%");
                }

                System.Threading.Thread.Sleep(250); // wait to prevent constant cli writes
            }

            Console.Write($"\rSuccessfully recompiled {tasks.Count} {shaderType} templates. Serializing...");

            // serialize
            foreach (var task in tasks)
            {
                if (!Cache.TagCache.TryGetTag(task.Result.Name + ".pixl", out task.Result.Template.PixelShader))
                    task.Result.Template.PixelShader = Cache.TagCache.AllocateTag<PixelShader>(task.Result.Name);
                if (!Cache.TagCache.TryGetTag(task.Result.Name + ".vtsh", out task.Result.Template.VertexShader))
                    task.Result.Template.VertexShader = Cache.TagCache.AllocateTag<VertexShader>(task.Result.Name);

                Cache.Serialize(stream, task.Result.Template.PixelShader, task.Result.PixelShader);
                Cache.Serialize(stream, task.Result.Template.VertexShader, task.Result.VertexShader);
                Cache.Serialize(stream, task.Result.Tag, task.Result.Template);

                (Cache as GameCacheHaloOnlineBase).SaveTagNames();

                ReserializeDependantsAsync(Cache, stream, task.Result.Template, task.Result.Dependants);
            }

            Console.Write($"\rSuccessfully recompiled {tasks.Count} {shaderType} templates. Serializing... Done");
            Console.WriteLine();

            // validation
            foreach (var task in tasks)
            {
                var rmt2 = Cache.Deserialize<RenderMethodTemplate>(stream, task.Result.Tag);
                var pixl = Cache.Deserialize<PixelShader>(stream, rmt2.PixelShader);

                if (rmt2.PixelShader.Name == null || rmt2.PixelShader.Name == "")
                    new TagToolWarning($"pixel_shader {rmt2.PixelShader.Index:X16} has no name");

                for (int i = 0; i < pixl.EntryPointShaders.Count; i++)
                {
                    bool entryNeeded = rmdf.EntryPoints.Any(x => (int)x.EntryPoint == i) && 
                        (glps.EntryPoints[i].DefaultCompiledShaderIndex == -1 && glps.EntryPoints[i].CategoryDependency.Count == 0);

                    if (pixl.EntryPointShaders[i].Count > 0 && !entryNeeded)
                        new TagToolWarning($"{rmt2.PixelShader.Name} has unneeded entry point shader {(TagTool.Shaders.EntryPoint)i}");

                    if (pixl.EntryPointShaders[i].Count == 0 && entryNeeded)
                        new TagToolWarning($"{rmt2.PixelShader.Name} missing entry point shader {(TagTool.Shaders.EntryPoint)i}");

                    if (pixl.EntryPointShaders[i].Count > 0 && pixl.EntryPointShaders[i].Offset >= pixl.Shaders.Count)
                        new TagToolWarning($"{rmt2.PixelShader.Name} has invalid compiled shader indices {i}");
                }
            }

            return true;
        }

        private object RecompileShaderType(Stream stream, string shaderType)
        {
            // check for rmdf
            if (!Cache.TagCache.TryGetTag($"shaders\\{shaderType}.rmdf", out CachedTag rmdfTag))
                return new TagToolError(CommandError.TagInvalid, $"Missing \"{shaderType}\" rmdf");

            var rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);

            if (rmdf.GlobalVertexShader == null || rmdf.GlobalPixelShader == null)
                return new TagToolError(CommandError.TagInvalid, "A global shader was missing from rmdf");

            List<CachedTag> regenTags = new List<CachedTag>();
            foreach (var tag in Cache.TagCache.NonNull())
            {
                if (tag.Group.Tag != "rmt2" ||
                    tag.Name.StartsWith("ms30") ||
                    !tag.Name.Split('\\')[1].StartsWith(shaderType + "_templates"))
                    continue;
                regenTags.Add(tag);
            }


            float percentageComplete = 0.00f;
            Console.Write($"\rRecompiling {shaderType} templates... {string.Format("{0:0.00}", percentageComplete)}%");

            for (int i = 0; i < regenTags.Count; i++)
            {
                List<byte> options = new List<byte>();

                foreach (var option in regenTags[i].Name.Split('\\')[2].Remove(0, 1).Split('_'))
                    options.Add(byte.Parse(option));

                // make up options count, may not work very well
                while (options.Count != rmdf.Categories.Count)
                    options.Add(0);

                GenerateShaderCommand.GenerateRenderMethodTemplate(Cache, stream, shaderType, options.ToArray(), rmdf, true);

                percentageComplete = ((float)i / (float)regenTags.Count) * 100.0f;
                Console.Write($"\rRecompiling {shaderType} templates... {string.Format("{0:0.00}", percentageComplete)}%");
            }

            Console.Write($"\rSuccessfully recompiled {regenTags.Count} {shaderType} templates");
            Console.WriteLine();

            return true;
        }
    }
}
