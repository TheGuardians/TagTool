using System;
using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Tags
{
    [TagStructure(Name = "player_action_set", Tag = "pact", Size = 0x11C)]
    public class PlayerActionSet : TagStructure
    {
        public int version;

        public List<WidgetData> Widget;

        public List<Action> Actions;

        [TagField(Flags = TagFieldFlags.Padding, Length = 0x100)]
        public byte[] Unused = new byte[0x100];

        [TagStructure(Size = 0x124)]
        public class WidgetData : TagStructure
        {
            [TagField(Length = 32)]
            public string Title;

            public WidgetType Type;

            public ushort Flags;

            [TagField(Flags = TagFieldFlags.Padding, Length = 0x100)]
            public byte[] Unused = new byte[0x100];

            public enum WidgetType : short
            {
                Wheel,
                Grid
            }
        }

        [TagStructure(Size = 0x15C)]
        public class Action : TagStructure
        {
            [TagField(Length = 32)]
            public string Title;

            [TagField(Length = 32)]
            public string IconName;

            public StringId AnimationEnter;

            public StringId AnimationIdle;

            public StringId AnimationExit;

            public ActionFlags Flags;

            public List<Unit.UnitCameraBlock> OverrideCamera;

            [TagField(Flags = TagFieldFlags.Padding, Length = 0x100)]
            public byte[] Unused = new byte[0x100];

            [Flags]
            public enum ActionFlags : int
            {
                HideWeapon = 1 << 0,
                ForceThirdPersonCamera = 1 << 1,
                InhibitMovement = 1 << 2,
                InhibitCameraMovement = 1 << 3,
                InhibitCancel = 1 << 4,
                DontPlayExitAnimationOnCancel = 1 << 5,
                HoldOnLastFrame = 1 << 6
            }
        }
    }
}
