using System.Collections.Generic;
using TagTool.IO;

namespace TagTool.Audio.Converter
{

    public class ASFHeader : SoundFile
    {
        public int TotalSize;

        ASFHeaderObject HeaderObject;
        ASFFilePropertiesHeaderObject FilePropertiesHeader;


        public ASFHeader()
        {
            HeaderObject = new ASFHeaderObject();
            FilePropertiesHeader = new ASFFilePropertiesHeaderObject();
        }

        override public void Write(EndianWriter writer)
        {
            HeaderObject.WriteChunk(writer);
            FilePropertiesHeader.WriteChunk(writer);
        }

        override public void Read(EndianReader reader)
        {
            HeaderObject.ReadChunk(reader);
            FilePropertiesHeader.ReadChunk(reader);
        }


    }

    abstract class ASFHeaderChunk : HeaderChunk
    {
        public byte[] GUID;
        public long HeaderSize;

        public static bool VerifyGUID(byte[] GUID1, byte[] GUID2)
        {
            if (GUID1.Length != 16 || GUID2.Length != 16)
                return false;

            for(int i = 0; i < 16; i++)
            {
                if (GUID2[i] != GUID1[i])
                    return false;
            }
            return true;
        }

        public abstract bool VerifyHeader(byte[] inputGUID);
    }

    class ASFHeaderObject : ASFHeaderChunk
    {
        public int ObjectCount;
        public byte Reserved1 = 0x01;
        public byte Reserved2 = 0x02;

        public ASFHeaderObject()
        {
            GUID = new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C };
        }

        public void SetObjectCount(int objectCount) => ObjectCount = objectCount;

        public override bool VerifyHeader(byte[] inputGUID)
        {
            return VerifyGUID(GUID, inputGUID) && HeaderSize >= 30 && Reserved1 == 0x01 && Reserved2 == 0x02;
        }

        public override void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.LittleEndian;
            var startPosition = reader.BaseStream.Position;

            var fileGUID = reader.ReadBytes(16);

            HeaderSize = reader.ReadInt64();
            ObjectCount = reader.ReadInt32();
            Reserved1 = reader.ReadByte();
            Reserved2 = reader.ReadByte();

            reader.BaseStream.Position = startPosition + HeaderSize;

            VerifyHeader(fileGUID);
        }

        public override void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            var startPosition = writer.BaseStream.Position;

            VerifyHeader(GUID);

            writer.Write(GUID);
            writer.Write(HeaderSize);
            writer.Write(ObjectCount);
            writer.Write(Reserved1);
            writer.Write(Reserved2);

            writer.BaseStream.Position = startPosition + HeaderSize;
        }
    }

    class ASFFilePropertiesHeaderObject : ASFHeaderChunk
    {
        public byte[] FileID;
        public long FileSize;
        public long CreationDate;
        public long DataPacketsCount;
        public long PlayDuration;
        public long SendDuration;
        public long Preroll;
        public int Flags;
        public int MinimumDataPacketSize;
        public int MaximumDataPacketSize;
        public int MaximumBitrate;

        public ASFFilePropertiesHeaderObject()
        {
            GUID = new byte[] { 0xA1, 0xDC, 0xAB, 0x8C, 0x47, 0xA9, 0xCF, 0x11, 0x8E, 0xE4, 0x00, 0xC0, 0x0C, 0x20, 0x53, 0x65 };
        }

        public override bool VerifyHeader(byte[] inputGUID)
        {
            return VerifyGUID(GUID, inputGUID) && HeaderSize >= 104 && FileID.Length == 16;
        }

        public override void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.LittleEndian;
            var startPosition = reader.BaseStream.Position;

            var fileGUID = reader.ReadBytes(16);

            HeaderSize = reader.ReadInt64();
            FileID = reader.ReadBytes(16);
            FileSize = reader.ReadInt64();
            CreationDate = reader.ReadInt64();
            DataPacketsCount = reader.ReadInt64();
            PlayDuration = reader.ReadInt64();
            SendDuration = reader.ReadInt64();
            Preroll = reader.ReadInt64();
            Flags = reader.ReadInt32();
            MinimumDataPacketSize = reader.ReadInt32();
            MaximumDataPacketSize = reader.ReadInt32();
            MaximumBitrate = reader.ReadInt32();

            VerifyHeader(fileGUID);

            reader.BaseStream.Position = startPosition + HeaderSize;
        }

        public override void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            var startPosition = writer.BaseStream.Position;

            VerifyHeader(GUID);

            writer.Write(GUID);
            writer.Write(HeaderSize);
            writer.Write(FileID);
            writer.Write(FileSize);
            writer.Write(CreationDate);
            writer.Write(DataPacketsCount);
            writer.Write(PlayDuration);
            writer.Write(SendDuration);
            writer.Write(Preroll);
            writer.Write(Flags);
            writer.Write(MinimumDataPacketSize);
            writer.Write(MaximumDataPacketSize);
            writer.Write(MaximumBitrate);

            writer.BaseStream.Position = startPosition + HeaderSize;
        }
    }

    class ASFHeaderExtensionObject : ASFHeaderChunk
    {
        private readonly byte[] Reserved1GUID = new byte[]{0x11, 0xD2, 0xD3, 0xAB, 0xBA, 0xA9, 0xCF, 0x11, 0x8E, 0xE6, 0x00, 0xC0, 0x0C, 0x20, 0x53, 0x65};

        public byte[] Reserved1;
        public short Reserved2;
        public int ExtensionDataSize;
        public byte[] ExtensionData;

        public ASFHeaderExtensionObject()
        {
            GUID = new byte[] { 0xB5, 0x03, 0xBF, 0x5F, 0x2E, 0xA9, 0xCF, 0x11, 0x8E, 0xE3, 0x00, 0xC0, 0x0C, 0x20, 0x53, 0x65 };
        }

        public override bool VerifyHeader(byte[] inputGUID)
        {
            return VerifyGUID(GUID, inputGUID) && HeaderSize >= 46 && Reserved2 == 0x6 && VerifyGUID(Reserved1GUID, Reserved1);
        }

        public override void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.LittleEndian;
            var startPosition = reader.BaseStream.Position;

            var fileGUID = reader.ReadBytes(16);

            HeaderSize = reader.ReadInt64();
            Reserved1 = reader.ReadBytes(16);
            Reserved2 = reader.ReadInt16();
            ExtensionDataSize = reader.ReadInt32();
            ExtensionData = reader.ReadBytes(ExtensionDataSize);

            reader.BaseStream.Position = startPosition + HeaderSize;
            VerifyHeader(fileGUID);
        }

        public override void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            var startPosition = writer.BaseStream.Position;

            VerifyHeader(GUID);

            writer.Write(GUID);
            writer.Write(HeaderSize);
            writer.Write(Reserved1);
            writer.Write(Reserved2);
            writer.Write(ExtensionDataSize);
            writer.Write(ExtensionData);

            writer.BaseStream.Position = startPosition + HeaderSize;
        }
    }


    class ASFExtentedContentDescriptionObject : ASFHeaderChunk
    {
        
        public ASFExtentedContentDescriptionObject()
        {
            GUID = new byte[] { 0x40, 0xA4, 0xD0, 0xD2, 0x07, 0xE3, 0xD2, 0x11, 0x97, 0xF0, 0x00, 0xA0, 0xC9, 0x5E, 0xA8, 0x50 };
        }

        public override bool VerifyHeader(byte[] inputGUID)
        {
            return VerifyGUID(GUID, inputGUID) && HeaderSize >= 26;
        }

        public override void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.LittleEndian;
            var startPosition = reader.BaseStream.Position;

            var fileGUID = reader.ReadBytes(16);


            reader.BaseStream.Position = startPosition + HeaderSize;
            VerifyHeader(fileGUID);
        }

        public override void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            var startPosition = writer.BaseStream.Position;

            VerifyHeader(GUID);

            writer.Write(GUID);
            writer.Write(HeaderSize);

            writer.BaseStream.Position = startPosition + HeaderSize;
        }
    }

    class ASFCodecListObject : ASFHeaderChunk
    {

        public ASFCodecListObject()
        {
            GUID = new byte[] { 0x40, 0x52, 0xD1, 0x86, 0x1D, 0x31, 0xD0, 0x11, 0xA3, 0xA4, 0x00, 0xA0, 0xC9, 0x03, 0x48, 0xF6 };
        }

        public override bool VerifyHeader(byte[] inputGUID)
        {
            return VerifyGUID(GUID, inputGUID) && HeaderSize >= 44;
        }

        public override void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.LittleEndian;
            var startPosition = reader.BaseStream.Position;

            var fileGUID = reader.ReadBytes(16);


            reader.BaseStream.Position = startPosition + HeaderSize;
            VerifyHeader(fileGUID);
        }

        public override void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            var startPosition = writer.BaseStream.Position;

            VerifyHeader(GUID);

            writer.Write(GUID);
            writer.Write(HeaderSize);

            writer.BaseStream.Position = startPosition + HeaderSize;
        }
    }

    class ASFStreamPropertiesObject : ASFHeaderChunk
    {

        public ASFStreamPropertiesObject()
        {
            GUID = new byte[] { 0x91, 0x07, 0xDC, 0xB7, 0xB7, 0xA9, 0xCF, 0x11, 0x8E, 0xE6, 0x00, 0xC0, 0x0C, 0x20, 0x53, 0x65 };
        }

        public override bool VerifyHeader(byte[] inputGUID)
        {
            return VerifyGUID(GUID, inputGUID) && HeaderSize >= 78;
        }

        public override void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.LittleEndian;
            var startPosition = reader.BaseStream.Position;

            var fileGUID = reader.ReadBytes(16);


            reader.BaseStream.Position = startPosition + HeaderSize;
            VerifyHeader(fileGUID);
        }

        public override void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            var startPosition = writer.BaseStream.Position;

            VerifyHeader(GUID);

            writer.Write(GUID);
            writer.Write(HeaderSize);

            writer.BaseStream.Position = startPosition + HeaderSize;
        }
    }

    class ASFStreamBitratePropertiesObject : ASFHeaderChunk
    {

        public ASFStreamBitratePropertiesObject()
        {
            GUID = new byte[] { 0xCE, 0x75, 0xF8, 0x7B, 0x8D, 0x46, 0xD1, 0x11, 0x8D, 0x82, 0x00, 0x60, 0x97, 0xC9, 0xA2, 0xB2 };
        }

        public override bool VerifyHeader(byte[] inputGUID)
        {
            return VerifyGUID(GUID, inputGUID) && HeaderSize >= 26;
        }

        public override void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.LittleEndian;
            var startPosition = reader.BaseStream.Position;

            var fileGUID = reader.ReadBytes(16);


            reader.BaseStream.Position = startPosition + HeaderSize;
            VerifyHeader(fileGUID);
        }

        public override void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            var startPosition = writer.BaseStream.Position;

            VerifyHeader(GUID);

            writer.Write(GUID);
            writer.Write(HeaderSize);

            writer.BaseStream.Position = startPosition + HeaderSize;
        }
    }

    class ASFDataObject : ASFHeaderChunk
    {

        public ASFDataObject()
        {
            GUID = new byte[] { 0x36, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C };
        }

        public override bool VerifyHeader(byte[] inputGUID)
        {
            return VerifyGUID(GUID, inputGUID) && HeaderSize >= 50;
        }

        public override void ReadChunk(EndianReader reader)
        {
            reader.Format = EndianFormat.LittleEndian;
            var startPosition = reader.BaseStream.Position;

            var fileGUID = reader.ReadBytes(16);


            reader.BaseStream.Position = startPosition + HeaderSize;
            VerifyHeader(fileGUID);
        }

        public override void WriteChunk(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            var startPosition = writer.BaseStream.Position;

            VerifyHeader(GUID);

            writer.Write(GUID);
            writer.Write(HeaderSize);

            writer.BaseStream.Position = startPosition + HeaderSize;
        }
    }
}
