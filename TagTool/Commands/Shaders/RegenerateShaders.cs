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

        public void Regenerate(string _template_type)
        {
            foreach (var instance in CacheContext.TagCache.Index)
            {
                if (instance == null)
                    continue;

                var type = TagDefinition.Find(instance.Group.Tag);
                if (type != typeof(Shader)) continue;

                TagSerializationContext rmsh_context = null;
                Shader rmsh_definition = null;
                TagSerializationContext rmdf_context = null;
                RenderMethodDefinition render_definition = null;
                TagSerializationContext rmt2_context = null;
                RenderMethodTemplate rmt2_definition = null;
                TagSerializationContext pixl_context = null;
                PixelShader pixel_shader = null;

                // get shader
                var rmsh_stream = CacheContext.OpenTagCacheRead();
                rmsh_context = new TagSerializationContext(rmsh_stream, CacheContext, instance);
                rmsh_definition = CacheContext.Deserializer.Deserialize(rmsh_context, type) as Shader;


                // get render method definitions
                var rmdf_stream = CacheContext.OpenTagCacheRead();
                rmdf_context = new TagSerializationContext(rmdf_stream, CacheContext, rmsh_definition.BaseRenderMethod);
                render_definition = CacheContext.Deserializer.Deserialize(rmdf_context, type) as RenderMethodDefinition;
                rmdf_stream.Close();

                // make sure this is the correct render method definition
                {
                    var tag_index = CacheContext.TagCache.Index.ToList().IndexOf(rmsh_definition.BaseRenderMethod);
                    var name = CacheContext.TagNames.ContainsKey(tag_index) ? CacheContext.TagNames[tag_index] : null;
                    if (name != "shaders\\shader") continue;
                }

                // get the render method template
                var rmt2_stream = CacheContext.OpenTagCacheRead();
                rmt2_context = new TagSerializationContext(rmt2_stream, CacheContext, rmsh_definition.ShaderProperties[0].Template);
                rmt2_definition = CacheContext.Deserializer.Deserialize(rmt2_context, type) as RenderMethodTemplate;
                rmt2_stream.Close();

                // get the pixl shader that will be rebuilt
                var pixl_stream = CacheContext.OpenTagCacheReadWrite();
                pixl_context = new TagSerializationContext(pixl_stream, CacheContext, rmt2_definition.PixelShader);
                pixel_shader = CacheContext.Deserializer.Deserialize(pixl_context, type) as PixelShader;

                // extract the render method definition arguments to rebuild the shader again
                List<int> shader_template_args = new List<int>();
                foreach (var template_arg in rmsh_definition.Unknown)
                {
                    shader_template_args.Add((int)template_arg.Unknown);
                }

                // regenerate the shader
                RegenerateShader(pixel_shader, shader_template_args.ToArray(), _template_type);

                // save contents
                CacheContext.Serialize(pixl_context, pixel_shader);
                pixl_stream.Close();
            }

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

                        Console.WriteLine(bytecode?.Length ?? -1);
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