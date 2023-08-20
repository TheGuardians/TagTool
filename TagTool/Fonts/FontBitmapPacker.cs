using System;
using System.Diagnostics;
using System.IO;

namespace TagTool.Fonts
{
    public static class FontBitmapPacker
    {
        // TODO: cleanup

        public static byte[] DecodeCharacterPixels(byte[] data)
        {
            return DecodeA4R4G4B4(UnpackCharacterPixels(data));
        }

        public static byte[] EncodeCharacterPixels(byte[] data)
        {
            return PackCharacterPixels(EncodeA4R4G4B4(data));
        }


        private static byte[] PackCharacterPixels(byte[] data)
        {
            var destStream = new MemoryStream();
            var writer = new BinaryWriter(destStream);

            ushort base_pixel = 0xFFF;
            int characters_remaining = data.Length / 2;
            int characters_encoded;
            int run_length_plus_one;
            bool v17;
            int run_length;
            int packed_size = 0;
            int src = 0;

            var ms = new MemoryStream(data);
            var reader = new BinaryReader(ms);
            var source_pixels = new ushort[data.Length];
            for (int i = 0; i < characters_remaining; i++)
                source_pixels[i] = reader.ReadUInt16();

            while (characters_remaining > 0)
            {
                characters_encoded = 0;
                run_length = GetRunLength(base_pixel, source_pixels, src, characters_remaining);
                if (characters_remaining <= 1)
                    run_length_plus_one = 0;
                else
                    run_length_plus_one = GetRunLength(base_pixel, source_pixels, src + 1, characters_remaining - 1);

                ushort v16 = EncodePixel(source_pixels[src]);

                v17 = ((source_pixels[src] & 0xFFF) - base_pixel) != 0;
                if (characters_remaining > 1 && !v17)
                    v17 = ((source_pixels[src + 1] & 0xFFF) - base_pixel) != 0;

                Debug.Assert(run_length <= characters_remaining);
                Debug.Assert(run_length_plus_one <= (characters_remaining - 1));

                if (v17)
                {
                    base_pixel = (ushort)(source_pixels[src] & 0xFFF);
                    packed_size += 3;

                    writer.Write((byte)0);
                    writer.Write((byte)((source_pixels[src] & 0xFF00) >> 8));
                    writer.Write((byte)source_pixels[src]);
                    characters_encoded = 1;
                }
                else if (run_length < 2)
                {
                    if (characters_remaining < 2 || run_length_plus_one > 1)
                    {
                        int v13 = 0;
                        if (run_length_plus_one >= 2)
                        {
                            if (EncodePixel(source_pixels[src + 1]) != 0)
                            {
                                Debug.Assert(EncodePixel(source_pixels[src + 1]) == 0x7);

                                switch (Math.Min(run_length_plus_one, 4))
                                {
                                    case 2:
                                        v13 = 3;
                                        break;
                                    case 3:
                                        v13 = 2;
                                        break;
                                    default:
                                        v13 = 1;
                                        break;
                                }
                            }
                            else
                            {
                                switch (Math.Min(run_length_plus_one, 5))
                                {
                                    case 2:
                                        v13 = 7;
                                        break;
                                    case 3:
                                        v13 = 6;
                                        break;
                                    case 4:
                                        v13 = 5;
                                        break;
                                    case 5:
                                        v13 = 4;
                                        break;
                                    default:
                                        throw new Exception("unreachable");
                                }
                            }
                        }
                        ++packed_size;
                        writer.Write((byte)((8 * (EncodePixel(source_pixels[src]) & 7)) | 0x80 | v13));
                        switch (v13)
                        {
                            case 0:
                                characters_encoded = 1;
                                break;
                            case 1:
                            case 5:
                                characters_encoded = 5;
                                break;
                            case 2:
                            case 6:
                                characters_encoded = 4;
                                break;
                            case 3:
                            case 7:
                                characters_encoded = 3;
                                break;
                            case 4:
                                characters_encoded = 6;
                                break;
                            default:
                                Debug.Fail("unreachable");
                                break;
                        }
                    }
                    else
                    {
                        ++packed_size;
                        int v4 = (8 * EncodePixel(source_pixels[src])) | 0xC0;
                        writer.Write((byte)(v4 | EncodePixel(source_pixels[src + 1])));
                        characters_encoded = 2;
                    }
                }
                else
                {
                    int v14 = v16 != 0 ? 1 : 0;
                    int v19;
                    if (run_length <= 63)
                        v19 = run_length;
                    else
                        v19 = 63;
                    ++packed_size;

                    writer.Write((byte)((v14 << 6) | v19));
                    characters_encoded = v19;
                }
                characters_remaining -= characters_encoded;
                src += characters_encoded;
            }

            writer.Flush();
            return destStream.ToArray();
        }

        private static byte[] UnpackCharacterPixels(byte[] data)
        {
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms);

            ushort base_pixel = 0xFFF;
            int unpacked_size = 0;

            for (int i = 0; i < data.Length;)
            {
                byte code = data[i];
                ushort decoded_pixel;

                int v14 = (code >> 6) & 3;
                if (v14 == 0 || v14 == 1)
                {
                    int size = code & 0x3F;
                    if (size != 0 || v14 != 0)
                    {
                        if (v14 != 0)
                            decoded_pixel = DecodePixel(base_pixel, 7);
                        else
                            decoded_pixel = DecodePixel(base_pixel, 0);

                        unpacked_size += size;
                        for (int j = 0; j < size; ++j)
                            writer.Write(decoded_pixel);
                        i++;
                    }
                    else
                    {
                        ++unpacked_size;
                        decoded_pixel = (ushort)(data[i + 1] << 8 | data[i + 2]);
                        writer.Write(decoded_pixel);
                        base_pixel = (ushort)(decoded_pixel & 0xFFF);
                        i += 3;
                    }
                }
                else if (v14 == 2)
                {
                    ++unpacked_size;
                    decoded_pixel = DecodePixel(base_pixel, (byte)((code >> 3) & 7));
                    writer.Write(decoded_pixel);

                    int size;
                    switch (code & 7)
                    {
                        case 0:
                            size = 0;
                            break;
                        case 1:
                            size = 4;
                            decoded_pixel = DecodePixel(base_pixel, 7);
                            break;
                        case 2:
                            size = 3;
                            decoded_pixel = DecodePixel(base_pixel, 7);
                            break;
                        case 3:
                            size = 2;
                            decoded_pixel = DecodePixel(base_pixel, 7);
                            break;
                        case 4:
                            size = 5;
                            decoded_pixel = DecodePixel(base_pixel, 0);
                            break;
                        case 5:
                            size = 4;
                            decoded_pixel = DecodePixel(base_pixel, 0);
                            break;
                        case 6:
                            size = 3;
                            decoded_pixel = DecodePixel(base_pixel, 0);
                            break;
                        case 7:
                            size = 2;
                            decoded_pixel = DecodePixel(base_pixel, 0);
                            break;
                        default:
                            throw new Exception("unreachable");
                    }

                    if (size != 0)
                    {
                        unpacked_size += size;
                        for (int j = 0; j < size; ++j)
                        {
                            writer.Write(decoded_pixel);
                        }
                    }
                    i++;
                }
                else
                {
                    unpacked_size += 2;
                    ushort a = DecodePixel(base_pixel, (byte)((code >> 3) & 7));
                    ushort b = DecodePixel(base_pixel, (byte)(code & 7));
                    writer.Write(a);
                    writer.Write(b);
                    i++;
                }
            }

            writer.Flush();
            return ms.ToArray();
        }

        private static ushort DecodePixel(ushort base_pixel, byte quantized_pixel)
        {
            return (ushort)((quantized_pixel << 13) | ((quantized_pixel & 1) << 12) | base_pixel);
        }

        private static byte EncodePixel(ushort pixel)
        {
            return (byte)((pixel >> 13) & 7);
        }

        private static int GetRunLength(ushort base_pixel, ushort[] source_pixels, int source_offset, long source_pixels_count)
        {
            int length = 0;
            byte encoded_pixel = EncodePixel(source_pixels[source_offset + length]);
            if (encoded_pixel == 0 || encoded_pixel == 7)
            {
                do
                {
                    if (encoded_pixel != EncodePixel(source_pixels[source_offset + length]))
                        break;
                    if ((source_pixels[source_offset + length] & 0xFFF) != base_pixel)
                        break;
                    ++length;
                } while (length != source_pixels_count);
            }
            return length;
        }

        private static byte[] DecodeA4R4G4B4(byte[] data)
        {
            byte[] buffer = new byte[data.Length * 2];
            int j = 0;
            for (int i = 0; i < data.Length; i += 2)
            {
                buffer[j++] = (byte)((data[i] & 0xF) | (((data[i]) & 0xF) << 4));
                buffer[j++] = (byte)((data[i] >> 4) | ((data[i] >> 4) << 4));
                buffer[j++] = (byte)((data[i + 1] & 0xF) | (((data[i + 1]) & 0xF) << 4));
                buffer[j++] = (byte)((data[i + 1] >> 4) | ((data[i + 1] >> 4) << 4));
            }
            return buffer;
        }

        private static byte[] EncodeA4R4G4B4(byte[] data)
        {
            byte[] buffer = new byte[data.Length / 2];
            int j = 0;
            for (int i = 0; i < data.Length; i += 4)
            {
                buffer[j++] = (byte)((data[i] >> 4) & 0xF | ((data[i + 1] >> 4) << 4));
                buffer[j++] = (byte)((data[i + 2] >> 4) | ((data[i + 3] >> 4) << 4));
            }
            return buffer;
        }
    }
}
