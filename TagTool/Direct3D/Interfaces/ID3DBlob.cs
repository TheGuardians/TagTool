using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Direct3D.Interfaces
{
	[ComImport, Guid("8BA5FB08-5195-40e2-AC58-0D989C3A0102")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ID3DBlob
	{
		[PreserveSig]
		IntPtr GetBufferPointer();
		[PreserveSig]
		uint GetBufferSize();
	}
}
