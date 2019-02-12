namespace TagTool.Audio
{
    public enum Compression : sbyte
    {
        UnknownWTF = 0x1,       //Unknown Xbox format
        PCM = 0x3,              //RAW PCM
        XMA = 0x7,              //XMA
        MP3 = 0x8,              //MP3
        FSB4 = 0x9,             //FMOD System Bank Type 4
    }
}