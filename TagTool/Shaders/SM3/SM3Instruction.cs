using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Shaders.SM3
{
    class SM3Instruction
    {
        public List<SM3Instruction> ChildInstructions = new List<SM3Instruction>();
        public string Operation;
        public List<string> Args;
        public String Comment { get; set; }
        public bool IsScalar = false;
        public bool IgnoreInstruction = false;
        public bool RequiresFixup = false;
        public bool InstructionFirstIndexIsOutput = true;

        public SM3Instruction(string operation, List<string> args, bool is_scalar = false)
        {
            Operation = operation;
            Args = args;
            IsScalar = is_scalar;
        }

        public SM3Instruction(SM3Ext.SM3ExtInstruction sm3ext_instruction)
        {
            Operation = sm3ext_instruction.Operation;
            Args = sm3ext_instruction.Args;
            Comment = sm3ext_instruction.Comment;
            IsScalar = sm3ext_instruction.IsScalar;

            switch (Operation)
            {
                case "exec":
                case "exece":
                case "test":
                case "serialize":
                case "alloc":
                case "kill_gt":
                    IgnoreInstruction = true; //TODO: Should probs figure out what this is actualy for lol
                    InstructionFirstIndexIsOutput = false;
                    break;
                case "vfetch_full":
                    //TODO: This must be manually fixed in a fix
                    RequiresFixup = true; // This is create an error if the function is not dealt with
                    break;
                case "tfetch2D":
                    Operation = "texld";

                    var original_arg0 = Args[0];
                    var component = original_arg0.Split('.');
                    var dest_register = component[0];
#if DEBUG
                    if (component.Length > 2) throw new Exception("what the fuck???");
#endif
                    var indices = component.Length > 1 ? component[1] : null;

                    Args = new List<string>
                    {
                        indices == null ? Args[0] : "TSO",
                        Args[1],
                        Args[2].Replace("tf","s")
                    };



                    if (indices != null)
                    {
                        indices = indices.Replace('_', ' ').TrimEnd().Replace(' ', '_');

                        // Check this until its fixed up
                        foreach (var index in indices)
                        {
                            if (index == '1' || index == '_') throw new NotImplementedException();
                        }

                        if (indices.Length == 4)
                        {
                            Args[0] = original_arg0; // No need to change this register
                        }
                        else
                        {
                            //TODO: I'm sure this could be improved, but for now lets just go and replace the fake ass
                            // register TSO : Temporay Sampler Output with an allocated one further down the road

                            var source_indices = "xyzw".Substring(0, indices.Length);

                            var child = new SM3Instruction("mov", new List<string> { $"{dest_register}.{indices}", $"TSO.{source_indices}" }, true);
                            child.Comment = "^ tfetch2D fixup";
                            ChildInstructions.Add(child);

                        }
                    }

                    break;
                case "sqrt":

                    // Change sqrt to the reciprocal square root and then do the reciprocal as well
                    // sqrt(x) = rcp (         rsq(x))
                    // sqrt(x) = 1.0 / (1.0 / sqrt(x))

                    Operation = "rsq";

                    {
                        var child = new SM3Instruction("rcp", new List<string> { this.Args[0], this.Args[0] }, true);
                        child.Comment = "^ sqrt";
                        ChildInstructions.Add(child);
                    }

                    break;
                case "mulsc":
                    Operation = "mul";
                    break;
                case "muls_prev":
                    Operation = "mul";

                    /*
                     * This function does A *= B
                     */
                    Args = new List<string> { Args[0], Args[0], Args[1] }; // a = a * b
                    break;
                case "movs":
                    Operation = "mov";
                    break;
                case "dp4":
                case "dp3":
                case "mov":
                case "mad":
                case "add":
                case "mul":
                case "rsq":
                case "rcp":
                case "min":
                    break; // Supported Instructions End
                default:
                    throw new NotImplementedException("SM3Instruction conversion not supported for " + Operation);
            }
        }

        public override string ToString()
        {
            string output = $"{Operation} ";
            output += String.Join(", ", Args);
            if (!String.IsNullOrWhiteSpace(Comment))
            {
                output += $" // {Comment}";
            }
            if (IgnoreInstruction)
            {
                output = "// " + output;
            }
            return output.Trim();
        }
    }
}
