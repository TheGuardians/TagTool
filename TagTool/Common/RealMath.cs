using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Common
{
    public class RealMath
    {
        public static float SafeDiv(float x, float y) => y == 0 ? 0.0f : x / y;

        public static float SafeRcp(float x) => x == 0 ? 0.0f : 1.0f / x;
    }
}
