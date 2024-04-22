using TagTool.Cache;
using TagTool.Commands.Common;
using System.Collections.Generic;
using System;

namespace TagTool.Commands.Modding
{
    class SetModTypeCommand : Command
    {
        public GameCacheModPackage Cache { get; }

        public SetModTypeCommand(GameCacheModPackage cache) :
            base(true,

                "ModTypes",
                "Output current type flags",

                "ModTypes [Mainmenu Multiplayer Campaign Firefight]",

                "Sets the Mod Type flags. enter the types of the mod package. Separated by a space")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count == 0)
            {
                Console.WriteLine("Flags: " + Cache.BaseModPackage.Header.ModifierFlags.ToString().Replace("SignedBit", ""));
                return true;
            }

            Cache.BaseModPackage.Header.ModifierFlags = Cache.BaseModPackage.Header.ModifierFlags & ModifierFlags.SignedBit;

            for (int x = 0; x < args.Count; x++)
            {
                if (Enum.TryParse<ModifierFlags>(args[x].ToLower(), out var value) && args[x] != "SignedBit")
                {
                    Cache.BaseModPackage.Header.ModifierFlags |= value;
                }
                else
                    return new TagToolError(CommandError.CustomError, $"Could not parse \"{args[x]}\" as mod type");
            }

            Console.WriteLine("Flags Set: " + Cache.BaseModPackage.Header.ModifierFlags.ToString().Replace("SignedBit", ""));

            return true;
        }
    }
}