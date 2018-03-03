using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderGenerator
{
    public class WaterTemplateShaderGenerator
    {
        #region Enums

        public enum WaveShape
        {
            Default,
            None,
            Bump
        }

        public enum WaterColor
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
