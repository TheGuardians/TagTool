using System;
using System.Collections.Generic;
using System.Collections;
using TagTool.Tags;
using TagTool.Common;
using TagTool.Cache;

namespace TagTool.Commands.Editing
{
    class ListFieldsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private TagStructureInfo Structure { get; }
        private object Value { get; }
        
        public ListFieldsCommand(HaloOnlineCacheContext cacheContext, TagStructureInfo structure, object value)
            : base(true,

                  "ListFields",
                  $"Lists the fields in the current {structure.Types[0].Name} definition.",

                  "ListFields",

                  $"Lists the fields in the current {structure.Types[0].Name} definition.")
        {
            CacheContext = cacheContext;
            Structure = structure;
            Value = value;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;

            var match = (args.Count == 1);
            var token = match ? args[0].ToLower() : "";

			foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(Structure))
			{
				if (tagFieldInfo.Attribute != null && tagFieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.Padding))
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
				else if (fieldType == typeof(StringId))
					valueString = CacheContext.GetString((StringId)fieldValue);
				else if (fieldType == typeof(CachedTagInstance))
				{
					var instance = (CachedTagInstance)fieldValue;

					var tagName = instance?.Name ?? $"0x{instance.Index:X4}";

					valueString = $"[0x{instance.Index:X4}] {tagName}.{CacheContext.GetString(instance.Group.Name)}";
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
                else if (tagFieldInfo.FieldType.IsArray && tagFieldInfo.Attribute.Length != 0)
                {
                    valueString = fieldValue == null ? "null" : $"[{tagFieldInfo.Attribute.Length}] {{ ";
                    var valueArray = (Array)fieldValue;

                    if (fieldValue != null)
                    {
                        for (var i = 0; i < tagFieldInfo.Attribute.Length; i++)
                            valueString += $"{valueArray.GetValue(i)}{((i + 1) < tagFieldInfo.Attribute.Length ? "," : "")} ";

                        valueString += "}";
                    }
                }
                else if (fieldType.GetInterface(typeof(IList).Name) != null)
                    valueString =
                        ((IList)fieldValue).Count != 0 ?
                            $"{{...}}[{((IList)fieldValue).Count}]" :
                        "null";
                else
					valueString = fieldValue.ToString();
#if !DEBUG
                }
                catch (Exception e)
                {
                    valueString = $"<ERROR MESSAGE=\"{e.Message}\" />";
                }
#endif

				var fieldName = $"{tagFieldInfo.DeclaringType.FullName}.{tagFieldInfo.Name}".Replace("+", ".");
				var documentationNode = EditTagContextFactory.Documentation.SelectSingleNode($"//member[starts-with(@name, 'F:{fieldName}')]");

                Console.WriteLine("{0}: {1} = {2} {3}", nameString, typeString, valueString,
					documentationNode != null ?
						$":: {documentationNode.FirstChild.InnerText.Replace("\r\n", "").TrimStart().TrimEnd()}" :
						"");
			}

			return true;
        }
    }
}
