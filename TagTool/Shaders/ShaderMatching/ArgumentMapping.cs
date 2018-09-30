using System;

namespace TagTool.Shaders.ShaderMatching
{
    class ArgumentMapping
    {
        public string ParameterName;
        public int ShaderIndex = -1;
        public int RegisterIndex;
        public int EDRegisterIndex = -1;
        public int ArgumentIndex;
        public int ArgumentMappingsTagblockIndex;
        public TagTool.Shaders.ShaderParameter.RType RegisterType;
    }
}
