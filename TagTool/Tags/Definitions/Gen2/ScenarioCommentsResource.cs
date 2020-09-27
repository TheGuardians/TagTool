using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_comments_resource", Tag = "/**/", Size = 0xC)]
    public class ScenarioCommentsResource : TagStructure
    {
        public List<EditorCommentDefinition> Comments;
        
        [TagStructure(Size = 0x130)]
        public class EditorCommentDefinition : TagStructure
        {
            public RealPoint3d Position;
            public TypeValue Type;
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 256)]
            public string Comment;
            
            public enum TypeValue : int
            {
                Generic
            }
        }
    }
}

