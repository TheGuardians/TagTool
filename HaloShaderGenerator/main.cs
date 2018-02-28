using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Utilities;

namespace HaloShaderGenerator
{
    class main
    {
        static void Main()
        {
            var src = File.ReadAllText("pixel.hlsl");
            var defines = new DirectXUtilities.MacroDefine[] { };
            if(DirectXUtilities.CompilePCShader(src, defines, "", "main", "ps_3_0", 0, out byte[] shader, out string errors, out string constanttable))
            {

            }
            else
            {
                throw new Exception(errors);
            }

        }
    }
}
