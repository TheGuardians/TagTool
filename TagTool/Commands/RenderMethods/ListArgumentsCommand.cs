using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderMethods
{
    class ListArgumentsCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private RenderMethod Definition { get; }

        public ListArgumentsCommand(GameCache cache, CachedTag tag, RenderMethod definition)
            : base(true,

                 "ListArguments",
                 "Lists the arguments of the render_method.",

                 "ListArguments [Full] [Head] {SearchTarget:String}...",

                 "e.g. ListArguments environment_map:* fresnel_color:0.5* head full\n" +
                 "e.g. ListArguments use_material_texture:true material_texture:*unsc*\n\n" +

                  "- [Full] displays all arguments rather than hiding non matched.\n" +
                  "- [Head] displays shader and template paths at top of result.\n" +
                  "- Boolean fields can be matched using 'true', 'false' or '*'.\n" +
                  "- Wildcard '*' matches 0+ characters or bool in either state.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            var invalidArgs = new List<string>();
            var filter = (Terms: new List<(string Subject, string Needle)>(), ShowFullResult: false, ShowHead: false);
            StringBuilder output = new StringBuilder();

            while (args.Count >= 1)
            {
                var option = args[0].ToLower().Split(':');
                args.RemoveAt(0);

                // Is tag group
                if (option.Length == 1)
                {
                    if (option[0] == "full")
                        filter.ShowFullResult = true;
                    if (option[0] == "head")
                        filter.ShowHead = true;
                }
                // Is filter
                else
                {
                    // Grab option value from next arg if needed
                    if (option[1] == "")
                    {
                        if (args.Count == 0 || args[0].Contains(":"))
                        {
                            invalidArgs.Add(option[0] + ":");
                            continue;
                        }
                        else
                        {
                            option[1] = args[0].ToLower();
                            args.RemoveAt(0);
                        }
                    }
                    filter.Terms.Add((option[0], option[1]));
                }
            }

            
            // Warn of invalid tag groups/options
            if (invalidArgs.Count > 0)
                return new TagToolError(CommandError.ArgInvalid, $"\"{String.Join(", ", invalidArgs.ToArray())}\"");


            if (filter.Terms.Count == 0)
                filter.ShowFullResult = true;


            if (filter.ShowFullResult)
                output.AppendLine("\n  Options");

            RenderMethodDefinition shader;
            using (var cacheStream = Cache.OpenCacheRead())
                shader = Cache.Deserialize<RenderMethodDefinition>(cacheStream, Definition.BaseRenderMethod);

            for (var i = 0; i < Definition.Options.Count; i++)
            {
                var category = Cache.StringTable.GetString(shader.Categories[i].Name);
                var option = Cache.StringTable.GetString(shader.Categories[i].ShaderOptions[Definition.Options[i].OptionIndex].Name);

                if (filter.ShowFullResult)
                    output.AppendLine(String.Format("    {0} - {1}", category, option));
            }

            foreach (var property in Definition.ShaderProperties)
            {
                RenderMethodTemplate template;
                
                using (var cacheStream = Cache.OpenCacheRead())
                    template = Cache.Deserialize<RenderMethodTemplate>(cacheStream, property.Template);

                if (filter.ShowHead)
                    output.AppendLine(String.Format($"\n\n{Tag}\n  {property.Template}"));


                if (template.BooleanParameterNames.Count > 0)
                {
                    var boolBitArray = new BitArray(BitConverter.GetBytes(property.BooleanConstants));

                    if (filter.ShowFullResult)
                        output.AppendLine("\n  Boolean");
                    
                    for (var i = 0; i < template.BooleanParameterNames.Count; i++)
                    {
                        var paramName = Cache.StringTable.GetString(template.BooleanParameterNames[i].Name);
                        var index = -1;
                        if (filter.Terms.Count > 0)
                        {
                            index = filter.Terms.FindIndex(term => term.Subject == paramName && (term.Needle == "*" || term.Needle == (boolBitArray[i] ? "true" : "false")));
                            if (index > -1)
                                filter.Terms.RemoveAt(index);
                        }
                        if (filter.ShowFullResult || index > -1)
                            output.AppendLine(String.Format("    [{0}] {1}", boolBitArray[i] ? "X" : " ", paramName));
                    }
                }


                //if (template.IntegerParameterNames.Count > 0)
                // Integer parameters appear to be unused


                if (template.TextureParameterNames.Count > 0)
                {
                    if (filter.ShowFullResult)
                        output.AppendLine("\n  Texture");

                    for (int i = 0; i < template.TextureParameterNames.Count; i++)
                    {
                        var paramName = Cache.StringTable.GetString(template.TextureParameterNames[i].Name);
                        var paramValue = property.TextureConstants[i].Bitmap.ToString();
                        var index = -1;
                        if (filter.Terms.Count > 0)
                        {
                            index = filter.Terms.FindIndex(term => term.Subject == paramName && WildcardMatch(term.Needle, paramValue));
                            if (index > -1)
                                filter.Terms.RemoveAt(index);
                        }
                        if (filter.ShowFullResult || index > -1)
                            output.AppendLine(String.Format("    {0}: {1}", paramName, paramValue));
                    }
                }


                if (template.RealParameterNames.Count > 0)
                {
                    if (filter.ShowFullResult)
                        output.AppendLine("\n  Real");

                    for (int i = 0; i < template.RealParameterNames.Count; i++)
                    {
                        var paramName = Cache.StringTable.GetString(template.RealParameterNames[i].Name);
                        var paramValues = property.RealConstants[i].Values;
                        var index = -1;
                        if (filter.Terms.Count > 0)
                        {
                            index = filter.Terms.FindIndex(term => term.Subject == paramName && paramValues.Any(val => WildcardMatch(term.Needle, val.ToString())));
                            if (index > -1)
                                filter.Terms.RemoveAt(index);
                        }
                        if (filter.ShowFullResult || index > -1)
                            output.AppendLine(String.Format("    {0}: {1} {2} {3} {4}", paramName, paramValues[0], paramValues[1], paramValues[2], paramValues[3]));
                    }
                }
            }

            if (filter.Terms.Count == 0)
                Console.Write(output);

            return true;
        }
        private bool WildcardMatch(string needle, string haystack) {
            return Regex.IsMatch(haystack.ToLower(), "^" + Regex.Escape(needle).Replace("\\*", ".*") + "$");
        }
    }
}