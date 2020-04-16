using System;

namespace TagTool.Shaders.ShaderMatching
{
    public class RegisterID
    {
        public string Name;
        public ShaderParameter.RType Type;

        public RegisterID(string name, ShaderParameter.RType type)
        {
            Name = name;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && Name == ((RegisterID)obj).Name && Type == ((RegisterID)obj).Type)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() + Type.GetHashCode();
        }
    }
}
