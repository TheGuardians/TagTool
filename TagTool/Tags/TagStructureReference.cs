using TagTool.Serialization;

namespace TagTool.Tags
{
    /// <summary>
    /// Points to a D3D-related object.
    /// </summary>
    [TagStructure(Size = 0xC)]
    public class TagStructureReference<TDefinition>
    {
        /// <summary>
        /// The definition data for the object.
        /// </summary>
        [TagField(Pointer = true)]
        public TDefinition Definition;

        /// <summary>
        /// The address of the object in memory.
        /// This should be set to 0 because it will be used at runtime.
        /// </summary>
        public uint RuntimeAddress;

        /// <summary>
        /// The address of the structure definition in memory.
        /// This should be set to 0 because it will be used at runtime.
        /// </summary>
        public uint DefinitionAddress;
    }
}
