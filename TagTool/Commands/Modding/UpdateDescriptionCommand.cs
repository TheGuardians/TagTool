using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Cache.HaloOnline;
using System;
using TagTool.Commands.Tags;
using System.IO;
using TagTool.Cache.Gen3;
using TagTool.IO;

namespace TagTool.Commands.Modding
{
    class UpdateDescriptionCommand : Command
    {
        private GameCacheModPackage Cache;

        public UpdateDescriptionCommand(GameCacheModPackage cache) :
            base(true,

                "UpdateDescription",
                "Update description of mod package.\n",
                "UpdateDescription",
                "Update description of mod package.\n")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return false;

            Cache.BaseModPackage.CreateDescription();

            return true;

        }
    }
}