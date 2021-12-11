using System.IO;
using System.Linq;
using TagTool.Bitmaps;
using TagTool.Bitmaps.DDS;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Lighting
{
    public class CachedLightmap
    {
        public int Width;
        public int Height;
        public float[] MaxLs;
        public byte[] LinearSH;
        public byte[] Intensity;

        private static readonly string LinearSH_BitmapFilename = "linear_sh.dds";
        private static readonly string Intensity_BitmapFilename = "intensity.dds";
        private static readonly string Info_Filename = "info.txt";

        public bool Load(string path)
        {
            path = Path.GetFullPath(path);
            var linearSHPath = Path.Combine(path, LinearSH_BitmapFilename);
            var intensityPath = Path.Combine(path, Intensity_BitmapFilename);
            var infoPath = Path.Combine(path, Info_Filename);

            if (!File.Exists(infoPath))
                return false;

            var info = File.ReadAllLines(infoPath);
            using (var reader = File.OpenText(infoPath))
            {
                var line = reader.ReadLine();
                var dimensions = info[0].Split('x').Select(int.Parse).ToArray();
                Width = dimensions[0];
                Height = dimensions[1];

                MaxLs = new float[5];
                for (int i = 0; i < MaxLs.Length; i++)
                {
                    line = reader.ReadLine();
                    MaxLs[i] = float.Parse(line);
                }
            }

            var linearDDS = LoadDDS(linearSHPath);
            var intensityDDS = LoadDDS(intensityPath);
            LinearSH = linearDDS.BitmapData;
            Intensity = intensityDDS.BitmapData;

            return true;
        }

        public void Store(string path)
        {
            path = Path.GetFullPath(path);
            Directory.CreateDirectory(path);

            var linearSHBitmapPath = Path.Combine(path, LinearSH_BitmapFilename);
            var intensityBitmapPath = Path.Combine(path, Intensity_BitmapFilename);
            var infoPath = Path.Combine(path, Info_Filename);

            using (var writer = File.CreateText(infoPath))
            {
                writer.WriteLine($"{Width}x{Height}");
                for (int i = 0; i < 5; i++)
                    writer.WriteLine($"{MaxLs[i]}");
            }
            StoreDDS(linearSHBitmapPath, Width, Height, 8, BitmapType.Array, BitmapFormat.Dxt5, LinearSH);
            StoreDDS(intensityBitmapPath, Width, Height, 2, BitmapType.Array, BitmapFormat.Dxt5, Intensity);
        }

        private DDSFile LoadDDS(string path)
        {
            using (var reader = new EndianReader(File.OpenRead(path)))
            {
                var dds = new DDSFile();
                dds.Read(reader);
                return dds;
            }
        }

        private static void StoreDDS(string filePath, int width, int height, int depth, BitmapType type, BitmapFormat format, byte[] data)
        {
            var fi = new FileInfo(filePath);
            fi.Directory.Create();

            var bitmap = new BaseBitmap();
            bitmap.Width = width;
            bitmap.Height = height;
            bitmap.Depth = depth;
            bitmap.Type = type;
            bitmap.Format = format;
            bitmap.UpdateFormat(format);
            bitmap.Data = data;
            var dds = new DDSFile(bitmap);
            using (var writer = new EndianWriter(File.Create(fi.FullName)))
                dds.Write(writer);
        }

        public void ImportIntoLbsp(GameCacheHaloOnlineBase cache, Stream stream, ScenarioLightmapBspData Lbsp)
        {
            var linearSHBitmap = cache.Deserialize<Bitmap>(stream, Lbsp.LightmapSHCoefficientsBitmap);
            var intensityBitmap = cache.Deserialize<Bitmap>(stream, Lbsp.LightmapDominantLightDirectionBitmap);

            var linearSH = new BaseBitmap();
            linearSH.Type = BitmapType.Texture3D;
            linearSH.Data = LinearSH;
            linearSH.Width = Width;
            linearSH.Height = Height;
            linearSH.Depth = 8;
            linearSH.UpdateFormat(BitmapFormat.Dxt5);

            var intensity = new BaseBitmap();
            intensity.Type = BitmapType.Texture3D;
            intensity.Data = Intensity;
            intensity.Width = Width;
            intensity.Height = Height;
            intensity.Depth = 2;
            intensity.UpdateFormat(BitmapFormat.Dxt5);

            Lbsp.CoefficientsMapScale = new Common.LuminanceScale[9];
            for (int i = 0; i < 5; i++)
                Lbsp.CoefficientsMapScale[i] = new Common.LuminanceScale() { Scale = MaxLs[i] };

            ImportBitmap(cache, stream, Lbsp.LightmapSHCoefficientsBitmap, linearSHBitmap, 0, linearSH);
            ImportBitmap(cache, stream, Lbsp.LightmapDominantLightDirectionBitmap, intensityBitmap, 0, intensity);
        }

        private static void ImportBitmap(GameCacheHaloOnlineBase cache, Stream stream, CachedTag tag, Bitmap bitmap, int imageIndex, BaseBitmap bitmapImport)
        {
            BitmapTextureInteropResource resource = BitmapUtils.CreateEmptyBitmapTextureInteropResource();
            resource.Texture.Definition.PrimaryResourceData = new TagData(bitmapImport.Data);
            resource.Texture.Definition.Bitmap = BitmapUtils.CreateBitmapTextureInteropDefinition(bitmapImport);
            var reference = cache.ResourceCache.CreateBitmapResource(resource);
            bitmap.Images[imageIndex] = BitmapUtils.CreateBitmapImageFromResourceDefinition(resource.Texture.Definition.Bitmap);
            bitmap.HardwareTextures[imageIndex] = reference;
            cache.Serialize(stream, tag, bitmap);
        }
    }
}
