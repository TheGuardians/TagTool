using System;
using static TagTool.Audio.FMOD;

namespace TagTool.Audio
{
    public static class FMODTagTool
    {
        public static FMODSystem System;

        static FMODTagTool()
        {
            FMOD_RESULT result;

            result = FMODSystem.Create(out System);
            if (result != FMOD_RESULT.OK)
                throw new Exception($"Failed to create fmod system. {result}");

            result = System.Init(256, FMOD_INITFLAGS.NORMAL);
            if (result != FMOD_RESULT.OK)
                throw new Exception($"Failed to initialize fmod system. {result}");
        }
    }
}
