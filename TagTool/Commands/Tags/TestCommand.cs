using HaloShaderGenerator.Globals;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands
{

    public class TestCommand : Command
    {
        GameCache Cache;

        public TestCommand(GameCache cache) : base(false, "Test", "Test", "Test", "Test")
        {
            Cache = cache;
        }

        public void RegenShaderGLVS()
        {
            using (var stream = Cache.OpenCacheReadWrite())
            {
                var generator = new HaloShaderGenerator.Shader.ShaderGenerator();
                // recompile glvs

                var tag = Cache.TagCache.GetTag(@"shaders\shader_shared_vertex_shaders", "glvs");

                var glvs = Cache.Deserialize<GlobalVertexShader>(stream, tag);
                // world rigid skinned
                for (int i = 0; i < 3; i++)
                {
                    var vertexBlock = glvs.VertexTypes[i];
                    for (int j = 0; j < vertexBlock.DrawModes.Count; j++)
                    {
                        var entryPoint = vertexBlock.DrawModes[j];
                        var entryPointEnum = (ShaderStage)j;
                        if (entryPoint.ShaderIndex != -1)
                        {
                            if (generator.IsEntryPointSupported(entryPointEnum) && generator.IsVertexShaderShared(entryPointEnum))
                            {
                                var result = generator.GenerateSharedVertexShader((HaloShaderGenerator.Globals.VertexType)i, entryPointEnum);
                                glvs.Shaders[entryPoint.ShaderIndex] = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateVertexShaderBlock(Cache, result);
                            }
                        }
                    }
                }

                Cache.Serialize(stream, tag, glvs);

                // recompile glps

                tag = Cache.TagCache.GetTag(@"shaders\shader_shared_pixel_shaders", "glps");
                var glps = Cache.Deserialize<GlobalPixelShader>(stream, tag);

                for (int i = 0; i < 2; i++)
                {
                    var result = generator.GenerateSharedPixelShader(ShaderStage.Shadow_Generate, 2, i);
                    glps.Shaders[i] = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GeneratePixelShaderBlock(Cache, result);
                }
                Cache.Serialize(stream, tag, glps);

            }
        }

        public byte[] GetCacheRawData(uint resourceAddress, int size)
        {
            var cacheFileType = (resourceAddress & 0xC0000000) >> 30;
            int fileOffset = (int)(resourceAddress & 0x3FFFFFFF);

            GameCacheGen2 sourceCache;

            if (cacheFileType != 0)
            {
                string filename = "";
                switch (cacheFileType)
                {
                    case 1:
                        filename = Path.Combine(Cache.Directory.FullName, "mainmenu.map");
                        break;
                    case 2:
                        filename = Path.Combine(Cache.Directory.FullName, "shared.map");
                        break;
                    case 3:
                        filename = Path.Combine(Cache.Directory.FullName, "single_player_shared.map");
                        break;

                }
                // TODO: make this a function call with a stored reference to caches in the base cache or something better than this
                sourceCache = (GameCacheGen2)GameCache.Open(new FileInfo(filename));
            }
            else
                sourceCache = (GameCacheGen2)Cache;

            var stream = sourceCache.OpenCacheRead();

            var reader = new EndianReader(stream, Cache.Endianness);

            reader.SeekTo(fileOffset);
            var data = reader.ReadBytes(size);

            reader.Close();

            return data;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return false;


            using (var stream = Cache.OpenCacheRead())
            {
                var ugh = Cache.Deserialize<SoundCacheFileGestalt>(stream, Cache.TagCache.GetTag("i've got a lovely bunch of coconuts.ugh!"));


                foreach (var tag in Cache.TagCache.NonNull())
                {
                    if (tag.IsInGroup("snd!"))
                    {
                        var soundTag = Cache.Deserialize<Sound>(stream, tag);
                    }
                }
            }

            return true;
        }
    }
}

