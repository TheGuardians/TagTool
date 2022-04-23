using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
    {
        public void ConvertStructure(TagStructure input, TagStructure output, Type inputtype, Type outputtype)
        {
            var inputinfo = TagStructure.GetTagStructureInfo(inputtype, Gen2Cache.Version, Gen2Cache.Platform);
            var outputinfo = TagStructure.GetTagStructureInfo(outputtype, Cache.Version, Cache.Platform);
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(inputinfo.Types[0], inputinfo.Version, inputinfo.CachePlatform))
            {
                foreach (var outputFieldInfo in TagStructure.GetTagFieldEnumerable(outputinfo.Types[0], outputinfo.Version, outputinfo.CachePlatform))
                {
                    if (tagFieldInfo.Name == outputFieldInfo.Name)
                    {
                        //if field types match, just assign value
                        if (tagFieldInfo.FieldType == outputFieldInfo.FieldType)
                        {
                            outputFieldInfo.SetValue(output, tagFieldInfo.GetValue(input));
                        }
                        //if its a sub-tagstructure, iterate into it
                        else if (tagFieldInfo.FieldType == typeof(TagStructure) &&
                            outputFieldInfo.FieldType == typeof(TagStructure))
                        {
                            ConvertStructure((TagStructure)tagFieldInfo.GetValue(input), (TagStructure)outputFieldInfo.GetValue(output), tagFieldInfo.FieldType, outputFieldInfo.FieldType);
                        }
                        //if its a tagblock, call convertlist to iterate through and convert each one and return a converted list
                        else if (tagFieldInfo.FieldType.IsGenericType && tagFieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>) &&
                            outputFieldInfo.FieldType.IsGenericType && tagFieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            object inputlist = tagFieldInfo.GetValue(input);
                            outputFieldInfo.SetValue(output, ConvertList(inputlist, tagFieldInfo, outputFieldInfo));
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
                        break;
                    }
                }
            }
        }
        public object ConvertList(object inputlist, TagFieldInfo inputFieldInfo, TagFieldInfo outputFieldInfo)
        {
            IEnumerable<object> enumerable = inputlist as IEnumerable<object>;
            if (enumerable == null)
                throw new InvalidOperationException("listData must be enumerable");

            var outputlist = Activator.CreateInstance(outputFieldInfo.FieldType);
            Type outputelementType = outputFieldInfo.FieldType.GenericTypeArguments[0];
            Type inputelementType = inputFieldInfo.FieldType.GenericTypeArguments[0];
            var addMethod = outputFieldInfo.FieldType.GetMethod("Add");
            foreach (object item in enumerable.OfType<object>())
            {
                var outputelement = Activator.CreateInstance(outputelementType);
                ConvertStructure((TagStructure)item, (TagStructure)outputelement, inputelementType, outputelementType);
                addMethod.Invoke(outputlist, new[] { outputelement });
            }
            return outputlist;
        }
        public static bool EnumTryParse<T>(string input, out T theEnum, Type enumType)
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
