using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TagTool.Commands;

namespace TagTool.IO
{
    public static class XCompress
    {
        private static readonly bool Is64Bit = Environment.Is64BitProcess;

        public enum XMemCodecType
        {
            Default = 0,
            LZX = 1
        }

        public static int XMemCreateDecompressionContext(XMemCodecType codecType, IntPtr pCodecParams, int flags, ref IntPtr pContext)
        {
            if (Is64Bit)
                return XMemCreateDecompressionContext64(codecType, pCodecParams, flags, ref pContext);
            else
                return XMemCreateDecompressionContext32(codecType, pCodecParams, flags, ref pContext);
        }

        public static void XMemDestroyDecompressionContext(IntPtr pContext)
        {
            if (Is64Bit)
                XMemDestroyDecompressionContext64(pContext);
            else
                XMemDestroyDecompressionContext32(pContext);
        }

        public static int XMemResetDecompressionContext(IntPtr pContext)
        {
            if (Is64Bit)
                return XMemResetDecompressionContext64(pContext);
            else
                return XMemResetDecompressionContext32(pContext);
        }

        public static int XMemDecompressStream(IntPtr pContext, byte[] pDestination, ref int pDestSize, byte[] pSource, ref int pSrcSize)
        {
            if (Is64Bit)
                return XMemDecompressStream64(pContext, pDestination, ref pDestSize, pSource, ref pSrcSize);
            else
                return XMemDecompressStream32(pContext, pDestination, ref pDestSize, pSource, ref pSrcSize);
        }

        #region x86
        [DllImport("xcompress32.dll", EntryPoint = "XMemCreateDecompressionContext")]
        private static extern int XMemCreateDecompressionContext32(XMemCodecType codecType, IntPtr pCodecParams, int flags, ref IntPtr pContext);

        [DllImport("xcompress32.dll", EntryPoint = "XMemDestroyDecompressionContext")]
        private static extern void XMemDestroyDecompressionContext32(IntPtr pContext);

        [DllImport("xcompress32.dll", EntryPoint = "XMemResetDecompressionContext")]
        private static extern int XMemResetDecompressionContext32(IntPtr pContext);

        [DllImport("xcompress32.dll", EntryPoint = "XMemDecompressStream")]
        private static extern int XMemDecompressStream32(IntPtr pContext, byte[] pDestination, ref int pDestSize, byte[] pSource, ref int pSrcSize);
        #endregion

        #region x64
        [DllImport("xcompress64.dll", EntryPoint = "XMemCreateDecompressionContext")]
        private static extern int XMemCreateDecompressionContext64(XMemCodecType codecType,IntPtr pCodecParams,int flags, ref IntPtr pContext);

        [DllImport("xcompress64.dll", EntryPoint = "XMemDestroyDecompressionContext")]
        private static extern void XMemDestroyDecompressionContext64(IntPtr pContext);

        [DllImport("xcompress64.dll", EntryPoint = "XMemResetDecompressionContext")]
        private static extern int XMemResetDecompressionContext64(IntPtr pContext);

        [DllImport("xcompress64.dll", EntryPoint = "XMemDecompressStream")]
        private static extern int XMemDecompressStream64(IntPtr pContext, byte[] pDestination, ref int pDestSize, byte[] pSource, ref int pSrcSize);
        #endregion
    }
}
