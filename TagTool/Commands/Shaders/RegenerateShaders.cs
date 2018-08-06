using TagTool.Cache;
using TagTool.Commands;
using TagTool.Geometry;
using TagTool.Serialization;
using TagTool.Shaders;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tags;
using System.Linq;
using System.Text;
using HaloShaderGenerator.Enums;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace TagTool.Commands.Shaders
{
    public class RegenerateShaders : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public RegenerateShaders(GameCacheContext cacheContext) :
            base(true,

                "RegenerateShaders",
                "Regenerates all of the shaders of a template type",

                "RegenerateShaders <template_type>",
                "Regenerates all of the shaders of a template type")
        {
            CacheContext = cacheContext as HaloOnlineCacheContext;
        }

        struct ParameterGroup
        {
            string Name;
            ShaderParameter Parameter;

            public ParameterGroup(string name)
            {
                Name = name;
                Parameter = new ShaderParameter();
            }
        }

        public override object Execute(List<string> args)
        {
            if (args.Count <= 0) return false;

            Regenerate(args[0]?.ToLower());

            return true;
        }

        class TagSerializationPair
        {
            public CachedTagInstance tag;
            public object definition;
        }

        private void ProcessInstance(
            int index,
            IEnumerable<CachedTagInstance> shader_instances,
            CachedTagInstance shader_rmdf,
            ConcurrentBag<int> serilaizedRmt2,
            ConcurrentBag<TagSerializationPair> serializationPairs,
            MemoryStream[] read_memory_streams,
            Mutex[] mutices
            )
        {
            var stream_index = Mutex.WaitAny(mutices);
            var mutex = mutices[stream_index];
            var memory_stream = read_memory_streams[stream_index];

            var instance = shader_instances.ElementAt(index);

            // get shader
            RenderMethod rm_shader_definition = null;
            Type rm_shader_type = null;
            string rmdf_name = null;
            {
                memory_stream.Position = 0;
                var rmsh_context = new TagSerializationContext(memory_stream, CacheContext, instance);

                // if there is no dependency on the rmsh, early exit
                if (shader_rmdf != null && !rmsh_context.Tag.Dependencies.Contains(shader_rmdf.Index))
                {
                    mutex.ReleaseMutex();
                    return;
                }
                rm_shader_type = TagDefinition.Find(rmsh_context.Tag.Group.Tag);
                rm_shader_definition = CacheContext.Deserializer.Deserialize(rmsh_context, rm_shader_type) as RenderMethod;
                // double check to make sure this is the correct render method definition
                if (shader_rmdf != null && rm_shader_definition.BaseRenderMethod.Index != shader_rmdf.Index)
                {
                    mutex.ReleaseMutex();
                    return;
                }

                var rmdf_index = rm_shader_definition.BaseRenderMethod.Index;
                rmdf_name = CacheContext.TagNames.ContainsKey(rmdf_index) ? CacheContext.TagNames[rmdf_index] : rmdf_index.ToString("X");
            }

            // get render method template
            RenderMethodTemplate rmt2_definition = null;
            TagSerializationContext rmt2_context = null;
            string rmt2_name = null;
            {
                memory_stream.Position = 0;
                rmt2_context = new TagSerializationContext(memory_stream, CacheContext, rm_shader_definition.ShaderProperties[0].Template);

                // Skip previously finished RMT2 tags
                if (serilaizedRmt2.Contains(rmt2_context.Tag.Index))
                {
                    mutex.ReleaseMutex();
                    return;
                }

                serilaizedRmt2.Add(rmt2_context.Tag.Index);

                rmt2_definition = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(rmt2_context);

                // console output
                rmt2_name = CacheContext.TagNames.ContainsKey(rmt2_context.Tag.Index) ? CacheContext.TagNames[rmt2_context.Tag.Index] : rmt2_context.Tag.Index.ToString("X");
                Console.WriteLine($"Regenerating {rmt2_name}");
            }

            // extract the render method definition arguments to rebuild the shader again
            List<int> shader_template_args = new List<int>();
            foreach (var template_arg in rm_shader_definition.Unknown)
            {
                shader_template_args.Add((int)template_arg.Unknown);
            }

            // get the pixl shader that will be rebuilt
            memory_stream.Position = 0;
            var pixl_context = new TagSerializationContext(memory_stream, CacheContext, rmt2_definition.PixelShader);
            var pixel_shader = CacheContext.Deserializer.Deserialize<PixelShader>(pixl_context);

            //TODO: This is slow, there should be a faster way to do this, maybe a Dictionary with tag index???
            //NOTE: This might seem redundant, and it kind of is. But this ensures that we can just pass any
            // shader in here, but also support a custom RMDF in the future, ie. Halo 3 + Halo 3 ODST + Halo Reach
            // in the same rendering engine suite
            // Also "Shaders" includes shader, beam, contrail etc. this is important too

            string template_type = null;
            switch(rmdf_name)
            {
                case "shaders\\shader":
                    template_type = "shader_template";

                    //TODO: Regenerate all shaders based on RMT2 and set invalid shaders back to null
                    RegenerateShader(rmt2_definition, pixel_shader, shader_template_args.ToArray(), template_type);

                    break;
                default:
                    Console.WriteLine($"Error: Unknown rmdf {rmdf_name}");
                    Console.WriteLine($"Skipping {rmt2_name}");
                    mutex.ReleaseMutex();
                    return;
            }

            serializationPairs.Add(new TagSerializationPair { tag = pixl_context.Tag, definition = pixel_shader });
            serializationPairs.Add(new TagSerializationPair { tag = rmt2_context.Tag, definition = rmt2_definition });

            mutex.ReleaseMutex();
        }

        private static readonly Type[] SupportedShaderTypes = {
            typeof(ShaderBlack),
            typeof(ShaderDecal),
            typeof(ShaderFoliage),
            typeof(ShaderHalogram),
            typeof(ShaderZonly),
            typeof(ShaderCustom),
            typeof(ShaderWater),
            typeof(ShaderTerrain),
            typeof(ShaderCortana),
            typeof(ShaderScreen),
            typeof(Shader)
        };

        public void Regenerate(string template_name)
        {
            var then = DateTime.Now;

            var base_stream = CacheContext.OpenTagCacheReadWrite();
            var memory_stream = new MemoryStream();
            {
                base_stream.Position = 0;
                base_stream.CopyTo(memory_stream);
                base_stream.Position = 0;
            }

            ConcurrentBag<TagSerializationPair> serialization_pairs_bag = new ConcurrentBag<TagSerializationPair>();
            ConcurrentBag<int> serilaizedRmt2 = new ConcurrentBag<int>();


            Type template_type = null;
            CachedTagInstance shader_rmdf = null;
            switch (template_name)
            {
                case "shader_templates":
                case "shader_template":
                    template_type = typeof(Shader);
                    shader_rmdf = CacheContext.GetTag<RenderMethodDefinition>("shaders\\shader");
                    break;

                case "*":
                    break;
                case null:
                default:
                    Console.WriteLine("Invalid template_type");
                    break;
            }

            IEnumerable<CachedTagInstance> shader_instances = null;
            if (template_type != null)
            {
                shader_instances = CacheContext.TagCache.Index.Where(instance => instance != null && TagDefinition.Find(instance.Group.Tag) == template_type);
            }
            else
            {
                shader_instances = CacheContext.TagCache.Index.Where(instance => instance != null && SupportedShaderTypes.Contains(TagDefinition.Find(instance.Group.Tag)));
            }

            var num_shader_instances = shader_instances.Count();
            Task[] tasks = new Task[num_shader_instances];

            // create a memory stream with the given number of threads
            var threads_count = Environment.ProcessorCount;
            var read_memory_streams = new MemoryStream[threads_count];
            var read_memory_mutex = new Mutex[threads_count];
            for (var i = 0; i < threads_count; i++)
            {
                read_memory_streams[i] = new MemoryStream();
                {
                    memory_stream.Position = 0;
                    memory_stream.CopyTo(read_memory_streams[i]);
                    memory_stream.Position = 0;
                }

                read_memory_mutex[i] = new Mutex();
            }



            for (var i = 0; i < num_shader_instances; i++)
            {
                var task = new TaskFactory().StartNew(delegate (object index) {

                    ProcessInstance(
                        (int)index,
                        shader_instances,
                        shader_rmdf,
                        serilaizedRmt2,
                        serialization_pairs_bag,
                        read_memory_streams,
                        read_memory_mutex
                        );

                }, i);
                tasks[i] = task;
                //task.Wait();
            }
            Task.WaitAll(tasks);

            int before = serialization_pairs_bag.Count;
            var serialization_pairs = serialization_pairs_bag.Where(sp => sp != null).GroupBy(sp => sp.tag.Index).Select(g => g.First());

            Console.WriteLine($"Generation completed in {(DateTime.Now - then).TotalSeconds} seconds {Math.Round(100.0f * (float)serialization_pairs_bag.Count / (float)before, 2)}% efficiency");
#if DEBUG
            Debug.WriteLine($"Generation completed in {(DateTime.Now - then).TotalSeconds} seconds {Math.Round(100.0f * (float)serialization_pairs_bag.Count / (float)before, 2)}% efficiency");
#endif

            // serialize modified tags
            foreach (var sp in serialization_pairs_bag)
            {
                memory_stream.Position = 0;
                //var tag = CacheContext.GetTag(sp.tag.Index);
                var new_context = new TagSerializationContext(memory_stream, CacheContext, sp.tag);
                CacheContext.Serialize(new_context, sp.definition);
            }

            base_stream.Position = 0;
            memory_stream.Position = 0;

            base_stream.SetLength(0);
            memory_stream.CopyTo(base_stream);

            base_stream.Close();
            memory_stream.Close();

            Console.WriteLine($"Finished in {(DateTime.Now - then).TotalSeconds} seconds");
#if DEBUG
            Debug.WriteLine($"Finished in {(DateTime.Now - then).TotalSeconds} seconds");
#endif
        }

        void RegenerateShader(RenderMethodTemplate rmt2, PixelShader pixl, Int32[] shader_args, string shader_type)
        {
            switch (shader_type)
            {
                case "shader_templates":
                case "shader_template":

                    var GenerateShader = typeof(HaloShaderGenerator.HaloShaderGenerator).GetMethod("GenerateShader");

                    //TODO: Lets just replace albedo for now, we need more advanced RMSH > RMT2 > PIXL code for this
                    // but albedo is always index 0 in an RMSH generated template
                    var drawmodebitmasl = rmt2.DrawModeBitmask;
                    RenderMethodTemplate.ShaderModeBitmask newbitmask = 0;
                    if ((drawmodebitmasl | RenderMethodTemplate.ShaderModeBitmask.Albedo) != 0)
                    {
                        if (HaloShaderGenerator.HaloShaderGenerator.IsShaderSuppored(ShaderType.Shader, ShaderStage.Albedo))
                        {
                            var GenerateShaderArgs = CreateArguments(GenerateShader, ShaderStage.Albedo, shader_args);

                            byte[] bytecode = GenerateShader.Invoke(null, GenerateShaderArgs) as byte[];

                            if (bytecode == null) return;

                            var offset = pixl.DrawModes[(int)RenderMethodTemplate.ShaderMode.Albedo].Offset;
                            var count = pixl.DrawModes[(int)RenderMethodTemplate.ShaderMode.Albedo].Count;

                            pixl.Shaders[offset].PCShaderBytecode = bytecode;

                            newbitmask |= RenderMethodTemplate.ShaderModeBitmask.Albedo;
                        }
                        // todo, disable it. but for now, we'll just keep the other shaders here
                    }

                    if ((drawmodebitmasl | RenderMethodTemplate.ShaderModeBitmask.Active_Camo) != 0)
                    {
                        if (HaloShaderGenerator.HaloShaderGenerator.IsShaderSuppored(ShaderType.Shader, ShaderStage.Active_Camo))
                        {
                            var GenerateShaderArgs = CreateArguments(GenerateShader, ShaderStage.Active_Camo, shader_args);

                            byte[] bytecode = GenerateShader.Invoke(null, GenerateShaderArgs) as byte[];

                            if (bytecode == null) return;

                            var offset = pixl.DrawModes[(int)RenderMethodTemplate.ShaderMode.Active_Camo].Offset;
                            var count = pixl.DrawModes[(int)RenderMethodTemplate.ShaderMode.Active_Camo].Count;

                            pixl.Shaders[offset].PCShaderBytecode = bytecode;

                            newbitmask |= RenderMethodTemplate.ShaderModeBitmask.Active_Camo;
                        }
                        // todo, disable it. but for now, we'll just keep the other shaders here
                    }

                    //rmt2.DrawModeBitmask = newbitmask;

                    break;

                default:
                    break;
            }
        }

        public object[] CreateArguments(MethodInfo method, ShaderStage stage, Int32[] template)
        {
            var _params = method.GetParameters();
            object[] input_params = new object[_params.Length];

            for (int i = 0; i < _params.Length; i++)
            {
                if (i == 0) input_params[0] = stage;
                else
                {
                    var template_index = i - 1;
                    if (template_index < template.Length)
                    {
                        var _enum = Enum.ToObject(_params[i].ParameterType, template[template_index]);
                        input_params[i] = _enum;
                    }
                    else
                    {
                        var _enum = Enum.ToObject(_params[i].ParameterType, 0);
                        input_params[i] = _enum;
                    }

                }
            }

            return input_params;
        }
    }
}