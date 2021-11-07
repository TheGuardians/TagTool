using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using TagTool.Commands.Common;

namespace TagTool.Commands.Porting
{
    public class SetPortingOptionCommand : Command
    {
        public SetPortingOptionCommand()
               : base(true,

                     "SetPortingOption",
                     "Sets an option to be used when porting.",

                     "SetPortingOption <option> <value>",

                     BuildHelpMessage())

        {
            PortingOptions.Current = new PortingOptions();
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2)
                return new TagToolError(CommandError.ArgCount);

            var field = typeof(PortingOptions)
                .GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                .FirstOrDefault(x => string.Compare(x.Name, args[0], true) == 0);

            if (field == null)
                return new TagToolError(CommandError.ArgInvalid, $"Unknown porting option: '{args[0]}'");


            object value;

            try
            {
                if (field.FieldType.IsEnum)
                    value = Enum.Parse(field.FieldType, args[1], true);
                else
                    value = Convert.ChangeType(args[1], field.FieldType);
            }
            catch
            {
                return new TagToolError(CommandError.ArgInvalid, $"Invalid value given for porting option: '{args[0]}', value: '{args[1]}'");
            }

            var oldValue = field.GetValue(PortingOptions.Current);
            field.SetValue(PortingOptions.Current, value);

            Console.WriteLine($"{oldValue} -> {value}");
            return true;
        }

        private static string BuildHelpMessage()
        {
            var writer = new StringBuilder();
            writer.AppendLine("Sets an option to be used when porting");
            writer.AppendLine("Available Options:");
            writer.AppendLine();

            var fields = typeof(PortingOptions).GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            var padCount = fields.Max(x => x.Name.Length);

            foreach (var field in fields.OrderBy(x => x.Name))
            {
                var attr = field.GetCustomAttribute<DescriptionAttribute>();
                writer.AppendLine($"{field.Name.PadRight(padCount)} - {attr.Description}");
                if (field.FieldType.IsEnum)
                    writer.AppendLine($"{"".PadRight(padCount)} - {{{string.Join(", ", Enum.GetNames(field.FieldType))}}}");
            }
            return writer.ToString();
        }
    }
}
