namespace TagTool.Lighting
{
    public unsafe class DxtCompression
    {
        // TODO: consolidate with BitmapDecoder

        public static void DecompressImage(float* rgba, int width, int height, byte* blocks)
        {
            float* targetRgba = stackalloc float[4 * 16];

            for (int y = 0; y < height; y += 4)
            {
                // initialise the block input
                byte* sourceBlock = blocks;
                int bytesPerBlock = 16;
                sourceBlock += ((y / 4) * ((width + 3) / 4)) * bytesPerBlock;

                for (int x = 0; x < width; x += 4)
                {
                    // decompress the block
                    Decompress(targetRgba, sourceBlock, false);

                    // write the decompressed pixels to the correct image locations
                    float* sourcePixel = targetRgba;
                    for (int py = 0; py < 4; ++py)
                    {
                        for (int px = 0; px < 4; ++px)
                        {
                            // get the target location
                            int sx = x + px;
                            int sy = y + py;

                            // write if we're in the image
                            if (sx < width && sy < height)
                            {
                                // copy the rgba value
                                float* targetPixel = rgba + 4 * (width * sy + sx);
                                for (int i = 0; i < 4; i++)
                                    targetPixel[i] = sourcePixel[i];
                            }

                            // advance to the next pixel
                            sourcePixel += 4;
                        }
                    }

                    // advance
                    sourceBlock += bytesPerBlock;
                }
            }
        }

        public static void Decompress(float* rgba, byte* block, bool isDxt1)
        {
            byte* alphaBlock = block;
            byte* colourBlock = &block[8];
            DecompressColor(rgba, colourBlock, isDxt1);
            DecompressAlphaDxt5(rgba, alphaBlock);
        }

        public static void DecompressAlphaDxt5(float* rgba, byte* bytes)
        {
            // get the two alpha values
            int alpha0 = bytes[0];
            int alpha1 = bytes[1];

            // compare the values to build the codebook
            byte* codes = stackalloc byte[8];
            codes[0] = (byte)alpha0;
            codes[1] = (byte)alpha1;
            if (alpha0 <= alpha1)
            {
                // use 5-alpha codebook
                for (int i = 1; i < 5; ++i)
                    codes[1 + i] = (byte)(((5 - i) * alpha0 + i * alpha1) / 5);
                codes[6] = 0;
                codes[7] = 255;
            }
            else
            {
                // use 7-alpha codebook
                for (int i = 1; i < 7; ++i)
                    codes[1 + i] = (byte)(((7 - i) * alpha0 + i * alpha1) / 7);
            }

            // decode the indices
            byte* indices = stackalloc byte[16];
            byte* src = bytes + 2;
            byte* dest = indices;
            for (int i = 0; i < 2; ++i)
            {
                // grab 3 bytes
                int value = 0;
                for (int j = 0; j < 3; ++j)
                {
                    int b = *src++;
                    value |= (b << 8 * j);
                }

                // unpack 8 3-bit values from it
                for (int j = 0; j < 8; ++j)
                {
                    int index = (value >> 3 * j) & 0x7;
                    *dest++ = (byte)index;
                }
            }

            // write out the indexed codebook values
            for (int i = 0; i < 16; ++i)
                rgba[4 * i + 3] = (float)(codes[indices[i]] / 255.0f);
        }

        public static void DecompressColor(float* rgba, byte* block, bool isDxt1)
        {
            // get the block bytes
            byte* bytes = block;

            // unpack the endpoints
            byte* codes = stackalloc byte[16];
            int a = Unpack565(bytes, codes);
            int b = Unpack565(bytes + 2, codes + 4);

            // generate the midpoints
            for (int i = 0; i < 3; ++i)
            {
                int c = codes[i];
                int d = codes[4 + i];

                if (isDxt1 && a <= b)
                {
                    codes[8 + i] = (byte)((c + d) / 2);
                    codes[12 + i] = 0;
                }
                else
                {
                    codes[8 + i] = (byte)((2 * c + d) / 3);
                    codes[12 + i] = (byte)((c + 2 * d) / 3);
                }
            }

            // fill in alpha for the intermediate values
            codes[8 + 3] = 255;
            codes[12 + 3] = (byte)((isDxt1 && a <= b) ? 0 : 255);

            // unpack the indices
            byte* indices = stackalloc byte[16];
            for (int i = 0; i < 4; ++i)
            {
                byte* ind = indices + 4 * i;
                byte packed = bytes[4 + i];

                ind[0] = (byte)(packed & 0x3);
                ind[1] = (byte)((packed >> 2) & 0x3);
                ind[2] = (byte)((packed >> 4) & 0x3);
                ind[3] = (byte)((packed >> 6) & 0x3);
            }

            // store out the colours
            for (int i = 0; i < 16; ++i)
            {
                byte offset = (byte)(4 * indices[i]);
                for (int j = 0; j < 4; ++j)
                    rgba[4 * i + j] = codes[offset + j] / 255.0f;
            }
        }

        public static int Unpack565(byte* packed, byte* color)
        {
            int value = packed[0] | (packed[1] << 8);
            byte r = (byte)((value >> 11) & 0x1f);
            byte g = (byte)((value >> 5) & 0x3f);
            byte b = (byte)(value & 0x1f);
            color[0] = (byte)((r << 3) | (r >> 2));
            color[1] = (byte)((g << 2) | (g >> 4));
            color[2] = (byte)((b << 3) | (b >> 2));
            color[3] = 255;
            return value;
        }
    }
}
