using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderGenerator
{
    public class DecalTemplateShaderGenerator
    {
        #region Enums

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
