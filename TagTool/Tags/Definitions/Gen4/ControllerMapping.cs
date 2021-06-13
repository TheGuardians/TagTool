using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "controller_mapping", Tag = "cnmp", Size = 0x5C)]
    public class ControllerMapping : TagStructure
    {
        // set to 0 for instant unzoom on trigger release (trigger style) or 15 for toggle (thumbstick style)
        public int AutoZoomOutTicks;
        public GamepadButtonDefinition Jump;
        public GamepadButtonDefinition SwitchWeapon;
        public GamepadButtonDefinition ContextualAction;
        public GamepadButtonDefinition MeleeAttack;
        public GamepadButtonDefinition Equipment;
        public GamepadButtonDefinition ThrowGrenade;
        public GamepadButtonDefinition PrimaryFire;
        public GamepadButtonDefinition Crouch;
        public GamepadButtonDefinition ZoomZoomScope;
        public GamepadButtonDefinition SwitchGrenadePrev;
        public GamepadButtonDefinition SwitchGrenadeNext;
        public GamepadButtonDefinition SecondaryFire;
        public GamepadButtonDefinition TertiaryFire;
        public GamepadButtonDefinition VehicleTrickPrimary;
        public GamepadButtonDefinition VehicleTrickSecondary;
        public GamepadButtonDefinition SecondaryContextualAction;
        public GamepadButtonDefinition RadioMessage;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public GamepadButtonDefinition LeanLeft;
        public GamepadButtonDefinition LeanRight;
        public GamepadButtonDefinition NightVision;
        public GamepadButtonDefinition Accept;
        public GamepadButtonDefinition Cancel;
        public GamepadButtonDefinition MachinimaLowerWeapon;
        public GamepadButtonDefinition MachinimaCameraEnable;
        public GamepadButtonDefinition MachinimaCameraControl;
        public GamepadButtonDefinition MachinimaCameraDebug;
        public GamepadButtonDefinition LiftEditor;
        public GamepadButtonDefinition DropEditor;
        public GamepadButtonDefinition PushToTalk;
        public GamepadButtonDefinition CinematicSkip;
        public GamepadButtonDefinition Fireteam;
        public GamepadButtonDefinition Regroup;
        public GamepadButtonDefinition ActivateMinimap;
        public GamepadButtonDefinition RequisitionMenu;
        public GamepadButtonDefinition LoadoutMenu;
        // aka sprint
        public GamepadButtonDefinition HeroAssist;
        public GamepadButtonDefinition Ordnance;
        public GamepadButtonDefinition SkipKillcam;
        public GamepadButtonDefinition MantisFirePrimary;
        public GamepadButtonDefinition MantisFireSecondary;
        public GamepadButtonDefinition MantisMeleeAttack;
        public GamepadButtonDefinition MantisCrouch;
        
        public enum GamepadButtonDefinition : short
        {
            LeftTrigger,
            RightTrigger,
            DpadUp,
            DpadDown,
            DpadLeft,
            DpadRight,
            Start,
            Back,
            LeftThumb,
            RightThumb,
            ButtonA,
            ButtonB,
            ButtonX,
            ButtonY,
            LeftBumper,
            RightBumper
        }
    }
}
