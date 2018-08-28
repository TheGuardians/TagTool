using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TagTool
{
    abstract class Include
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int OpenCallBack(IntPtr thisPtr, D3D.INCLUDE_TYPE includeType, IntPtr fileNameRef, IntPtr pParentData, ref IntPtr dataRef, ref int bytesRef);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int CloseCallBack(IntPtr thisPtr, IntPtr pData);

        public IntPtr NativePointer;
        private OpenCallBack _openCallback;
        private CloseCallBack _closeCallback;

        public Include()
        {
            // Allocate object layout in memory 
            // - 1 pointer to VTBL table
            // - following that the VTBL itself - with 2 function pointers for Open and Close methods
            NativePointer = Marshal.AllocHGlobal(IntPtr.Size * 3);

            // Write pointer to vtbl
            IntPtr vtblPtr = IntPtr.Add(NativePointer, IntPtr.Size);
            Marshal.WriteIntPtr(NativePointer, vtblPtr);
            _openCallback = new OpenCallBack(Open);
            Marshal.WriteIntPtr(vtblPtr, Marshal.GetFunctionPointerForDelegate(_openCallback));
            _closeCallback = new CloseCallBack(Close);
            Marshal.WriteIntPtr(IntPtr.Add(vtblPtr, IntPtr.Size), Marshal.GetFunctionPointerForDelegate(_closeCallback));
        }

        private int Open(IntPtr thisPtr, D3D.INCLUDE_TYPE includeType, IntPtr pFileName, IntPtr pParentData, ref IntPtr ppData, ref int pBytes)
        {
            string result = null;
            int hresult = Open(includeType, Marshal.PtrToStringAnsi(pFileName), ref result);

            if (hresult != 0)
            {
                return hresult;
            }

            if (result != null)
            {
                byte[] data = Encoding.ASCII.GetBytes(result + char.MinValue);
                var ptr = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, ptr, data.Length);

                ppData = ptr;
                pBytes = data.Length;
            }

            return hresult;
        }

        private int Close(IntPtr thisPtr, IntPtr pData)
        {
            return Close(pData);
        }

        protected abstract int Open(D3D.INCLUDE_TYPE includeType, string filename, ref string result);
        protected virtual int Close(IntPtr pData)
        {
            if (pData != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pData);
            }
            return 0;
        }
    }
}
