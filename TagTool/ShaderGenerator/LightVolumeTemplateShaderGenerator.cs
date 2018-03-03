using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderGenerator
{
    class LightVolumeTemplateShaderGenerator : IShaderGenerator
    {
        #region Enums

        public enum Albedo
        {
            Diffuse_Only
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

        public enum Fog
        {
            Off,
            On
        }

        #endregion
    }
}
