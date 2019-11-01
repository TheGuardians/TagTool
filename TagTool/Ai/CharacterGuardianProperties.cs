using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x18)]
    public class CharacterGuardianProperties : TagStructure
    {
        public float Surgetime; //length of time for which the guardian surges
        public float Surgedelay; //minimum enforced delay between surges
        public float ProximitySurge; //surge when our target gets closer than this to me (0 value defaults to 2wu)
        public float Phasetime; //length of time it takes the guardian to get to its phase destination
        public float Phasedistance; //Minimum distance that I will consider phasing
        public float Targetphasedistance; //Minimum distance from my target that I will phase to
    }
}