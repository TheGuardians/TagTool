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
        [TagField(ValidTags = new [] { "eqip" })]
        // special version of RS used by player to position droppod location
        public CachedTag DroppodUiRemoteStrike;
        [TagField(ValidTags = new [] { "scen" })]
        public CachedTag DropPod;
        [TagField(ValidTags = new [] { "effe" })]
        // spawned at drop location at start of fanfare delay
        public CachedTag DropPodWarning;
        [TagField(ValidTags = new [] { "effe" })]
        // used by random ordnance drop system for air drops
        public CachedTag DropPodLocator;
    }
}
