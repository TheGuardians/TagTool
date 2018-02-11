using TagTool.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TagTool.Serialization
{
    /// <summary>
    /// Allows easy enumeration over a tag structure's elements and filtering by version.
    /// </summary>
    public class TagFieldEnumerator
    {
        private static readonly TagFieldAttribute DefaultFieldAttribute = new TagFieldAttribute();

        private readonly List<FieldInfo> _fields = new List<FieldInfo>();
        private int _nextIndex;

        /// <summary>
        /// Constructs an enumerator over a tag structure.
        /// </summary>
        /// <param name="info">The info for the structure. Only fields which match the version used to create the info will be enumerated over.</param>
        public TagFieldEnumerator(TagStructureInfo info)
        {
            Info = info;
            Begin();
        }

        /// <summary>
        /// Gets the info that was used to construct the enumerator.
        /// </summary>
        public TagStructureInfo Info { get; private set; }

        /// <summary>
        /// Gets information about the current field.
        /// </summary>
        public FieldInfo Field { get; private set; }

        /// <summary>
        /// Gets the current property's <see cref="TagFieldAttribute"/>.
        /// </summary>
        public TagFieldAttribute Attribute { get; private set; }
        
        /// <summary>
        /// Moves to the next tag field in the structure.
        /// This must be called before accessing any of the other properties.
        /// </summary>
        /// <returns><c>true</c> if the enumerator moved to a new element, or <c>false</c> if the end of the structure has been reached.</returns>
        public bool Next()
        {
            do
            {
                if (_fields == null || _nextIndex >= _fields.Count)
                    return false;
                Field = _fields[_nextIndex];
                _nextIndex++;
            } while (!GetCurrentPropertyInfo());
            return true;
        }

        private void Begin()
        {
            // Build the field list. Scan through the type's inheritance
            // hierarchy and add any fields belonging to tag structures.
            foreach (var type in Info.Types.Reverse<Type>())
            {
                var typeFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                Array.Sort(typeFields, (x, y) => x.MetadataToken - y.MetadataToken); // Ensure that fields are in declaration order - GetFields does NOT guarantee this!
                _fields.AddRange(typeFields);
            }
        }

        public FieldInfo Find(Predicate<FieldInfo> match) =>
            _fields.Find(match);

        private bool GetCurrentPropertyInfo()
        {
            // If the field has a TagFieldAttribute, use it, otherwise use the default
            Attribute = Field.GetCustomAttributes(typeof(TagFieldAttribute), false).FirstOrDefault() as TagFieldAttribute ?? DefaultFieldAttribute;
            if (Attribute.Offset >= Info.TotalSize)
                throw new InvalidOperationException("Offset for property \"" + Field.Name + "\" is outside of its structure");
            
            return CacheVersionDetection.IsBetween(Info.Version, Attribute.MinVersion, Attribute.MaxVersion);
        }
    }
}
