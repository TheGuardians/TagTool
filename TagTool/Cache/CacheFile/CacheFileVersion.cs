namespace TagTool.Cache
{
    public enum CacheFileVersion : int
    {
        Invalid = 0x0,
        HaloXbox = 0x5,
        HaloPC = 0x7,
        Halo2 = 0x8,
        Halo3Beta = 0x9,
        Halo3 = 0xB,
        HaloReach = 0xC,
        HaloOnline = 0x12,
        HaloCustomEdition = 0x261
    }
}