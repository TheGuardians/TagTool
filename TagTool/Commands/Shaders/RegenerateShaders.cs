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

            ListType(args[0]);

            return true;
        }

        public void ListType(string _template_type)
        {
            using (var stream = CacheContext.OpenTagCacheRead())
            {
                foreach (var instance in CacheContext.TagCache.Index)
                {
                    if (instance == null)
                        continue;

                    var type = TagDefinition.Find(instance.Group.Tag);
                    if (type != typeof(PixelShader)) continue;

                    var tag_index = CacheContext.TagCache.Index.ToList().IndexOf(instance);
                    var name = CacheContext.TagNames.ContainsKey(tag_index) ? CacheContext.TagNames[tag_index] : null;
                    if (name == null) continue;
                    if (!name.Contains("\\")) continue; // Probbaly an unnamed tag
                    var template_type = name.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    if (_template_type != "*" && !(template_type == _template_type || template_type == _template_type + "s")) continue;

                    var context = new TagSerializationContext(stream, CacheContext, instance);
                    var definition = CacheContext.Deserializer.Deserialize(context, type);

                    if (instance == null) continue;

                    switch (definition)
                    {
                        case PixelShader pixel_shader:

                            RegenerateShader(pixel_shader, name, _template_type);

                            break;
                    }
                }
            }

        }

        void RegenerateShader(PixelShader shader, string name, string shader_type)
        {
            var shader_stage = ShaderStage.Default;
            foreach (var _shader_stage in Enum.GetValues(typeof(ShaderStage)).Cast<ShaderStage>())
            {

            }

            Int32[] shader_args;
            try
            {
                var right = name.Split('\\').Last();
                var args = right.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                shader_args = Array.ConvertAll(args, Int32.Parse);
            }
            catch
            {
                Console.WriteLine("Invalid shader arguments! (could not parse to Int32[].)"); return;
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