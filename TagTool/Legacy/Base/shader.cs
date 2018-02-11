using System.Collections.Generic;

namespace TagTool.Legacy
{
    public abstract class shader
    {
        public int BaseShaderTagID;
        public List<ShaderProperties> Properties;

        public shader()
        {
            Properties = new List<ShaderProperties>();
        }

        public abstract class ShaderProperties
        {
            public int TemplateTagID;
            public List<ShaderMap> ShaderMaps;
            public List<Tiling> Tilings;

            public ShaderProperties()
            {
                ShaderMaps = new List<ShaderMap>();
                Tilings = new List<Tiling>();
            }

            public abstract class ShaderMap
            {
                public int BitmapTagID;
                public int Type;
                public int TilingIndex;
            }

            public abstract class Tiling
            {
                public float UTiling;  // A colour
                public float VTiling;  // R colour
                public float Unknown0; // G colour
                public float Unknown1; // B colour
            }
        }
    }
}
