using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TagTool.Direct3D.Enums;
using TagTool.Direct3D.Interfaces;

namespace TagTool.Direct3D.Functions
{
	public class Disassemble
	{
		/// <summary>
		/// Disassembles HLSL shader bytecode.
		/// </summary>
		/// <param name="shader">The shader to disassemble.</param>
		/// <returns>The disassembled shader if successful, or <c>null</c> otherwise.</returns>
		public unsafe Disassemble(byte[] shader, out string asm)
		{
			ID3DBlob blob = null;
			try
			{
				int result;
				fixed (byte* buffer = shader)
					result = D3DDisassemble((IntPtr)buffer, (uint)shader.Length, 0, null, out blob);
				if (result != (int)ReturnCode.S_OK || blob == null)
				{
					asm = null;
					return;
				}
				var textPtr = blob.GetBufferPointer();
				asm = Marshal.PtrToStringAnsi(textPtr);
			}
			finally
			{
				if (blob != null)
					Marshal.ReleaseComObject(blob);
			}
		}

		// Docs: https://msdn.microsoft.com/en-us/library/windows/desktop/dd607326(v=vs.85).aspx
		[DllImport("d3dcompiler_47.dll", CharSet = CharSet.Auto)]
		[PreserveSig]
		private static extern int D3DDisassemble(
			IntPtr pSrcData,
			uint SrcDataSize,
			uint Flags,
			[MarshalAs(UnmanagedType.LPStr)] string szComments,
			out ID3DBlob ppDisassembly
		);
	}
}
