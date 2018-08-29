using System;
using System.Runtime.InteropServices;

namespace TagTool
{
    public static class D3D
    {
        public enum INCLUDE_TYPE : int
        {
            D3D_INCLUDE_LOCAL = 0,
            D3D_INCLUDE_SYSTEM = 1,
            D3D10_INCLUDE_LOCAL = D3D_INCLUDE_LOCAL,
            D3D10_INCLUDE_SYSTEM = D3D_INCLUDE_SYSTEM,
            D3D_INCLUDE_FORCE_DWORD = 0x7fffffff
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct SHADER_MACRO
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string Name;
            [MarshalAs(UnmanagedType.LPStr)]
            public string Definition;
        }

        [Guid("8BA5FB08-5195-40e2-AC58-0D989C3A0102")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ID3DBlob
        {
            [PreserveSig]
            IntPtr GetBufferPointer();
            [PreserveSig]
            int GetBufferSize();
        }

        [PreserveSig]
        [DllImport("D3DCompiler_47.dll")]
        public extern static int D3DCompileFromFile(
            [MarshalAs(UnmanagedType.LPTStr)] string pFilename,
            [In, Out] SHADER_MACRO[] pDefines,
            IntPtr pInclude,
            [MarshalAs(UnmanagedType.LPStr)] string pEntrypoint,
            [MarshalAs(UnmanagedType.LPStr)] string pTarget,
            uint flags1,
            uint flags2,
            ref ID3DBlob ppCode,
            ref ID3DBlob ppErrorMsgs);


        [PreserveSig]
        [DllImport("D3DCompiler_47.dll")]
        public extern static int D3DAssemble(
            [In] byte[] pSrcData,
            [In] UIntPtr SrcDataSize,
            [MarshalAs(UnmanagedType.LPStr)] string pSourceName,
            [In, Out] SHADER_MACRO[] pDefines,
            IntPtr pInclude,
            uint Flags,
            ref ID3DBlob ppCode,
            ref ID3DBlob ppErrorMsgs);

        [PreserveSig]
        [DllImport("D3DCompiler_47.dll")]
        public extern static int D3DCompile(
            [In] byte[] pSrcData,
            [In] UIntPtr SrcDataSize,
            [MarshalAs(UnmanagedType.LPStr)] string pSourceName,
            [In, Out] SHADER_MACRO[] pDefines,
            IntPtr pInclude,
            [MarshalAs(UnmanagedType.LPStr)] string pEntrypoint,
            [MarshalAs(UnmanagedType.LPStr)] string pTarget,
            uint flags1,
            uint flags2,
            ref ID3DBlob ppCode,
            ref ID3DBlob ppErrorMsgs);

        [PreserveSig]
        [DllImport("D3DCompiler_47.dll")]
        public extern static int D3DDisassemble(
            [In] byte[] pSrcData,
            [In] UIntPtr SrcDataSize,
            uint flags,
            [MarshalAs(UnmanagedType.LPStr)] string szComments,
            ref ID3DBlob ppDisassembly);
    }
}
