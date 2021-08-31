using System;
using System.IO;
using System.Runtime.InteropServices;
using TagTool.Commands;

namespace TagTool.Geometry
{
	public static class MeshOptimizer
	{
		[DllImport("meshoptimizer32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint meshopt_stripifyBound(UIntPtr IndexCount);
		public static uint StripifyBound(int IndexCount)
		{
			return meshopt_stripifyBound((UIntPtr)IndexCount);
		}

		[DllImport("meshoptimizer32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint meshopt_stripify(uint[] Destination, uint[] Indices, UIntPtr IndexCount, UIntPtr VertexCount, UIntPtr RestartIndex);
		public static uint Stripify(uint[] Destination, uint[] Indices, int IndexCount, int VertexCount, int RestartIndex)
		{
			return meshopt_stripify(Destination, Indices, (UIntPtr)IndexCount, (UIntPtr)VertexCount, (UIntPtr)RestartIndex);
		}

		[DllImport("meshoptimizer32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		private static extern void meshopt_optimizeVertexCacheStrip(uint[] Destination, uint[] Indices, UIntPtr IndexCount, UIntPtr VertexCount);
		public static void OptimizeVertexCacheStrip(uint[] Destination, uint[] Indices, int IndexCount, int VertexCount)
		{
			meshopt_optimizeVertexCacheStrip(Destination, Indices, (UIntPtr)IndexCount, (UIntPtr)VertexCount);
		}

		[DllImport("meshoptimizer32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		public static extern void meshopt_encodeIndexVersion(UIntPtr IndexCount);

		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr LoadLibrary(string libraryName);

		static MeshOptimizer()
		{
			LoadLibrary(Path.Combine(Program.TagToolDirectory, "Tools", "meshoptimizer32.dll"));
			meshopt_encodeIndexVersion((UIntPtr)1);
		}
	}
}
