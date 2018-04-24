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
	public class Compile
	{
		/// <summary>
		/// Compiles HLSL sourcecode to bytecode.
		/// </summary>
		/// <param name="code"> HLSL source code to compile. </param>
		/// <param name="main"> Name of the main entry point in the HLSL source. </param>
		/// <param name="profile"> Shader profile to compile the source against (i.e. ps_3_0). </param>
		/// <param name="errors">Pointer to a string to receive error messages. Will be set to <c>null</c> if errors are not available.</param>
		/// <returns>The bytecode for the shader if successful, or <c>null</c> otherwise.</returns>
		public Compile(string code, string main, string profile, out string errors, out byte[] data)
		{
			ID3DBlob codeBlob = null, errorBlob = null;
			errors = null;

			try
			{
				var result = D3DCompile(code, (uint)code.Length, null, IntPtr.Zero, IntPtr.Zero, main, profile, 0, 0, out codeBlob, out errorBlob);
				if (result != (int)ReturnCode.S_OK || codeBlob == null)
				{
					if (errorBlob == null)
						data = null;
					var errorPtr = errorBlob.GetBufferPointer();
					errors = Marshal.PtrToStringAnsi(errorPtr);
					data = null;
					return;
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


		/// <summary>
		///  Docs: https://msdn.microsoft.com/en-us/library/windows/desktop/dd607324(v=vs.85).aspx
		/// </summary>
		[DllImport("d3dcompiler_47.dll", CharSet = CharSet.Auto)]
		[PreserveSig]
		private static extern int D3DCompile(
			[MarshalAs(UnmanagedType.LPStr)] string pSrcData,
			uint SrcDataSize,
			[MarshalAs(UnmanagedType.LPStr)] string pSourceName,
			IntPtr pDefines,
			IntPtr pInclude,
			[MarshalAs(UnmanagedType.LPStr)] string pEntrypoint,
			[MarshalAs(UnmanagedType.LPStr)] string pTarget,
			uint Flags1,
			uint Flags2,
			out ID3DBlob ppCode,
			out ID3DBlob ppErrorMsgs
		);
	}
}
