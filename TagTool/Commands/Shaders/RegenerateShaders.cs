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
                "",

                "RegenerateShaders",
                "")
        {
            CacheContext = cacheContext as HaloOnlineCacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count <= 0) return false;

            Regenerate(args[0]?.ToLower());

            return true;
        }

        class ProcessInstanceResult
        {
            public CachedTagInstance tag;
            public object definition;
        }

        private bool GetShader(
            Mutex mutex,
            Stream stream,
            CachedTagInstance instance,
            IEnumerable<int> shader_rmdfs_ids,
            out RenderMethod rm_shader_definition,
            out Type rm_shader_type,
            out TagSerializationContext rm_shader_context,
            out string rmdf_name
            )
        {
            // get shader
            rm_shader_definition = null;
            rm_shader_type = null;
            rm_shader_context = null;
            rmdf_name = null;
            {
                stream.Position = 0;
                rm_shader_context = new TagSerializationContext(stream, CacheContext, instance);

                // if there is no dependency on the rmsh, early exit

                if (shader_rmdfs_ids != null && !rm_shader_context.Tag.Dependencies.Intersect(shader_rmdfs_ids).Any())
                {
                    mutex.ReleaseMutex();
                    return true;
                }
                rm_shader_type = TagDefinition.Find(rm_shader_context.Tag.Group.Tag);
                rm_shader_definition = CacheContext.Deserializer.Deserialize(rm_shader_context, rm_shader_type) as RenderMethod;
                // double check to make sure this is the correct render method definition
                if (shader_rmdfs_ids != null && !shader_rmdfs_ids.Contains(rm_shader_definition.BaseRenderMethod.Index))
                {
                    mutex.ReleaseMutex();
                    return true;
                }

                var rmdf_index = rm_shader_definition.BaseRenderMethod.Index;
                rmdf_name = CacheContext.TagNames.ContainsKey(rmdf_index) ? CacheContext.TagNames[rmdf_index] : rmdf_index.ToString("X");
            }

            return false;
        }

        private bool GetRenderMethodTemplate(
            Mutex mutex,
            Stream stream,
            CachedTagInstance instance,
            ConcurrentBag<int> serilaizedRmt2,
            RenderMethod rm_shader_definition,
            out RenderMethodTemplate rmt2_definition,
            out TagSerializationContext rmt2_context,
            out string rmt2_name
            )
        {
            // get render method template
            rmt2_definition = null;
            rmt2_context = null;
            rmt2_name = null;
            {
                stream.Position = 0;
                rmt2_context = new TagSerializationContext(stream, CacheContext, rm_shader_definition.ShaderProperties[0].Template);

                // Skip previously finished RMT2 tags
                if (serilaizedRmt2.Contains(rmt2_context.Tag.Index))
                {
                    mutex.ReleaseMutex();
                    return true;
                }

                serilaizedRmt2.Add(rmt2_context.Tag.Index);

                rmt2_definition = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(rmt2_context);

                // console output
                rmt2_name = CacheContext.TagNames.ContainsKey(rmt2_context.Tag.Index) ? CacheContext.TagNames[rmt2_context.Tag.Index] : rmt2_context.Tag.Index.ToString("X");
                Console.WriteLine($"Regenerating {rmt2_name}");
            }

            return false;
        }

        private void ProcessInstance(
            int index,
            CachedTagInstance[] shader_instances,
            IEnumerable<int> shader_rmdfs_ids,
            ConcurrentBag<int> serilaizedRmt2,
            ConcurrentBag<ProcessInstanceResult> serializationPairs,
            MemoryStream[] read_memory_streams,
            Mutex[] mutices
            )
        {
            var stream_index = Mutex.WaitAny(mutices);
            var mutex = mutices[stream_index];
            var memory_stream = read_memory_streams[stream_index];
            var instance = shader_instances.ElementAt(index);

            bool get_shader_error = GetShader(
                mutex,
                memory_stream,
                instance,
                shader_rmdfs_ids,
                out RenderMethod rm_shader_definition,
                out Type rm_shader_type,
                out TagSerializationContext rm_shader_context,
                out string rmdf_name
                );
            if (get_shader_error) return;

            bool get_render_method_template_error = GetRenderMethodTemplate(
                mutex,
                memory_stream,
                instance,
                serilaizedRmt2,
                rm_shader_definition,
                out RenderMethodTemplate rmt2_definition,
                out TagSerializationContext rmt2_context,
                out string rmt2_name
                );
            if (get_render_method_template_error) return;

            // extract the render method definition arguments to rebuild the shader again
            List<int> shader_template_args = new List<int>();
            foreach (var template_arg in rm_shader_definition.RenderMethodDefinitionOptionIndices)
            {
                shader_template_args.Add((int)template_arg.OptionIndex);
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

            //TODO Clean this shit up
            string template_type = null;
            switch (rmdf_name)
            {
                case @"shaders\shader":
                    template_type = "shader_template";
                    //TODO: Regenerate all shaders based on RMT2 and set invalid shaders back to null
                    _RegenerateShaders(rmt2_definition, pixel_shader, shader_template_args.ToArray(), template_type);
                    break;
                case @"shaders\cortana":
                    template_type = "cortana_template";
                    _RegenerateShaders(rmt2_definition, pixel_shader, shader_template_args.ToArray(), template_type);
                    break;
                default:
                    Console.WriteLine($"Error: Unknown rmdf {rmdf_name}");
                    Console.WriteLine($"Skipping {rmt2_name}");
                    mutex.ReleaseMutex();
                    return;
            }

            serializationPairs.Add(new ProcessInstanceResult { tag = pixl_context.Tag, definition = pixel_shader });
            serializationPairs.Add(new ProcessInstanceResult { tag = rmt2_context.Tag, definition = rmt2_definition });

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

        public void GetRMDF_IDs(string template_name, out IEnumerable<int> shader_rmdfs_ids, out Type template_type)
        {
            template_type = null;
            shader_rmdfs_ids = null;

            IEnumerable<CachedTagInstance> shader_rmdfs = null;
            switch (template_name)
            {
                case "shader_templates":
                case "shader_template":
                    template_type = typeof(Shader);
                    shader_rmdfs = CacheContext.TagNames.Where(item => item.Value == @"shaders\shader").Select(item => CacheContext.GetTag(item.Key)).ToList();
                    break;
                case "cortana_templates":
                case "cortana_template":
                    template_type = typeof(ShaderCortana);
                    shader_rmdfs = CacheContext.TagNames.Where(item => item.Value == @"shaders\cortana").Select(item => CacheContext.GetTag(item.Key)).ToList();
                    break;
                case null:
                case "*":
                    break;
                default:
                    Console.WriteLine("Invalid template_type");
                    break;
            }
            if (shader_rmdfs != null)
            {
                shader_rmdfs = shader_rmdfs.Where(rmdf => TagDefinition.Find(rmdf.Group.Tag) == typeof(RenderMethodDefinition));
                shader_rmdfs_ids = shader_rmdfs.Select(rmdf => rmdf.Index);
                shader_rmdfs_ids = new int[] { shader_rmdfs_ids.FirstOrDefault() };
            }
        }

        public CachedTagInstance[] GetShaderInstances(Type template_type)
        {
            CachedTagInstance[] shader_instances = null;
            if (template_type != null)
            {
                shader_instances = CacheContext.TagCache.Index.Where(instance => instance != null && TagDefinition.Find(instance.Group.Tag) == template_type).ToArray();
            }
            else
            {
                shader_instances = CacheContext.TagCache.Index.Where(instance => instance != null && SupportedShaderTypes.Contains(TagDefinition.Find(instance.Group.Tag))).ToArray();
            }
            return shader_instances;
        }

        public void Regenerate(string template_name)
        {
            var then = DateTime.Now;
            GetRMDF_IDs(template_name, out IEnumerable<int> shader_rmdfs_ids, out Type template_type);
            var shader_instances = GetShaderInstances(template_type);

            var base_stream = CacheContext.OpenTagCacheReadWrite();
            var memory_stream = new MemoryStream();
            {
                base_stream.Position = 0;
                base_stream.CopyTo(memory_stream);
                base_stream.Position = 0;
            }

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

            ConcurrentBag<ProcessInstanceResult> serialization_pairs_bag = new ConcurrentBag<ProcessInstanceResult>();
            ConcurrentBag<int> serilaizedRmt2 = new ConcurrentBag<int>();
            var num_shader_instances = shader_instances.Count();
            Task[] tasks = new Task[num_shader_instances];
            for (var i = 0; i < num_shader_instances; i++)
            {
                var task = new TaskFactory().StartNew(delegate (object index) {

                    ProcessInstance(
                        (int)index,
                        shader_instances,
                        shader_rmdfs_ids,
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

        bool RegenerateShader(
            RenderMethodTemplate rmt2,
            PixelShader pixl,
            Int32[] shader_args,
            ShaderType type,
            ShaderStage shaderstage,
            RenderMethodTemplate.ShaderModeBitmask bit,
            RenderMethodTemplate.ShaderMode mode
            )
        {
            MethodInfo method = null;
            switch(type)
            {
                case ShaderType.Shader:
                    method = typeof(HaloShaderGenerator.HaloShaderGenerator).GetMethod("GenerateShader");
                    break;
                case ShaderType.Cortana:
                    method = typeof(HaloShaderGenerator.HaloShaderGenerator).GetMethod("GenerateShaderCortana");
                    break;
                default:
                    return false;
            }
            if (method == null) return false;

            //TODO: Rewrite this crazyness
            if ((rmt2.DrawModeBitmask | bit) != 0)
            {
                if (HaloShaderGenerator.HaloShaderGenerator.IsShaderSuppored(type, shaderstage))
                {
                    var GenerateShaderArgs = CreateArguments(method, shaderstage, shader_args);

                    //TODO: Remove this
                    switch (shaderstage)
                    {
                        case ShaderStage.Albedo:
                            Albedo albedo = GenerateShaderArgs.Where(x => x.GetType() == typeof(Albedo)).Cast<Albedo>().FirstOrDefault();
                            if (albedo > Albedo.Two_Detail_Black_Point) return false; // saber territory
                            Bump_Mapping bumpmapping = GenerateShaderArgs.Where(x => x.GetType() == typeof(Bump_Mapping)).Cast<Bump_Mapping>().FirstOrDefault();
                            if (bumpmapping > Bump_Mapping.Detail) return false; // saber territory
                            break;
                    }

                    var shaderGeneratorResult = method.Invoke(null, GenerateShaderArgs) as HaloShaderGenerator.ShaderGeneratorResult;

                    if (shaderGeneratorResult?.Bytecode == null) return false;

                    var offset = pixl.DrawModes[(int)mode].Offset;
                    var count = pixl.DrawModes[(int)mode].Count;

                    var pixelShaderBlock = Porting.PortTagCommand.GeneratePixelShaderBlock(CacheContext, shaderGeneratorResult);
                    pixl.Shaders[offset] = pixelShaderBlock;

                    rmt2.DrawModeBitmask |= bit;

                    return true;
                }
                // todo, disable it. but for now, we'll just keep the other shaders here
            }
            return false;
        }

        void _RegenerateShaders(RenderMethodTemplate rmt2, PixelShader pixl, Int32[] shader_args, string shader_type)
        {
            switch (shader_type)
            {
                case "cortana_templates":
                case "cortana_template":
                    {
                        RegenerateShader(
                            rmt2,
                            pixl,
                            shader_args,
                            ShaderType.Cortana,
                            ShaderStage.Active_Camo,
                            RenderMethodTemplate.ShaderModeBitmask.Active_Camo,
                            RenderMethodTemplate.ShaderMode.Active_Camo
                        );
                    }
                    break;
                    

                case "shader_templates":
                case "shader_template":
                    {
                        //RegenerateShader(
                        //    rmt2,
                        //    pixl,
                        //    shader_args,
                        //    ShaderType.Shader,
                        //    ShaderStage.Albedo,
                        //    RenderMethodTemplate.ShaderModeBitmask.Albedo,
                        //    RenderMethodTemplate.ShaderMode.Albedo
                        //);

                        //RegenerateShader(
                        //    rmt2,
                        //    pixl,
                        //    shader_args,
                        //    ShaderType.Shader,
                        //    ShaderStage.Active_Camo,
                        //    RenderMethodTemplate.ShaderModeBitmask.Active_Camo,
                        //    RenderMethodTemplate.ShaderMode.Active_Camo
                        //);

                        RegenerateShader(
                            rmt2,
                            pixl,
                            shader_args,
                            ShaderType.Shader,
                            ShaderStage.Static_Prt_Ambient,
                            RenderMethodTemplate.ShaderModeBitmask.Static_Prt_Ambient,
                            RenderMethodTemplate.ShaderMode.Static_Prt_Ambient
                        );

                        //RegenerateShader(
                        //    rmt2,
                        //    pixl,
                        //    shader_args,
                        //    ShaderType.Shader,
                        //    ShaderStage.Static_Prt_Linear,
                        //    RenderMethodTemplate.ShaderModeBitmask.Static_Prt_Linear,
                        //    RenderMethodTemplate.ShaderMode.Static_Prt_Linear
                        //);
                        //RegenerateShader(
                        //    rmt2,
                        //    pixl,
                        //    shader_args,
                        //    ShaderType.Shader,
                        //    ShaderStage.Static_Prt_Quadratic,
                        //    RenderMethodTemplate.ShaderModeBitmask.Static_Prt_Quadratic,
                        //    RenderMethodTemplate.ShaderMode.Static_Prt_Quadratic
                        //);
                    }
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