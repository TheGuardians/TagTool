using TagTool.Serialization;

namespace TagTool.Common
{
    /// <summary>
    /// Points to a D3D-related object.
    /// </summary>
    [TagStructure(Size = 0xC)]
    public class D3DPointer<TDefinition>
    {
        /// <summary>
        /// The definition data for the object.
        /// </summary>
        [TagField(Pointer = true)]
        public TDefinition Definition;

        /// <summary>
        /// The address of the object in memory.
        /// This should be set to 0 because it will be filled in by the game.
        /// </summary>
        public uint Address;

        public int UnusedC;
    }
}
