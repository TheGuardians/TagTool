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

            Regenerate(args[0]);

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
            string _template_type,
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
            Shader rmsh_definition = null;
            {
                memory_stream.Position = 0;
                var rmsh_context = new TagSerializationContext(memory_stream, CacheContext, instance);

                // if there is no dependency on the rmsh, early exit
                if (!rmsh_context.Tag.Dependencies.Contains(shader_rmdf.Index))
                {
                    mutex.ReleaseMutex();
                    return;
                }
                rmsh_definition = CacheContext.Deserializer.Deserialize<Shader>(rmsh_context);
                // double check to make sure this is the correct render method definition
                if (rmsh_definition.BaseRenderMethod.Index != shader_rmdf.Index)
                {
                    mutex.ReleaseMutex();
                    return;
                }

            }

            // get render method template
            RenderMethodTemplate rmt2_definition = null;
            string rmt2_name = null;
            {
                memory_stream.Position = 0;
                var rmt2_context = new TagSerializationContext(memory_stream, CacheContext, rmsh_definition.ShaderProperties[0].Template);

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
            foreach (var template_arg in rmsh_definition.Unknown)
            {
                shader_template_args.Add((int)template_arg.Unknown);
            }

            // get the pixl shader that will be rebuilt
            memory_stream.Position = 0;
            var pixl_context = new TagSerializationContext(memory_stream, CacheContext, rmt2_definition.PixelShader);
            var pixel_shader = CacheContext.Deserializer.Deserialize<PixelShader>(pixl_context);

            //TODO: Regenerate all shaders based on RMT2 and set invalid shaders back to null
            // regenerate the pixel shader
            RegenerateShader(pixel_shader, shader_template_args.ToArray(), _template_type);

            serializationPairs.Add(new TagSerializationPair { tag = pixl_context.Tag, definition = pixel_shader });

            mutex.ReleaseMutex();
        }

        public void Regenerate(string _template_type)
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

            var shader_instances = CacheContext.TagCache.Index.Where(instance => instance != null && TagDefinition.Find(instance.Group.Tag) == typeof(Shader));
            var shader_rmdf = CacheContext.GetTag<RenderMethodDefinition>("shaders\\shader");

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
                        _template_type,
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

        void RegenerateShader(PixelShader shader, Int32[] shader_args, string shader_type)
        {
            //TODO: Lets just replace albedo for now, we need more advanced RMSH > RMT2 > PIXL code for this
            // but albedo is always index 0 in an RMSH generated template
            var shader_stage = ShaderStage.Albedo;
            foreach (var _shader_stage in Enum.GetValues(typeof(ShaderStage)).Cast<ShaderStage>())
            {

            }

            byte[] bytecode = null;

            switch (shader_type)
            {
                case "shader_templates":
                case "shader_template":

                    if (HaloShaderGenerator.HaloShaderGenerator.IsShaderSuppored(ShaderType.Shader, ShaderStage.Albedo))
                    {
                        var GenerateShader = typeof(HaloShaderGenerator.HaloShaderGenerator).GetMethod("GenerateShader");
                        var GenerateShaderArgs = CreateArguments(GenerateShader, shader_stage, shader_args);
                        bytecode = GenerateShader.Invoke(null, GenerateShaderArgs) as byte[];

                        //Console.WriteLine(bytecode?.Length ?? -1);
                    }
                    break;

                default:
                    break;
            }

            if (bytecode == null) return;

            var shader_data_block = new PixelShaderBlock
            {
                PCShaderBytecode = bytecode
            };

            var _definition = shader;
            var existing_block = _definition.Shaders[0];
            //shader_data_block.PCParameters = existing_block.PCParameters;
            //TODO: Set parameters
            //shader_data_block.PCParameters = shader_gen_result.Parameters;

            _definition.Shaders[0] = shader_data_block;
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