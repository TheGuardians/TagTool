using TagTool.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TagTool.Commands.Common
{
    class SetLocaleCommand : Command
    {
        public SetLocaleCommand()
            : base(true,

                  "SetLocale",
                  "Changes the parsing locale of numbers to the specified locale.",

                  "SetLocale <locale>",

                  "Use a culture name from https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo(vs.71).aspx")
        {
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return false;

            CultureInfo ci;

        #if !DEBUG
            try
            {
        #endif
                ci = CultureInfo.GetCultureInfo(args[0]);
        #if !DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        #endif

            CultureInfo.DefaultThreadCurrentCulture = ci;

            return true;
        }
    }
}