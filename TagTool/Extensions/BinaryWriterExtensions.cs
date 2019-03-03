using TagTool.Common;
using TagTool.IO;
using System;
using System.IO;
using System.Linq;

namespace Sytem.IO
{
    public static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, short value, EndianFormat format)
        {
            if (format == EndianFormat.BigEndian)
                writer.Write(BitConverter.GetBytes(value).Reverse().ToArray());
            else
                writer.Write(value);
        }

        public static void Write(this BinaryWriter writer, ushort value, EndianFormat format)
        {
            if (format == EndianFormat.BigEndian)
                writer.Write(BitConverter.GetBytes(value).Reverse().ToArray());
            else
                writer.Write(value);
        }

        public static void Write(this BinaryWriter writer, Half value, EndianFormat format = EndianFormat.LittleEndian)
        {
            writer.Write(value.value, format);
        }

        public static void Write(this BinaryWriter writer, Point2d value, EndianFormat format = EndianFormat.LittleEndian)
        {
            writer.Write(value.X, format);
            writer.Write(value.Y, format);
        }

        public static void Write(this BinaryWriter writer, Rectangle2d value, EndianFormat format = EndianFormat.LittleEndian)
        {
            writer.Write(value.Top, format);
            writer.Write(value.Left, format);
            writer.Write(value.Bottom, format);
            writer.Write(value.Right, format);
        }

        public static void Write(this BinaryWriter writer, DatumIndex value, EndianFormat format = EndianFormat.LittleEndian)
        {
            writer.Write(value.Value, format);
        }
    }
}
