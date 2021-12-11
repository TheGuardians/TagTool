using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TagTool.Common;

namespace TagTool.Lighting
{
    public class LightmapCompressor
    {
        public int MaxDegreeOfParallelism = -1;
        public int ProgressUpdateInterval = 1000;
        public List<LightmapCompressionTask> Tasks = new List<LightmapCompressionTask>();
        public event Action<float> ProgressUpdated;

        public void AddTask(LightmapCompressionTask task)
        {
            Tasks.Add(task);
        }

        public void Run()
        {
            var progressPoll = new Stopwatch();
            progressPoll.Start();

            float[] progress = new float[Tasks.Count];

            var options = new ParallelOptions();
            options.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
            Parallel.ForEach(Tasks, options, task =>
            {
                var index = task.Index;
                var progressSink = new ProgressSink(ProgressUpdateInterval);
                progressSink.Updated += value =>
                {
                    progress[task.Index] = value;

                    float aggregateProgress = 0;
                    bool shouldReport = false;
                    lock (progressPoll)
                    {
                        if (progressPoll.ElapsedMilliseconds >= ProgressUpdateInterval)
                        {
                            progressPoll.Restart();
                            aggregateProgress = progress.Average();
                            shouldReport = true;
                        }
                    }

                    if (shouldReport)
                        ProgressUpdated?.Invoke(progress.Average());
                };
                CompressLightmapBitmap(task.Width, task.Height, task.Input, task.Dxt0, task.Dxt1, out task.MaxL, progressSink);
            });

            ProgressUpdated?.Invoke(1.0f);
        }

        private static void CompressLightmapBitmap(int width, int height, RealRgbColor[] pixels, byte[] dxt0, byte[] dxt1, out float maxL, IProgress<float> progress = null)
        {
            var colors = new RealRgbColor[pixels.Length];
            var alphas = new float[pixels.Length];
            ConvertToLuvwSpace(pixels, colors, alphas);
            maxL = alphas.Max();
            if (maxL < 0.0001f)
                maxL = 0.0001f;

            var invSqrtMaxL = 1 / Math.Sqrt(maxL);
            int destOffset = 0;

            var dxtEncoder = new FauxDxtEncoder();
            var colorResidual = new RealRgbColor[16];
            var alphaResidual = new float[16];
            var blockAlphas = new float[16];
            var blockColors = new RealRgbColor[16];

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    for (int py = 0; py < 4; py++)
                    {
                        for (int px = 0; px < 4; px++)
                        {
                            int sourceIndex = px + x + width * (py + y);
                            int destIndex = py * 4 + px;
                            blockAlphas[destIndex] = (float)(Math.Sqrt(alphas[sourceIndex]) * invSqrtMaxL);
                            blockColors[destIndex] = colors[sourceIndex];
                        }
                    }

                    var alphaBlock0 = dxtEncoder.EncodeAlphaBlock(blockAlphas, alphaResidual);

                    for (int i = 0; i < 16; i++)
                    {
                        if (alphaResidual[i] >= 0.0001f)
                            blockAlphas[i] = Math.Min(Math.Max(0.0f, alphaResidual[i]), 1.0f);
                        else
                            blockAlphas[i] = 0;
                    }

                    var alphaBlock1 = dxtEncoder.EncodeAlphaBlock(blockAlphas, alphaResidual);

                    var colorBlock0 = dxtEncoder.EncodeColorBlock(blockColors, colorResidual);

                    for (int i = 0; i < 16; i++)
                    {
                        blockColors[i].Red = colorResidual[i].Red + 0.5f;
                        blockColors[i].Green = colorResidual[i].Green + 0.5f;
                        blockColors[i].Blue = colorResidual[i].Blue + 0.5f;
                    }
                    var colorBlock1 = dxtEncoder.EncodeColorBlock(blockColors, colorResidual);

                    alphaBlock0.Pack(destOffset, dxt0);
                    colorBlock0.Pack(destOffset + 8, dxt0);
                    alphaBlock1.Pack(destOffset, dxt1);
                    colorBlock1.Pack(destOffset + 8, dxt1);
                    destOffset += 16;

                    progress?.Report((float)(destOffset * 1.0 / pixels.Length));
                }
            }

            progress?.Report(1.0f);
        }

        private static void ConvertToLuvwSpace(RealRgbColor[] rgb, RealRgbColor[] uvw, float[] l)
        {
            for (int i = 0; i < rgb.Length; i++)
            {
                var c = rgb[i];
                l[i] = (float)Math.Sqrt(c.Red * c.Red + c.Green * c.Green + c.Blue * c.Blue);
                if (l[i] >= 0.0001f)
                {
                    uvw[i].Red = ((c.Red / l[i]) + 1.0f) * 0.5f;
                    uvw[i].Green = ((c.Green / l[i]) + 1.0f) * 0.5f;
                    uvw[i].Blue = ((c.Blue / l[i]) + 1.0f) * 0.5f;
                }
                else
                {
                    l[i] = 0.0f;
                    uvw[i].Red = 0;
                    uvw[i].Green = 0;
                    uvw[i].Blue = 0;
                }
            }
        }

        class ProgressSink : IProgress<float>
        {
            public Stopwatch stopwatch;
            public event Action<float> Updated;
            public int Interval = 200;

            public ProgressSink(int interval)
            {
                Interval = interval;
                stopwatch = new Stopwatch();
                stopwatch.Start();
            }

            public void Report(float value)
            {
                if (stopwatch.ElapsedMilliseconds >= Interval || value == 1.0f)
                {
                    stopwatch.Restart();
                    Updated?.Invoke(value);
                }
            }
        }
    }

    public class LightmapCompressionTask
    {
        public int Index;
        public RealRgbColor[] Input;
        public int Width;
        public int Height;

        public float MaxL;
        public byte[] Dxt0;
        public byte[] Dxt1;

        public LightmapCompressionTask(int index, RealRgbColor[] input, int width, int height)
        {
            Index = index;
            Input = input;
            Width = width;
            Height = height;

            Dxt0 = new byte[input.Length];
            Dxt1 = new byte[input.Length];
        }
    }
}
