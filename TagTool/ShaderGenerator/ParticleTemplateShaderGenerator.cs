using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderGenerator
{
    public class ParticleTemplateShaderGenerator
    {
        #region Enums

        public enum Albedo
        {
            Diffuse_Only,
            Diffuse_Plus_Billboard_Alpha,
            Palettized,
            Palettized_Plus_Billboard_Alpha,
            Diffuse_Plus_Sprite_Alpha,
            Palettized_Plus_Sprite_Alpha
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

        public enum Specialized_Rendering
        {
            None,
            Distortion,
            Distortion_Expensive,
            Distortion_Diffuse,
            Distortion_Expensive_Diffuse
        }

        public enum Lighting
        {
            None,
            Per_Vertex_Ravi_Order_3,
            Per_Vertex_Ravi_Order_0
        }

        public enum Render_Targets
        {
            LDR_and_HDR,
            LDR_Only
        }

        public enum Depth_Fade
        {
            Off,
            On
        }

        public enum Black_Point
        {
            Off,
            On
        }

        public enum Fog
        {
            Off,
            On
        }

        public enum Frame_Blend
        {
            Off,
            On
        }

        public enum Self_Illumination
        {
            None,
            Constant_Color
        }

        #endregion
    }
}
