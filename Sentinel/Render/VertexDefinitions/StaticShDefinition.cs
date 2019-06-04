using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class StaticShDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[] { VertexElement.VertexDeclarationEnd });
        }

        public override Dictionary<int, Type> GetStreamTypes() => new Dictionary<int, Type>();
    }
}