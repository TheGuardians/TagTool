using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class TinyPositionVertexDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(0, 0, DeclarationType.Short4N, DeclarationMethod.Default, DeclarationUsage.Position, 0),

                VertexElement.VertexDeclarationEnd
            });
        }

        public override Dictionary<int, Type> GetStreamTypes() => new Dictionary<int, Type>
        {
            [0] = typeof(Stream0Data)
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream0Data
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] Position; // Short4N
        }
    }
}