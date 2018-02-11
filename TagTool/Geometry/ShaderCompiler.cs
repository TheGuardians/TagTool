using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TagTool.Geometry
{
    /// <summary>
    /// Utility class for invoking the D3D shader compiler functions.
    /// </summary>
    public static class ShaderCompiler
	{
		/// <summary>
		/// Assembles HLSL shader assembly.
		/// </summary>
		/// <param name="code">The code to assemble.</param>
		/// <param name="errors">Pointer to a string to receive error messages. Will be set to <c>null</c> if errors are not available.</param>
		/// <returns>The bytecode for the shader if successful, or <c>null</c> otherwise.</returns>
		public static byte[] Assemble(string code, out string errors)
		{
			ID3DBlob codeBlob = null, errorBlob = null;
			errors = null;
			try
			{
				var result = D3DAssemble(code, (uint)code.Length, null, IntPtr.Zero, IntPtr.Zero, 0, out codeBlob, out errorBlob);
				if (result != S_OK || codeBlob == null)
				{
					if (errorBlob == null)
						return null;
					var errorPtr = errorBlob.GetBufferPointer();
					errors = Marshal.PtrToStringAnsi(errorPtr);
					return null;
				}
				var bytes = new byte[codeBlob.GetBufferSize()];
				var bufferPtr = codeBlob.GetBufferPointer();
				Marshal.Copy(bufferPtr, bytes, 0, bytes.Length);
				return bytes;
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
		/// Disassembles HLSL shader bytecode.
		/// </summary>
		/// <param name="shader">The shader to disassemble.</param>
		/// <returns>The disassembled shader if successful, or <c>null</c> otherwise.</returns>
		public static unsafe string Disassemble(byte[] shader)
		{
			ID3DBlob blob = null;
			try
			{
				int result;
				fixed (byte* buffer = shader)
					result = D3DDisassemble((IntPtr)buffer, (uint)shader.Length, 0, null, out blob);
				if (result != S_OK || blob == null)
					return null;
				var textPtr = blob.GetBufferPointer();
				var text = Marshal.PtrToStringAnsi(textPtr);
				return text;
			}
			finally
			{
				if (blob != null)
					Marshal.ReleaseComObject(blob);
			}
		}

		/// <summary>
		/// Compiles HLSL sourcecode to bytecode.
		/// </summary>
		/// <param name="code"> HLSL source code to compile. </param>
		/// <param name="main"> Name of the main entry point in the HLSL source. </param>
		/// <param name="profile"> Shader profile to compile the source against (i.e. ps_3_0). </param>
		/// <param name="errors">Pointer to a string to receive error messages. Will be set to <c>null</c> if errors are not available.</param>
		/// <returns>The bytecode for the shader if successful, or <c>null</c> otherwise.</returns>
		public static byte[] Compile(string code, string main, string profile, out string errors)
		{
			ID3DBlob codeBlob = null, errorBlob = null;
			errors = null;

			try
			{
				var result = D3DCompile(code, (uint)code.Length, null, IntPtr.Zero, IntPtr.Zero, main, profile, 0, 0, out codeBlob, out errorBlob);
				if (result != S_OK || codeBlob == null)
				{
					if (errorBlob == null)
						return null;
					var errorPtr = errorBlob.GetBufferPointer();
					errors = Marshal.PtrToStringAnsi(errorPtr);
					return null;
				}
				var bytes = new byte[codeBlob.GetBufferSize()];
				var bufferPtr = codeBlob.GetBufferPointer();
				Marshal.Copy(bufferPtr, bytes, 0, bytes.Length);
				return bytes;
			}
			finally
			{
				if (codeBlob != null)
					Marshal.ReleaseComObject(codeBlob);
				if (errorBlob != null)
					Marshal.ReleaseComObject(errorBlob);
			}
		}

		public static bool PrintError(string error)
		{
			if (string.IsNullOrEmpty(error))
				return false;
			else
			{
				using (StringReader reader = new StringReader(error))
				{
					string line;
					while ((line = reader.ReadLine()) != null)
						Console.WriteLine(line.Substring(line.LastIndexOf('\\') + 1));
				}
				return true;
			}
		}

		#region Native Functions

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

		[DllImport("d3dcompiler_47.dll", CharSet = CharSet.Auto)]
		[PreserveSig]
		private static extern int D3DDisassemble(
			IntPtr pSrcData,
			uint SrcDataSize,
			uint Flags,
			[MarshalAs(UnmanagedType.LPStr)] string szComments,
			out ID3DBlob ppDisassembly
		);

		// Used the documentation here: https://msdn.microsoft.com/en-us/library/windows/desktop/dd607324(v=vs.85).aspx
		// and combined that with D3DAssemble from above to get what's here.
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

		private const int S_OK = 0;

		[ComImport, Guid("8BA5FB08-5195-40e2-AC58-0D989C3A0102")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		private interface ID3DBlob
		{
			[PreserveSig]
			IntPtr GetBufferPointer();
			[PreserveSig]
			uint GetBufferSize();
		}

		#endregion Native Functions
	}
}
