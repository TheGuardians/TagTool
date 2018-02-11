using TagTool.Geometry;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "particle_model", Tag = "pmdf", Size = 0x90)]
    public class ParticleModel
    {
        public RenderGeometry Geometry;
        public List<MVariant> MVariants;

        [TagStructure(Size = 0x10)]
        public class MVariant
        {
            [TagField(Length = 4)]
            public float[] RuntimeMCount = new float[4];
        }
    }
}