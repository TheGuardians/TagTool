using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class StaticPerPixelDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(4, 0, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 1),

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
            public Vector2 Texcoord;
        }
    }
}