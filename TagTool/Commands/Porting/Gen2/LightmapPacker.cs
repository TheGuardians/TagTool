using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TagTool.Ai;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.IO;
using TagTool.Lighting;

namespace TagTool.Commands.Porting.Gen2
{
    public class LightmapPacker
    {
        private int[] SizeClasses = new int[] { 32,64,128,256,512,1024,2048,4096};
        private List<BitmapContainer> bitmapContainers = new List<BitmapContainer>();
        private int lightmapLayer = 0;
        private int targetLightmapSize = 0;
        public Dictionary<int, int[]> clusterBitmapOffsets = new Dictionary<int, int[]>();
        private float[] MaxLs = new float[9];
        private List<int[]> OffsetWrites = new List<int[]>();

        public bool AddBitmap(int width, int height, int cluster_index, List<RealRgbColor[]> coefficients)
        {
            var newBitmap = new BitmapContainer();
            newBitmap.Size = new int[] { width, height };
            newBitmap.ClusterIndex = cluster_index;
            newBitmap.coefficients = coefficients;
            bitmapContainers.Add(newBitmap);
            return true;
        }

        public CachedLightmap Pack()
        {
            NestBitmaps();
            MemoryStream layeredLightmapData = new MemoryStream();
            for(var i = 0; i < 4; i++)
            {
                lightmapLayer = i;
                BitmapContainer lightmapBitmap = new BitmapContainer
                {
                    Size = new int[] { targetLightmapSize, targetLightmapSize },
                    ChildBitmaps = bitmapContainers,
                };
                byte[] lightmapLayerData = new byte[targetLightmapSize * targetLightmapSize * 4];
                MemoryStream layerStream = new MemoryStream(lightmapLayerData);
                EndianWriter writer = new EndianWriter(layerStream);
                WriteNestedBitmap(writer, lightmapBitmap, new int[] { 0, 0 }, false);
                layerStream.WriteTo(layeredLightmapData);
                lightmapLayerData = new byte[targetLightmapSize * targetLightmapSize * 4];
                layerStream = new MemoryStream(lightmapLayerData);
                writer = new EndianWriter(layerStream);
                WriteNestedBitmap(writer, lightmapBitmap, new int[] { 0, 0 }, true);
                layerStream.WriteTo(layeredLightmapData);
            }
            var result = new CachedLightmap
            {
                MaxLs = MaxLs,
                Height = targetLightmapSize,
                Width = targetLightmapSize,
                LinearSH = layeredLightmapData.ToArray()
            };

            return result;
        }
        private void WriteNestedBitmap(EndianWriter writer, BitmapContainer bitmap, int[] offset, bool useDummyData)
        {
            List<int> bitmapSlots = new List<int> { 0, 0, 0, 0 };           
            if(bitmap.ChildBitmaps.Count > 0)
            {
                bitmap.ChildBitmaps = bitmap.ChildBitmaps.OrderByDescending(c => c.GetBitmapShape()).ToList();
                foreach (var nested in bitmap.ChildBitmaps)
                {
                    int targetSlot = bitmapSlots.FindIndex(s => s == 0);
                    int[] newOffset = new int[2];
                    switch (targetSlot)
                    {
                        case 0:
                            newOffset = offset;
                            break;
                        case 1:
                            newOffset = new int[] { offset[0] + (bitmap.Width() / 2), offset[1] };
                            break;
                        case 2:
                            newOffset = new int[] { offset[0], offset[1] + (bitmap.Height() / 2) };
                            break;
                        case 3:
                            newOffset = new int[] { offset[0] + (bitmap.Width() / 2), offset[1] + (bitmap.Height() / 2) };
                            break;
                    }
                    WriteNestedBitmap(writer, nested, newOffset, useDummyData);
                    switch (nested.GetBitmapShape())
                    {
                        case BitmapContainer.BitmapShape.Square:
                            bitmapSlots[targetSlot] = 1;
                            break;
                        case BitmapContainer.BitmapShape.WideBoi:
                            bitmapSlots[targetSlot] = 1;
                            bitmapSlots[targetSlot + 1] = 1;
                            break;
                        case BitmapContainer.BitmapShape.LongBoi:
                            bitmapSlots[targetSlot] = 1;
                            bitmapSlots[targetSlot + 2] = 1;
                            break;
                    }
                }
            }
            //no child bitmaps, so this bitmap should be written out
            else
            {
                //seek to offset then write out line by line
                writer.BaseStream.Position = (offset[1] * targetLightmapSize + offset[0]) * 4;
                OffsetWrites.Add(offset);
                float MaxL = 0.0f;
                ArgbColor convertedColor = new ArgbColor();
                //layer 0
                for (var y = 0; y < bitmap.Height(); y++)
                {
                    for(var x = 0; x < bitmap.Width(); x++)
                    {
                        var val = bitmap.coefficients[lightmapLayer][bitmap.Width() * y + x];
                        //even layers get filled with dummy data
                        if (!useDummyData)
                        {
                            float L = (float)Math.Sqrt(val.Red * val.Red + val.Green * val.Green + val.Blue * val.Blue);
                            if (L > MaxL)
                                MaxL = L;
                            convertedColor = new ArgbColor(byte.MaxValue, (byte)(val.Red * 255),
                            (byte)(val.Green * 255), (byte)(val.Blue * 255));
                        }                          
                        else
                            convertedColor = CreateDummySHData(val);
                        writer.Write(convertedColor.GetValue());
                    }
                    writer.BaseStream.Position = ((offset[1] + y) * targetLightmapSize + offset[0]) * 4;
                }
                
                if (!useDummyData)
                {
                    MaxLs[lightmapLayer] = MaxL;
                    //store offset of final cluster bitmap write location
                    if(lightmapLayer == 0)
                        clusterBitmapOffsets.Add(bitmap.ClusterIndex, offset);
                }                    
            }                            
        }

        private bool NestBitmaps()
        {
            foreach (var container in bitmapContainers)
            {
                int minSize = container.Size.Min();
                int maxSize = container.Size.Max();
                if(minSize < SizeClasses[0] || maxSize > SizeClasses.Last() ||
                    maxSize > 2 * minSize)
                {
                    new TagToolError(CommandError.CustomMessage, $"Bitmap of dimensions {container.Size[0]},{container.Size[1]} not supported yet by packer! Aborting!");
                    return false;
                }
            }

            //overall goal size for the lightmap is 2x the dimension of the largest child bitmap, but may end up larger
            int maxChildSize = bitmapContainers.Select(b => b.MaxSize()).Max();

            for(var i = 0; i < SizeClasses.Length; i++)
            {              
                List<BitmapContainer> currentMinSize = bitmapContainers.Where(b => b.MinSize() == SizeClasses[i]).ToList();
                //if there are more than four of the current size, we need to upgrade a size further for the total lightmap
                if (SizeClasses[i] >= maxChildSize && currentMinSize.Count <= 4)
                {
                    targetLightmapSize = SizeClasses[i + 1];
                    break;
                }
                bitmapContainers.RemoveAll(b => currentMinSize.Contains(b));
                if (currentMinSize.Count == 0)
                    continue;
                Stack<BitmapContainer> squares = new Stack<BitmapContainer>(currentMinSize.Where(b => b.GetBitmapShape() == BitmapContainer.BitmapShape.Square).ToArray());
                Stack<BitmapContainer> longBois = new Stack<BitmapContainer>(currentMinSize.Where(b => b.GetBitmapShape() == BitmapContainer.BitmapShape.LongBoi).ToArray());
                Stack<BitmapContainer> wideBois = new Stack<BitmapContainer>(currentMinSize.Where(b => b.GetBitmapShape() == BitmapContainer.BitmapShape.WideBoi).ToArray());

                NestRectangleBitmaps(squares, longBois, SizeClasses[i + 1]);
                NestRectangleBitmaps(squares, wideBois, SizeClasses[i + 1]);
                while(squares.Count > 3)
                    bitmapContainers.Add(new BitmapContainer
                    {
                        Size = new int[] { SizeClasses[i + 1], SizeClasses[i + 1] },
                        ChildBitmaps = new List<BitmapContainer> { squares.Pop(), squares.Pop(), squares.Pop(), squares.Pop() }
                    });
                if(squares.Count > 0)
                    bitmapContainers.Add(new BitmapContainer
                    {
                        Size = new int[] { SizeClasses[i + 1], SizeClasses[i + 1] },
                        ChildBitmaps = squares.ToList()
                    });
            }
            return true;
        }

        private void NestRectangleBitmaps(Stack<BitmapContainer> squares, Stack<BitmapContainer> rectangles, int sizeClass)
        {
            while (rectangles.Count > 1)
            {
                bitmapContainers.Add(new BitmapContainer
                {
                    Size = new int[] { sizeClass, sizeClass },
                    ChildBitmaps = new List<BitmapContainer> { rectangles.Pop(), rectangles.Pop() }
                });
            }
            while (rectangles.Count > 0)
            {
                if (squares.Count > 1)
                    bitmapContainers.Add(new BitmapContainer
                    {
                        Size = new int[] { sizeClass, sizeClass },
                        ChildBitmaps = new List<BitmapContainer> { rectangles.Pop(), squares.Pop(), squares.Pop() }
                    });
                else if(squares.Count > 0)
                    bitmapContainers.Add(new BitmapContainer
                    {
                        Size = new int[] { sizeClass, sizeClass },
                        ChildBitmaps = new List<BitmapContainer> { rectangles.Pop(), squares.Pop() }
                    });
                else
                    bitmapContainers.Add(new BitmapContainer
                    {
                        Size = new int[] { sizeClass, sizeClass },
                        ChildBitmaps = new List<BitmapContainer> { rectangles.Pop() }
                    });
            }
        }

        private ArgbColor CreateDummySHData(RealRgbColor color)
        {
            double Red = (((color.Red + 1) / 2) + 0.5) - color.Red;
            double Green = (((color.Green + 1) / 2) + 0.5) - color.Green;
            double Blue = (((color.Blue + 1) / 2) + 0.5) - color.Blue;
            return new ArgbColor(byte.MaxValue, (byte)(Red * 255), (byte)(Green * 255), (byte)(Blue * 255));
        }
        public class BitmapContainer
        {
            public int[] Size = new int[2];
            public int Width() { return Size[0]; }
            public int Height() { return Size[1]; }
            public BitmapShape GetBitmapShape()
            {
                if (Size[0] == Size[1])
                    return BitmapShape.Square;
                else if (Size[0] > Size[1])
                    return BitmapShape.WideBoi;
                else
                    return BitmapShape.LongBoi;
            }
            public int MaxSize() { return Size.Max(); }
            public int MinSize() { return Size.Min(); }
            public int ClusterIndex;
            public List<RealRgbColor[]> coefficients = new List<RealRgbColor[]>();
            public List<BitmapContainer> ChildBitmaps = new List<BitmapContainer>();

            public enum BitmapShape
            {
                Square,
                WideBoi,
                LongBoi
            }
        }
    }
}
