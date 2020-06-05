using System;

namespace TagTool.Shaders.ShaderMatching
{
    public class RegisterID
    {
        public int RegisterIndex;
        public ShaderParameter.RType Type;

        public RegisterID(int registerIndex, ShaderParameter.RType type)
        {
            RegisterIndex = registerIndex;
            Type = type;
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
