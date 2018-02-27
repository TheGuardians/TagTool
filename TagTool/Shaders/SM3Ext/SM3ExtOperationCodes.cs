using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Shaders.SM3Ext
{
    // This information comes from https://github.com/benvanik/xenia/blob/master/src/xenia/gpu/shader_translator.cc
    // This information https://github.com/benvanik/xenia/blob/master/src/xenia/gpu/ucode.h

    static class SM3ExtOperationCodes
    {
        public class SM3OperationInformation
        {
            public enum OperationType
            {
                Vector,
                Scalar,
                System
            }

            public string Name { get; } = null;
            public int? ID { get; } = null;
            public int? Unk1 { get; } = null;
            public int? Unk2 { get; } = null;
            public bool IsScalar = false;
            public bool IsSystem = false;
            public bool IsVector = false;
            public OperationType Type;

            public SM3OperationInformation(string name, int id, int unk, int unk2, OperationType type)
            {
                Name = name;
                ID = id;
                Unk1 = unk;
                Unk2 = unk2;
                IsScalar = type == OperationType.Scalar;
                IsSystem = type == OperationType.System;
                IsVector = type == OperationType.Vector;
                Type = type;
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

        public static SM3OperationInformation GetSM3ExtSystemOPCode(string name)
        {
            if (SystemOPCode.ContainsKey(name))
                return SystemOPCode[name];
            return null;
        }

        public static SM3OperationInformation GetSM3ExtScalarOPCode(int id)
        {
            foreach (var opcode in ScalarOPCode)
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

        // Defines control flow opcodes used to schedule instructions.
        enum ControlFlowOpcode : UInt32
        {
            // No-op - used to fill space.
            kNop = 0,
            // Executes fetch or ALU instructions.
            exec = 1,
            // Executes fetch or ALU instructions then ends execution.
            kExecEnd = 2,
            // Conditionally executes based on a bool const.
            kCondExec = 3,
            // Conditionally executes based on a bool const then ends execution.
            kCondExecEnd = 4,
            // Conditionally executes based on the current predicate.
            kCondExecPred = 5,
            // Conditionally executes based on the current predicate then ends execution.
            kCondExecPredEnd = 6,
            // Starts a loop that must be terminated with kLoopEnd.
            kLoopStart = 7,
            // Continues or breaks out of a loop started with kLoopStart.
            kLoopEnd = 8,
            // Conditionally calls a function.
            // A return address is pushed to the stack to be used by a kReturn.
            kCondCall = 9,
            // Returns from the current function as called by kCondCall.
            // This is a no-op if not in a function.
            kReturn = 10,
            // Conditionally jumps to an arbitrary address based on a bool const.
            kCondJmp = 11,
            // Allocates output values.
            kAlloc = 12,
            // Conditionally executes based on the current predicate.
            // Optionally resets the predicate value.
            kCondExecPredClean = 13,
            // Conditionally executes based on the current predicate then ends execution.
            // Optionally resets the predicate value.
            kCondExecPredCleanEnd = 14,
            // Hints that no more vertex fetches will be performed.
            kMarkVsFetchDone = 15,
        };

        public static Dictionary<string, SM3OperationInformation> SystemOPCode = new Dictionary<string, SM3OperationInformation>() {
            { ControlFlowOpcode.kNop.ToString(), new SM3OperationInformation(ControlFlowOpcode.kNop.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // No-op - used to fill space.
            { ControlFlowOpcode.exec.ToString(), new SM3OperationInformation(ControlFlowOpcode.exec.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Executes fetch or ALU instructions.
            { ControlFlowOpcode.kExecEnd.ToString(), new SM3OperationInformation(ControlFlowOpcode.kExecEnd.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Executes fetch or ALU instructions then ends execution.
            { ControlFlowOpcode.kCondExec.ToString(), new SM3OperationInformation(ControlFlowOpcode.kCondExec.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Conditionally executes based on a bool const.
            { ControlFlowOpcode.kCondExecEnd.ToString(), new SM3OperationInformation(ControlFlowOpcode.kCondExecEnd.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Conditionally executes based on a bool const then ends execution.
            { ControlFlowOpcode.kCondExecPred.ToString(), new SM3OperationInformation(ControlFlowOpcode.kCondExecPred.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Conditionally executes based on the current predicate.
            { ControlFlowOpcode.kCondExecPredEnd.ToString(), new SM3OperationInformation(ControlFlowOpcode.kCondExecPredEnd.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Conditionally executes based on the current predicate then ends execution.
            { ControlFlowOpcode.kLoopStart.ToString(), new SM3OperationInformation(ControlFlowOpcode.kLoopStart.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Starts a loop that must be terminated with kLoopEnd.
            { ControlFlowOpcode.kLoopEnd.ToString(), new SM3OperationInformation(ControlFlowOpcode.kLoopEnd.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Continues or breaks out of a loop started with kLoopStart.
            { ControlFlowOpcode.kCondCall.ToString(), new SM3OperationInformation(ControlFlowOpcode.kCondCall.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Conditionally calls a function. A return address is pushed to the stack to be used by a kReturn.
            { ControlFlowOpcode.kReturn.ToString(), new SM3OperationInformation(ControlFlowOpcode.kReturn.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Returns from the current function as called by kCondCall. This is a no-op if not in a function.
            { ControlFlowOpcode.kCondJmp.ToString(), new SM3OperationInformation(ControlFlowOpcode.kCondJmp.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Conditionally jumps to an arbitrary address based on a bool const.
            { ControlFlowOpcode.kAlloc.ToString(), new SM3OperationInformation(ControlFlowOpcode.kAlloc.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Allocates output values.
            { ControlFlowOpcode.kCondExecPredClean.ToString(), new SM3OperationInformation(ControlFlowOpcode.kCondExecPredClean.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Conditionally executes based on the current predicate. Optionally resets the predicate value.
            { ControlFlowOpcode.kCondExecPredCleanEnd.ToString(), new SM3OperationInformation(ControlFlowOpcode.kCondExecPredCleanEnd.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Conditionally executes based on the current predicate then ends execution. Optionally resets the predicate value.
            { ControlFlowOpcode.kMarkVsFetchDone.ToString(), new SM3OperationInformation(ControlFlowOpcode.kMarkVsFetchDone.ToString(), 0, 0, 0, SM3OperationInformation.OperationType.System) }, // Hints that no more vertex fetches will be performed.
        };

        public static Dictionary<string, SM3OperationInformation> ScalarOPCode = new Dictionary<string, SM3OperationInformation>() {
                {"adds", new SM3OperationInformation("adds",0,1,2, SM3OperationInformation.OperationType.Scalar)},
                {"adds_prev", new SM3OperationInformation("adds_prev",1,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"muls", new SM3OperationInformation("muls",2,1,2, SM3OperationInformation.OperationType.Scalar)},
                {"muls_prev", new SM3OperationInformation("muls_prev",3,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"muls_prev2", new SM3OperationInformation("muls_prev2",4,1,2, SM3OperationInformation.OperationType.Scalar)},
                {"maxs", new SM3OperationInformation("maxs",5,1,2, SM3OperationInformation.OperationType.Scalar)},
                {"mins", new SM3OperationInformation("mins",6,1,2, SM3OperationInformation.OperationType.Scalar)},
                {"seqs", new SM3OperationInformation("seqs",7,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"sgts", new SM3OperationInformation("sgts",8,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"sges", new SM3OperationInformation("sges",9,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"snes", new SM3OperationInformation("snes",10,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"frcs", new SM3OperationInformation("frcs",11,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"truncs", new SM3OperationInformation("truncs",12,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"floors", new SM3OperationInformation("floors",13,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"exp", new SM3OperationInformation("exp",14,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"logc", new SM3OperationInformation("logc",15,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"log", new SM3OperationInformation("log",16,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"rcpc", new SM3OperationInformation("rcpc",17,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"rcpf", new SM3OperationInformation("rcpf",18,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"rcp", new SM3OperationInformation("rcp",19,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"rsqc", new SM3OperationInformation("rsqc",20,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"rsqf", new SM3OperationInformation("rsqf",21,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"rsq", new SM3OperationInformation("rsq",22,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"maxas", new SM3OperationInformation("maxas",23,1,2, SM3OperationInformation.OperationType.Scalar)},
                {"maxasf", new SM3OperationInformation("maxasf",24,1,2, SM3OperationInformation.OperationType.Scalar)},
                {"subs", new SM3OperationInformation("subs",25,1,2, SM3OperationInformation.OperationType.Scalar)},
                {"subs_prev", new SM3OperationInformation("subs_prev",26,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"setp_eq", new SM3OperationInformation("setp_eq",27,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"setp_ne", new SM3OperationInformation("setp_ne",28,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"setp_gt", new SM3OperationInformation("setp_gt",29,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"setp_ge", new SM3OperationInformation("setp_ge",30,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"setp_inv", new SM3OperationInformation("setp_inv",31,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"setp_pop", new SM3OperationInformation("setp_pop",32,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"setp_clr", new SM3OperationInformation("setp_clr",33,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"setp_rstr", new SM3OperationInformation("setp_rstr",34,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"kills_eq", new SM3OperationInformation("kills_eq",35,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"kills_gt", new SM3OperationInformation("kills_gt",36,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"kills_ge", new SM3OperationInformation("kills_ge",37,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"kills_ne", new SM3OperationInformation("kills_ne",38,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"kills_one", new SM3OperationInformation("kills_one",39,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"sqrt", new SM3OperationInformation("sqrt",40,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"UNKNOWN", new SM3OperationInformation("UNKNOWN",41,0,0, SM3OperationInformation.OperationType.Scalar)},
                {"mulsc", new SM3OperationInformation("mulsc",42,2,1, SM3OperationInformation.OperationType.Scalar)},
                {"mulsc2", new SM3OperationInformation("mulsc",43,2,1, SM3OperationInformation.OperationType.Scalar)},
                {"addsc", new SM3OperationInformation("addsc",44,2,1, SM3OperationInformation.OperationType.Scalar)},
                {"addsc2", new SM3OperationInformation("addsc",45,2,1, SM3OperationInformation.OperationType.Scalar)},
                {"subsc", new SM3OperationInformation("subsc",46,2,1, SM3OperationInformation.OperationType.Scalar)},
                {"subsc2", new SM3OperationInformation("subsc",47,2,1, SM3OperationInformation.OperationType.Scalar)},
                {"sin", new SM3OperationInformation("sin",48,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"cos", new SM3OperationInformation("cos",49,1,1, SM3OperationInformation.OperationType.Scalar)},
                {"retain_prev", new SM3OperationInformation("retain_prev",50,1,1, SM3OperationInformation.OperationType.Scalar)},
            };

        public static Dictionary<string, SM3OperationInformation> VectorOPCode = new Dictionary<string, SM3OperationInformation>() {
                {"adds", new SM3OperationInformation("adds",0,1,2, SM3OperationInformation.OperationType.Vector)},
                {"adds_prev", new SM3OperationInformation("adds_prev",1,1,1, SM3OperationInformation.OperationType.Vector)},
                {"muls", new SM3OperationInformation("muls",2,1,2, SM3OperationInformation.OperationType.Vector)},
                {"muls_prev", new SM3OperationInformation("muls_prev",3,1,1, SM3OperationInformation.OperationType.Vector)},
                {"muls_prev2", new SM3OperationInformation("muls_prev2",4,1,2, SM3OperationInformation.OperationType.Vector)},
                {"maxs", new SM3OperationInformation("maxs",5,1,2, SM3OperationInformation.OperationType.Vector)},
                {"mins", new SM3OperationInformation("mins",6,1,2, SM3OperationInformation.OperationType.Vector)},
                {"seqs", new SM3OperationInformation("seqs",7,1,1, SM3OperationInformation.OperationType.Vector)},
                {"sgts", new SM3OperationInformation("sgts",8,1,1, SM3OperationInformation.OperationType.Vector)},
                {"sges", new SM3OperationInformation("sges",9,1,1, SM3OperationInformation.OperationType.Vector)},
                {"snes", new SM3OperationInformation("snes",10,1,1, SM3OperationInformation.OperationType.Vector)},
                {"frcs", new SM3OperationInformation("frcs",11,1,1, SM3OperationInformation.OperationType.Vector)},
                {"truncs", new SM3OperationInformation("truncs",12,1,1, SM3OperationInformation.OperationType.Vector)},
                {"floors", new SM3OperationInformation("floors",13,1,1, SM3OperationInformation.OperationType.Vector)},
                {"exp", new SM3OperationInformation("exp",14,1,1, SM3OperationInformation.OperationType.Vector)},
                {"logc", new SM3OperationInformation("logc",15,1,1, SM3OperationInformation.OperationType.Vector)},
                {"log", new SM3OperationInformation("log",16,1,1, SM3OperationInformation.OperationType.Vector)},
                {"rcpc", new SM3OperationInformation("rcpc",17,1,1, SM3OperationInformation.OperationType.Vector)},
                {"rcpf", new SM3OperationInformation("rcpf",18,1,1, SM3OperationInformation.OperationType.Vector)},
                {"rcp", new SM3OperationInformation("rcp",19,1,1, SM3OperationInformation.OperationType.Vector)},
                {"rsqc", new SM3OperationInformation("rsqc",20,1,1, SM3OperationInformation.OperationType.Vector)},
                {"rsqf", new SM3OperationInformation("rsqf",21,1,1, SM3OperationInformation.OperationType.Vector)},
                {"rsq", new SM3OperationInformation("rsq",22,1,1, SM3OperationInformation.OperationType.Vector)},
                {"maxas", new SM3OperationInformation("maxas",23,1,2, SM3OperationInformation.OperationType.Vector)},
                {"maxasf", new SM3OperationInformation("maxasf",24,1,2, SM3OperationInformation.OperationType.Vector)},
                {"subs", new SM3OperationInformation("subs",25,1,2, SM3OperationInformation.OperationType.Vector)},
                {"subs_prev", new SM3OperationInformation("subs_prev",26,1,1, SM3OperationInformation.OperationType.Vector)},
                {"setp_eq", new SM3OperationInformation("setp_eq",27,1,1, SM3OperationInformation.OperationType.Vector)},
                {"setp_ne", new SM3OperationInformation("setp_ne",28,1,1, SM3OperationInformation.OperationType.Vector)},
                {"setp_gt", new SM3OperationInformation("setp_gt",29,1,1, SM3OperationInformation.OperationType.Vector)},
            };
    }
}
