namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "custom_object", Tag = "cobj", Size = 0x4)]
    public class CustomObject : GameObject
    {
        public CustomObjectTypeValue CustomObjectType;

        public enum CustomObjectTypeValue : int
        {
            Cube
        }
    }
}