namespace TagTool.Audio
{
    /// <summary>
    /// Only PCM little endian, MP3 and FSB4 are supported by Halo Online
    /// </summary>
    public enum Compression : sbyte
    {
        PCM_BigEndian = 0x0,    //RAW PCM (big endian)
        XboxADPCM = 0x1,        //Xbox ADPCM
        IMAADPCM = 0x2,         //IMA APDCM
        PCM = 0x3,              //RAW PCM (little endian)
        WMA = 0x4,              //WMA (windows media audio)
        XMA = 0x7,              //XMA
        MP3 = 0x8,              //MP3
        FSB4 = 0x9,             //FMOD System Bank Type 4
        OGG = 0x20,             // OGG container, Vorbis encoding
    }
}