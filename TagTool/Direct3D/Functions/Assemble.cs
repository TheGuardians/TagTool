using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TagTool.Direct3D.Interfaces;

namespace TagTool.Direct3D.Functions
{
	public class Assemble
	{
		/// <summary>
		/// Assembles HLSL shader assembly.
		/// </summary>
		/// <param name="code">The code to assemble.</param>
		/// <param name="errors">Pointer to a string to receive error messages. Will be set to <c>null</c> if errors are not available.</param>
		/// <returns>The bytecode for the shader if successful, or <c>null</c> otherwise.</returns>
		public Assemble(string code, out string errors, out byte[] data)
		{
			ID3DBlob codeBlob = null, errorBlob = null;
			errors = null;
			try
			{
				var result = D3DAssemble(code, (uint)code.Length, null, IntPtr.Zero, IntPtr.Zero, 0, out codeBlob, out errorBlob);
				if (result != (int)ReturnCode.S_OK || codeBlob == null)
				{
					if (errorBlob == null)
						data = null;
					var errorPtr = errorBlob.GetBufferPointer();
					errors = Marshal.PtrToStringAnsi(errorPtr);
					data = null;
				}
				data = new byte[codeBlob.GetBufferSize()];
				var bufferPtr = codeBlob.GetBufferPointer();
				Marshal.Copy(bufferPtr, data, 0, data.Length);
			}
			finally
			{
				if (codeBlob != null)
					Marshal.ReleaseComObject(codeBlob);
				if (errorBlob != null)
					Marshal.ReleaseComObject(errorBlob);
			}
		}

		// This API is undocumented but it seems to work
		// http://www.ogre3d.org/forums/viewtopic.php?f=4&t=69209&start=100#p467402
		[DllImport("d3dcompiler_47.dll", CharSet = CharSet.Auto)]
		[PreserveSig]
		private static extern int D3DAssemble(
			[MarshalAs(UnmanagedType.LPStr)] string pSrcData,
			uint SrcDataSize,
			[MarshalAs(UnmanagedType.LPStr)] string pSourceName,
			IntPtr pDefines,
			IntPtr pInclude,
			uint Flags,
			out ID3DBlob ppCode,
			out ID3DBlob ppErrorMsgs
		);
	}
}
