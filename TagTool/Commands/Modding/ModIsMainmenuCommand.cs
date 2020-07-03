using TagTool.Cache;
using System.Collections.Generic;
using System;

namespace TagTool.Commands.Modding
{
    class ModIsMainmenuCommand : Command
    {
        public GameCacheModPackage Cache { get; }

        public ModIsMainmenuCommand(GameCacheModPackage cache) :
            base(true,

                "ModIsMainmenu",
                "Sets the Menu Flag value",

                "ModIsMainmenu <1/0>",

                "Sets the Menu Flag value")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            if (args[0] == "1")
                Cache.BaseModPackage.Header.ModifierFlags |= ModifierFlags.Mainmenu;
            else if (args[0] == "0")
                Cache.BaseModPackage.Header.ModifierFlags &= ~ModifierFlags.Mainmenu;
            else
                return false;

            Console.WriteLine(Cache.BaseModPackage.Header.ModifierFlags.ToString());
            return true;
        }
    }
}