using System;

namespace TagTool.Shaders.ShaderMatching
{
    public class RegisterID
    {
        public int RegisterIndex;
        public ShaderParameter.RType Type;
        public int FunctionIndex;
        public int SourceIndex;

        public RegisterID(int registerIndex, ShaderParameter.RType type, int functionIndex = -1, int sourceIndex = -1)
        {
            RegisterIndex = registerIndex;
            Type = type;
            FunctionIndex = functionIndex;
            SourceIndex = sourceIndex;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && RegisterIndex == ((RegisterID)obj).RegisterIndex && Type == ((RegisterID)obj).Type)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return RegisterIndex.GetHashCode() + Type.GetHashCode();
        }
    }
}
