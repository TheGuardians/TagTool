using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x20)]
    public class SoundDistanceParameters : TagStructure
    {
        // don't obstruct below this distance
        public float DontObstructDistance; // world units
                                           // don't play below this distance
        public float DontPlayDistance; // world units
                                       // start playing at full volume at this distance
        public float AttackDistance; // world units
                                     // start attenuating at this distance
        public float MinimumDistance; // world units
                                      // set attenuation to sustain db at this distance
        public float SustainBeginDistance; // world units
                                           // continue attenuating to silence at this distance
        public float SustainEndDistance; // world units
                                         // the distance beyond which this sound is no longer audible
        public float MaximumDistance; // world units
                                      // the amount of attenuation between sustain begin and end
        public float SustainDb; // dB
    }
}
