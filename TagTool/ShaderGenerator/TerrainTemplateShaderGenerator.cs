using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderGenerator
{
    public class TerrainTemplateShaderGenerator : IShaderGenerator
    {
        #region Enums

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
