using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Direct3D.Functions;
using TagTool.ShaderGenerator.Types;
using TagTool.Shaders;
using TagTool.Util;

namespace TagTool.ShaderGenerator
{
    public class DecalTemplateShaderGenerator : TemplateShaderGenerator
    {
        static string ShaderFile { get; } = "ShaderGenerator/shader_code/decals_template.hlsl";

        public DecalTemplateShaderGenerator(
            GameCacheContext cacheContext,
            Albedo albedo,
            Blend_Mode blend_mode,
            Render_Pass render_pass,
            Specular specular,
            Bump_Mapping bump_mapping,
            Tinting tinting) : base(
                albedo,
                blend_mode,
                render_pass,
                specular,
                bump_mapping,
                tinting)
        {

            this.CacheContext = cacheContext;
        }

        #region Implemented Features Check

        protected override MultiValueDictionary<Type, object> ImplementedEnums { get; set; } = new MultiValueDictionary<Type, object>
        {
            {typeof(Albedo), Albedo.DiffuseOnly },
            {typeof(Albedo), Albedo.Palettized_Plus_Alpha },
            {typeof(Albedo), Albedo.Diffuse_Plus_Alpha },

            {typeof(Bump_Mapping), Bump_Mapping.Leave },
            {typeof(Bump_Mapping), Bump_Mapping.Standard },

            //{typeof(Blend_Mode), Blend_Mode.Opaque },
            //{typeof(Blend_Mode), Blend_Mode.Additive },
            //{typeof(Blend_Mode), Blend_Mode.Pre_Multiplied_Alpha },
            
            {typeof(Blend_Mode), Blend_Mode.Multiply },
            {typeof(Blend_Mode), Blend_Mode.Alpha_Blend },
            {typeof(Blend_Mode), Blend_Mode.Double_Multiply },

            {typeof(Render_Pass), Render_Pass.Pre_Lighting},
            {typeof(Tinting), Tinting.None },
            {typeof(Tinting), Tinting.Unmodulated },
            {typeof(Specular), Specular.Leave },
            {typeof(Specular), Specular.Modulate },
        };

        #endregion

        #region TemplateShaderGenerator

        public override ShaderGeneratorResult Generate()
        {
#if DEBUG
            CheckImplementedParameters();
#endif

            var shader_parameters = GenerateShaderParameters(58, 0, 0);
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

        #endregion

        #region Uniforms/Registers

        protected override MultiValueDictionary<object, object> Uniforms { get; set; } = new MultiValueDictionary<object, object>
        {
            // These appear to be apart of some kind of global structure
            {0, new TemplateParameter(typeof(Int32), "g_exposure", ShaderParameter.RType.Vector) {SpecificOffset = 0 } },
            {0, new TemplateParameter(typeof(Int32), "g_alt_exposure", ShaderParameter.RType.Vector) {SpecificOffset = 12 } },
            {0, new TemplateParameter(typeof(Int32), "fade", ShaderParameter.RType.Vector) {SpecificOffset = 32 } },

            {Albedo.DiffuseOnly, new TemplateParameter(typeof(Albedo), "unknown_sampler0", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.DiffuseOnly, new TemplateParameter(typeof(Albedo), "unknown_sampler1", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.DiffuseOnly, new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },

            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "unknown_sampler0", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "unknown_sampler1", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "alpha_map", ShaderParameter.RType.Sampler) },
            {Albedo.Palettized_Plus_Alpha, new TemplateParameter(typeof(Albedo), "palette", ShaderParameter.RType.Sampler) },

            {Albedo.Diffuse_Plus_Alpha, new TemplateParameter(typeof(Albedo), "unknown_sampler0", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Diffuse_Plus_Alpha, new TemplateParameter(typeof(Albedo), "unknown_sampler1", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Diffuse_Plus_Alpha, new TemplateParameter(typeof(Albedo), "base_map", ShaderParameter.RType.Sampler) },
            {Albedo.Diffuse_Plus_Alpha, new TemplateParameter(typeof(Albedo), "alpha_map", ShaderParameter.RType.Sampler) },

            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "unknown_sampler0", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "unknown_sampler1", ShaderParameter.RType.Sampler) {Enabled = false } },
            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "change_color_map", ShaderParameter.RType.Sampler) },
            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "primary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "secondary_change_color", ShaderParameter.RType.Vector) },
            {Albedo.Change_Color, new TemplateParameter(typeof(Albedo), "tertiary_change_color", ShaderParameter.RType.Vector) },

            {Bump_Mapping.Standard, new TemplateParameter(typeof(Bump_Mapping), "bump_map", ShaderParameter.RType.Sampler) },
            {Bump_Mapping.Standard, new TemplateParameter(typeof(Bump_Mapping), "bump_map_xform", ShaderParameter.RType.Vector) },

            {Tinting.Unmodulated, new TemplateParameter(typeof(Tinting), "tint_color", ShaderParameter.RType.Vector) },
            {Tinting.Unmodulated, new TemplateParameter(typeof(Tinting), "intensity", ShaderParameter.RType.Vector) },

            {Tinting.Fully_Modulated, new TemplateParameter(typeof(Tinting), "tint_color", ShaderParameter.RType.Vector) },
            {Tinting.Fully_Modulated, new TemplateParameter(typeof(Tinting), "intensity", ShaderParameter.RType.Vector) },
        };

        #endregion

        #region Enums

        public Albedo albedo => (Albedo)EnumValues[0];
        public Blend_Mode blend_mode => (Blend_Mode)EnumValues[1];
        public Render_Pass render_pass => (Render_Pass)EnumValues[2];
        public Specular specular => (Specular)EnumValues[3];
        public Bump_Mapping bump_mapping => (Bump_Mapping)EnumValues[4];
        public Tinting tinting => (Tinting)EnumValues[5];

        public enum Albedo
        {
            DiffuseOnly,
            Palettized,
            Palettized_Plus_Alpha,
            Diffuse_Plus_Alpha,
            Emblem_Change_Color,
            Change_Color,
            Diffuse_Plus_Alpha_Mask,
            Palettized_Plus_Alpha_Mask,
            Vector_Alpha,
            Vector_Alpha_Drop_Shadow
        }

        public enum Blend_Mode
        {
            Opaque,
            Additive,
            Multiply,
            Alpha_Blend,
            Double_Multiply,
            Maximum,
            Multiply_Add,
            Add_Src_Times_DstAlpha,
            Add_Src_Times_SrcAlpha,
            Inv_Alpha_Blend,
            Pre_Multiplied_Alpha
        }

        public enum Render_Pass
        {
            Pre_Lighting,
            Post_Lighting
        }

        public enum Specular
        {
            Leave,
            Modulate
        }

        public enum Bump_Mapping
        {
            Leave,
            Standard,
            Standard_Mask
        }

        public enum Tinting
        {
            None,
            Unmodulated,
            Partially_Modulated,
            Fully_Modulated
        }

        #endregion
    }
}
