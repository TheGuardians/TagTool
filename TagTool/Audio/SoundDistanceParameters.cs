using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x10, MaxVersion = Cache.CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x20, MinVersion = Cache.CacheVersion.HaloReach)]
    public class SoundDistanceParameters : TagStructure
    {
        [TagField(MinVersion = Cache.CacheVersion.HaloReach)]
        public float DontObstructDistance;  // don't obstruct below this distance (world units)

        public float DontPlayDistance; //don't play below this distance (world units)
        public float AttackDistance; //start playing at full volume at this distance (world units)
        public float MinimumDistance; //start attenuating at this distance (world units)

        [TagField(MinVersion = Cache.CacheVersion.HaloReach)]
        public float SustainBeginDistance; //set attenuation to sustain db at this distance (world units)

        [TagField(MinVersion = Cache.CacheVersion.HaloReach)]
        public float SustainEndDistance; // continue attenuating to silence at this distance (world units)

        public float MaximumDistance; //the distance beyond which this sound is no longer audible (world units)

        [TagField(MinVersion = Cache.CacheVersion.HaloReach)]
        public float SustainDB; // the amount of attenuation between sustain begin and end dB
    }
}
