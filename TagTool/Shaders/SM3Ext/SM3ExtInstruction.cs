using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Shaders.SM3Ext
{
    class SM3ExtInstruction
    {
        bool IsConcurrent { get; }
        public string Operation;
        public List<string> Args;
        public String Comment { get; set; }
        private ArgumentsLayout Layout = null;
        public bool IsScalar = false;



        string OriginalInstructionString { get; }

        //SM3ExtOperationCodes.SM3OperationInformation Operation { get; } // TODO: Down the track


        public SM3ExtInstruction PreviousInstruction;
        public SM3ExtInstruction ParentInstruction;

        private class ArgumentsLayout
        {
            public ArgumentsLayout(bool has_destination = false, int num_args = 0)
            {
                HasDestination = has_destination;
                NumArguments = num_args;
            }
            public bool HasDestination = false;
            public int NumArguments = 0;
            public int ExpectedArgsCount => NumArguments + (HasDestination ? 1 : 0);
        }

        private Dictionary<string, ArgumentsLayout> OperationArgumentsMap = new Dictionary<string, ArgumentsLayout>()
        {
            // Strange Xbox Stuff
            {"exec", new ArgumentsLayout()},
            {"exece", new ArgumentsLayout()},
            {"serialize", new ArgumentsLayout()},
            {"alloc", new ArgumentsLayout(false, 1)},

            // Xbox 360 Specific
            {"vfetch_full", new ArgumentsLayout(true, 2)},
            {"tfetch2D", new ArgumentsLayout(true, 2)},
            {"kill_gt", new ArgumentsLayout(false, 3)},


            // More Normal Stuff
            {"dp4", new ArgumentsLayout(true, 2)},
            {"dp3", new ArgumentsLayout(true, 2)},
            {"mov", new ArgumentsLayout(true, 1)},
            {"movs", new ArgumentsLayout(true, 1)}, // Move scalar
            {"sqrt", new ArgumentsLayout(true, 1) },


            {"mad", new ArgumentsLayout(true, 3)},
            {"add", new ArgumentsLayout(true, 2) },
            {"mul", new ArgumentsLayout(true, 2) },
            {"mulsc", new ArgumentsLayout(true, 2) },
            {"rsq", new ArgumentsLayout(true, 1) },
            {"rcp", new ArgumentsLayout(true, 1) },

            // Unsorted
            {"min", new ArgumentsLayout(true, 2) },
        };

        private bool IsArgScalar(string args)
        {
            var arg_components = args.Split('.');
            if (arg_components.Length < 2) return false;
            if (arg_components.Length > 2) throw new Exception("what the fuck?");
            if (arg_components[1].Length == 1) return true;
            return false;
        }

        private bool IsArgScalar(IEnumerable<string> args, int max_index = Int32.MaxValue)
        {
            var args_count = args.Count();
            for (var i = 0; i < args_count; i++)
            {
                if (i >= max_index) continue;
                var arg = args.ElementAt(i);
                if (!IsArgScalar(arg)) return false;
            }
            return args_count > 0;
        }

        public override string ToString()
        {
            string output = $"{Operation} ";
            output += String.Join(", ", Args);
            if (!String.IsNullOrWhiteSpace(Comment))
            {
                output += $" // {Comment}";
            }
            return output.Trim();
        }

        public SM3ExtInstruction(string instruction, SM3ExtInstruction previous_instruction, SM3ExtInstruction parent_instruction = null)
        {
            PreviousInstruction = previous_instruction;
            ParentInstruction = parent_instruction;

            OriginalInstructionString = instruction;
            IsConcurrent = instruction.Contains(" + ");
            instruction = instruction.Replace(" + ", "");
            instruction = instruction.Split(new[] { "//" }, StringSplitOptions.None)[0]; // Take the left side of any comments
            instruction = instruction.Trim();

            if (String.IsNullOrWhiteSpace(instruction)) return;
            if (instruction.StartsWith("//")) return; // Is a comment

            var op_codes = instruction.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            Operation = op_codes[0];

            Layout = OperationArgumentsMap[Operation];
            if (Layout == null) throw new NotImplementedException($"Unknown SM3Ext operation {Operation}");

            Args = op_codes.Skip(1).Where((arg) =>
            {
                // Prune invalid args
                if (arg.StartsWith("FetchValidOnly")) return false;
                return true;
            }).ToList();
            if (Args.Count != Layout.ExpectedArgsCount) throw new Exception("Incorrect arguments count");


            



            //var vector_op_code = SM3ExtOperationCodes.GetSM3ExtVectorOPCode(opcode);
            //var scalar_op_code = SM3ExtOperationCodes.GetSM3ExtScalarOPCode(opcode);
            //var system_op_code = SM3ExtOperationCodes.GetSM3ExtSystemOPCode(opcode);

            IsScalar = IsArgScalar(Args);
        }

        //public SM3Instruction ConvertToSM3()
        //{
        //    return null;
        //}
    }
}
