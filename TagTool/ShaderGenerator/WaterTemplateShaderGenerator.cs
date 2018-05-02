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
	public class WaterTemplateShaderGenerator : TemplateShaderGenerator
    {
        protected override string ShaderFile { get; } = "ShaderGenerator/shader_code/water.hlsl";
        protected override string ShaderGeneratorType => "water_template";
        protected override List<DirectX.MacroDefine> TemplateDefinitions => new List<DirectX.MacroDefine>
        {
            new DirectX.MacroDefine {Name = "_debug_color", Definition = "float4(1, 0, 0, 0)" }
        };

        public WaterTemplateShaderGenerator(GameCacheContext cacheContext, TemplateShaderGenerator.Drawmode drawmode, Int32[] args, Int32 arg_pos = 0) : base(
                drawmode,
                (Wave_Shape)GetNextTemplateArg(args, ref arg_pos),
				(Water_Color)GetNextTemplateArg(args, ref arg_pos),
				(Reflection)GetNextTemplateArg(args, ref arg_pos),
				(Refraction)GetNextTemplateArg(args, ref arg_pos),
				(Bank_Alpha)GetNextTemplateArg(args, ref arg_pos),
				(Appearance)GetNextTemplateArg(args, ref arg_pos),
				(Global_Shape)GetNextTemplateArg(args, ref arg_pos),
				(Foam)GetNextTemplateArg(args, ref arg_pos))
		{
			this.CacheContext = cacheContext;
		}

		#region Implemented Features Check

		protected override MultiValueDictionary<Type, object> ImplementedEnums => new MultiValueDictionary<Type, object>
		{

		};

		#endregion
        
		#region Uniforms/Registers

		protected override MultiValueDictionary<object, TemplateParameter> Uniforms => new MultiValueDictionary<object, TemplateParameter>
		{

		};

        #endregion

        #region Enums

        public Wave_Shape wave_shape => (Wave_Shape)EnumValues[0];
        public Water_Color water_color => (Water_Color)EnumValues[1];
        public Reflection reflection => (Reflection)EnumValues[2];
        public Refraction refraction => (Refraction)EnumValues[3];
        public Bank_Alpha bank_alpha => (Bank_Alpha)EnumValues[4];
        public Appearance appearance => (Appearance)EnumValues[5];
        public Global_Shape global_shape => (Global_Shape)EnumValues[6];
        public Foam foam => (Foam)EnumValues[7];

        public enum Wave_Shape
        {
            Default,
            None,
            Bump
        }

        public enum Water_Color
        {
            Pure,
            Texture
        }

        public enum Reflection
        {
            None,
            Static,
            Dynamic
        }

        public enum Refraction
        {
            None,
            Dynamic
        }

        public enum Bank_Alpha
        {
            None,
            Depth,
            Paint
        }

        public enum Appearance
        {
            Default
        }

        public enum Global_Shape
        {
            None,
            Paint,
            Depth
        }

        public enum Foam
        {
            None,
            Auto,
            Paint,
            Both
        }

        #endregion
    }
}
