using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Geometry;
using TagTool.Utilities;

namespace TagTool.ShaderGenerator
{
    public partial class ShaderGenerator
    {
        private static Dictionary<Type, List<object>> ImplementedEnums = new Dictionary<Type, List<object>>
        {
            {typeof(Albedo), new List<object> { Albedo.Constant_Color } }
        };





        private static IEnumerable<DirectXUtilities.MacroDefine> GenerateEnumDefinitions(Type _enum)
        {
            List<DirectXUtilities.MacroDefine> definitions = new List<DirectXUtilities.MacroDefine>();

            var values = Enum.GetValues(_enum);

            foreach (var value in values)
            {
                definitions.Add(new DirectXUtilities.MacroDefine
                {
                    Name = $"{_enum.Name}_{value}",
                    Definition = Convert.ChangeType(value, Enum.GetUnderlyingType(_enum)).ToString()
                });
            }

            return definitions;
        }

        private static DirectXUtilities.MacroDefine GenerateEnumValueDefinition(object value, string prefix = "")
        {
            Type _enum = value.GetType();
            var values = Enum.GetValues(_enum);

            return new DirectXUtilities.MacroDefine
            {
                Name = $"{_enum.Name}",
                Definition = $"{_enum.Name}_{value}".ToLower()
            };
        }

        private static List<DirectXUtilities.MacroDefine> GenerateParametersDefinitions(Parameters _params)
        {
            List<DirectXUtilities.MacroDefine> definitions = new List<DirectXUtilities.MacroDefine>();

            definitions.Add(GenerateEnumValueDefinition(_params.albedo, "albedo"));
            definitions.Add(GenerateEnumValueDefinition(_params.bump_mapping, "bump_mapping"));
            definitions.Add(GenerateEnumValueDefinition(_params.alpha_test, "alpha_test"));
            definitions.Add(GenerateEnumValueDefinition(_params.specular_mask, "specular_mask"));
            definitions.Add(GenerateEnumValueDefinition(_params.material_model, "material_model"));
            definitions.Add(GenerateEnumValueDefinition(_params.environment_mapping, "environment_mapping"));
            definitions.Add(GenerateEnumValueDefinition(_params.self_illumination, "self_illumination"));
            definitions.Add(GenerateEnumValueDefinition(_params.blend_mode, "blend_mode"));
            definitions.Add(GenerateEnumValueDefinition(_params.parallax, "parallax"));
            definitions.Add(GenerateEnumValueDefinition(_params.misc, "misc"));
            definitions.Add(GenerateEnumValueDefinition(_params.distortion, "distortion"));
            definitions.Add(GenerateEnumValueDefinition(_params.soft_fade, "soft_fade"));

            return definitions;
        }

        private static void CheckImplementedParameters(object value)
        {
            if (ImplementedEnums.ContainsKey(value.GetType()))
            {
                var list = ImplementedEnums[value.GetType()];
                if (list.Contains(value)) return;
            }
            var message = $"{value.GetType().Name} has not implemented {value}";
            //throw new NotImplementedException(message);]
            Console.WriteLine(message);
        }

        private static void CheckImplementedParameters(Parameters _params)
        {
            CheckImplementedParameters(_params.albedo);
            CheckImplementedParameters(_params.bump_mapping);
            CheckImplementedParameters(_params.alpha_test);
            CheckImplementedParameters(_params.specular_mask);
            CheckImplementedParameters(_params.material_model);
            CheckImplementedParameters(_params.environment_mapping);
            CheckImplementedParameters(_params.self_illumination);
            CheckImplementedParameters(_params.blend_mode);
            CheckImplementedParameters(_params.parallax);
            CheckImplementedParameters(_params.misc);
            CheckImplementedParameters(_params.distortion);
            CheckImplementedParameters(_params.soft_fade);
        }

        public static IEnumerable<DirectXUtilities.MacroDefine> GenerateEnumsDefinitions()
        {
            var defs_Albedo = GenerateEnumDefinitions(typeof(Albedo));
            var defs_Bump_Mapping = GenerateEnumDefinitions(typeof(Bump_Mapping));
            var defs_Alpha_Test = GenerateEnumDefinitions(typeof(Alpha_Test));
            var defs_Specular_Mask = GenerateEnumDefinitions(typeof(Specular_Mask));
            var defs_Material_Model = GenerateEnumDefinitions(typeof(Material_Model));
            var defs_Environment_Mapping = GenerateEnumDefinitions(typeof(Environment_Mapping));
            var defs_Self_Illumination = GenerateEnumDefinitions(typeof(Self_Illumination));
            var defs_Blend_Mode = GenerateEnumDefinitions(typeof(Blend_Mode));
            var defs_Parallax = GenerateEnumDefinitions(typeof(Parallax));
            var defs_Misc = GenerateEnumDefinitions(typeof(Misc));
            var defs_Distortion = GenerateEnumDefinitions(typeof(Distortion));
            var defs_Soft_fade = GenerateEnumDefinitions(typeof(Soft_Fade));
            var defs = (new List<DirectXUtilities.MacroDefine> { })
                .Concat(defs_Albedo)
                .Concat(defs_Bump_Mapping)
                .Concat(defs_Alpha_Test)
                .Concat(defs_Specular_Mask)
                .Concat(defs_Material_Model)
                .Concat(defs_Environment_Mapping)
                .Concat(defs_Self_Illumination)
                .Concat(defs_Blend_Mode)
                .Concat(defs_Parallax)
                .Concat(defs_Misc)
                .Concat(defs_Distortion)
                .Concat(defs_Soft_fade);
            return defs;
        }

        public class Parameters
        {
            public Albedo albedo;
            public Bump_Mapping bump_mapping;
            public Alpha_Test alpha_test;
            public Specular_Mask specular_mask;
            public Material_Model material_model;
            public Environment_Mapping environment_mapping;
            public Self_Illumination self_illumination;
            public Blend_Mode blend_mode;
            public Parallax parallax;
            public Misc misc;
            public Distortion distortion;
            public Soft_Fade soft_fade;
        }

        public static byte[] GenerateSource(Parameters parameters)
        {
#if DEBUG
            CheckImplementedParameters(parameters);
#endif

            //TODO: Think about the easiest way to do this
            //var type_defs = GenerateEnumsDefinitions();
            var value_defs = GenerateParametersDefinitions(parameters);

            //var result = DirectXUtilities.CompilePCShaderFromFile(
            //    "ShaderGenerator/shader_code/shader_template.hlsl",
            //    value_defs.ToArray(),
            //    "main",
            //    "ps_3_0",
            //    0,
            //    out byte[] ShaderBytecode,
            //    out string ErrorMsgs,
            //    out string ConstantTable
            //    );
            //if(!result)
            //{
            //    throw new Exception(ErrorMsgs);
            //}

            byte[] compiled_shader;
            {
                var shader_file = "ShaderGenerator/shader_code/shader_template.hlsl";
                var entry_point = "main";
                //Include include = null;
                var profile = "ps_3_0";
                // ShaderFlags flags = ShaderFlags.Debug;
                //using (var stream = ShaderLoader.CompileShaderFromFile(shader_file, entry_point, include, profile, flags))
                //{

                //    stream.Seek(0, System.IO.SeekOrigin.Begin);
                //    compiled_shader = new byte[stream.Length];
                //    stream.Read(compiled_shader, 0, (int)stream.Length);
                //}

                var result = Utilities.DirectXUtilities.CompilePCShaderFromFile(
                    shader_file,
                    value_defs.ToArray(),
                    entry_point,
                    profile,
                    0,
                    0,
                    out byte[] Shader,
                    out string ErrorMsgs
                    );

                if (!result) throw new Exception(ErrorMsgs);
                compiled_shader = Shader;
            }

            Console.WriteLine();
            Console.WriteLine(ShaderCompiler.Disassemble(compiled_shader));
            Console.WriteLine();

            return compiled_shader;
        }






    }
}
