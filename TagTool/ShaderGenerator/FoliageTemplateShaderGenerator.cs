using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderGenerator
{
    class FoliageTemplateShaderGenerator : IShaderGenerator
    {
        #region Enums

        public enum Albedo
        {
            Default
        }

        public enum Alpha_Test
        {
            None,
            Simple
        }

        public enum Material_Model
        {
            Default
        }

        #endregion
    }
}
