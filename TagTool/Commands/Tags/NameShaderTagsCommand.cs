using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Tags
{
    class NameShaderTagsCommand : Command
    {
        private GameCacheHaloOnlineBase Cache { get; }

        public NameShaderTagsCommand(GameCacheHaloOnlineBase cache)
            : base(false,

                  "NameShaderTags",
                  "Attempts to name various shader tags in the current cache.",

                  "NameShaderTags",
                  "Attempts to name various shader tags in the current cache.")
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

        static readonly Dictionary<string, string> TagTypes = new Dictionary<string, string>
        {
            ["rmsh"] = "shader",
            ["rmd "] = "decal",
            ["rmhg"] = "halogram",
            ["rmw "] = "water",
            ["rmbk"] = "black",
            ["rmtr"] = "terrain",
            ["rmcs"] = "custom",
            ["rmfl"] = "foliage",
            ["rmss"] = "screen",
            ["rmct"] = "cortana",
            ["rmzo"] = "zonly",
            ["rmgl"] = "glass",
            ["prt3"] = "particle",
            ["ltvl"] = "light_volume",
            ["beam"] = "beam",
            ["decs"] = "decal",
            ["cntl"] = "contrail"
        };

        static private Dictionary<int, string> NamedRmt2s = new Dictionary<int, string>();
        static private List<int> NamedRmdfs = new List<int>();

        public override object Execute(List<string> args)
        {
            NamedRmt2s.Clear();

            using (var stream = Cache.OpenCacheRead())
            {
                // templated
                foreach (var tag in Cache.TagCache.NonNull())
                {
                    if (!TagTypes.ContainsKey(tag.Group.Tag.ToString()))
                        continue;

                    var definition = Cache.Deserialize(stream, tag);

                    if (tag.IsInGroup("rm  "))
                    {
                        RenderMethod renderMethod = definition as RenderMethod;
                        ProcessShader(stream, renderMethod, TagTypes[tag.Group.Tag.ToString()]);
                    }
                    else
                    {
                        ProcessFxShaderSystem(stream, definition, TagTypes[tag.Group.Tag.ToString()]);
                    }
                }

                // chud
                var chgd = Cache.Deserialize<ChudGlobalsDefinition>(stream, Cache.TagCache.FindFirstInGroup("chgd"));
                for (int i = 0; i < chgd.HudShaders.Count; i++)
                {
                    var chudShader = (HaloShaderGenerator.Globals.ChudShader)i;
                    if (chgd.HudShaders[i].PixelShader != null)
                        chgd.HudShaders[i].PixelShader.Name = $@"rasterizer\shaders\{chudShader}";
                    if (chgd.HudShaders[i].VertexShader != null)
                        chgd.HudShaders[i].VertexShader.Name = $@"rasterizer\shaders\{chudShader}";
                }

                // rasg
                var rasg = Cache.Deserialize<RasterizerGlobals>(stream, Cache.TagCache.FindFirstInGroup("rasg"));
                for (int i = 0; i < rasg.DefaultShaders.Count; i++)
                {
                    string name;
                    if (Cache.Version == CacheVersion.HaloOnline700123 && i == 110)
                        name = "unknown_6E";
                    else
                        name = ((HaloShaderGenerator.Globals.ExplicitShader)i).ToString();

                    if (rasg.DefaultShaders[i].PixelShader != null)
                        rasg.DefaultShaders[i].PixelShader.Name = $@"rasterizer\shaders\{name}";
                    if (rasg.DefaultShaders[i].VertexShader != null)
                        rasg.DefaultShaders[i].VertexShader.Name = $@"rasterizer\shaders\{name}";
                }
            }

            Cache.SaveTagNames();

            return true;
        }

        private string BuildRmt2Name(RenderMethod renderMethod, string type)
        {
            return $@"shaders\{type}_templates\_{string.Join("_", renderMethod.RenderMethodDefinitionOptionIndices.Select(i => i.OptionIndex))}";
        }

        private void NameRmdf(Stream stream, CachedTag rmdfTag, string type)
        {
            var rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);

            rmdfTag.Name = $@"shaders\{type}";
            rmdf.GlobalVertexShader.Name = $@"shaders\{type}_shared_vertex_shaders";
            rmdf.GlobalPixelShader.Name = $@"shaders\{type}_shared_pixel_shaders";

            NamedRmdfs.Add(rmdfTag.Index);
        }

        private void ProcessShader(Stream stream, RenderMethod renderMethod, string type)
        {
            if (!NamedRmdfs.Contains(renderMethod.BaseRenderMethod.Index))
                NameRmdf(stream, renderMethod.BaseRenderMethod, type);

            string rmt2Name = BuildRmt2Name(renderMethod, type);

            int rmt2Index = renderMethod.ShaderProperties[0].Template.Index;

            if (NamedRmt2s.ContainsKey(rmt2Index) && NamedRmt2s[rmt2Index] != rmt2Name)
                Console.WriteLine($"Name confliction at 0x{rmt2Index:X4}:\n{NamedRmt2s[rmt2Index]}\n{rmt2Name}");

            NamedRmt2s[rmt2Index] = rmt2Name;
            renderMethod.ShaderProperties[0].Template.Name = rmt2Name;

            var rmt2 = Cache.Deserialize<RenderMethodTemplate>(stream, renderMethod.ShaderProperties[0].Template);
            if (rmt2.PixelShader != null)
                rmt2.PixelShader.Name = rmt2Name;
            if (rmt2.VertexShader != null)
                rmt2.VertexShader.Name = rmt2Name;
        }

        private void ProcessFxShaderSystem(Stream stream, object definition, string type)
        {
            switch (definition)
            {
                case Particle prt3:
                    ProcessShader(stream, prt3.RenderMethod, type);
                    break;
                case LightVolumeSystem ltvl:
                    foreach (var sys in ltvl.LightVolumes)
                        ProcessShader(stream, sys.RenderMethod, type);
                    break;
                case BeamSystem beam:
                    foreach (var sys in beam.Beams)
                        ProcessShader(stream, sys.RenderMethod, type);
                    break;
                case DecalSystem decs:
                    foreach (var sys in decs.Decal)
                        ProcessShader(stream, sys.RenderMethod, type);
                    break;
                case ContrailSystem cntl:
                    foreach (var sys in cntl.Contrails)
                        ProcessShader(stream, sys.RenderMethod, type);
                    break;
            }
        }
    }
}
