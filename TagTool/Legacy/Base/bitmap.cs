using TagTool.Bitmaps;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Legacy
{
    public abstract class bitmap
    {
        public List<Sequence> Sequences;
        public List<BitmapData> Bitmaps;
        public List<RawChunkA> RawChunkAs;
        public List<RawChunkB> RawChunkBs;

        public bitmap()
        {
            Sequences = new List<Sequence>();
            Bitmaps = new List<BitmapData>();
            RawChunkAs = new List<RawChunkA>();
            RawChunkBs = new List<RawChunkB>();
        }

        public abstract class Sequence
        {
            public string Name;
            public int FirstSubmapIndex;
            public int BitmapCount;
            public List<Sprite> Sprites;

            public Sequence()
            {
                Sprites = new List<Sprite>();
            }

            public abstract class Sprite
            {
                public int SubmapIndex;
                public float Left, Right, Top, Bottom;
                public RealQuaternion RegPoint;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public abstract class BitmapData
        {
            public string Class;
            public int Width, Height, Depth;
            public Bitmask Flags;
            public BitmapType Type;
            public BitmapFormat Format;
            public Bitmask MoreFlags;
            public int RegX, RegY;
            public int MipmapCount;
            public int InterleavedIndex;
            public int Index2;
            public int PixelsOffset;
            public int PixelsSize;

            public virtual int VirtualWidth
            {
                get
                {
                    int var;
                    switch (Format)
                    {
                        case BitmapFormat.A8:
                        case BitmapFormat.Y8:
                        case BitmapFormat.AY8:
                        case BitmapFormat.A8Y8:
                        case BitmapFormat.A8R8G8B8:
                        case BitmapFormat.A4R4G4B4:
                        case BitmapFormat.R5G6B5:
                            var = 32;
                            break;

                        default:
                            var = 128;
                            break;
                    }

                    return (Width % var == 0) ? Width : Width + (var - (Width % var));
                }
            }

            public virtual int VirtualHeight
            {
                get
                {
                    int var;
                    switch (Format)
                    {
                        case BitmapFormat.A8:
                        case BitmapFormat.Y8:
                        case BitmapFormat.AY8:
                        case BitmapFormat.A8Y8:
                        case BitmapFormat.A8R8G8B8:
                        case BitmapFormat.A4R4G4B4:
                        case BitmapFormat.R5G6B5:
                            var = 32;
                            break;

                        default:
                            var = 128;
                            break;
                    }

                    return (Height % var == 0) ? Height : Height + (var - (Height % var));
                }
            }

            public virtual int RawSize
            {
                get
                {
                    int size = 0;
                    switch (Format)
                    {
                        case BitmapFormat.Ctx1:
                        case BitmapFormat.Dxt1:
                        case BitmapFormat.Dxt3aMono:
                        case BitmapFormat.Dxt3aAlpha:
                        case BitmapFormat.Dxt5a:
                        case BitmapFormat.Dxt5aMono:
                        case BitmapFormat.Dxt5aAlpha:
                            size = VirtualWidth * VirtualHeight / 2;
                            break;
                        case BitmapFormat.A8:
                        case BitmapFormat.Y8:
                        case BitmapFormat.AY8:
                        case BitmapFormat.Dxt3:
                        case BitmapFormat.Dxt5:
                        case BitmapFormat.Dxn:
                        case BitmapFormat.DxnMonoAlpha:
                        case BitmapFormat.A4R4G4B4Font:
                            size = VirtualWidth * VirtualHeight;
                            break;
                        case BitmapFormat.A4R4G4B4:
                        case BitmapFormat.A1R5G5B5:
                        case BitmapFormat.A8Y8:
                        //case BitmapFormat.U8V8:
                        case BitmapFormat.R5G6B5:
                            size = VirtualWidth * VirtualHeight * 2;
                            break;
                        case BitmapFormat.A8R8G8B8:
                        case BitmapFormat.X8R8G8B8:
                            size = VirtualWidth * VirtualHeight * 4;
                            break;
                        default:
                            return 0;
                    }

                    if (Type == BitmapType.CubeMap)
                        size *= 6;

                    return size;
                }
            }

            public virtual int BlockSize
            {
                get
                {
                    switch (Format)
                    {
                        case BitmapFormat.Ctx1:
                        case BitmapFormat.Dxt1:
                        case BitmapFormat.Dxt3aMono:
                        case BitmapFormat.Dxt3aAlpha:
                        case BitmapFormat.Dxt5a:
                        case BitmapFormat.Dxt5aMono:
                        case BitmapFormat.Dxt5aAlpha:
                            return 2;

                        case BitmapFormat.A8:
                        case BitmapFormat.Y8:
                        case BitmapFormat.AY8:
                        case BitmapFormat.A4R4G4B4Font:
                            return 1;

                        case BitmapFormat.Dxt3:
                        case BitmapFormat.Dxt5:
                        case BitmapFormat.Dxn:
                        case BitmapFormat.DxnMonoAlpha:
                            return 2;
                        case BitmapFormat.A4R4G4B4:
                        case BitmapFormat.A1R5G5B5:
                        case BitmapFormat.A8Y8:
                        case BitmapFormat.R5G6B5:
                            return 2;
                        case BitmapFormat.A8R8G8B8:
                        case BitmapFormat.X8R8G8B8:
                            return 4;
                        default:
                            return 0;
                    }
                }
            }
        }

        public class RawChunkA
        {
            public int RawID;
        }

        public class RawChunkB
        {
            public int RawID;
        }
    }
}
