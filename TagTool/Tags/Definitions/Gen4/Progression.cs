using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "progression", Tag = "prog", Size = 0x4C)]
    public class Progressionglobals : TagStructure
    {
        public int OrdnancePointsGainedOnKillingAnotherPlayer;
        public int OrdnancePointsGainedOnKillingAnNpc;
        public int OrdnancePointsLostOnDeath;
        // special version of RS used by player to position droppod location
        [TagField(ValidTags = new [] { "eqip" })]
        public CachedTag DroppodUiRemoteStrike;
        [TagField(ValidTags = new [] { "scen" })]
        public CachedTag DropPod;
        // spawned at drop location at start of fanfare delay
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DropPodWarning;
        // used by random ordnance drop system for air drops
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DropPodLocator;
    }
}
