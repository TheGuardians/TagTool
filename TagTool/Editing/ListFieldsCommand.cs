using System;
using System.Collections.Generic;
using System.Collections;
using BlamCore.Serialization;
using BlamCore.Common;
using BlamCore.Cache;
using BlamCore.Commands;

namespace TagTool.Editing
{
    class ListFieldsCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private TagStructureInfo Structure { get; }
        private object Value { get; }
        
        public ListFieldsCommand(GameCacheContext cacheContext, TagStructureInfo structure, object value)
            : base(CommandFlags.Inherit,

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
            
            var enumerator = new TagFieldEnumerator(Structure);

            while (enumerator.Next())
            {
                if (enumerator.Attribute != null && enumerator.Attribute.Padding == true)
                    continue;

                var nameString = enumerator.Field.Name;

                if (match && !nameString.ToLower().Contains(token))
                    continue;

                var fieldType = enumerator.Field.FieldType;
                var fieldValue = enumerator.Field.GetValue(Value);

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
                    else if (fieldType == typeof(StringId))
                        valueString = CacheContext.GetString((StringId)fieldValue);
                    else if (fieldType == typeof(CachedTagInstance))
                    {
                        var instance = (CachedTagInstance)fieldValue;

                        var tagName = CacheContext.TagNames.ContainsKey(instance.Index) ?
                            CacheContext.TagNames[instance.Index] :
                            $"0x{instance.Index:X4}";

                        valueString = $"[0x{instance.Index:X4}] {tagName}.{CacheContext.GetString(instance.Group.Name)}";
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

                var fieldName = $"{enumerator.Field.DeclaringType.FullName}.{enumerator.Field.Name}".Replace("+", ".");
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
