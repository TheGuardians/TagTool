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

            List<TagSerializationPair> serializationPairs = new List<TagSerializationPair>();
            HashSet<int> serilaizedRmt2 = new HashSet<int>();

            var shader_instances = CacheContext.TagCache.Index.Where(instance => instance != null && TagDefinition.Find(instance.Group.Tag) == typeof(Shader));
            var shader_rmdf = CacheContext.GetTag<RenderMethodDefinition>("shaders\\shader");

            foreach (var instance in shader_instances)
            {
                // get shader
                Shader rmsh_definition = null;
                {
                    memory_stream.Position = 0;
                    var rmsh_context = new TagSerializationContext(memory_stream, CacheContext, instance);

                    // if there is no dependency on the rmsh, early exit
                    if (!rmsh_context.Tag.Dependencies.Contains(shader_rmdf.Index))
                    {
                        continue;
                    }
                    rmsh_definition = CacheContext.Deserializer.Deserialize<Shader>(rmsh_context);
                    // double check to make sure this is the correct render method definition
                    if (rmsh_definition.BaseRenderMethod.Index != shader_rmdf.Index)
                    {
                        continue;
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
                        continue;
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
            }
            int before = serializationPairs.Count;
            serializationPairs = serializationPairs.Where(sp => sp != null).GroupBy(sp => sp.tag.Index).Select(g => g.First()).ToList();

            Console.WriteLine($"Generation completed in {(DateTime.Now - then).TotalSeconds} seconds {Math.Round(100.0f * (float)serializationPairs.Count / (float)before, 2)}% efficiency");
            Debug.WriteLine($"Generation completed in {(DateTime.Now - then).TotalSeconds} seconds {Math.Round(100.0f * (float)serializationPairs.Count / (float)before, 2)}% efficiency");

            // serialize modified tags
            foreach (var sp in serializationPairs)
            {
                memory_stream.Position = 0;
                var tag = CacheContext.GetTag(sp.tag.Index);
                var new_context = new TagSerializationContext(memory_stream, CacheContext, tag);
                CacheContext.Serialize(new_context, sp.definition);
            }

            base_stream.Position = 0;
            memory_stream.Position = 0;

            base_stream.SetLength(0);
            memory_stream.CopyTo(base_stream);

            base_stream.Close();
            memory_stream.Close();

            Console.WriteLine($"Finished in {(DateTime.Now - then).TotalSeconds} seconds");
            Debug.WriteLine($"Finished in {(DateTime.Now - then).TotalSeconds} seconds");
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