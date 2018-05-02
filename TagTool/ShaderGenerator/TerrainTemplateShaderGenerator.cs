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
    public class TerrainTemplateShaderGenerator : TemplateShaderGenerator
    {
        protected override string ShaderGeneratorType => "terrain_template";

        bool use_detail_bumps
        {
            get
            {
                int number_of_detail_bumps = 0;
                if (material_0 != Material_0.Off) number_of_detail_bumps++;
                if (material_1 != Material_1.Off) number_of_detail_bumps++;
                if (material_2 != Material_2.Off) number_of_detail_bumps++;
                if (material_3 != Material_3.Off) number_of_detail_bumps++;

                return number_of_detail_bumps <= 3;
            }
        }

        protected override List<DirectX.MacroDefine> TemplateDefinitions
        {
            get
            {
                var definitions = new List<DirectX.MacroDefine>
                {
                    new DirectX.MacroDefine {Name = "_debug_color", Definition = "float4(1, 0, 0, 0)" },
                    new DirectX.MacroDefine {Name = "Albedo", Definition = "albedo_terrain"},
                    new DirectX.MacroDefine {Name = "Bump_Mapping", Definition = "bump_mapping_terrain"}
                };

                if(use_detail_bumps)
                    definitions.Add(new DirectX.MacroDefine { Name = "flag_use_detail_bumps", Definition = "1" });

                return definitions;
            }
        }


        public TerrainTemplateShaderGenerator(GameCacheContext cacheContext, TemplateShaderGenerator.Drawmode drawmode, Int32[] args, Int32 arg_pos = 0) : base(
                drawmode,
                (Blending)GetNextTemplateArg(args, ref arg_pos),
                (Environment_Map)GetNextTemplateArg(args, ref arg_pos),
                (Material_0)GetNextTemplateArg(args, ref arg_pos),
                (Material_1)GetNextTemplateArg(args, ref arg_pos),
                (Material_2)GetNextTemplateArg(args, ref arg_pos),
                (Material_3)GetNextTemplateArg(args, ref arg_pos))
        {
            this.CacheContext = cacheContext;
        }

        #region Implemented Features Check

        protected override MultiValueDictionary<Type, object> ImplementedEnums => new MultiValueDictionary<Type, object>
        {
            {typeof(Blending), Blending.Morph },
            {typeof(Material_0), Material_0.Off },
            {typeof(Material_0), Material_0.Diffuse_Only },
            {typeof(Material_0), Material_0.Diffuse_Plus_Specular },
            {typeof(Material_1), Material_1.Off },
            {typeof(Material_1), Material_1.Diffuse_Only },
            {typeof(Material_1), Material_1.Diffuse_Plus_Specular },
            {typeof(Material_2), Material_2.Off },
            {typeof(Material_2), Material_2.Diffuse_Only },
            {typeof(Material_2), Material_2.Diffuse_Plus_Specular },
            {typeof(Material_3), Material_3.Off },
            {typeof(Material_3), Material_3.Diffuse_Only },
            {typeof(Material_3), Material_3.Diffuse_Plus_Specular },
        };

        #endregion

        #region Uniforms/Registers

        protected override MultiValueDictionary<object, TemplateParameter> Uniforms => use_detail_bumps ? Uniforms_With_Detail_Bump : Uniforms_No_Detail_Bump;

        protected MultiValueDictionary<object, TemplateParameter> Uniforms_With_Detail_Bump => new MultiValueDictionary<object, TemplateParameter>
        {
            {Blending.Morph, new TemplateParameter(typeof(Blending), "debug_tint", ShaderParameter.RType.Vector) },
            {Blending.Morph, new TemplateParameter(typeof(Blending), "blend_map_xform", ShaderParameter.RType.Vector) },
            {Blending.Morph, new TemplateParameter(typeof(Blending), "global_albedo_tint", ShaderParameter.RType.Vector) },
            {Blending.Morph, new TemplateParameter(typeof(Blending), "blend_map", ShaderParameter.RType.Sampler) },
            {Blending.Morph, new TemplateParameter(typeof(Blending), "unknown_map0", ShaderParameter.RType.Sampler) { Enabled = false } },

            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "base_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "detail_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "bump_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "detail_bump_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "base_map_m_0_xform", ShaderParameter.RType.Vector) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "detail_map_m_0_xform", ShaderParameter.RType.Vector) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "bump_map_m_0_xform", ShaderParameter.RType.Vector) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "detail_bump_m_0_xform", ShaderParameter.RType.Vector) },

            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "base_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "detail_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "bump_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "detail_bump_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "base_map_m_0_xform", ShaderParameter.RType.Vector) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "detail_map_m_0_xform", ShaderParameter.RType.Vector) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "bump_map_m_0_xform", ShaderParameter.RType.Vector) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "detail_bump_m_0_xform", ShaderParameter.RType.Vector) },

            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "base_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "detail_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "bump_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "detail_bump_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "base_map_m_1_xform", ShaderParameter.RType.Vector) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "detail_map_m_1_xform", ShaderParameter.RType.Vector) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "bump_map_m_1_xform", ShaderParameter.RType.Vector) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "detail_bump_m_1_xform", ShaderParameter.RType.Vector) },

            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "base_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "detail_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "bump_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "detail_bump_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "base_map_m_1_xform", ShaderParameter.RType.Vector) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "detail_map_m_1_xform", ShaderParameter.RType.Vector) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "bump_map_m_1_xform", ShaderParameter.RType.Vector) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "detail_bump_m_1_xform", ShaderParameter.RType.Vector) },

            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "base_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "detail_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "bump_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "detail_bump_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "base_map_m_2_xform", ShaderParameter.RType.Vector) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "detail_map_m_2_xform", ShaderParameter.RType.Vector) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "bump_map_m_2_xform", ShaderParameter.RType.Vector) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "detail_bump_m_2_xform", ShaderParameter.RType.Vector) },

            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "base_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "detail_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "bump_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "detail_bump_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "base_map_m_2_xform", ShaderParameter.RType.Vector) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "detail_map_m_2_xform", ShaderParameter.RType.Vector) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "bump_map_m_2_xform", ShaderParameter.RType.Vector) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "detail_bump_m_2_xform", ShaderParameter.RType.Vector) },

            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "base_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "detail_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "bump_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "detail_bump_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "base_map_m_3_xform", ShaderParameter.RType.Vector) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "detail_map_m_3_xform", ShaderParameter.RType.Vector) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "bump_map_m_3_xform", ShaderParameter.RType.Vector) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "detail_bump_m_3_xform", ShaderParameter.RType.Vector) },

            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "base_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "detail_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "bump_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "detail_bump_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "base_map_m_3_xform", ShaderParameter.RType.Vector) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "detail_map_m_3_xform", ShaderParameter.RType.Vector) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "bump_map_m_3_xform", ShaderParameter.RType.Vector) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "detail_bump_m_3_xform", ShaderParameter.RType.Vector) },
        };

        private MultiValueDictionary<object, TemplateParameter> Uniforms_No_Detail_Bump { get; set; } = new MultiValueDictionary<object, TemplateParameter>
        {
            {Blending.Morph, new TemplateParameter(typeof(Blending), "debug_tint", ShaderParameter.RType.Vector) },
            {Blending.Morph, new TemplateParameter(typeof(Blending), "blend_map_xform", ShaderParameter.RType.Vector) },
            {Blending.Morph, new TemplateParameter(typeof(Blending), "global_albedo_tint", ShaderParameter.RType.Vector) },
            {Blending.Morph, new TemplateParameter(typeof(Blending), "blend_map", ShaderParameter.RType.Sampler) },
            {Blending.Morph, new TemplateParameter(typeof(Blending), "unknown_map0", ShaderParameter.RType.Sampler) { Enabled = false } },

            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "base_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "detail_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "bump_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "base_map_m_0_xform", ShaderParameter.RType.Vector) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "detail_map_m_0_xform", ShaderParameter.RType.Vector) },
            {Material_0.Diffuse_Only, new TemplateParameter(typeof(Material_0), "bump_map_m_0_xform", ShaderParameter.RType.Vector) },

            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "base_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "detail_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "bump_map_m_0", ShaderParameter.RType.Sampler) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "base_map_m_0_xform", ShaderParameter.RType.Vector) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "detail_map_m_0_xform", ShaderParameter.RType.Vector) },
            {Material_0.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_0), "bump_map_m_0_xform", ShaderParameter.RType.Vector) },

            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "base_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "detail_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "bump_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "base_map_m_1_xform", ShaderParameter.RType.Vector) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "detail_map_m_1_xform", ShaderParameter.RType.Vector) },
            {Material_1.Diffuse_Only, new TemplateParameter(typeof(Material_1), "bump_map_m_1_xform", ShaderParameter.RType.Vector) },

            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "base_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "detail_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "bump_map_m_1", ShaderParameter.RType.Sampler) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "base_map_m_1_xform", ShaderParameter.RType.Vector) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "detail_map_m_1_xform", ShaderParameter.RType.Vector) },
            {Material_1.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_1), "bump_map_m_1_xform", ShaderParameter.RType.Vector) },

            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "base_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "detail_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "bump_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "base_map_m_2_xform", ShaderParameter.RType.Vector) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "detail_map_m_2_xform", ShaderParameter.RType.Vector) },
            {Material_2.Diffuse_Only, new TemplateParameter(typeof(Material_2), "bump_map_m_2_xform", ShaderParameter.RType.Vector) },

            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "base_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "detail_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "bump_map_m_2", ShaderParameter.RType.Sampler) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "base_map_m_2_xform", ShaderParameter.RType.Vector) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "detail_map_m_2_xform", ShaderParameter.RType.Vector) },
            {Material_2.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_2), "bump_map_m_2_xform", ShaderParameter.RType.Vector) },

            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "base_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "detail_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "bump_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "base_map_m_3_xform", ShaderParameter.RType.Vector) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "detail_map_m_3_xform", ShaderParameter.RType.Vector) },
            {Material_3.Diffuse_Only, new TemplateParameter(typeof(Material_3), "bump_map_m_3_xform", ShaderParameter.RType.Vector) },

            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "base_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "detail_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "bump_map_m_3", ShaderParameter.RType.Sampler) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "base_map_m_3_xform", ShaderParameter.RType.Vector) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "detail_map_m_3_xform", ShaderParameter.RType.Vector) },
            {Material_3.Diffuse_Plus_Specular, new TemplateParameter(typeof(Material_3), "bump_map_m_3_xform", ShaderParameter.RType.Vector) },
        };

        #endregion

        #region Enums

        public Blending blending => (Blending)EnumValues[0];
        public Environment_Map environment_map => (Environment_Map)EnumValues[1];
        public Material_0 material_0 => (Material_0)EnumValues[2];
        public Material_1 material_1 => (Material_1)EnumValues[3];
        public Material_2 material_2 => (Material_2)EnumValues[4];
        public Material_3 material_3 => (Material_3)EnumValues[5];

        public enum Blending
        {
            Morph,
            Dynamic_Morph
        }

        public enum Environment_Map
        {
            None,
            Per_Pixel,
            Dynamic
        }

        public enum Material_0
        {
            Diffuse_Only,
            Diffuse_Plus_Specular,
            Off
        }

        public enum Material_1
        {
            Diffuse_Only,
            Diffuse_Plus_Specular,
            Off
        }

        public enum Material_2
        {
            Diffuse_Only,
            Diffuse_Plus_Specular,
            Off
        }

        public enum Material_3
        {
            Off,
            Diffuse_Only, // four_material_shaders_disable_detail_bump
            Diffuse_Plus_Specular, //(four_material_shaders_disable_detail_bump),
        }

        #endregion
    }
}
