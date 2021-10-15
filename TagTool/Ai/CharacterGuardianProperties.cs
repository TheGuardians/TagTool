using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x18)]
    public class CharacterGuardianProperties : TagStructure
    {
        public float SurgeTime; //length of time for which the guardian surges (seconds)
        public float SurgeDelay; //minimum enforced delay between surges (seconds)
        public float ProximitySurgeDistance; //surge when our target gets closer than this to me (0 value defaults to 2wu)
        public float PhaseTime; //length of time it takes the guardian to get to its phase destination (seconds)
        public float MinPhaseDistance; //Minimum distance that I will consider phasing (world units)
        public float TargetPhaseDistance; //Minimum distance from my target that I will phase to (world units)
    }
}