using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "ai_dialogue_globals", Tag = "adlg", Size = 0x2C)]
    public class AiDialogueGlobals : TagStructure
    {
        public List<VocalizationDefinitionsBlock0> Vocalizations;
        public List<VocalizationPatternsBlock> Patterns;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<DialogueDataBlock> DialogueData;
        public List<InvoluntaryDataBlock> InvoluntaryData;
        
        [TagStructure(Size = 0x60)]
        public class VocalizationDefinitionsBlock0 : TagStructure
        {
            public StringId Vocalization;
            public StringId ParentVocalization;
            public short ParentIndex;
            public PriorityValue Priority;
            public FlagsValue Flags;
            /// <summary>
            /// how does the speaker of this vocalization direct his gaze?
            /// </summary>
            public GlanceBehaviorValue GlanceBehavior;
            /// <summary>
            /// how does someone who hears me behave?
            /// </summary>
            public GlanceRecipientBehaviorValue GlanceRecipientBehavior;
            public PerceptionTypeValue PerceptionType;
            public MaxCombatStatusValue MaxCombatStatus;
            public AnimationImpulseValue AnimationImpulse;
            public OverlapPriorityValue OverlapPriority;
            /// <summary>
            /// Minimum delay time between playing the same permutation
            /// </summary>
            public float SoundRepetitionDelay; // minutes
            /// <summary>
            /// How long to wait to actually start the vocalization
            /// </summary>
            public float AllowableQueueDelay; // seconds
            /// <summary>
            /// How long to wait to actually start the vocalization
            /// </summary>
            public float PreVocDelay; // seconds
            /// <summary>
            /// How long into the vocalization the AI should be notified
            /// </summary>
            public float NotificationDelay; // seconds
            /// <summary>
            /// How long speech is suppressed in the speaking unit after vocalizing
            /// </summary>
            public float PostVocDelay; // seconds
            /// <summary>
            /// How long before the same vocalization can be repeated
            /// </summary>
            public float RepeatDelay; // seconds
            /// <summary>
            /// Inherent weight of this vocalization
            /// </summary>
            public float Weight; // [0-1]
            /// <summary>
            /// speaker won't move for the given amount of time
            /// </summary>
            public float SpeakerFreezeTime;
            /// <summary>
            /// listener won't move for the given amount of time (from start of vocalization)
            /// </summary>
            public float ListenerFreezeTime;
            public SpeakerEmotionValue SpeakerEmotion;
            public ListenerEmotionValue ListenerEmotion;
            public float PlayerSkipFraction;
            public float SkipFraction;
            public StringId SampleLine;
            public List<ResponseBlock> Reponses;
            public List<VocalizationDefinitionsBlock1> Children;
            
            public enum PriorityValue : short
            {
                None,
                Recall,
                Idle,
                Comment,
                IdleResponse,
                Postcombat,
                Combat,
                Status,
                Respond,
                Warn,
                Act,
                React,
                Involuntary,
                Scream,
                Scripted,
                Death
            }
            
            [Flags]
            public enum FlagsValue : uint
            {
                Immediate = 1 << 0,
                Interrupt = 1 << 1,
                CancelLowPriority = 1 << 2
            }
            
            public enum GlanceBehaviorValue : short
            {
                None,
                GlanceSubjectShort,
                GlanceSubjectLong,
                GlanceCauseShort,
                GlanceCauseLong,
                GlanceFriendShort,
                GlanceFriendLong
            }
            
            public enum GlanceRecipientBehaviorValue : short
            {
                None,
                GlanceSubjectShort,
                GlanceSubjectLong,
                GlanceCauseShort,
                GlanceCauseLong,
                GlanceFriendShort,
                GlanceFriendLong
            }
            
            public enum PerceptionTypeValue : short
            {
                None,
                Speaker,
                Listener
            }
            
            public enum MaxCombatStatusValue : short
            {
                Asleep,
                Idle,
                Alert,
                Active,
                Uninspected,
                Definite,
                Certain,
                Visible,
                ClearLos,
                Dangerous
            }
            
            public enum AnimationImpulseValue : short
            {
                None,
                Shakefist,
                Cheer,
                SurpriseFront,
                SurpriseBack,
                Taunt,
                Brace,
                Point,
                Hold,
                Wave,
                Advance,
                Fallback
            }
            
            public enum OverlapPriorityValue : short
            {
                None,
                Recall,
                Idle,
                Comment,
                IdleResponse,
                Postcombat,
                Combat,
                Status,
                Respond,
                Warn,
                Act,
                React,
                Involuntary,
                Scream,
                Scripted,
                Death
            }
            
            public enum SpeakerEmotionValue : short
            {
                None,
                Asleep,
                Amorous,
                Happy,
                Inquisitive,
                Repulsed,
                Disappointed,
                Shocked,
                Scared,
                Arrogant,
                Annoyed,
                Angry,
                Pensive,
                Pain
            }
            
            public enum ListenerEmotionValue : short
            {
                None,
                Asleep,
                Amorous,
                Happy,
                Inquisitive,
                Repulsed,
                Disappointed,
                Shocked,
                Scared,
                Arrogant,
                Annoyed,
                Angry,
                Pensive,
                Pain
            }
            
            [TagStructure(Size = 0xC)]
            public class ResponseBlock : TagStructure
            {
                public StringId VocalizationName;
                public FlagsValue Flags;
                public short VocalizationIndexPostProcess;
                public ResponseTypeValue ResponseType;
                public short DialogueIndexImport;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Nonexclusive = 1 << 0,
                    TriggerResponse = 1 << 1
                }
                
                public enum ResponseTypeValue : short
                {
                    Friend,
                    Enemy,
                    Listener,
                    Joint,
                    Peer
                }
            }
            
            [TagStructure(Size = 0x60)]
            public class VocalizationDefinitionsBlock1 : TagStructure
            {
                public StringId Vocalization;
                public StringId ParentVocalization;
                public short ParentIndex;
                public PriorityValue Priority;
                public FlagsValue Flags;
                /// <summary>
                /// how does the speaker of this vocalization direct his gaze?
                /// </summary>
                public GlanceBehaviorValue GlanceBehavior;
                /// <summary>
                /// how does someone who hears me behave?
                /// </summary>
                public GlanceRecipientBehaviorValue GlanceRecipientBehavior;
                public PerceptionTypeValue PerceptionType;
                public MaxCombatStatusValue MaxCombatStatus;
                public AnimationImpulseValue AnimationImpulse;
                public OverlapPriorityValue OverlapPriority;
                /// <summary>
                /// Minimum delay time between playing the same permutation
                /// </summary>
                public float SoundRepetitionDelay; // minutes
                /// <summary>
                /// How long to wait to actually start the vocalization
                /// </summary>
                public float AllowableQueueDelay; // seconds
                /// <summary>
                /// How long to wait to actually start the vocalization
                /// </summary>
                public float PreVocDelay; // seconds
                /// <summary>
                /// How long into the vocalization the AI should be notified
                /// </summary>
                public float NotificationDelay; // seconds
                /// <summary>
                /// How long speech is suppressed in the speaking unit after vocalizing
                /// </summary>
                public float PostVocDelay; // seconds
                /// <summary>
                /// How long before the same vocalization can be repeated
                /// </summary>
                public float RepeatDelay; // seconds
                /// <summary>
                /// Inherent weight of this vocalization
                /// </summary>
                public float Weight; // [0-1]
                /// <summary>
                /// speaker won't move for the given amount of time
                /// </summary>
                public float SpeakerFreezeTime;
                /// <summary>
                /// listener won't move for the given amount of time (from start of vocalization)
                /// </summary>
                public float ListenerFreezeTime;
                public SpeakerEmotionValue SpeakerEmotion;
                public ListenerEmotionValue ListenerEmotion;
                public float PlayerSkipFraction;
                public float SkipFraction;
                public StringId SampleLine;
                public List<ResponseBlock> Reponses;
                public List<VocalizationDefinitionsBlock2> Children;
                
                public enum PriorityValue : short
                {
                    None,
                    Recall,
                    Idle,
                    Comment,
                    IdleResponse,
                    Postcombat,
                    Combat,
                    Status,
                    Respond,
                    Warn,
                    Act,
                    React,
                    Involuntary,
                    Scream,
                    Scripted,
                    Death
                }
                
                [Flags]
                public enum FlagsValue : uint
                {
                    Immediate = 1 << 0,
                    Interrupt = 1 << 1,
                    CancelLowPriority = 1 << 2
                }
                
                public enum GlanceBehaviorValue : short
                {
                    None,
                    GlanceSubjectShort,
                    GlanceSubjectLong,
                    GlanceCauseShort,
                    GlanceCauseLong,
                    GlanceFriendShort,
                    GlanceFriendLong
                }
                
                public enum GlanceRecipientBehaviorValue : short
                {
                    None,
                    GlanceSubjectShort,
                    GlanceSubjectLong,
                    GlanceCauseShort,
                    GlanceCauseLong,
                    GlanceFriendShort,
                    GlanceFriendLong
                }
                
                public enum PerceptionTypeValue : short
                {
                    None,
                    Speaker,
                    Listener
                }
                
                public enum MaxCombatStatusValue : short
                {
                    Asleep,
                    Idle,
                    Alert,
                    Active,
                    Uninspected,
                    Definite,
                    Certain,
                    Visible,
                    ClearLos,
                    Dangerous
                }
                
                public enum AnimationImpulseValue : short
                {
                    None,
                    Shakefist,
                    Cheer,
                    SurpriseFront,
                    SurpriseBack,
                    Taunt,
                    Brace,
                    Point,
                    Hold,
                    Wave,
                    Advance,
                    Fallback
                }
                
                public enum OverlapPriorityValue : short
                {
                    None,
                    Recall,
                    Idle,
                    Comment,
                    IdleResponse,
                    Postcombat,
                    Combat,
                    Status,
                    Respond,
                    Warn,
                    Act,
                    React,
                    Involuntary,
                    Scream,
                    Scripted,
                    Death
                }
                
                public enum SpeakerEmotionValue : short
                {
                    None,
                    Asleep,
                    Amorous,
                    Happy,
                    Inquisitive,
                    Repulsed,
                    Disappointed,
                    Shocked,
                    Scared,
                    Arrogant,
                    Annoyed,
                    Angry,
                    Pensive,
                    Pain
                }
                
                public enum ListenerEmotionValue : short
                {
                    None,
                    Asleep,
                    Amorous,
                    Happy,
                    Inquisitive,
                    Repulsed,
                    Disappointed,
                    Shocked,
                    Scared,
                    Arrogant,
                    Annoyed,
                    Angry,
                    Pensive,
                    Pain
                }
                
                [TagStructure(Size = 0xC)]
                public class ResponseBlock : TagStructure
                {
                    public StringId VocalizationName;
                    public FlagsValue Flags;
                    public short VocalizationIndexPostProcess;
                    public ResponseTypeValue ResponseType;
                    public short DialogueIndexImport;
                    
                    [Flags]
                    public enum FlagsValue : ushort
                    {
                        Nonexclusive = 1 << 0,
                        TriggerResponse = 1 << 1
                    }
                    
                    public enum ResponseTypeValue : short
                    {
                        Friend,
                        Enemy,
                        Listener,
                        Joint,
                        Peer
                    }
                }
                
                [TagStructure(Size = 0x60)]
                public class VocalizationDefinitionsBlock2 : TagStructure
                {
                    public StringId Vocalization;
                    public StringId ParentVocalization;
                    public short ParentIndex;
                    public PriorityValue Priority;
                    public FlagsValue Flags;
                    /// <summary>
                    /// how does the speaker of this vocalization direct his gaze?
                    /// </summary>
                    public GlanceBehaviorValue GlanceBehavior;
                    /// <summary>
                    /// how does someone who hears me behave?
                    /// </summary>
                    public GlanceRecipientBehaviorValue GlanceRecipientBehavior;
                    public PerceptionTypeValue PerceptionType;
                    public MaxCombatStatusValue MaxCombatStatus;
                    public AnimationImpulseValue AnimationImpulse;
                    public OverlapPriorityValue OverlapPriority;
                    /// <summary>
                    /// Minimum delay time between playing the same permutation
                    /// </summary>
                    public float SoundRepetitionDelay; // minutes
                    /// <summary>
                    /// How long to wait to actually start the vocalization
                    /// </summary>
                    public float AllowableQueueDelay; // seconds
                    /// <summary>
                    /// How long to wait to actually start the vocalization
                    /// </summary>
                    public float PreVocDelay; // seconds
                    /// <summary>
                    /// How long into the vocalization the AI should be notified
                    /// </summary>
                    public float NotificationDelay; // seconds
                    /// <summary>
                    /// How long speech is suppressed in the speaking unit after vocalizing
                    /// </summary>
                    public float PostVocDelay; // seconds
                    /// <summary>
                    /// How long before the same vocalization can be repeated
                    /// </summary>
                    public float RepeatDelay; // seconds
                    /// <summary>
                    /// Inherent weight of this vocalization
                    /// </summary>
                    public float Weight; // [0-1]
                    /// <summary>
                    /// speaker won't move for the given amount of time
                    /// </summary>
                    public float SpeakerFreezeTime;
                    /// <summary>
                    /// listener won't move for the given amount of time (from start of vocalization)
                    /// </summary>
                    public float ListenerFreezeTime;
                    public SpeakerEmotionValue SpeakerEmotion;
                    public ListenerEmotionValue ListenerEmotion;
                    public float PlayerSkipFraction;
                    public float SkipFraction;
                    public StringId SampleLine;
                    public List<ResponseBlock> Reponses;
                    public List<VocalizationDefinitionsBlock3> Children;
                    
                    public enum PriorityValue : short
                    {
                        None,
                        Recall,
                        Idle,
                        Comment,
                        IdleResponse,
                        Postcombat,
                        Combat,
                        Status,
                        Respond,
                        Warn,
                        Act,
                        React,
                        Involuntary,
                        Scream,
                        Scripted,
                        Death
                    }
                    
                    [Flags]
                    public enum FlagsValue : uint
                    {
                        Immediate = 1 << 0,
                        Interrupt = 1 << 1,
                        CancelLowPriority = 1 << 2
                    }
                    
                    public enum GlanceBehaviorValue : short
                    {
                        None,
                        GlanceSubjectShort,
                        GlanceSubjectLong,
                        GlanceCauseShort,
                        GlanceCauseLong,
                        GlanceFriendShort,
                        GlanceFriendLong
                    }
                    
                    public enum GlanceRecipientBehaviorValue : short
                    {
                        None,
                        GlanceSubjectShort,
                        GlanceSubjectLong,
                        GlanceCauseShort,
                        GlanceCauseLong,
                        GlanceFriendShort,
                        GlanceFriendLong
                    }
                    
                    public enum PerceptionTypeValue : short
                    {
                        None,
                        Speaker,
                        Listener
                    }
                    
                    public enum MaxCombatStatusValue : short
                    {
                        Asleep,
                        Idle,
                        Alert,
                        Active,
                        Uninspected,
                        Definite,
                        Certain,
                        Visible,
                        ClearLos,
                        Dangerous
                    }
                    
                    public enum AnimationImpulseValue : short
                    {
                        None,
                        Shakefist,
                        Cheer,
                        SurpriseFront,
                        SurpriseBack,
                        Taunt,
                        Brace,
                        Point,
                        Hold,
                        Wave,
                        Advance,
                        Fallback
                    }
                    
                    public enum OverlapPriorityValue : short
                    {
                        None,
                        Recall,
                        Idle,
                        Comment,
                        IdleResponse,
                        Postcombat,
                        Combat,
                        Status,
                        Respond,
                        Warn,
                        Act,
                        React,
                        Involuntary,
                        Scream,
                        Scripted,
                        Death
                    }
                    
                    public enum SpeakerEmotionValue : short
                    {
                        None,
                        Asleep,
                        Amorous,
                        Happy,
                        Inquisitive,
                        Repulsed,
                        Disappointed,
                        Shocked,
                        Scared,
                        Arrogant,
                        Annoyed,
                        Angry,
                        Pensive,
                        Pain
                    }
                    
                    public enum ListenerEmotionValue : short
                    {
                        None,
                        Asleep,
                        Amorous,
                        Happy,
                        Inquisitive,
                        Repulsed,
                        Disappointed,
                        Shocked,
                        Scared,
                        Arrogant,
                        Annoyed,
                        Angry,
                        Pensive,
                        Pain
                    }
                    
                    [TagStructure(Size = 0xC)]
                    public class ResponseBlock : TagStructure
                    {
                        public StringId VocalizationName;
                        public FlagsValue Flags;
                        public short VocalizationIndexPostProcess;
                        public ResponseTypeValue ResponseType;
                        public short DialogueIndexImport;
                        
                        [Flags]
                        public enum FlagsValue : ushort
                        {
                            Nonexclusive = 1 << 0,
                            TriggerResponse = 1 << 1
                        }
                        
                        public enum ResponseTypeValue : short
                        {
                            Friend,
                            Enemy,
                            Listener,
                            Joint,
                            Peer
                        }
                    }
                    
                    [TagStructure(Size = 0x60)]
                    public class VocalizationDefinitionsBlock3 : TagStructure
                    {
                        public StringId Vocalization;
                        public StringId ParentVocalization;
                        public short ParentIndex;
                        public PriorityValue Priority;
                        public FlagsValue Flags;
                        /// <summary>
                        /// how does the speaker of this vocalization direct his gaze?
                        /// </summary>
                        public GlanceBehaviorValue GlanceBehavior;
                        /// <summary>
                        /// how does someone who hears me behave?
                        /// </summary>
                        public GlanceRecipientBehaviorValue GlanceRecipientBehavior;
                        public PerceptionTypeValue PerceptionType;
                        public MaxCombatStatusValue MaxCombatStatus;
                        public AnimationImpulseValue AnimationImpulse;
                        public OverlapPriorityValue OverlapPriority;
                        /// <summary>
                        /// Minimum delay time between playing the same permutation
                        /// </summary>
                        public float SoundRepetitionDelay; // minutes
                        /// <summary>
                        /// How long to wait to actually start the vocalization
                        /// </summary>
                        public float AllowableQueueDelay; // seconds
                        /// <summary>
                        /// How long to wait to actually start the vocalization
                        /// </summary>
                        public float PreVocDelay; // seconds
                        /// <summary>
                        /// How long into the vocalization the AI should be notified
                        /// </summary>
                        public float NotificationDelay; // seconds
                        /// <summary>
                        /// How long speech is suppressed in the speaking unit after vocalizing
                        /// </summary>
                        public float PostVocDelay; // seconds
                        /// <summary>
                        /// How long before the same vocalization can be repeated
                        /// </summary>
                        public float RepeatDelay; // seconds
                        /// <summary>
                        /// Inherent weight of this vocalization
                        /// </summary>
                        public float Weight; // [0-1]
                        /// <summary>
                        /// speaker won't move for the given amount of time
                        /// </summary>
                        public float SpeakerFreezeTime;
                        /// <summary>
                        /// listener won't move for the given amount of time (from start of vocalization)
                        /// </summary>
                        public float ListenerFreezeTime;
                        public SpeakerEmotionValue SpeakerEmotion;
                        public ListenerEmotionValue ListenerEmotion;
                        public float PlayerSkipFraction;
                        public float SkipFraction;
                        public StringId SampleLine;
                        public List<ResponseBlock> Reponses;
                        public List<VocalizationDefinitionsBlock4> Children;
                        
                        public enum PriorityValue : short
                        {
                            None,
                            Recall,
                            Idle,
                            Comment,
                            IdleResponse,
                            Postcombat,
                            Combat,
                            Status,
                            Respond,
                            Warn,
                            Act,
                            React,
                            Involuntary,
                            Scream,
                            Scripted,
                            Death
                        }
                        
                        [Flags]
                        public enum FlagsValue : uint
                        {
                            Immediate = 1 << 0,
                            Interrupt = 1 << 1,
                            CancelLowPriority = 1 << 2
                        }
                        
                        public enum GlanceBehaviorValue : short
                        {
                            None,
                            GlanceSubjectShort,
                            GlanceSubjectLong,
                            GlanceCauseShort,
                            GlanceCauseLong,
                            GlanceFriendShort,
                            GlanceFriendLong
                        }
                        
                        public enum GlanceRecipientBehaviorValue : short
                        {
                            None,
                            GlanceSubjectShort,
                            GlanceSubjectLong,
                            GlanceCauseShort,
                            GlanceCauseLong,
                            GlanceFriendShort,
                            GlanceFriendLong
                        }
                        
                        public enum PerceptionTypeValue : short
                        {
                            None,
                            Speaker,
                            Listener
                        }
                        
                        public enum MaxCombatStatusValue : short
                        {
                            Asleep,
                            Idle,
                            Alert,
                            Active,
                            Uninspected,
                            Definite,
                            Certain,
                            Visible,
                            ClearLos,
                            Dangerous
                        }
                        
                        public enum AnimationImpulseValue : short
                        {
                            None,
                            Shakefist,
                            Cheer,
                            SurpriseFront,
                            SurpriseBack,
                            Taunt,
                            Brace,
                            Point,
                            Hold,
                            Wave,
                            Advance,
                            Fallback
                        }
                        
                        public enum OverlapPriorityValue : short
                        {
                            None,
                            Recall,
                            Idle,
                            Comment,
                            IdleResponse,
                            Postcombat,
                            Combat,
                            Status,
                            Respond,
                            Warn,
                            Act,
                            React,
                            Involuntary,
                            Scream,
                            Scripted,
                            Death
                        }
                        
                        public enum SpeakerEmotionValue : short
                        {
                            None,
                            Asleep,
                            Amorous,
                            Happy,
                            Inquisitive,
                            Repulsed,
                            Disappointed,
                            Shocked,
                            Scared,
                            Arrogant,
                            Annoyed,
                            Angry,
                            Pensive,
                            Pain
                        }
                        
                        public enum ListenerEmotionValue : short
                        {
                            None,
                            Asleep,
                            Amorous,
                            Happy,
                            Inquisitive,
                            Repulsed,
                            Disappointed,
                            Shocked,
                            Scared,
                            Arrogant,
                            Annoyed,
                            Angry,
                            Pensive,
                            Pain
                        }
                        
                        [TagStructure(Size = 0xC)]
                        public class ResponseBlock : TagStructure
                        {
                            public StringId VocalizationName;
                            public FlagsValue Flags;
                            public short VocalizationIndexPostProcess;
                            public ResponseTypeValue ResponseType;
                            public short DialogueIndexImport;
                            
                            [Flags]
                            public enum FlagsValue : ushort
                            {
                                Nonexclusive = 1 << 0,
                                TriggerResponse = 1 << 1
                            }
                            
                            public enum ResponseTypeValue : short
                            {
                                Friend,
                                Enemy,
                                Listener,
                                Joint,
                                Peer
                            }
                        }
                        
                        [TagStructure(Size = 0x60)]
                        public class VocalizationDefinitionsBlock4 : TagStructure
                        {
                            public StringId Vocalization;
                            public StringId ParentVocalization;
                            public short ParentIndex;
                            public PriorityValue Priority;
                            public FlagsValue Flags;
                            /// <summary>
                            /// how does the speaker of this vocalization direct his gaze?
                            /// </summary>
                            public GlanceBehaviorValue GlanceBehavior;
                            /// <summary>
                            /// how does someone who hears me behave?
                            /// </summary>
                            public GlanceRecipientBehaviorValue GlanceRecipientBehavior;
                            public PerceptionTypeValue PerceptionType;
                            public MaxCombatStatusValue MaxCombatStatus;
                            public AnimationImpulseValue AnimationImpulse;
                            public OverlapPriorityValue OverlapPriority;
                            /// <summary>
                            /// Minimum delay time between playing the same permutation
                            /// </summary>
                            public float SoundRepetitionDelay; // minutes
                            /// <summary>
                            /// How long to wait to actually start the vocalization
                            /// </summary>
                            public float AllowableQueueDelay; // seconds
                            /// <summary>
                            /// How long to wait to actually start the vocalization
                            /// </summary>
                            public float PreVocDelay; // seconds
                            /// <summary>
                            /// How long into the vocalization the AI should be notified
                            /// </summary>
                            public float NotificationDelay; // seconds
                            /// <summary>
                            /// How long speech is suppressed in the speaking unit after vocalizing
                            /// </summary>
                            public float PostVocDelay; // seconds
                            /// <summary>
                            /// How long before the same vocalization can be repeated
                            /// </summary>
                            public float RepeatDelay; // seconds
                            /// <summary>
                            /// Inherent weight of this vocalization
                            /// </summary>
                            public float Weight; // [0-1]
                            /// <summary>
                            /// speaker won't move for the given amount of time
                            /// </summary>
                            public float SpeakerFreezeTime;
                            /// <summary>
                            /// listener won't move for the given amount of time (from start of vocalization)
                            /// </summary>
                            public float ListenerFreezeTime;
                            public SpeakerEmotionValue SpeakerEmotion;
                            public ListenerEmotionValue ListenerEmotion;
                            public float PlayerSkipFraction;
                            public float SkipFraction;
                            public StringId SampleLine;
                            public List<ResponseBlock> Reponses;
                            public List<VocalizationDefinitionsBlock5> Children;
                            
                            public enum PriorityValue : short
                            {
                                None,
                                Recall,
                                Idle,
                                Comment,
                                IdleResponse,
                                Postcombat,
                                Combat,
                                Status,
                                Respond,
                                Warn,
                                Act,
                                React,
                                Involuntary,
                                Scream,
                                Scripted,
                                Death
                            }
                            
                            [Flags]
                            public enum FlagsValue : uint
                            {
                                Immediate = 1 << 0,
                                Interrupt = 1 << 1,
                                CancelLowPriority = 1 << 2
                            }
                            
                            public enum GlanceBehaviorValue : short
                            {
                                None,
                                GlanceSubjectShort,
                                GlanceSubjectLong,
                                GlanceCauseShort,
                                GlanceCauseLong,
                                GlanceFriendShort,
                                GlanceFriendLong
                            }
                            
                            public enum GlanceRecipientBehaviorValue : short
                            {
                                None,
                                GlanceSubjectShort,
                                GlanceSubjectLong,
                                GlanceCauseShort,
                                GlanceCauseLong,
                                GlanceFriendShort,
                                GlanceFriendLong
                            }
                            
                            public enum PerceptionTypeValue : short
                            {
                                None,
                                Speaker,
                                Listener
                            }
                            
                            public enum MaxCombatStatusValue : short
                            {
                                Asleep,
                                Idle,
                                Alert,
                                Active,
                                Uninspected,
                                Definite,
                                Certain,
                                Visible,
                                ClearLos,
                                Dangerous
                            }
                            
                            public enum AnimationImpulseValue : short
                            {
                                None,
                                Shakefist,
                                Cheer,
                                SurpriseFront,
                                SurpriseBack,
                                Taunt,
                                Brace,
                                Point,
                                Hold,
                                Wave,
                                Advance,
                                Fallback
                            }
                            
                            public enum OverlapPriorityValue : short
                            {
                                None,
                                Recall,
                                Idle,
                                Comment,
                                IdleResponse,
                                Postcombat,
                                Combat,
                                Status,
                                Respond,
                                Warn,
                                Act,
                                React,
                                Involuntary,
                                Scream,
                                Scripted,
                                Death
                            }
                            
                            public enum SpeakerEmotionValue : short
                            {
                                None,
                                Asleep,
                                Amorous,
                                Happy,
                                Inquisitive,
                                Repulsed,
                                Disappointed,
                                Shocked,
                                Scared,
                                Arrogant,
                                Annoyed,
                                Angry,
                                Pensive,
                                Pain
                            }
                            
                            public enum ListenerEmotionValue : short
                            {
                                None,
                                Asleep,
                                Amorous,
                                Happy,
                                Inquisitive,
                                Repulsed,
                                Disappointed,
                                Shocked,
                                Scared,
                                Arrogant,
                                Annoyed,
                                Angry,
                                Pensive,
                                Pain
                            }
                            
                            [TagStructure(Size = 0xC)]
                            public class ResponseBlock : TagStructure
                            {
                                public StringId VocalizationName;
                                public FlagsValue Flags;
                                public short VocalizationIndexPostProcess;
                                public ResponseTypeValue ResponseType;
                                public short DialogueIndexImport;
                                
                                [Flags]
                                public enum FlagsValue : ushort
                                {
                                    Nonexclusive = 1 << 0,
                                    TriggerResponse = 1 << 1
                                }
                                
                                public enum ResponseTypeValue : short
                                {
                                    Friend,
                                    Enemy,
                                    Listener,
                                    Joint,
                                    Peer
                                }
                            }
                            
                            [TagStructure(Size = 0x60)]
                            public class VocalizationDefinitionsBlock5 : TagStructure
                            {
                                public StringId Vocalization;
                                public StringId ParentVocalization;
                                public short ParentIndex;
                                public PriorityValue Priority;
                                public FlagsValue Flags;
                                /// <summary>
                                /// how does the speaker of this vocalization direct his gaze?
                                /// </summary>
                                public GlanceBehaviorValue GlanceBehavior;
                                /// <summary>
                                /// how does someone who hears me behave?
                                /// </summary>
                                public GlanceRecipientBehaviorValue GlanceRecipientBehavior;
                                public PerceptionTypeValue PerceptionType;
                                public MaxCombatStatusValue MaxCombatStatus;
                                public AnimationImpulseValue AnimationImpulse;
                                public OverlapPriorityValue OverlapPriority;
                                /// <summary>
                                /// Minimum delay time between playing the same permutation
                                /// </summary>
                                public float SoundRepetitionDelay; // minutes
                                /// <summary>
                                /// How long to wait to actually start the vocalization
                                /// </summary>
                                public float AllowableQueueDelay; // seconds
                                /// <summary>
                                /// How long to wait to actually start the vocalization
                                /// </summary>
                                public float PreVocDelay; // seconds
                                /// <summary>
                                /// How long into the vocalization the AI should be notified
                                /// </summary>
                                public float NotificationDelay; // seconds
                                /// <summary>
                                /// How long speech is suppressed in the speaking unit after vocalizing
                                /// </summary>
                                public float PostVocDelay; // seconds
                                /// <summary>
                                /// How long before the same vocalization can be repeated
                                /// </summary>
                                public float RepeatDelay; // seconds
                                /// <summary>
                                /// Inherent weight of this vocalization
                                /// </summary>
                                public float Weight; // [0-1]
                                /// <summary>
                                /// speaker won't move for the given amount of time
                                /// </summary>
                                public float SpeakerFreezeTime;
                                /// <summary>
                                /// listener won't move for the given amount of time (from start of vocalization)
                                /// </summary>
                                public float ListenerFreezeTime;
                                public SpeakerEmotionValue SpeakerEmotion;
                                public ListenerEmotionValue ListenerEmotion;
                                public float PlayerSkipFraction;
                                public float SkipFraction;
                                public StringId SampleLine;
                                public List<ResponseBlock> Reponses;
                                public List<GNullBlock> Unknown;
                                
                                public enum PriorityValue : short
                                {
                                    None,
                                    Recall,
                                    Idle,
                                    Comment,
                                    IdleResponse,
                                    Postcombat,
                                    Combat,
                                    Status,
                                    Respond,
                                    Warn,
                                    Act,
                                    React,
                                    Involuntary,
                                    Scream,
                                    Scripted,
                                    Death
                                }
                                
                                [Flags]
                                public enum FlagsValue : uint
                                {
                                    Immediate = 1 << 0,
                                    Interrupt = 1 << 1,
                                    CancelLowPriority = 1 << 2
                                }
                                
                                public enum GlanceBehaviorValue : short
                                {
                                    None,
                                    GlanceSubjectShort,
                                    GlanceSubjectLong,
                                    GlanceCauseShort,
                                    GlanceCauseLong,
                                    GlanceFriendShort,
                                    GlanceFriendLong
                                }
                                
                                public enum GlanceRecipientBehaviorValue : short
                                {
                                    None,
                                    GlanceSubjectShort,
                                    GlanceSubjectLong,
                                    GlanceCauseShort,
                                    GlanceCauseLong,
                                    GlanceFriendShort,
                                    GlanceFriendLong
                                }
                                
                                public enum PerceptionTypeValue : short
                                {
                                    None,
                                    Speaker,
                                    Listener
                                }
                                
                                public enum MaxCombatStatusValue : short
                                {
                                    Asleep,
                                    Idle,
                                    Alert,
                                    Active,
                                    Uninspected,
                                    Definite,
                                    Certain,
                                    Visible,
                                    ClearLos,
                                    Dangerous
                                }
                                
                                public enum AnimationImpulseValue : short
                                {
                                    None,
                                    Shakefist,
                                    Cheer,
                                    SurpriseFront,
                                    SurpriseBack,
                                    Taunt,
                                    Brace,
                                    Point,
                                    Hold,
                                    Wave,
                                    Advance,
                                    Fallback
                                }
                                
                                public enum OverlapPriorityValue : short
                                {
                                    None,
                                    Recall,
                                    Idle,
                                    Comment,
                                    IdleResponse,
                                    Postcombat,
                                    Combat,
                                    Status,
                                    Respond,
                                    Warn,
                                    Act,
                                    React,
                                    Involuntary,
                                    Scream,
                                    Scripted,
                                    Death
                                }
                                
                                public enum SpeakerEmotionValue : short
                                {
                                    None,
                                    Asleep,
                                    Amorous,
                                    Happy,
                                    Inquisitive,
                                    Repulsed,
                                    Disappointed,
                                    Shocked,
                                    Scared,
                                    Arrogant,
                                    Annoyed,
                                    Angry,
                                    Pensive,
                                    Pain
                                }
                                
                                public enum ListenerEmotionValue : short
                                {
                                    None,
                                    Asleep,
                                    Amorous,
                                    Happy,
                                    Inquisitive,
                                    Repulsed,
                                    Disappointed,
                                    Shocked,
                                    Scared,
                                    Arrogant,
                                    Annoyed,
                                    Angry,
                                    Pensive,
                                    Pain
                                }
                                
                                [TagStructure(Size = 0xC)]
                                public class ResponseBlock : TagStructure
                                {
                                    public StringId VocalizationName;
                                    public FlagsValue Flags;
                                    public short VocalizationIndexPostProcess;
                                    public ResponseTypeValue ResponseType;
                                    public short DialogueIndexImport;
                                    
                                    [Flags]
                                    public enum FlagsValue : ushort
                                    {
                                        Nonexclusive = 1 << 0,
                                        TriggerResponse = 1 << 1
                                    }
                                    
                                    public enum ResponseTypeValue : short
                                    {
                                        Friend,
                                        Enemy,
                                        Listener,
                                        Joint,
                                        Peer
                                    }
                                }
                                
                                [TagStructure()]
                                public class GNullBlock : TagStructure
                                {
                                }
                            }
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class VocalizationPatternsBlock : TagStructure
        {
            public DialogueTypeValue DialogueType;
            public short VocalizationIndex;
            public StringId VocalizationName;
            public SpeakerTypeValue SpeakerType;
            public FlagsValue Flags;
            /// <summary>
            /// who/what am I speaking to/of?
            /// </summary>
            public ListenerTargetValue ListenerTarget;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            /// <summary>
            /// The relationship between the subject and the cause
            /// </summary>
            public HostilityValue Hostility;
            public DamageTypeValue DamageType;
            /// <summary>
            /// Speaker must have danger level of at least this much
            /// </summary>
            public DangerLevelValue DangerLevel;
            public AttitudeValue Attitude;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public SubjectActorTypeValue SubjectActorType;
            public CauseActorTypeValue CauseActorType;
            public CauseTypeValue CauseType;
            public SubjectTypeValue SubjectType;
            public StringId CauseAiTypeName;
            /// <summary>
            /// with respect to the subject, the cause is ...
            /// </summary>
            public SpatialRelationValue SpatialRelation;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public StringId SubjectAiTypeName;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public ConditionsValue Conditions;
            
            public enum DialogueTypeValue : short
            {
                Death,
                Unused,
                Unused1,
                Damage,
                DamageUnused1,
                DamageUnused2,
                SightedNew,
                SightedNewMajor,
                Unused2,
                SightedOld,
                SightedFirst,
                SightedSpecial,
                Unused3,
                HeardNew,
                Unused4,
                HeardOld,
                Unused5,
                Unused6,
                Unused7,
                AcknowledgeMultiple,
                Unused8,
                Unused9,
                Unused10,
                FoundUnit,
                FoundUnitPresearch,
                FoundUnitPursuit,
                FoundUnitSelfPreserving,
                FoundUnitRetreating,
                ThrowingGrenade,
                NoticedGrenade,
                Fighting,
                Charging,
                SuppressingFire,
                GrenadeUncover,
                Unused11,
                Unused12,
                Dive,
                Evade,
                Avoid,
                Surprised,
                Unused13,
                Unused14,
                Presearch,
                PresearchStart,
                Search,
                SearchStart,
                InvestigateFailed,
                UncoverFailed,
                PursuitFailed,
                InvestigateStart,
                AbandonedSearchSpace,
                AbandonedSearchTime,
                PresearchFailed,
                AbandonedSearchRestricted,
                InvestigatePursuitStart,
                PostcombatInspectBody,
                VehicleSlowDown,
                VehicleGetIn,
                Idle,
                Taunt,
                TauntReply,
                Retreat,
                RetreatFromScaryTarget,
                RetreatFromDeadLeader,
                RetreatFromProximity,
                RetreatFromLowShield,
                Flee,
                Cowering,
                Unused15,
                Unused16,
                Unused17,
                Cover,
                Covered,
                Unused18,
                Unused19,
                Unused20,
                PursuitStart,
                PursuitSyncStart,
                PursuitSyncJoin,
                PursuitSyncQuorum,
                Melee,
                Unused21,
                Unused22,
                Unused23,
                VehicleFalling,
                VehicleWoohoo,
                VehicleScared,
                VehicleCrazy,
                Unused24,
                Unused25,
                Leap,
                Unused26,
                Unused27,
                PostcombatWin,
                PostcombatLose,
                PostcombatNeutral,
                ShootCorpse,
                PostcombatStart,
                InspectBodyStart,
                PostcombatStatus,
                Unused28,
                VehicleEntryStartDriver,
                VehicleEnter,
                VehicleEntryStartGun,
                VehicleEntryStartPassenger,
                VehicleExit,
                EvictDriver,
                EvictGunner,
                EvictPassenger,
                Unused29,
                Unused30,
                NewOrderAdvance,
                NewOrderCharge,
                NewOrderFallback,
                NewOrderRetreat,
                NewOrderMoveon,
                NewOrderArrival,
                NewOrderEntervcl,
                NewOrderExitvcl,
                NewOrderFllplr,
                NewOrderLeaveplr,
                NewOrderSupport,
                Unused31,
                Unused32,
                Unused33,
                Unused34,
                Unused35,
                Unused36,
                Unused37,
                Unused38,
                Unused39,
                Unused40,
                Unused41,
                Unused42,
                Emerge,
                Unused43,
                Unused44,
                Unused45,
                Curse,
                Unused46,
                Unused47,
                Unused48,
                Threaten,
                Unused49,
                Unused50,
                Unused51,
                CoverFriend,
                Unused52,
                Unused53,
                Unused54,
                Strike,
                Unused55,
                Unused56,
                Unused57,
                Unused58,
                Unused59,
                Unused60,
                Unused61,
                Unused62,
                Gloat,
                Unused63,
                Unused64,
                Unused65,
                Greet,
                Unused66,
                Unused67,
                Unused68,
                Unused69,
                PlayerLook,
                PlayerLookLongtime,
                Unused70,
                Unused71,
                Unused72,
                Unused73,
                PanicGrenadeAttached,
                Unused74,
                Unused75,
                Unused76,
                Unused77,
                HelpResponse,
                Unused78,
                Unused79,
                Unused80,
                Remind,
                Unused81,
                Unused82,
                Unused83,
                Unused84,
                WeaponTradeBetter,
                WeaponTradeWorse,
                WeaponReadeEqual,
                Unused85,
                Unused86,
                Unused87,
                Betray,
                Unused88,
                Forgive,
                Unused89,
                Reanimate,
                Unused90
            }
            
            public enum SpeakerTypeValue : short
            {
                Subject,
                Cause,
                Friend,
                Target,
                Enemy,
                Vehicle,
                Joint,
                Squad,
                Leader,
                JointLeader,
                Clump,
                Peer
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                SubjectVisible = 1 << 0,
                CauseVisible = 1 << 1,
                FriendsPresent = 1 << 2,
                SubjectIsSpeakerSTarget = 1 << 3,
                CauseIsSpeakerSTarget = 1 << 4,
                CauseIsPlayerOrSpeakerIsPlayerAlly = 1 << 5,
                SpeakerIsSearching = 1 << 6,
                SpeakerIsFollowingPlayer = 1 << 7,
                CauseIsPrimaryPlayerAlly = 1 << 8
            }
            
            public enum ListenerTargetValue : short
            {
                Subject,
                Cause,
                Friend,
                Target,
                Enemy,
                Vehicle,
                Joint,
                Squad,
                Leader,
                JointLeader,
                Clump,
                Peer
            }
            
            public enum HostilityValue : short
            {
                None,
                Self,
                Neutral,
                Friend,
                Enemy,
                Traitor
            }
            
            public enum DamageTypeValue : short
            {
                None,
                Falling,
                Bullet,
                Grenade,
                Explosive,
                Sniper,
                Melee,
                Flame,
                MountedWeapon,
                Vehicle,
                Plasma,
                Needle,
                Shotgun
            }
            
            public enum DangerLevelValue : short
            {
                None,
                BroadlyFacing,
                ShootingNear,
                ShootingAt,
                ExtremelyClose,
                ShieldDamage,
                ShieldExtendedDamage,
                BodyDamage,
                BodyExtendedDamage
            }
            
            public enum AttitudeValue : short
            {
                Normal,
                Timid,
                Aggressive
            }
            
            public enum SubjectActorTypeValue : short
            {
                None,
                Elite,
                Jackal,
                Grunt,
                Hunter,
                Engineer,
                Assassin,
                Player,
                Marine,
                Crew,
                CombatForm,
                InfectionForm,
                CarrierForm,
                Monitor,
                Sentinel,
                None1,
                MountedWeapon,
                Brute,
                Prophet,
                Bugger,
                Juggernaut
            }
            
            public enum CauseActorTypeValue : short
            {
                None,
                Elite,
                Jackal,
                Grunt,
                Hunter,
                Engineer,
                Assassin,
                Player,
                Marine,
                Crew,
                CombatForm,
                InfectionForm,
                CarrierForm,
                Monitor,
                Sentinel,
                None1,
                MountedWeapon,
                Brute,
                Prophet,
                Bugger,
                Juggernaut
            }
            
            public enum CauseTypeValue : short
            {
                None,
                Player,
                Actor,
                Biped,
                Body,
                Vehicle,
                Projectile,
                ActorOrPlayer,
                Turret,
                UnitInVehicle,
                UnitInTurret,
                Driver,
                Gunner,
                Passenger,
                Postcombat,
                PostcombatWon,
                PostcombatLost,
                PlayerMasterchief,
                PlayerDervish,
                Heretic,
                MajorlyScary,
                LastManInVehicle,
                Male,
                Female,
                Grenade
            }
            
            public enum SubjectTypeValue : short
            {
                None,
                Player,
                Actor,
                Biped,
                Body,
                Vehicle,
                Projectile,
                ActorOrPlayer,
                Turret,
                UnitInVehicle,
                UnitInTurret,
                Driver,
                Gunner,
                Passenger,
                Postcombat,
                PostcombatWon,
                PostcombatLost,
                PlayerMasterchief,
                PlayerDervish,
                Heretic,
                MajorlyScary,
                LastManInVehicle,
                Male,
                Female,
                Grenade
            }
            
            public enum SpatialRelationValue : short
            {
                None,
                VeryNear1wu,
                Near25wus,
                MediumRange5wus,
                Far10wus,
                VeryFar10wus,
                InFrontOf,
                Behind,
                AboveDelta1Wu,
                BelowDelta1Wu
            }
            
            [Flags]
            public enum ConditionsValue : uint
            {
                Asleep = 1 << 0,
                Idle = 1 << 1,
                Alert = 1 << 2,
                Active = 1 << 3,
                UninspectedOrphan = 1 << 4,
                DefiniteOrphan = 1 << 5,
                CertainOrphan = 1 << 6,
                VisibleEnemy = 1 << 7,
                ClearLosEnemy = 1 << 8,
                DangerousEnemy = 1 << 9,
                NoVehicle = 1 << 10,
                VehicleDriver = 1 << 11,
                VehiclePassenger = 1 << 12
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class DialogueDataBlock : TagStructure
        {
            public short StartIndexPostprocess;
            public short LengthPostprocess;
        }
        
        [TagStructure(Size = 0x4)]
        public class InvoluntaryDataBlock : TagStructure
        {
            public short InvoluntaryVocalizationIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
    }
}

