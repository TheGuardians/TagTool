using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Direct3D.Enums
{
	/// <summary>
	/// Enum names acquired from: https://msdn.microsoft.com/en-us/library/windows/desktop/ff476174(v=vs.85).aspx
	/// </summary>
	public enum ReturnCode
	{
		// No error occurred.
		S_OK = 0,

		// Alternate success value, indicating a successful but nonstandard completion 
		// (the precise meaning depends on context).
		S_FALSE,

		// The method call isn't implemented with the passed parameter combination.
		E_NOTIMPL,

		// Direct3D could not allocate sufficient memory to complete the call.
		E_OUTOFMEMORY,

		// An invalid parameter was passed to the returning function.
		E_INVALIDARG,

		// Attempted to create a device with the debug layer enabled and the layer is not installed.
		E_FAIL,

		// The previous blit operation that is transferring information to or from this surface is incomplete.
		D3DERR_WASSTILLDRAWING,

		// The method call is invalid. For example, a method's parameter may not be a valid pointer.
		D3DERR_INVALIDCALL
	}
}
