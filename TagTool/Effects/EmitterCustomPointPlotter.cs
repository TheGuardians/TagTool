using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Cache;
using TagTool.Commands.Common;
using System.Diagnostics;

namespace TagTool.Effects
{
    public class EmitterCustomPointPlotter
    {
        private GameCache CacheContext { get; set; }
        private Stream CacheStream { get; set; }
        private Effect.Event.ParticleSystem.Emitter Emitter { get; set; }
        private string TagName { get; set; }
        private string EmitterName { get; set; }
        private int EmitterIndex { get; set; }
        private Random Random { get; set; }
        
        private float EmissionRadius { get; set; }
        private Bounds<float> EmissionRadiusRange { get; set; }
        private float EmissionAngle { get; set; }
        private Bounds<float> EmissionAngleRange { get; set; }

        private const int MaxPoints = 120; // adjust as needed. TODO: scale

        public EmitterCustomPointPlotter(GameCache cacheContext, Stream cacheStream, Effect.Event.ParticleSystem.Emitter emitter, string tagName, int emitterIndex)
        {
            CacheContext = cacheContext;
            CacheStream = cacheStream;
            Emitter = emitter;
            TagName = tagName;
            EmitterName = cacheContext.StringTable.GetString(emitter.Name);
            EmitterIndex = emitterIndex;
            Random = new Random(0xFFFF);

            EmissionRadius = ScalarPropertyEvaluateRandomFast(Emitter.EmissionRadius, out float emissionRadiusMin, out float emissionRadiusMax);
            EmissionAngle = ScalarPropertyEvaluateRandomFast(Emitter.EmissionAngle, out float emissionAngleMin, out float emissionAngleMax);
            EmissionRadiusRange = new Bounds<float> { Lower = emissionRadiusMin, Upper = emissionRadiusMax };
            EmissionAngleRange = new Bounds<float> { Lower = emissionAngleMin, Upper = emissionAngleMax };
        }

        private float ScalarPropertyEvaluateRandomFast(ParticlePropertyScalar property, out float min, out float max)
        {
            if (property.RuntimeMFlags.HasFlag(ParticlePropertyScalar.EditablePropertiesFlags.IsConstant))
            {
                min = property.RuntimeMConstantValue;
                max = property.RuntimeMConstantValue;
                return property.RuntimeMConstantValue;
            }

            if (property.Function.Data[2] == 0) // _is_scalar
            {
                min = BitConverter.ToSingle(property.Function.Data, 4);
                max = BitConverter.ToSingle(property.Function.Data, 8);

                // This is a quick eval - it does not interpolate according to the function
                float s = (float)Random.NextDouble();
                return (1.0f - s) * min + max * s;
            }

            new TagToolWarning("Emitter scalar property set as colour. Evaluating as 0");
            min = 0.0f;
            max = 0.0f;
            return 0.0f;
        }

        // todo: offset
        private RealVector3d GetCompressionScale(out int compressionX, out int compressionY, out int compressionZ)
        {
            var axisScale = new RealVector3d {
                I = Math.Max(1.0f, EmissionRadiusRange.Upper * Emitter.AxisScale.X),
                J = Math.Max(1.0f, EmissionRadiusRange.Upper * Emitter.AxisScale.Y),
                K = Math.Max(1.0f, EmissionRadiusRange.Upper * Emitter.AxisScale.Z)
            };

            axisScale = 32767.0f / axisScale;
            compressionX = (int)axisScale.I;
            compressionY = (int)axisScale.J;
            compressionZ = (int)axisScale.K;
            return 1.0f / axisScale;
        }

        private List<ParticleEmitterCustomPoints.Point> ConvertPlane(int points, int compressionX, int compressionY, int compressionZ)
        {
            List<ParticleEmitterCustomPoints.Point> customPoints = new List<ParticleEmitterCustomPoints.Point>();
            for (int i = 0; i < points; i++)
            {
                ParticleEmitterCustomPoints.Point point = new ParticleEmitterCustomPoints.Point
                {
                    PositionX = (short)((2.0f * (float)Random.NextDouble() - 1.0f) * EmissionRadiusRange.Upper * Emitter.AxisScale.X * compressionX),
                    PositionY = (short)((2.0f * (float)Random.NextDouble() - 1.0f) * EmissionRadiusRange.Upper * Emitter.AxisScale.Y * compressionY),
                    PositionZ = (short)((2.0f * (float)Random.NextDouble() - 1.0f) * EmissionRadiusRange.Upper * Emitter.AxisScale.Z * compressionZ),
                    // Planes have a constant velocity of {1,0,0}
                    // Emission angle may cause random directions... not much can be done
                    NormalX = 127,
                    NormalY = 0,
                    NormalZ = 0,
                    Correlation = 0
                };
                customPoints.Add(point);
            }
            return customPoints;
        }
        private List<ParticleEmitterCustomPoints.Point> ConvertCube(int points, int compressionX, int compressionY, int compressionZ)
        {
            List<ParticleEmitterCustomPoints.Point> customPoints = new List<ParticleEmitterCustomPoints.Point>();
            for (int i = 0; i < points; i++)
            {
                RealVector3d cubePoint = new RealVector3d
                {
                    I = (2.0f * EmissionRadiusRange.Upper * (float)Random.NextDouble() - EmissionRadiusRange.Upper),
                    J = (2.0f * EmissionRadiusRange.Upper * (float)Random.NextDouble() - EmissionRadiusRange.Upper),
                    K = (2.0f * EmissionRadiusRange.Upper * (float)Random.NextDouble() - EmissionRadiusRange.Upper)
                };
                ParticleEmitterCustomPoints.Point point = new ParticleEmitterCustomPoints.Point
                {
                    PositionX = (short)(cubePoint.I * Emitter.AxisScale.X * compressionX),
                    PositionY = (short)(cubePoint.J * Emitter.AxisScale.Y * compressionY),
                    PositionZ = (short)(cubePoint.K * Emitter.AxisScale.Z * compressionZ),
                    Correlation = 0
                };
                //cubePoint = RealVector3d.Normalize(cubePoint);
                point.NormalX = (sbyte)(cubePoint.I * 127.0f);
                point.NormalY = (sbyte)(cubePoint.J * 127.0f);
                point.NormalZ = (sbyte)(cubePoint.K * 127.0f);
                customPoints.Add(point);
            }
            return customPoints;
        }
        private List<ParticleEmitterCustomPoints.Point> ConvertCylinder(int points, int compressionX, int compressionY, int compressionZ)
        {
            List<ParticleEmitterCustomPoints.Point> customPoints = new List<ParticleEmitterCustomPoints.Point>();
            for (int i = 0; i < points; i++)
            {
                float randomCircular = (float)(Random.NextDouble() * 2 * Math.PI);
                float randomizedRadius = (float)Random.NextDouble() * EmissionRadiusRange.Upper;
                RealVector3d cylinderPoint = new RealVector3d
                {
                    I = (2.0f * (float)Random.NextDouble() - 1.0f),
                    J = randomizedRadius * (float)Math.Sin(randomCircular),
                    K = randomizedRadius * (float)Math.Cos(randomCircular)
                };
                ParticleEmitterCustomPoints.Point point = new ParticleEmitterCustomPoints.Point
                {
                    PositionX = (short)(cylinderPoint.I * Emitter.AxisScale.X * compressionX),
                    PositionY = (short)(cylinderPoint.J * Emitter.AxisScale.Y * compressionY),
                    PositionZ = (short)(cylinderPoint.K * Emitter.AxisScale.Z * compressionZ),
                    Correlation = 0
                };
                //cylinderPoint = RealVector3d.Normalize(cylinderPoint);
                point.NormalX = (sbyte)(cylinderPoint.I * 127.0f);
                point.NormalY = (sbyte)(cylinderPoint.J * 127.0f);
                point.NormalZ = (sbyte)(cylinderPoint.K * 127.0f);
                customPoints.Add(point);
            }
            return customPoints;
        }
        private List<ParticleEmitterCustomPoints.Point> ConvertUnweightedLine(int points, int compressionX, int compressionY, int compressionZ)
        {
            List<ParticleEmitterCustomPoints.Point> customPoints = new List<ParticleEmitterCustomPoints.Point>();
            for (int i = 0; i < points; i++)
            {
                float randomCircular = (float)(Random.NextDouble() * 2 * Math.PI);
                float randomAngle = (float)Random.NextDouble() * EmissionAngleRange.Upper * 0.017453292f;
                RealVector3d linePoint = new RealVector3d
                {
                    I = 0.0f,
                    J = (2.0f * EmissionRadiusRange.Upper * (float)Random.NextDouble() - EmissionRadiusRange.Upper),
                    K = 0.0f
                };
                ParticleEmitterCustomPoints.Point point = new ParticleEmitterCustomPoints.Point
                {
                    PositionX = (short)(linePoint.I * Emitter.AxisScale.X * compressionX),
                    PositionY = (short)(linePoint.J * Emitter.AxisScale.Y * compressionY),
                    PositionZ = (short)(linePoint.K * Emitter.AxisScale.Z * compressionZ),
                    Correlation = 0
                };
                RealVector3d lineNormal = new RealVector3d
                {
                    I = (float)Math.Cos(randomAngle),
                    J = (float)(Math.Sin(randomCircular) * Math.Sin(randomAngle)),
                    K = (float)(Math.Cos(randomCircular) * Math.Sin(randomAngle)),
                };
                //lineNormal = RealVector3d.Normalize(lineNormal);
                point.NormalX = (sbyte)(lineNormal.I * 127.0f);
                point.NormalY = (sbyte)(lineNormal.J * 127.0f);
                point.NormalZ = (sbyte)(lineNormal.K * 127.0f);
                customPoints.Add(point);
            }
            return customPoints;
        }
        private List<ParticleEmitterCustomPoints.Point> ConvertTube(int points, int compressionX, int compressionY, int compressionZ)
        {
            List<ParticleEmitterCustomPoints.Point> customPoints = new List<ParticleEmitterCustomPoints.Point>();
            for (int i = 0; i < points; i++)
            {
                float randomCircular = (float)(Random.NextDouble() * 2 * Math.PI);
                float maxEmissionAngleRadians = EmissionAngleRange.Upper * 0.017453292f;
                RealVector3d tubePoint = new RealVector3d
                {
                    I = 0.0f,
                    J = EmissionRadiusRange.Upper * (float)Math.Sin(randomCircular),
                    K = EmissionRadiusRange.Upper * (float)Math.Cos(randomCircular)
                };
                ParticleEmitterCustomPoints.Point point = new ParticleEmitterCustomPoints.Point
                {
                    PositionX = (short)(tubePoint.I * Emitter.AxisScale.X * compressionX),
                    PositionY = (short)(tubePoint.J * Emitter.AxisScale.Y * compressionY),
                    PositionZ = (short)(tubePoint.K * Emitter.AxisScale.Z * compressionZ),
                    Correlation = 0
                };
                RealVector3d tubeNormal = new RealVector3d
                {
                    I = (float)Math.Cos(maxEmissionAngleRadians),
                    J = (float)(Math.Sin(randomCircular) * Math.Sin(maxEmissionAngleRadians)),
                    K = (float)(Math.Cos(randomCircular) * Math.Sin(maxEmissionAngleRadians)),
                };
                //tubeNormal = RealVector3d.Normalize(tubeNormal);
                point.NormalX = (sbyte)(tubeNormal.I * 127.0f);
                point.NormalY = (sbyte)(tubeNormal.J * 127.0f);
                point.NormalZ = (sbyte)(tubeNormal.K * 127.0f);
                customPoints.Add(point);
            }
            return customPoints;
        }

        public bool ConvertEmitterToCustomPoints()
        {
            int pointsToGenerate = (int)(MaxPoints * EmissionRadiusRange.Upper);         

            ParticleEmitterCustomPoints pecp = new ParticleEmitterCustomPoints();
            pecp.Points = new List<ParticleEmitterCustomPoints.Point>();
            pecp.ParticleModel = null; // not needed
            pecp.CompressionScale = GetCompressionScale(out int compressionX, out int compressionY, out int compressionZ);
            pecp.CompressionOffset = new RealVector3d(0, 0, 0);

            switch (Emitter.EmissionShape)
            {
                case Effect.Event.ParticleSystem.Emitter.EmissionShapeValue.Plane:
                    pecp.Points = ConvertPlane(pointsToGenerate, compressionX, compressionY, compressionZ);
                    break;
                case Effect.Event.ParticleSystem.Emitter.EmissionShapeValue.Cube:
                    pecp.Points = ConvertCube(pointsToGenerate, compressionX, compressionY, compressionZ);
                    break;
                case Effect.Event.ParticleSystem.Emitter.EmissionShapeValue.Cylinder:
                    pecp.Points = ConvertCylinder(pointsToGenerate, compressionX, compressionY, compressionZ);
                    break;
                case Effect.Event.ParticleSystem.Emitter.EmissionShapeValue.UnweightedLine:
                    pecp.Points = ConvertUnweightedLine(pointsToGenerate, compressionX, compressionY, compressionZ);
                    break;
                case Effect.Event.ParticleSystem.Emitter.EmissionShapeValue.Tube:
                    pecp.Points = ConvertTube(pointsToGenerate, compressionX, compressionY, compressionZ);
                    break;
                default:
                    return false;
            }

            if (!CacheContext.TagCache.TryGetTag(TagName + $"_{EmitterName}.pecp", out CachedTag pecpTag))
                pecpTag = CacheContext.TagCache.AllocateTag<ParticleEmitterCustomPoints>(TagName + $"_{EmitterName}_{EmitterIndex}");
            CacheContext.Serialize(CacheStream, pecpTag, pecp);

            Emitter.CustomShape = pecpTag;
            Emitter.EmissionShape = Effect.Event.ParticleSystem.Emitter.EmissionShapeValue.CustomPoints;

            Console.WriteLine($"Converted emitter {EmitterName} from {Emitter.EmissionShape} to CustomPoints");
            return true;
        }
    }
}
