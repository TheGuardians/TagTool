using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Shaders
{
    // This information comes from https://github.com/benvanik/xenia/blob/master/src/xenia/gpu/shader_translator.cc

    static class SM3ExtOperationCodes
    {
        public class SM3OperationInformation
        {
            public string Name { get; } = null;
            public int? ID { get; } = null;
            public int? Unk1 { get; } = null;
            public int? Unk2 { get; } = null;
            public bool IsScalar = false;

            public SM3OperationInformation(string name, int id, int unk, int unk2, bool isscalar)
            {
                Name = name;
                ID = id;
                Unk1 = unk;
                Unk2 = unk2;
                IsScalar = isscalar;
            }
        }

        public static SM3OperationInformation GetSM3ExtScalarOPCode(string name)
        {
            if (ScalarOPCode.ContainsKey(name))
                return ScalarOPCode[name];
            return null;
        }

        public static SM3OperationInformation GetSM3ExtVectorOPCode(string name)
        {
            if (VectorOPCode.ContainsKey(name))
                return VectorOPCode[name];
            return null;
        }

        public static SM3OperationInformation GetSM3ExtScalarOPCode(int id)
        {
            foreach(var opcode in ScalarOPCode)
                if (opcode.Value.ID == id)
                    return opcode.Value;
            return null;
        }

        public static SM3OperationInformation GetSM3ExtVectorOPCode(int id)
        {
            foreach (var opcode in VectorOPCode)
                if (opcode.Value.ID == id)
                    return opcode.Value;
            return null;
        }

        public static Dictionary<string, SM3OperationInformation> ScalarOPCode = new Dictionary<string, SM3OperationInformation>() {
                {"adds", new SM3OperationInformation("adds",0,1,2, true)},
                {"adds_prev", new SM3OperationInformation("adds_prev",1,1,1, true)},
                {"muls", new SM3OperationInformation("muls",2,1,2, true)},
                {"muls_prev", new SM3OperationInformation("muls_prev",3,1,1, true)},
                {"muls_prev2", new SM3OperationInformation("muls_prev2",4,1,2, true)},
                {"maxs", new SM3OperationInformation("maxs",5,1,2, true)},
                {"mins", new SM3OperationInformation("mins",6,1,2, true)},
                {"seqs", new SM3OperationInformation("seqs",7,1,1, true)},
                {"sgts", new SM3OperationInformation("sgts",8,1,1, true)},
                {"sges", new SM3OperationInformation("sges",9,1,1, true)},
                {"snes", new SM3OperationInformation("snes",10,1,1, true)},
                {"frcs", new SM3OperationInformation("frcs",11,1,1, true)},
                {"truncs", new SM3OperationInformation("truncs",12,1,1, true)},
                {"floors", new SM3OperationInformation("floors",13,1,1, true)},
                {"exp", new SM3OperationInformation("exp",14,1,1, true)},
                {"logc", new SM3OperationInformation("logc",15,1,1, true)},
                {"log", new SM3OperationInformation("log",16,1,1, true)},
                {"rcpc", new SM3OperationInformation("rcpc",17,1,1, true)},
                {"rcpf", new SM3OperationInformation("rcpf",18,1,1, true)},
                {"rcp", new SM3OperationInformation("rcp",19,1,1, true)},
                {"rsqc", new SM3OperationInformation("rsqc",20,1,1, true)},
                {"rsqf", new SM3OperationInformation("rsqf",21,1,1, true)},
                {"rsq", new SM3OperationInformation("rsq",22,1,1, true)},
                {"maxas", new SM3OperationInformation("maxas",23,1,2, true)},
                {"maxasf", new SM3OperationInformation("maxasf",24,1,2, true)},
                {"subs", new SM3OperationInformation("subs",25,1,2, true)},
                {"subs_prev", new SM3OperationInformation("subs_prev",26,1,1, true)},
                {"setp_eq", new SM3OperationInformation("setp_eq",27,1,1, true)},
                {"setp_ne", new SM3OperationInformation("setp_ne",28,1,1, true)},
                {"setp_gt", new SM3OperationInformation("setp_gt",29,1,1, true)},
                {"setp_ge", new SM3OperationInformation("setp_ge",30,1,1, true)},
                {"setp_inv", new SM3OperationInformation("setp_inv",31,1,1, true)},
                {"setp_pop", new SM3OperationInformation("setp_pop",32,1,1, true)},
                {"setp_clr", new SM3OperationInformation("setp_clr",33,1,1, true)},
                {"setp_rstr", new SM3OperationInformation("setp_rstr",34,1,1, true)},
                {"kills_eq", new SM3OperationInformation("kills_eq",35,1,1, true)},
                {"kills_gt", new SM3OperationInformation("kills_gt",36,1,1, true)},
                {"kills_ge", new SM3OperationInformation("kills_ge",37,1,1, true)},
                {"kills_ne", new SM3OperationInformation("kills_ne",38,1,1, true)},
                {"kills_one", new SM3OperationInformation("kills_one",39,1,1, true)},
                {"sqrt", new SM3OperationInformation("sqrt",40,1,1, true)},
                {"UNKNOWN", new SM3OperationInformation("UNKNOWN",41,0,0, true)},
                {"mulsc", new SM3OperationInformation("mulsc",42,2,1, true)},
                {"mulsc", new SM3OperationInformation("mulsc",43,2,1, true)},
                {"addsc", new SM3OperationInformation("addsc",44,2,1, true)},
                {"addsc", new SM3OperationInformation("addsc",45,2,1, true)},
                {"subsc", new SM3OperationInformation("subsc",46,2,1, true)},
                {"subsc", new SM3OperationInformation("subsc",47,2,1, true)},
                {"sin", new SM3OperationInformation("sin",48,1,1, true)},
                {"cos", new SM3OperationInformation("cos",49,1,1, true)},
                {"retain_prev", new SM3OperationInformation("retain_prev",50,1,1, true)},
            };

        public static Dictionary<string, SM3OperationInformation> VectorOPCode = new Dictionary<string, SM3OperationInformation>() {
                {"adds", new SM3OperationInformation("adds",0,1,2, false)},
                {"adds_prev", new SM3OperationInformation("adds_prev",1,1,1, false)},
                {"muls", new SM3OperationInformation("muls",2,1,2, false)},
                {"muls_prev", new SM3OperationInformation("muls_prev",3,1,1, false)},
                {"muls_prev2", new SM3OperationInformation("muls_prev2",4,1,2, false)},
                {"maxs", new SM3OperationInformation("maxs",5,1,2, false)},
                {"mins", new SM3OperationInformation("mins",6,1,2, false)},
                {"seqs", new SM3OperationInformation("seqs",7,1,1, false)},
                {"sgts", new SM3OperationInformation("sgts",8,1,1, false)},
                {"sges", new SM3OperationInformation("sges",9,1,1, false)},
                {"snes", new SM3OperationInformation("snes",10,1,1, false)},
                {"frcs", new SM3OperationInformation("frcs",11,1,1, false)},
                {"truncs", new SM3OperationInformation("truncs",12,1,1, false)},
                {"floors", new SM3OperationInformation("floors",13,1,1, false)},
                {"exp", new SM3OperationInformation("exp",14,1,1, false)},
                {"logc", new SM3OperationInformation("logc",15,1,1, false)},
                {"log", new SM3OperationInformation("log",16,1,1, false)},
                {"rcpc", new SM3OperationInformation("rcpc",17,1,1, false)},
                {"rcpf", new SM3OperationInformation("rcpf",18,1,1, false)},
                {"rcp", new SM3OperationInformation("rcp",19,1,1, false)},
                {"rsqc", new SM3OperationInformation("rsqc",20,1,1, false)},
                {"rsqf", new SM3OperationInformation("rsqf",21,1,1, false)},
                {"rsq", new SM3OperationInformation("rsq",22,1,1, false)},
                {"maxas", new SM3OperationInformation("maxas",23,1,2, false)},
                {"maxasf", new SM3OperationInformation("maxasf",24,1,2, false)},
                {"subs", new SM3OperationInformation("subs",25,1,2, false)},
                {"subs_prev", new SM3OperationInformation("subs_prev",26,1,1, false)},
                {"setp_eq", new SM3OperationInformation("setp_eq",27,1,1, false)},
                {"setp_ne", new SM3OperationInformation("setp_ne",28,1,1, false)},
                {"setp_gt", new SM3OperationInformation("setp_gt",29,1,1, false)},
            };
    }
}
