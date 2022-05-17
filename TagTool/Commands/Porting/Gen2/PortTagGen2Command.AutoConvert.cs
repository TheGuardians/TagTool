﻿using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
    {
        public void TranslateTagStructure(TagStructure input, TagStructure output, Type inputtype, Type outputtype)
        {
            var inputinfo = TagStructure.GetTagStructureInfo(inputtype, Gen2Cache.Version, Gen2Cache.Platform);
            var outputinfo = TagStructure.GetTagStructureInfo(outputtype, Cache.Version, Cache.Platform);

            //use an ordered list so we can use binary searches to decrease iterations
            List<TagFieldInfo> outputfieldlist = TagStructure.GetTagFieldEnumerable(outputinfo.Types[0], outputinfo.Version, outputinfo.CachePlatform).ToList();
            outputfieldlist = outputfieldlist.OrderBy(n => n.Name).ToList();
            List<string> outputnamelist = outputfieldlist.Select(n => n.Name.ToUpper()).ToList();

            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(inputinfo.Types[0], inputinfo.Version, inputinfo.CachePlatform))
            {
                //don't bother converting the value if it is null, padding, or runtime
                if (tagFieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.Padding) ||
                    tagFieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.Runtime) ||
                    tagFieldInfo.GetValue(input) == null)
                    continue;

                //binary search to find a field in the output with a matching name
                string query = tagFieldInfo.Name.ToUpper();
                int matchindex = outputnamelist.BinarySearch(query);
                if (matchindex >= 0)
                {
                    var outputFieldInfo = outputfieldlist[matchindex];

                    //if field types match, just assign value
                    if (tagFieldInfo.FieldType == outputFieldInfo.FieldType)
                    {
                        outputFieldInfo.SetValue(output, tagFieldInfo.GetValue(input));
                    }
                    //if its a sub-tagstructure, iterate into it
                    else if (tagFieldInfo.FieldType.BaseType == typeof(TagStructure) &&
                        outputFieldInfo.FieldType.BaseType == typeof(TagStructure))
                    {
                        var outstruct = Activator.CreateInstance(outputFieldInfo.FieldType);
                        TranslateTagStructure((TagStructure)tagFieldInfo.GetValue(input), (TagStructure)outstruct, tagFieldInfo.FieldType, outputFieldInfo.FieldType);
                        outputFieldInfo.SetValue(output, outstruct);
                    }
                    //if its a tagblock, call convertlist to iterate through and convert each one and return a converted list
                    else if (tagFieldInfo.FieldType.IsGenericType && tagFieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>) &&
                        outputFieldInfo.FieldType.IsGenericType && tagFieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        object inputlist = tagFieldInfo.GetValue(input);
                        object outputlist = Activator.CreateInstance(outputFieldInfo.FieldType);
                        TranslateList(inputlist, outputlist, tagFieldInfo.FieldType, outputFieldInfo.FieldType);
                        outputFieldInfo.SetValue(output, outputlist);
                    }
                    //if its an enum, try to parse the value
                    else if (tagFieldInfo.FieldType.BaseType == typeof(Enum) &&
                        outputFieldInfo.FieldType.BaseType == typeof(Enum))
                    {
                        var outenum = Activator.CreateInstance(outputFieldInfo.FieldType);
                        if (EnumTryParse(tagFieldInfo.GetValue(input).ToString(), out outenum, outputFieldInfo.FieldType))
                        {
                            outputFieldInfo.SetValue(output, outenum);
                        }
                    }

                    //remove the matched values from the output lists to decrease the size of future searches
                    outputfieldlist.RemoveAt(matchindex);
                    outputnamelist.RemoveAt(matchindex);
                }
            }
        }
        public void TranslateList(object input, object output, Type inputtype, Type outputtype)
        {
            IEnumerable<object> enumerable = input as IEnumerable<object>;
            if (enumerable == null)
                throw new InvalidOperationException("listData must be enumerable");

            Type outputelementType = outputtype.GenericTypeArguments[0];
            Type inputelementType = inputtype.GenericTypeArguments[0];
            var addMethod = outputtype.GetMethod("Add");
            foreach (object item in enumerable.OfType<object>())
            {
                var outputelement = Activator.CreateInstance(outputelementType);
                TranslateTagStructure((TagStructure)item, (TagStructure)outputelement, inputelementType, outputelementType);
                addMethod.Invoke(output, new[] { outputelement });
            }
        }
        private static bool EnumTryParse<T>(string input, out T theEnum, Type enumType)
        {
            string setstring = "";
            bool result = false;
            foreach (string en in Enum.GetNames(enumType))
            {
                if (input.ToUpper().Contains(en.ToUpper()))
                {
                    if(result)
                        setstring += $",{en}";
                    else
                        setstring += $"{en}";
                    result = true;
                }
            }

            if(result)
                theEnum = (T)Enum.Parse(enumType, setstring, true);
            else
                theEnum = default(T);
            return result;
        }
    }
}
