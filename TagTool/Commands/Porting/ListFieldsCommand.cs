using System;
using System.Collections.Generic;
using System.Collections;
using TagTool.Serialization;
using TagTool.Common;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Commands.Porting
{
    class ListFieldsCommand : Command
    {
        private CacheFile BlamCache;
        private TagStructureInfo Structure { get; }
        private object Value { get; }
        
        public ListFieldsCommand(CacheFile blamCache, TagStructureInfo structure, object value)
            : base(true,

                  "ListFields",
                  $"Lists the fields in the current {structure.Types[0].Name} definition.",

                  "ListFields",

                  $"Lists the fields in the current {structure.Types[0].Name} definition.")
        {
            BlamCache = blamCache;
            Structure = structure;
            Value = value;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;

            var match = (args.Count == 1);
            var token = match ? args[0].ToLower() : "";

			foreach (var tagFieldInfo in ReflectionCache.GetTagFieldEnumerable(Structure))
			{
				if (tagFieldInfo.Attribute != null && tagFieldInfo.Attribute.Padding == true)
					continue;

				var nameString = tagFieldInfo.Name;

				if (match && !nameString.ToLower().Contains(token))
					continue;

				var fieldType = tagFieldInfo.FieldType;
				var fieldValue = tagFieldInfo.GetValue(Value);

				var typeString =
					fieldType.IsGenericType ?
						$"{fieldType.Name}<{fieldType.GenericTypeArguments[0].Name}>" :
					fieldType.Name;

				string valueString;

#if !DEBUG
                try
                {
#endif
				if (fieldValue == null)
					valueString = "null";
				else if (fieldType.GetInterface(typeof(IList).Name) != null)
					valueString =
						((IList)fieldValue).Count != 0 ?
							$"{{...}}[{((IList)fieldValue).Count}]" :
						"null";
				else if (fieldValue is StringId stringId)
					valueString = BlamCache.Version < CacheVersion.Halo3Retail ?
						BlamCache.Strings.GetItemByID((int)(stringId.Value & 0xFFFF)) :
						BlamCache.Strings.GetString(stringId);
				else if (fieldType == typeof(CachedTagInstance))
				{
					var instance = (CachedTagInstance)fieldValue;

					var item = BlamCache.IndexItems.GetItemByID(instance.Index);

					valueString = item == null ? "<null>" : $"[0x{instance.Index:X8}] {item.Name}.{item.GroupName}";
				}
				else if (fieldType == typeof(TagFunction))
				{
					var function = (TagFunction)fieldValue;
					valueString = "";
					foreach (var datum in function.Data)
						valueString += datum.ToString("X2");
				}
				else if (fieldType == typeof(PageableResource))
				{
					var pageable = (PageableResource)fieldValue;
					pageable.GetLocation(out var location);
					valueString = pageable == null ? "null" : $"{{ Location: {location}, Index: 0x{pageable.Page.Index:X4}, CompressedSize: 0x{pageable.Page.CompressedBlockSize:X8} }}";
				}
				else
					valueString = fieldValue.ToString();
#if !DEBUG
                }
                catch (Exception e)
                {
                    valueString = $"<ERROR MESSAGE=\"{e.Message}\" />";
                }
#endif

				Console.WriteLine("{0}: {1} = {2}", nameString, typeString, valueString);
			}

			return true;
        }
    }
}
