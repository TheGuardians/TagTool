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

namespace TagTool.Commands.Shaders
{
    class ListRegisters : Command
    {
        private GameCacheContext CacheContext { get; }

        public ListRegisters(GameCacheContext cacheContext) :
            base(CommandFlags.Inherit,

                "ListRegisters",
                "List Registers",

                "ListRegisters <template_type>",
                "List Registers")
        {
            CacheContext = cacheContext;
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

            // Probbaly a better way to do this, but nobody has made this easy...

            List<ShaderParameter> parameters = new List<ShaderParameter>();

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
                    if (_template_type != "*" && template_type != _template_type) continue;

                    var context = new TagSerializationContext(stream, CacheContext, instance);
                    var definition = CacheContext.Deserializer.Deserialize(context, type);

                    if (instance == null) continue;

                    switch (definition)
                    {
                        case PixelShader pixel_shader:

                            if (pixel_shader?.Shaders == null || pixel_shader.Shaders.Count == 0) continue;
                            if (pixel_shader.Shaders[0].PCParameters.Count == 0) continue;
                            parameters.AddRange(pixel_shader.Shaders[0].PCParameters);

                            break;
                    }

                    //CacheContext.Serializer.Serialize(context, definition);
                }
            }

            var unique = parameters.GroupBy(param => {
                var ParameterName = CacheContext.GetString(param.ParameterName);
                var RegisterIndex = param.RegisterIndex;
                var RegisterCount = param.RegisterCount;
                var RegisterType = param.RegisterType;

                //return $"{ParameterName}[{RegisterCount}]_{(int)RegisterType}{RegisterIndex}";
                return $"{ParameterName}[{RegisterCount}]_{(int)RegisterType}";
            }).Select(g => g.First()).ToList();
            unique.Sort((a, b) => a.RegisterIndex - b.RegisterIndex);
            unique.Sort((a, b) => CacheContext.GetString(a.ParameterName)[0] - CacheContext.GetString(b.ParameterName)[0]);

            var samplers = unique.Where(param => param.RegisterType == ShaderParameter.RType.Sampler);
            var booleans = unique.Where(param => param.RegisterType == ShaderParameter.RType.Boolean);
            var integers = unique.Where(param => param.RegisterType == ShaderParameter.RType.Integer);
            var vectors = unique.Where(param => param.RegisterType == ShaderParameter.RType.Vector);

            ListParams(samplers);
            ListParams(booleans);
            ListParams(integers);
            ListParams(vectors);
        }

        void ListParams(IEnumerable<ShaderParameter> shader_params)
        {
            foreach (var param in shader_params)
            {
                var ParameterName = CacheContext.GetString(param.ParameterName);
                var RegisterIndex = param.RegisterIndex;
                var RegisterCount = param.RegisterCount;

                if (RegisterCount == 1)
                {
                    switch (param.RegisterType)
                    {
                        case ShaderParameter.RType.Boolean:
                            Console.WriteLine($"uniform bool {ParameterName} : register(b{RegisterIndex});");
                            break;
                        case ShaderParameter.RType.Integer:
                            Console.WriteLine($"uniform int {ParameterName} : register(i{RegisterIndex});");
                            break;
                        case ShaderParameter.RType.Sampler:
                            Console.WriteLine($"uniform sampler {ParameterName} : register(s{RegisterIndex});");
                            break;
                        case ShaderParameter.RType.Vector:
                            Console.WriteLine($"uniform float4 {ParameterName} : register(c{RegisterIndex});");
                            break;
                    }
                }
                else
                {
                    switch (param.RegisterType)
                    {
                        case ShaderParameter.RType.Boolean:
                            Console.WriteLine($"uniform bool {ParameterName}[{RegisterCount}] : register(b{RegisterIndex});");
                            break;
                        case ShaderParameter.RType.Integer:
                            Console.WriteLine($"uniform int {ParameterName}[{RegisterCount}] : register(i{RegisterIndex});");
                            break;
                        case ShaderParameter.RType.Sampler:
                            Console.WriteLine($"uniform sampler {ParameterName}[{RegisterCount}] : register(s{RegisterIndex});");
                            break;
                        case ShaderParameter.RType.Vector:
                            Console.WriteLine($"uniform float4 {ParameterName}[{RegisterCount}] : register(s{RegisterIndex});");
                            break;
                    }
                }
            }
        }
    }
}