using TagTool.Havok;
using TagTool.IO;
using TagTool.Tags.Definitions;
using System;
using System.IO;
using TagTool.Serialization;
using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private PhysicsModel ConvertPhysicsModel(CachedTag instance, PhysicsModel phmo)
        {
            phmo.MoppData = HavokConverter.ConvertHkpMoppData(BlamCache.Version, CacheContext.Version, phmo.MoppData);
            return phmo;
        }
    }
}