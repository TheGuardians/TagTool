using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Cache.Gen2;
using System.IO;
using TagTool.Commands.Common;
using TagTool.Commands.Porting;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public TagStructure ConvertWidget(object gen2Tag)
        {
            switch (gen2Tag)
            {
                case TagTool.Tags.Definitions.Gen2.Antenna antenna:
                    return ConvertAntenna(antenna);
                case TagTool.Tags.Definitions.Gen2.PointPhysics pointPhysics:
                    return ConvertPointPhysics(pointPhysics);
                default:
                    return null;
            }
        }

        private TagStructure ConvertAntenna(TagTool.Tags.Definitions.Gen2.Antenna antenna)
        {
            Antenna newAntenna = new Antenna
            {
                AttachmentMarkerName = antenna.AttachmentMarkerName,
                Bitmaps = antenna.Bitmaps,
                Physics = antenna.Physics,
                SpringStrengthCoefficient = antenna.SpringStrengthCoefficient,
                FalloffPixels = antenna.FalloffPixels,
                CutoffPixels = antenna.CutoffPixels,

                // No bending :)
                PointOfBend = 0.0f,
                StartingBend = 0.0f,
                EndingBend = 0.0f,

                Vertices = new List<Antenna.AntennaVertex>()
            };

            float hermiteAccumHack = 0.0f;
            foreach (var oldVert in antenna.Vertices)
            {
                float c_yaw = (float)Math.Cos(oldVert.Angles.Yaw.Radians);
                float c_pitch = (float)Math.Cos(oldVert.Angles.Pitch.Radians);
                float s_yaw = (float)Math.Sin(oldVert.Angles.Yaw.Radians);
                float s_pitch = (float)Math.Sin(oldVert.Angles.Pitch.Radians);

                Antenna.AntennaVertex newVert = new Antenna.AntennaVertex
                {
                    Angles = oldVert.Angles,
                    Length = oldVert.Length,
                    SequenceIndex = oldVert.SequenceIndex,
                    Color = oldVert.Color,
                    LodColor = oldVert.LodColor,
                    HermiteT = hermiteAccumHack, // TODO: calculate correctly (points along hermite spline)
                    VectorToNext = new RealVector3d 
                    { 
                        I = c_yaw * c_pitch * oldVert.Length, 
                        J = c_yaw * s_pitch * oldVert.Length, 
                        K = s_yaw * oldVert.Length
                    }
                };

                hermiteAccumHack += 1.0f / antenna.Vertices.Count;
                newAntenna.RuntimeTotalLength += newVert.Length;
                newAntenna.Vertices.Add(newVert);
            }

            return newAntenna;
        }

        private TagStructure ConvertPointPhysics(TagTool.Tags.Definitions.Gen2.PointPhysics pointPhysics)
        {
            PointPhysics newPointPhysics = new PointPhysics();

            // TODO: calculation for RuntimeMassOverRadiusCubed

            newPointPhysics.Flags = (PointPhysics.PointPhysicsFlags)pointPhysics.Flags;
            newPointPhysics.Density = pointPhysics.Density;
            newPointPhysics.AirFriction = pointPhysics.AirFriction;
            newPointPhysics.WaterFriction = pointPhysics.WaterFriction;
            newPointPhysics.SurfaceFriction = pointPhysics.SurfaceFriction;
            newPointPhysics.Elasticity = pointPhysics.Elasticity;

            return newPointPhysics;
        }
    }
}
