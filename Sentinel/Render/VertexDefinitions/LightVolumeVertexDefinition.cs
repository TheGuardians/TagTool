using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class LightVolumeVertexDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(2, 0, DeclarationType.Short2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 1),

                VertexElement.VertexDeclarationEnd
            });
        }

        public override Dictionary<int, Type> GetStreamTypes() => new Dictionary<int, Type>
        {
            [2] = typeof(Stream2Data)
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream2Data
        {
            public short U, V; // Short2
        }
    }
}