using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Strings
{
    public enum StringIDType
    {
        Tag = 0x0,
        GUI = 0x1,
        ContentPrompts = 0x2,
        GameplayPrompts = 0x3,
        MultiplayerEvents = 0x4,
        Network = 0x5,
        Events = 0x6,
        Blf = 0x7,
        Globals = 0x8
    }
}
