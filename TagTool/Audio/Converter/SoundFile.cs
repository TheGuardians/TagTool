using TagTool.IO;

namespace TagTool.Audio.Converter
{
    /// <summary>
    /// Prototype of a sound file header. Add fields as needed.
    /// </summary>
    public abstract class SoundFile
    {
        public int HeaderSize;
        public bool isValid = false;
        
        abstract public void Write(EndianWriter writer);

        abstract public void Read(EndianReader reader);

    }
}
