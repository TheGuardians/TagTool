using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Cache.Gen2;
using System.IO;
using TagTool.Commands.Common;
using TagTool.Commands.Porting;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public TagStructure ConvertEffect(object gen2Tag)
        {
            switch (gen2Tag)
            {
                case TagTool.Tags.Definitions.Gen2.DamageEffect damage:
                    DamageEffect newdamage = new DamageEffect();
                    AutoConverter.TranslateTagStructure(damage, newdamage);
                    return newdamage;
                default:
                    return null;
            }
        }
    }
}
