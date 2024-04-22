using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "style_sheet_list", Tag = "uiss", Size = 0x30)]
    public class StyleSheetList : TagStructure
    {
        public List<UserInterfaceStyleSheetLanguagesBlock> StyleSheetLanguages;
        public List<UserInterfaceStyleSheetGradientBlock> Gradients;
        public List<UserInterfaceStyleSheetDropshadowBlock> Dropshadows;
        public List<UserInterfaceStyleSheetOuterGlowBlock> OuterGlows;
        
        [TagStructure(Size = 0x1C)]
        public class UserInterfaceStyleSheetLanguagesBlock : TagStructure
        {
            public StyleSheetLanguageEnum Language;
            public List<UserInterfaceStyleSheetsFontBlock> FontStyleSheets;
            public List<UserInterfaceStyleSheetsVisualBlock> VisualStyleSheets;
            
            public enum StyleSheetLanguageEnum : int
            {
                English,
                Japanese,
                German,
                French,
                Spanish,
                MexicanSpanish,
                Italian,
                Korean,
                ChineseTraditional,
                ChineseSimplified,
                Portuguese,
                Polish,
                Russian,
                Danish,
                Finnish,
                Dutch,
                Norwegian
            }
            
            [TagStructure(Size = 0x4C)]
            public class UserInterfaceStyleSheetsFontBlock : TagStructure
            {
                public StringId StyleSheetName;
                public List<UserInterfaceStyleSheetFontIdBlock> FontId;
                public List<UserInterfaceStyleSheetTextCaseBlock> TextCase;
                public List<UserInterfaceStyleSheetJustificationBlock> Justification;
                public List<UserInterfaceStyleSheetAlignmentBlock> Alignment;
                public List<UserInterfaceStyleSheetScaleBlock> Scale;
                public List<UserInterfaceStyleSheetFixedHeightBlock> FixedHeight;
                
                [TagStructure(Size = 0x4)]
                public class UserInterfaceStyleSheetFontIdBlock : TagStructure
                {
                    public GlobalFontIdEnum FontId;
                    
                    public enum GlobalFontIdEnum : int
                    {
                        TerminalFont,
                        Baksheesh15Font,
                        Baksheesh16Font,
                        Baksheesh17Font,
                        Baksheesh18Font,
                        Baksheesh20Font,
                        Baksheesh21Font,
                        Baksheesh22Font,
                        Baksheesh28Font,
                        Baksheesh36Font,
                        Baksheesh38Font,
                        BaksheeshBold16Font,
                        BaksheeshBold20Font,
                        BaksheeshBold21Font,
                        BaksheeshBold23Font,
                        BaksheeshBold24Font,
                        BaksheeshThin36,
                        BaksheeshThin42,
                        ArameRegular16,
                        ArameRegular18,
                        ArameRegular23,
                        ArameStencil16,
                        ArameStencil18,
                        ArameStencil23,
                        ArameThin14,
                        ArameThin16,
                        ArameThin18,
                        ArameThin23,
                        ArameExtra01,
                        ArameExtra02,
                        ArameExtra03
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class UserInterfaceStyleSheetTextCaseBlock : TagStructure
                {
                    public TextCaseEnum TextCase;
                    
                    public enum TextCaseEnum : int
                    {
                        Normal,
                        Uppercase,
                        Lowercase
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class UserInterfaceStyleSheetJustificationBlock : TagStructure
                {
                    public JustificationEnum Justification;
                    
                    public enum JustificationEnum : int
                    {
                        Left,
                        Right,
                        Center
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class UserInterfaceStyleSheetAlignmentBlock : TagStructure
                {
                    public AlignmentEnum Alignment;
                    
                    public enum AlignmentEnum : int
                    {
                        Top,
                        Bottom,
                        Center
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class UserInterfaceStyleSheetScaleBlock : TagStructure
                {
                    public float Value;
                }
                
                [TagStructure(Size = 0x4)]
                public class UserInterfaceStyleSheetFixedHeightBlock : TagStructure
                {
                    public float Height;
                }
            }
            
            [TagStructure(Size = 0x34)]
            public class UserInterfaceStyleSheetsVisualBlock : TagStructure
            {
                public StringId StyleSheetName;
                public List<UserInterfaceStyleSheetDropShadowStyleBlock> DropShadowStyle;
                public List<UserInterfaceStyleSheetColorBlock> TextColor;
                public List<UserInterfaceStyleSheetColorBlock> DropShadowColor;
                public StringId GradientName;
                public StringId DropshadowName;
                public StringId OuterGlowName;
                
                [TagStructure(Size = 0x4)]
                public class UserInterfaceStyleSheetDropShadowStyleBlock : TagStructure
                {
                    public DropShadowStyleEnum DropShadowStyle;
                    
                    public enum DropShadowStyleEnum : int
                    {
                        None,
                        Drop,
                        Outline
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class UserInterfaceStyleSheetColorBlock : TagStructure
                {
                    public RealArgbColor Color;
                }
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class UserInterfaceStyleSheetGradientBlock : TagStructure
        {
            public StringId GradientName;
            public RealArgbColor Color1;
            public RealArgbColor Color2;
            public RealArgbColor Color3;
            public RealArgbColor Color4;
            public float Angle;
            public float Scale;
            public RealVector2d Offset;
            public int NumberOfColors;
            public GradientShapeEnum GradientShape;
            
            public enum GradientShapeEnum : int
            {
                Linear,
                Circular,
                Diamond,
                Square
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class UserInterfaceStyleSheetDropshadowBlock : TagStructure
        {
            public StringId DropshadowName;
            public RealArgbColor Color;
            public float Angle;
            public float Distance;
            public float Spread;
            public float Size;
        }
        
        [TagStructure(Size = 0x1C)]
        public class UserInterfaceStyleSheetOuterGlowBlock : TagStructure
        {
            public StringId OuterGlowName;
            public RealArgbColor Color;
            public float Spread;
            public float Size;
        }
    }
}
