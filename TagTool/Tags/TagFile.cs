using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags
{
    [TagStructure(Name = "tag_file", Tag = "tagf", Size = 0x0)]
    public class TagFile : TagStructure
    {
        /// <summary>
        /// The version of the tag definition in the tag file.
        /// </summary>
        public CacheVersion Version;

        /// <summary>
        /// The group of the tag definition in the file.
        /// </summary>
        public TagGroup Group;

        /// <summary>
        /// All tags referenced by the tag instance in the file.
        /// </summary>
        public List<TagReference> TagReferences;

        /// <summary>
        /// All string ids referenced by the tag instance in the file.
        /// </summary>
        public List<StringReference> StringReferences;

        /// <summary>
        /// 
        /// </summary>
        public byte[] DefinitionData;

        /// <summary>
        /// A tag reference descriptor.
        /// </summary>
        [TagStructure(Size = 0x104)]
        public class TagReference : TagStructure
        {
            /// <summary>
            /// The group tag of the referenced tag.
            /// </summary>
            public Tag GroupTag;

            /// <summary>
            /// The name of the referenced tag.
            /// </summary>
            [TagField(Length = 256)]
            public string TagName;
        }

        /// <summary>
        /// A string reference descriptor.
        /// </summary>
        [TagStructure(Size = 0x100)]
        public class StringReference : TagStructure
        {
            /// <summary>
            /// The display value of the referenced string.
            /// </summary>
            [TagField(Length = 256)]
            public string Value;
        }
    }
}
