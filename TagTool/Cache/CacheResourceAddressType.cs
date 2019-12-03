namespace TagTool.Cache
{
    /// <summary>
    /// Resource address types.
    /// </summary>
    public enum CacheResourceAddressType
    {
        /// <summary>
        /// The address is a memory address.
        /// </summary>
        Memory,

        /// <summary>
        /// The address points to a location in the resource definition data.
        /// </summary>
        Definition,

        /// <summary>
        /// The address points to a location in the raw resource data.
        /// </summary>
        Resource
    }
}