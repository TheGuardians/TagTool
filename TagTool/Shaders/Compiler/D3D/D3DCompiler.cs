using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TagTool
{
    static class D3DCompiler
    {
        public static byte[] Compile(string Source, string Entrypoint, string Target, D3D.SHADER_MACRO[] Defines = null, uint Flags1 = 0, uint Flags2 = 0, string SourceName = null, Include include = null)
        {
            byte[] data = null;
            D3D.ID3DBlob ppCode = null;
            D3D.ID3DBlob ppErrorMsgs = null;

            // Null terminated defines array
            var _defines = Defines?.Concat(new D3D.SHADER_MACRO[] { new D3D.SHADER_MACRO() }).ToArray();
            var source_data = ASCIIEncoding.ASCII.GetBytes(Source + char.MinValue);
            var source_size = source_data.Length;

            var CompileResult = D3D.D3DCompile(source_data, new UIntPtr((uint)source_size), SourceName, _defines, include?.NativePointer ?? IntPtr.Zero, Entrypoint, Target, Flags1, Flags2, ref ppCode, ref ppErrorMsgs);

            if (ppErrorMsgs != null)
            {
                IntPtr errors = ppErrorMsgs.GetBufferPointer();
                int size = ppErrorMsgs.GetBufferSize();
                var error_text = Marshal.PtrToStringAnsi(errors);

                Marshal.ReleaseComObject(ppErrorMsgs);
                if (ppCode != null) Marshal.ReleaseComObject(ppCode);

                if (CompileResult != 0)
                {
                    throw new Exception(error_text);
                }
                else
                {
                    Console.WriteLine(error_text);
                }
            }

            if (ppCode != null)
            {
                IntPtr pData = ppCode.GetBufferPointer();
                int iSize = ppCode.GetBufferSize();

                data = new byte[iSize];
                Marshal.Copy(pData, data, 0, data.Length);

                Marshal.ReleaseComObject(ppCode);
            }

            return data;
        }

        public static byte[] CompileFromFile(string Filename, string Entrypoint, string Target, D3D.SHADER_MACRO[] Defines = null, uint Flags1 = 0, uint Flags2 = 0, Include include = null)
        {
            byte[] data = null;
            D3D.ID3DBlob ppCode = null;
            D3D.ID3DBlob ppErrorMsgs = null;

            // Null terminated defines array
            var _defines = Defines?.Concat(new D3D.SHADER_MACRO[] { new D3D.SHADER_MACRO() }).ToArray();

            var CompileResult = D3D.D3DCompileFromFile(Filename, _defines, include?.NativePointer ?? IntPtr.Zero, Entrypoint, Target, Flags1, Flags2, ref ppCode, ref ppErrorMsgs);

            if (ppErrorMsgs != null)
            {
                IntPtr errors = ppErrorMsgs.GetBufferPointer();
                int size = ppErrorMsgs.GetBufferSize();
                var error_text = Marshal.PtrToStringAnsi(errors);

                Marshal.ReleaseComObject(ppErrorMsgs);
                if (ppCode != null) Marshal.ReleaseComObject(ppCode);

                if (CompileResult != 0)
                {
                    throw new Exception(error_text);
                }
                else
                {
                    Console.WriteLine(error_text);
                }
            }

            if (ppCode != null)
            {
                IntPtr pData = ppCode.GetBufferPointer();
                int iSize = ppCode.GetBufferSize();

                data = new byte[iSize];
                Marshal.Copy(pData, data, 0, data.Length);

                Marshal.ReleaseComObject(ppCode);
            }

            return data;
        }

        public static byte[] Assemble(string Assembly, string filename = null, D3D.SHADER_MACRO[] Defines = null, uint Flags = 0, Include include = null)
        {
            byte[] data = null;
            D3D.ID3DBlob ppCode = null;
            D3D.ID3DBlob ppErrorMsgs = null;

            var macros = Defines?.Concat(new D3D.SHADER_MACRO[] { new D3D.SHADER_MACRO() }).ToArray();
            var source_data = ASCIIEncoding.ASCII.GetBytes(Assembly + char.MinValue);
            var source_size = source_data.Length;

            var CompileResult = D3D.D3DAssemble(source_data, new UIntPtr((uint)source_size), filename, Defines, include?.NativePointer ?? IntPtr.Zero, Flags, ref ppCode, ref ppErrorMsgs);

            if (ppErrorMsgs != null)
            {
                IntPtr errors = ppErrorMsgs.GetBufferPointer();
                int size = ppErrorMsgs.GetBufferSize();
                var error_text = Marshal.PtrToStringAnsi(errors);

                Marshal.ReleaseComObject(ppErrorMsgs);
                if (ppCode != null) Marshal.ReleaseComObject(ppCode);

                if (CompileResult != 0)
                {
                    throw new Exception(error_text);
                }
                else
                {
                    Console.WriteLine(error_text);
                }
            }

            if (ppCode != null)
            {
                IntPtr pData = ppCode.GetBufferPointer();
                int iSize = ppCode.GetBufferSize();

                data = new byte[iSize];
                Marshal.Copy(pData, data, 0, data.Length);

                Marshal.ReleaseComObject(ppCode);
            }

            return data;
        }

        public static string Disassemble(byte[] bytecode)
        {
            D3D.ID3DBlob ppDisassembly = null;

            var result = D3D.D3DDisassemble(bytecode, new UIntPtr((uint)bytecode.Length), 0, null, ref ppDisassembly);

            if(result != 0)
            {
                throw new Exception("Failed to disassembly shader");
            }

            string result_str = null;

            if (ppDisassembly != null)
            {
                IntPtr pData = ppDisassembly.GetBufferPointer();
                int iSize = ppDisassembly.GetBufferSize();

                var data = new byte[iSize];
                Marshal.Copy(pData, data, 0, data.Length);

                Marshal.ReleaseComObject(ppDisassembly);

                result_str = System.Text.Encoding.ASCII.GetString(data);
            }

            return result_str;
        }


    }
}
