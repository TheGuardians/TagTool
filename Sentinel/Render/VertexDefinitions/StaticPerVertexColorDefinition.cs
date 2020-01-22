using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class StaticPerVertexColorDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(4, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 3),

                VertexElement.VertexDeclarationEnd
            });
        }

        public override Dictionary<int, Type> GetStreamTypes() => new Dictionary<int, Type>
        {
            [4] = typeof(Stream4Data)
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream4Data
        {
            public Vector3 Texcoord;
        }
    }
}