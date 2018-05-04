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
using static TagTool.Tags.Definitions.RenderMethodTemplate.DrawModeRegisterOffsetBlock;
using static TagTool.Tags.Definitions.RenderMethodTemplate;

namespace TagTool.Commands.Shaders
{
    class ListRegistersRMT2 : Command
    {
        private GameCacheContext CacheContext { get; }

        public ListRegistersRMT2(GameCacheContext cacheContext) :
            base(CommandFlags.Inherit,

                "ListRegistersRMT2",
                "List Registers using RMT2 tags",

                "ListRegistersRMT2 [tag <tag_id>] <template_type>",
                "List Registers using RMT2 tags")
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
            string template = null;
            if (args.Count % 2 == 1) template = args[args.Count - 1];

            bool specific_tag = false;
            string specific_tag_id = "";

            for (int i=0;i<args.Count;i++)
            {
                switch (args[i])
                {
                    case "tag":
                        specific_tag = true;
                        specific_tag_id = args[++i];
                        break;
                }
            }

            ListRegisters(specific_tag, specific_tag_id, template);

            return true;
        }

        public List<ShaderMode> GetShaderModes(ShaderModeBitmask bitmask)
        {
            List<ShaderMode> modes = new List<ShaderMode>();
            if ((bitmask & ShaderModeBitmask.Default) != 0) modes.Add(ShaderMode.Default);
            if ((bitmask & ShaderModeBitmask.Albedo) != 0) modes.Add(ShaderMode.Albedo);
            if ((bitmask & ShaderModeBitmask.Static_Default) != 0) modes.Add(ShaderMode.Static_Default);
            if ((bitmask & ShaderModeBitmask.Static_Per_Pixel) != 0) modes.Add(ShaderMode.Static_Per_Pixel);
            if ((bitmask & ShaderModeBitmask.Static_Per_Vertex) != 0) modes.Add(ShaderMode.Static_Per_Vertex);
            if ((bitmask & ShaderModeBitmask.Static_Sh) != 0) modes.Add(ShaderMode.Static_Sh);
            if ((bitmask & ShaderModeBitmask.Static_Prt_Ambient) != 0) modes.Add(ShaderMode.Static_Prt_Ambient);
            if ((bitmask & ShaderModeBitmask.Static_Prt_Linear) != 0) modes.Add(ShaderMode.Static_Prt_Linear);
            if ((bitmask & ShaderModeBitmask.Static_Prt_Quadratic) != 0) modes.Add(ShaderMode.Static_Prt_Quadratic);
            if ((bitmask & ShaderModeBitmask.Dynamic_Light) != 0) modes.Add(ShaderMode.Dynamic_Light);
            if ((bitmask & ShaderModeBitmask.Shadow_Generate) != 0) modes.Add(ShaderMode.Shadow_Generate);
            if ((bitmask & ShaderModeBitmask.Shadow_Apply) != 0) modes.Add(ShaderMode.Shadow_Apply);
            if ((bitmask & ShaderModeBitmask.Active_Camo) != 0) modes.Add(ShaderMode.Active_Camo);
            if ((bitmask & ShaderModeBitmask.Lightmap_Debug_Mode) != 0) modes.Add(ShaderMode.Lightmap_Debug_Mode);
            if ((bitmask & ShaderModeBitmask.Static_Per_Vertex_Color) != 0) modes.Add(ShaderMode.Static_Per_Vertex_Color);
            if ((bitmask & ShaderModeBitmask.Water_Tessellation) != 0) modes.Add(ShaderMode.Water_Tessellation);
            if ((bitmask & ShaderModeBitmask.Water_Shading) != 0) modes.Add(ShaderMode.Water_Shading);
            if ((bitmask & ShaderModeBitmask.Dynamic_Light_Cinematic) != 0) modes.Add(ShaderMode.Dynamic_Light_Cinematic);
            if ((bitmask & ShaderModeBitmask.Z_Only) != 0) modes.Add(ShaderMode.Z_Only);
            if ((bitmask & ShaderModeBitmask.Sfx_Distort) != 0) modes.Add(ShaderMode.Sfx_Distort);

            return modes;
        }

        public bool IsShaderModeUsed(ShaderMode mode)
        {
            if (mode == ShaderMode.Shadow_Generate) return false;
            return true;
        }

        public void ListRegisters(bool specific_tag, string specific_tag_id, string _template_type)
        {
            var stream = CacheContext.OpenTagCacheRead();

            HashSet<string> strings = new HashSet<string>();
            strings.Add($"name,register_index,register_type,argument_index,offset_name,array_size,shader_mode,tag_name");

            IEnumerable<CachedTagInstance> tags;

            if(specific_tag)
            {
                tags = new CachedTagInstance[] { CacheContext.TagCache.Index[Convert.ToInt32(specific_tag_id, 16)] };
            }
            else
            {
                tags = CacheContext.TagCache.Index.Where(instance => instance != null && TagDefinition.Find(instance.Group.Tag) == typeof(RenderMethodTemplate));
            }

            foreach (var instance in tags)
            {
                //if (instance == null)
                //    continue;

                var type = TagDefinition.Find(instance.Group.Tag);
                if (type != typeof(RenderMethodTemplate)) throw new Exception("Invalid tag");

                var rmt2 = (RenderMethodTemplate)CacheContext.Deserializer.Deserialize(new TagSerializationContext(stream, CacheContext, instance), typeof(RenderMethodTemplate));

                string tag_name;
                {
                    var tag_index = CacheContext.TagCache.Index.ToList().IndexOf(instance);
                    var name = CacheContext.TagNames.ContainsKey(tag_index) ? CacheContext.TagNames[tag_index] : null;
                    if(_template_type != null)
                    {
                        if (name == null) continue;
                        if (!name.Contains("\\")) continue; // Probbaly an unnamed tag
                        var template_type = name.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries)[1];
                        if (_template_type != "*" && template_type != _template_type) continue;
                    }
                    tag_name = name;
                }

                var pixl = (PixelShader)CacheContext.Deserializer.Deserialize(new TagSerializationContext(stream, CacheContext, rmt2.PixelShader), typeof(PixelShader));

                var shader_modes = GetShaderModes(rmt2.DrawModeBitmask);

                foreach (var drawmode_register_map in rmt2.DrawModeRegisterOffsets)
                {
                    var drawmode_index = rmt2.DrawModeRegisterOffsets.IndexOf(drawmode_register_map);
                    if (drawmode_index >= shader_modes.Count) continue;

                    var shader_mode = shader_modes[drawmode_index];
                    if (!IsShaderModeUsed(shader_mode)) continue;

                    for (DrawModeRegisterOffsetType offset_type = 0; offset_type < DrawModeRegisterOffsetType.DrawModeRegisterOffsetType_Count; offset_type++)
                    {
                        var num_registers = drawmode_register_map.GetCount(offset_type);

                        for (var arguement_mapping_index = 0; arguement_mapping_index < num_registers; arguement_mapping_index++)
                        {
                            var index = drawmode_register_map.GetOffset(offset_type) + arguement_mapping_index;
                            var argument = rmt2.ArgumentMappings[index];

                            var shader_index = pixl.DrawModes[(int)shader_mode].Index;
                            var shader = pixl.Shaders[shader_index];

                            foreach(var param in shader.PCParameters)
                            {
                                if(param.RegisterIndex == argument.RegisterIndex)
                                {
                                    var name = CacheContext.GetString(param.ParameterName);
                                    var register_index = param.RegisterIndex;
                                    var register_type = param.RegisterType.ToString();
                                    var argument_index = argument.ArgumentIndex;
                                    var offset_name = offset_type.ToString();
                                    var array_size = param.RegisterCount;
                                    var shader_mode_str = shader_mode.ToString();


                                    strings.Add($"{name},{register_index},{register_type},{argument_index},{offset_name},{array_size},{shader_mode_str},{tag_name}");
                                }
                            }
                        }
                    }
                }
            }

            stream.Close();

            //Create the file.
            try
            {
                using (FileStream fs = File.Create("listregistersrmt2_output.csv"))
                {
                    foreach (var line in strings)
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes($"{line}\n");
                        fs.Write(info, 0, info.Length);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Failed to export file");
            }
            if(specific_tag)
            {
                foreach (var line in strings)
                {
                    Console.WriteLine(line);
                }
            }
            Console.WriteLine($"found {strings.Count}");
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