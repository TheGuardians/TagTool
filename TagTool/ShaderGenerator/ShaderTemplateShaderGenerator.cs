using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Direct3D.Functions;
using TagTool.Geometry;
using TagTool.ShaderGenerator.Types;
using TagTool.Shaders;
using TagTool.Util;

namespace TagTool.ShaderGenerator
{
    public class ShaderTemplateShaderGenerator : TemplateShaderGenerator
    {
        static string ShaderFile { get; } = "ShaderGenerator/shader_code/shader_template.hlsl";

        public ShaderTemplateShaderGenerator(
            GameCacheContext cacheContext,
            Albedo albedo,
            Bump_Mapping bump_mapping,
            Alpha_Test alpha_test,
            Specular_Mask specular_mask,
            Material_Model material_model,
            Environment_Mapping environment_mapping,
            Self_Illumination self_illumination,
            Blend_Mode blend_mode,
            Parallax parallax,
            Misc misc,
            Distortion distortion,
            Soft_Fade soft_fade)
        {
            this.CacheContext = cacheContext;
            this.albedo = albedo;
            this.bump_mapping = bump_mapping;
            this.alpha_test = alpha_test;
            this.specular_mask = specular_mask;
            this.material_model = material_model;
            this.environment_mapping = environment_mapping;
            this.self_illumination = self_illumination;
            this.blend_mode = blend_mode;
            this.parallax = parallax;
            this.misc = misc;
            this.distortion = distortion;
            this.soft_fade = soft_fade;
        }

        #region TemplateShaderGenerator

        public override ShaderGeneratorResult Generate()
        {
#if DEBUG
            CheckImplementedParameters();
#endif

            var shader_parameters = GenerateShaderParameters();
            Dictionary<string, string> file_overrides = new Dictionary<string, string>()
            {
                { "parameters.hlsl", GenerateUniformsFile(shader_parameters)}
            };

            List<DirectX.MacroDefine> definitions = new List<DirectX.MacroDefine>();
            definitions.AddRange(GenerateFunctionDefinition());
            definitions.AddRange(GenerateCompilationFlagDefinitions());

            var compiler = new Util.DirectX();
            compiler.SetCompilerFileOverrides(file_overrides);
            var result = compiler.CompilePCShaderFromFile(
                ShaderFile,
                definitions.ToArray(),
                "main",
                "ps_3_0",
                0,
                0,
                out byte[] ShaderBytecode,
                out string ErrorMsgs
            );
            if (!result) throw new Exception(ErrorMsgs);

            new Disassemble(ShaderBytecode, out string disassembly);

            Console.WriteLine();
            Console.WriteLine(disassembly);
            Console.WriteLine();

            return new ShaderGeneratorResult { ByteCode = ShaderBytecode, Parameters = shader_parameters };
        }

        public override List<TemplateParameter> GetShaderParametersList(object key)
        {
            Type type = key.GetType();
            if (Uniforms.ContainsKey((object)key))
            {
                var list = Uniforms[(object)key].Cast<TemplateParameter>().ToList();
                list = list.Where(param => param.Target_Type == type).ToList();
                return list;
            }
            return new List<TemplateParameter>();
        }

        #endregion

        #region Implemented Features Check

        public static MultiValueDictionary<Type, object> ImplementedEnums = new MultiValueDictionary<Type, object>
        {
            {typeof(Albedo), Albedo.Default },
            {typeof(Albedo), Albedo.Detail_Blend },
            {typeof(Albedo), Albedo.Constant_Color },
            {typeof(Albedo), Albedo.Two_Change_Color },
            {typeof(Albedo), Albedo.Four_Change_Color },
            {typeof(Albedo), Albedo.Two_Detail_Overlay },
            {typeof(Albedo), Albedo.Three_Detail_Blend },
            {typeof(Albedo), Albedo.Two_Detail },
            {typeof(Albedo), Albedo.Color_Mask },
            {typeof(Albedo), Albedo.Two_Detail_Black_Point },
            {typeof(Bump_Mapping), Bump_Mapping.Standard },
            {typeof(Bump_Mapping), Bump_Mapping.Detail },
            {typeof(Bump_Mapping), Bump_Mapping.Off },
            {typeof(Blend_Mode), Blend_Mode.Opaque },
        };

        private static void CheckImplementedParameters(params object[] values)
        {
            foreach (var value in values)
            {
                if (ImplementedEnums.ContainsKey(value.GetType()))
                    if (ImplementedEnums[value.GetType()].Contains(value)) continue;
                Console.WriteLine($"{value.GetType().Name} has not implemented {value}");
            }
        }

        private void CheckImplementedParameters()
        {
            CheckImplementedParameters(
                albedo,
                bump_mapping,
                alpha_test,
                specular_mask,
                material_model,
                environment_mapping,
                self_illumination,
                blend_mode,
                parallax,
                misc,
                distortion,
                soft_fade
            );
        }

        #endregion

        #region HLSL Generation



        private List<DirectX.MacroDefine> GenerateFunctionDefinition()
        {
            List<DirectX.MacroDefine> definitions = new List<DirectX.MacroDefine>();

            definitions.Add(GenerateEnumFuncDefinition(albedo));
            definitions.Add(GenerateEnumFuncDefinition(bump_mapping));
            definitions.Add(GenerateEnumFuncDefinition(alpha_test));
            definitions.Add(GenerateEnumFuncDefinition(specular_mask));
            definitions.Add(GenerateEnumFuncDefinition(material_model));
            definitions.Add(GenerateEnumFuncDefinition(environment_mapping));
            definitions.Add(GenerateEnumFuncDefinition(self_illumination));
            definitions.Add(GenerateEnumFuncDefinition(blend_mode));
            definitions.Add(GenerateEnumFuncDefinition(parallax));
            definitions.Add(GenerateEnumFuncDefinition(misc));
            definitions.Add(GenerateEnumFuncDefinition(distortion));
            definitions.Add(GenerateEnumFuncDefinition(soft_fade));

            return definitions;
        }

        private List<DirectX.MacroDefine> GenerateCompilationFlagDefinitions()
        {
            List<DirectX.MacroDefine> definitions = new List<DirectX.MacroDefine>();

            definitions.Add(GenerateEnumFlagDefinition(albedo));
            definitions.Add(GenerateEnumFlagDefinition(bump_mapping));
            definitions.Add(GenerateEnumFlagDefinition(alpha_test));
            definitions.Add(GenerateEnumFlagDefinition(specular_mask));
            definitions.Add(GenerateEnumFlagDefinition(material_model));
            definitions.Add(GenerateEnumFlagDefinition(environment_mapping));
            definitions.Add(GenerateEnumFlagDefinition(self_illumination));
            definitions.Add(GenerateEnumFlagDefinition(blend_mode));
            definitions.Add(GenerateEnumFlagDefinition(parallax));
            definitions.Add(GenerateEnumFlagDefinition(misc));
            definitions.Add(GenerateEnumFlagDefinition(distortion));
            definitions.Add(GenerateEnumFlagDefinition(soft_fade));

            return definitions;
        }

        private List<ShaderParameter> GenerateShaderParameters()
        {
            return GenerateShaderParameters(
                typeof(Albedo),
                typeof(Bump_Mapping),
                typeof(Alpha_Test),
                typeof(Specular_Mask),
                typeof(Material_Model),
                typeof(Environment_Mapping),
                typeof(Self_Illumination),
                typeof(Blend_Mode),
                typeof(Parallax),
                typeof(Misc),
                typeof(Distortion),
                typeof(Soft_Fade)
            );
        }

        private static IEnumerable<DirectX.MacroDefine> GenerateHLSLEnumDefinitions()
        {
            return GenerateHLSLEnumDefinitions(
                typeof(Albedo),
                typeof(Bump_Mapping),
                typeof(Alpha_Test),
                typeof(Specular_Mask),
                typeof(Material_Model),
                typeof(Environment_Mapping),
                typeof(Self_Illumination),
                typeof(Blend_Mode),
                typeof(Parallax),
                typeof(Misc),
                typeof(Distortion),
                typeof(Soft_Fade)
            );
        }

        #endregion

        #region Uniforms/Registers

        public static MultiValueDictionary<object, object> Uniforms = new MultiValueDictionary<object, object>
        {
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "albedo_unknown_s1", ShaderParameter.RType.Sampler) {enabled = false } }, // Manually added (Unknown bitmap)
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "albedo_color", ShaderParameter.RType.Vector) },
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Default,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added

            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "albedo_unknown_s1", ShaderParameter.RType.Sampler) {enabled = false } }, // Manually added (Unknown bitmap)
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map2", ShaderParameter.RType.Sampler) },
            {Albedo.Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map2_xform", ShaderParameter.RType.Vector) }, // Manually added

            {Albedo.Constant_Color,  new TemplateParameter(typeof(Albedo), "albedo_color", ShaderParameter.RType.Vector) },
            {Albedo.Constant_Color,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added

            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "albedo_unknown_s1", ShaderParameter.RType.Sampler) {enabled = false } }, // Manually added (Unknown bitmap)
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "change_color_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "change_color_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "primary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color,  new TemplateParameter(typeof(Albedo), "secondary_change_color", ShaderParameter.RType.Vector) },

            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "change_color_map", ShaderParameter.RType.Sampler) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "change_color_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "primary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "secondary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "tertiary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Four_Change_Color,  new TemplateParameter(typeof(Albedo), "quaternary_change_color", ShaderParameter.RType.Vector) },

            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "albedo_unknown_s1", ShaderParameter.RType.Sampler) {enabled = false } }, // Manually added (Unknown bitmap)
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map2", ShaderParameter.RType.Sampler) },
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map2_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Three_Detail_Blend,  new TemplateParameter(typeof(Albedo), "detail_map3_xform", ShaderParameter.RType.Vector) }, // Manually added

            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "albedo_unknown_s1", ShaderParameter.RType.Sampler) {enabled = false } }, // Manually added (Unknown bitmap)
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map2", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map2_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map_overlay", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map_overlay_xform", ShaderParameter.RType.Vector) }, // Manually added

            {Albedo.Two_Detail,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail,  new TemplateParameter(typeof(Albedo), "albedo_unknown_s1", ShaderParameter.RType.Sampler) {enabled = false } }, // Manually added (Unknown bitmap)
            {Albedo.Two_Detail,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail,  new TemplateParameter(typeof(Albedo), "detail_map2", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail,  new TemplateParameter(typeof(Albedo), "detail_map2_xform", ShaderParameter.RType.Vector) }, // Manually added
            
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "albedo_color", ShaderParameter.RType.Vector) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "albedo_color2", ShaderParameter.RType.Vector) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "albedo_color3", ShaderParameter.RType.Vector) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "color_mask_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "color_mask_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Color_Mask,  new TemplateParameter(typeof(Albedo), "neutral_gray", ShaderParameter.RType.Vector) },


            {Albedo.Two_Detail_Black_Point,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Black_Point,  new TemplateParameter(typeof(Albedo), "albedo_unknown_s1", ShaderParameter.RType.Sampler) {enabled = false } }, // Manually added (Unknown bitmap)
            {Albedo.Two_Detail_Black_Point,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Black_Point,  new TemplateParameter(typeof(Albedo), "base_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail_Black_Point,  new TemplateParameter(typeof(Albedo), "detail_map_xform", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail_Black_Point,  new TemplateParameter(typeof(Albedo), "debug_tint", ShaderParameter.RType.Vector) }, // Manually added
            {Albedo.Two_Detail_Black_Point,  new TemplateParameter(typeof(Albedo), "detail_map2", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Detail_Black_Point,  new TemplateParameter(typeof(Albedo), "detail_map2_xform", ShaderParameter.RType.Vector) }, // Manually added
            



            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "change_color_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "primary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "secondary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "primary_change_color_anim", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Anim_Overlay,  new TemplateParameter(typeof(Albedo), "secondary_change_color_anim", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color0", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color1", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color2", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color3", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset1", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset2", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_fresnel_power", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "change_color_map", ShaderParameter.RType.Sampler) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "primary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "secondary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "primary_change_color_anim", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "secondary_change_color_anim", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color0", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color1", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color2", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color3", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset1", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset2", ShaderParameter.RType.Vector) },
            {Albedo.Two_Change_Color_Chameleon,  new TemplateParameter(typeof(Albedo), "chameleon_fresnel_power", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_mask_map", ShaderParameter.RType.Sampler) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color0", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color1", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color2", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color3", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset1", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_color_offset2", ShaderParameter.RType.Vector) },
            {Albedo.Chameleon_Masked,  new TemplateParameter(typeof(Albedo), "chameleon_fresnel_power", ShaderParameter.RType.Vector) },
            {Albedo.Color_Mask_Hard_Light,  new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask_Hard_Light,  new TemplateParameter(typeof(Albedo), "detail_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask_Hard_Light,  new TemplateParameter(typeof(Albedo), "color_mask_map", ShaderParameter.RType.Sampler) },
            {Albedo.Color_Mask_Hard_Light,  new TemplateParameter(typeof(Albedo), "albedo_color", ShaderParameter.RType.Vector) },

            {Bump_Mapping.Standard,  new TemplateParameter(typeof(Bump_Mapping), "bump_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Standard,  new TemplateParameter(typeof(Bump_Mapping),"bump_map_xform", ShaderParameter.RType.Vector) }, // Manual

            {Bump_Mapping.Detail,  new TemplateParameter(typeof(Bump_Mapping),"bump_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Detail,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Detail,  new TemplateParameter(typeof(Bump_Mapping),"bump_map_xform", ShaderParameter.RType.Vector) }, // Manual
            {Bump_Mapping.Detail,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_map_xform", ShaderParameter.RType.Vector) }, // Manual
            {Bump_Mapping.Detail,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_coefficient", ShaderParameter.RType.Vector) },

            {Bump_Mapping.Detail_Masked,  new TemplateParameter(typeof(Bump_Mapping),"bump_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Detail_Masked,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Detail_Masked,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_mask_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Detail_Masked,  new TemplateParameter(typeof(Bump_Mapping),"bump_detail_coefficient", ShaderParameter.RType.Vector) },

            { Alpha_Test.Simple,  new TemplateParameter(typeof(Alpha_Test),"alpha_test_map", ShaderParameter.RType.Sampler) },
            {Specular_Mask.From_Texture,  new TemplateParameter(typeof(Specular_Mask),"specular_mask_texture", ShaderParameter.RType.Sampler) },
            {Specular_Mask.From_Color_Texture,  new TemplateParameter(typeof(Specular_Mask),"specular_mask_texture", ShaderParameter.RType.Sampler) },
            {Material_Model.Diffuse_Only,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"specular_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"fresnel_color", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"use_fresnel_color_environment", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"fresnel_color_environment", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"fresnel_power", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"roughness", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"environment_map_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"order3_area_specular", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"use_material_texture", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"material_texture", ShaderParameter.RType.Sampler) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"g_sampler_cc0236", ShaderParameter.RType.Sampler) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"g_sampler_dd0236", ShaderParameter.RType.Sampler) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"g_sampler_c78d78", ShaderParameter.RType.Sampler) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"albedo_blend_with_specular_tint", ShaderParameter.RType.Boolean) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"albedo_blend", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"analytical_anti_shadow_control", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_color", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_power", ShaderParameter.RType.Vector) },
            {Material_Model.Cook_Torrance,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_albedo_blend", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"normal_specular_power", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"normal_specular_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"glancing_specular_power", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"glancing_specular_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"fresnel_curve_steepness", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"environment_map_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"order3_area_specular", ShaderParameter.RType.Boolean) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"albedo_specular_tint_blend", ShaderParameter.RType.Vector) },
            {Material_Model.Two_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"analytical_anti_shadow_control", ShaderParameter.RType.Vector) },
            {Material_Model.Foliage,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"fresnel_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"fresnel_curve_steepness", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"fresnel_curve_bias", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"roughness", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Glass,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"diffuse_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"analytical_specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"area_specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"specular_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"specular_power", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"specular_map", ShaderParameter.RType.Sampler) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"environment_map_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"environment_map_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"fresnel_curve_steepness", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"rim_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"rim_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"rim_power", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"rim_start", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"rim_maps_transition_ratio", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"ambient_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"ambient_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"occlusion_parameter_map", ShaderParameter.RType.Sampler) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"subsurface_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"subsurface_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"subsurface_propagation_bias", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"subsurface_normal_detail", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"subsurface_map", ShaderParameter.RType.Sampler) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"transparence_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"transparence_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"transparence_normal_bias", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"transparence_normal_detail", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"transparence_map", ShaderParameter.RType.Sampler) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"final_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Organism,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"specular_coefficient", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"roughness", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"environment_map_specular_contribution", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"specular_tint", ShaderParameter.RType.Vector) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"order3_area_specular", ShaderParameter.RType.Boolean) },
            {Material_Model.Single_Lobe_Phong,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"use_material_texture0", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"use_material_texture1", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"material_texture", ShaderParameter.RType.Sampler) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"no_dynamic_lights", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"g_sampler_cc0236", ShaderParameter.RType.Sampler) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"g_sampler_dd0236", ShaderParameter.RType.Sampler) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"g_sampler_c78d78", ShaderParameter.RType.Sampler) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"bump_detail_map0", ShaderParameter.RType.Sampler) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"bump_detail_map0_blend_factor", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"specular_coefficient0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"specular_tint0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"fresnel_color0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"fresnel_power0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"albedo_blend0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"roughness0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution0", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"order3_area_specular0", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"diffuse_coefficient1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"specular_coefficient1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"specular_tint1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"fresnel_color1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"fresnel_color_environment1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"fresnel_power1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"albedo_blend1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"roughness1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"area_specular_contribution1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"analytical_specular_contribution1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"environment_map_specular_contribution1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"order3_area_specular1", ShaderParameter.RType.Boolean) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_coefficient1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_color1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_power1", ShaderParameter.RType.Vector) },
            {Material_Model.Car_Paint,  new TemplateParameter(typeof(Material_Model),"rim_fresnel_albedo_blend1", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Per_Pixel,  new TemplateParameter(typeof(Environment_Mapping),"environment_map", ShaderParameter.RType.Sampler) },
            {Environment_Mapping.Per_Pixel,  new TemplateParameter(typeof(Environment_Mapping),"env_tint_color", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Per_Pixel,  new TemplateParameter(typeof(Environment_Mapping),"env_roughness_scale", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Dynamic,  new TemplateParameter(typeof(Environment_Mapping),"env_tint_color", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Dynamic,  new TemplateParameter(typeof(Environment_Mapping),"dynamic_environment_map_0", ShaderParameter.RType.Sampler) },
            {Environment_Mapping.Dynamic,  new TemplateParameter(typeof(Environment_Mapping),"dynamic_environment_map_1", ShaderParameter.RType.Sampler) },
            {Environment_Mapping.Dynamic,  new TemplateParameter(typeof(Environment_Mapping),"env_roughness_scale", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"flat_environment_map", ShaderParameter.RType.Sampler) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"env_tint_color", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"flat_envmap_matrix_x", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"flat_envmap_matrix_y", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"flat_envmap_matrix_z", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"hemisphere_percentage", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"env_bloom_override", ShaderParameter.RType.Vector) },
            {Environment_Mapping.From_Flat_Texture,  new TemplateParameter(typeof(Environment_Mapping),"env_bloom_override_intensity", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Custom_Map,  new TemplateParameter(typeof(Environment_Mapping),"environment_map", ShaderParameter.RType.Sampler) },
            {Environment_Mapping.Custom_Map,  new TemplateParameter(typeof(Environment_Mapping),"env_tint_color", ShaderParameter.RType.Vector) },
            {Environment_Mapping.Custom_Map,  new TemplateParameter(typeof(Environment_Mapping),"env_roughness_scale", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Simple,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination._3_Channel_Self_Illum,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination._3_Channel_Self_Illum,  new TemplateParameter(typeof(Self_Illumination),"channel_a", ShaderParameter.RType.Vector) },
            {Self_Illumination._3_Channel_Self_Illum,  new TemplateParameter(typeof(Self_Illumination),"channel_b", ShaderParameter.RType.Vector) },
            {Self_Illumination._3_Channel_Self_Illum,  new TemplateParameter(typeof(Self_Illumination),"channel_c", ShaderParameter.RType.Vector) },
            {Self_Illumination._3_Channel_Self_Illum,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"noise_map_a", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"noise_map_b", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"color_medium", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"color_wide", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"color_sharp", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"alpha_mask_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"thinness_medium", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"thinness_wide", ShaderParameter.RType.Vector) },
            {Self_Illumination.Plasma,  new TemplateParameter(typeof(Self_Illumination),"thinness_sharp", ShaderParameter.RType.Vector) },
            {Self_Illumination.From_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.From_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Illum_Detail,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Illum_Detail,  new TemplateParameter(typeof(Self_Illumination),"self_illum_detail_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Illum_Detail,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.Illum_Detail,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Meter,  new TemplateParameter(typeof(Self_Illumination),"meter_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Meter,  new TemplateParameter(typeof(Self_Illumination),"meter_color_off", ShaderParameter.RType.Vector) },
            {Self_Illumination.Meter,  new TemplateParameter(typeof(Self_Illumination),"meter_color_on", ShaderParameter.RType.Vector) },
            {Self_Illumination.Meter,  new TemplateParameter(typeof(Self_Illumination),"meter_value", ShaderParameter.RType.Vector) },
            {Self_Illumination.Self_Illum_Times_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Self_Illum_Times_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.Self_Illum_Times_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Self_Illum_Times_Diffuse,  new TemplateParameter(typeof(Self_Illumination),"primary_change_color_blend", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple_With_Alpha_Mask,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Simple_With_Alpha_Mask,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple_With_Alpha_Mask,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple_Four_Change_Color,  new TemplateParameter(typeof(Self_Illumination),"self_illum_map", ShaderParameter.RType.Sampler) },
            {Self_Illumination.Simple_Four_Change_Color,  new TemplateParameter(typeof(Self_Illumination),"self_illum_color", ShaderParameter.RType.Vector) },
            {Self_Illumination.Simple_Four_Change_Color,  new TemplateParameter(typeof(Self_Illumination),"self_illum_intensity", ShaderParameter.RType.Vector) },
            {Parallax.Simple,  new TemplateParameter(typeof(Parallax),"height_map", ShaderParameter.RType.Sampler) },
            {Parallax.Simple,  new TemplateParameter(typeof(Parallax),"height_scale", ShaderParameter.RType.Vector) },
            {Parallax.Interpolated,  new TemplateParameter(typeof(Parallax),"height_map", ShaderParameter.RType.Sampler) },
            {Parallax.Interpolated,  new TemplateParameter(typeof(Parallax),"height_scale", ShaderParameter.RType.Vector) },
            {Parallax.Simple_Detail,  new TemplateParameter(typeof(Parallax),"height_map", ShaderParameter.RType.Sampler) },
            {Parallax.Simple_Detail,  new TemplateParameter(typeof(Parallax),"height_scale", ShaderParameter.RType.Vector) },
            {Parallax.Simple_Detail,  new TemplateParameter(typeof(Parallax),"height_scale_map", ShaderParameter.RType.Sampler) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"distort_map", ShaderParameter.RType.Sampler) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"distort_scale", ShaderParameter.RType.Vector) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"soft_fresnel_enabled", ShaderParameter.RType.Boolean) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"soft_fresnel_power", ShaderParameter.RType.Vector) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"soft_z_enabled", ShaderParameter.RType.Boolean) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"soft_z_range", ShaderParameter.RType.Vector) },
            {Distortion.On,  new TemplateParameter(typeof(Distortion),"depth_map", ShaderParameter.RType.Sampler) },
        };

        #endregion

        #region Enums

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

        public enum Albedo
        {
            Default,
            Detail_Blend,
            Constant_Color,
            Two_Change_Color,
            Four_Change_Color,
            Three_Detail_Blend,
            Two_Detail_Overlay,
            Two_Detail,
            Color_Mask,
            Two_Detail_Black_Point,
            Two_Change_Color_Anim_Overlay,
            Chameleon,
            Two_Change_Color_Chameleon,
            Chameleon_Masked,
            Color_Mask_Hard_Light,
            Two_Change_Color_Tex_Overlay,
            Chameleon_Albedo_Masked
        }

        public enum Bump_Mapping
        {
            Off,
            Standard,
            Detail,
            Detail_Masked,
            Detail_Plus_Detail_Masked
        }

        public enum Alpha_Test
        {
            None,
            Simple
        }

        public enum Specular_Mask
        {
            No_Specular_Mask,
            From_Diffuse,
            From_Texture,
            From_Color_Texture
        }

        public enum Material_Model
        {
            Diffuse_Only,
            Cook_Torrance,
            Two_Lobe_Phong,
            Foliage,
            None,
            Glass,
            Organism,
            Single_Lobe_Phong,
            Car_Paint,
            Hair
        }

        public enum Environment_Mapping
        {
            None,
            Per_Pixel,
            Dynamic,
            From_Flat_Texture,
            Custom_Map
        }

        public enum Self_Illumination
        {
            Off,
            Simple,
            _3_Channel_Self_Illum,
            Plasma,
            From_Diffuse,
            Illum_Detail,
            Meter,
            Self_Illum_Times_Diffuse,
            Simple_With_Alpha_Mask,
            Simple_Four_Change_Color,
            Illum_Detail_World_Space_Four_Cc
        }

        public enum Blend_Mode
        {
            Opaque,
            Additive,
            Multiply,
            Alpha_Blend,
            Double_Multiply,
            Pre_Multiplied_Alpha
        }

        public enum Parallax
        {
            Off,
            Simple,
            Interpolated,
            Simple_Detail
        }

        public enum Misc
        {
            First_Person_Never,
            First_Person_Sometimes,
            First_Person_Always,
            First_Person_Never_With_rotating_Bitmaps
        }

        public enum Distortion
        {
            Off,
            On
        }

        public enum Soft_Fade
        {
            Off,
            On
        }

        #endregion
    }
}
