using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderDisassembler
{
	public enum ControlFlowOpcode : uint
	{
		cnop = 0x00, // No-op - used to fill space.
		exec = 0x01, // Executes fetch or ALU instructions.
		exece = 0x02, // Executes fetch or ALU instructions then ends execution.
		cexec = 0x03, // Conditionally executes based on a bool const.
		cexece = 0x04, // Conditionally executes based on a bool const then ends execution.
		cexec_pred = 0x05, // Conditionally executes based on the current predicate.
		cexece_pred = 0x06, // Conditionally executes based on the current predicate then ends execution.
		loop = 0x07, // Starts a loop that must be terminated with kLoopEnd.
		endloop = 0x08, // Continues or breaks out of a loop started with kLoopStart.
		ccall = 0x09, // Conditionally calls a function.  A return address is pushed to the stack to be used by a kReturn.
		ret = 0x0A, // Returns from the current function as called by kCondCall. This is a no-op if not in a function.
		cjmp = 0x0B, // Conditionally jumps to an arbitrary address based on a bool const.
		alloc = 0x0C, // Allocates output values.
		cexec_pred_clean = 0x0D, // Conditionally executes based on the current predicate. Optionally resets the predicate value.
		cexece_pred_clean = 0x0E, // Conditionally executes based on the current predicate then ends execution. Optionally resets the predicate value.
		vfetche = 0x0F, // Hints that no more vertex fetches will be performed.
	}
	public enum AddressMode : uint
	{
		Relative = 0x00,
		Absolute = 0x01,
	}
	public enum AllocationType : uint
	{
		None = 0x00,
		Position = 0x01,
		Interpolators = 0x02,
		Pixel = 0x02,
		Memory = 0x03,
	}

	// Instruction data for ControlFlowOpcode::kExec and kExecEnd.
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Exec
	{
		public uint address;
		public uint count;
		public uint is_yeild;
		public uint serialize;
		public uint vc_hi;  // Vertex cache?

		public uint vc_lo;
		public uint reserved0;
		public uint clean;
		public uint reserved1;
		public AddressMode address_mode;
		public ControlFlowOpcode opcode;
	}

	// Instruction data for ControlFlowOpcode::kCondExec and kCondExecEnd.
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CondExec
	{
		public uint address;
		public uint count;
		public uint is_yeild;
		public uint serialize;
		public uint vc_hi;  // Vertex cache?

		public uint vc_lo;
		public uint bool_address;
		public uint condition;
		public AddressMode address_mode;
		public ControlFlowOpcode opcode;
	}

	// Instruction data for ControlFlowOpcode::kCondExecPred, kCondExecPredEnd,
	// kCondExecPredClean, kCondExecPredCleanEnd.
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CondExecPred
	{
		public uint address;
		public uint count;
		public uint is_yeild;
		public uint serialize;
		public uint vc_hi;  // Vertex cache?

		public uint vc_lo;
		public uint reserved0;
		public uint clean;
		public uint condition;
		public AddressMode address_mode;
		public ControlFlowOpcode opcode;
	}

	// Instruction data for ControlFlowOpcode::kLoopStart.
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LoopStart
	{
		public uint address;
		public uint is_repeat;
		public uint reserved0;
		public uint loop_id;
		public uint reserved1;

		public uint reserved2;
		public AddressMode address_mode;
		public ControlFlowOpcode opcode;
	}

	// Instruction data for ControlFlowOpcode::kLoopEnd.
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LoopEnd
	{
		public uint address;
		public uint reserved0;
		public uint loop_id;
		public uint is_predicated_break;
		public uint reserved1;

		public uint reserved2;
		public uint condition;
		public AddressMode address_mode;
		public ControlFlowOpcode opcode;
	}

	// Instruction data for ControlFlowOpcode::kCondCall.
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CondCall
	{
		public uint address;
		public uint is_unconditional;
		public uint is_predicated;
		public uint reserved0;

		public uint reserved1;
		public uint bool_address;
		public uint condition;
		public AddressMode address_mode;
		public ControlFlowOpcode opcode;
	}

	// Instruction data for ControlFlowOpcode::kReturn.
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Return
	{
		public uint reserved0;

		public uint reserved1;
		public AddressMode address_mode;
		public ControlFlowOpcode opcode;
	}

	// Instruction data for ControlFlowOpcode::kCondJmp.
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CondJmp
	{
		public uint address;
		public uint is_unconditional;
		public uint is_predicated;
		public uint reserved0;

		public uint reserved1;
		public uint direction;
		public uint bool_address;
		public uint condition;
		public AddressMode address_mode;
		public ControlFlowOpcode opcode;
	}

	// Instruction data for ControlFlowOpcode::kAlloc.
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Alloc
	{
		public uint size;
		public uint reserved0;

		public uint reserved1;
		public uint is_unserialized;
		public AllocationType alloc_type;
		public uint reserved2;
		public ControlFlowOpcode opcode;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ControlFlowInstruction
	{
		public Exec exec;
		public CondExec cond_exec;
		public CondExecPred cond_exec_pred;
		public LoopStart loop_start;
		public LoopEnd loop_end;
		public CondCall cond_call;
		public Return ret;
		public CondJmp cond_jmp;
		public Alloc alloc;
		public ControlFlowOpcode opcode;

		// True if the given control flow opcode executes ALU or fetch
		// instructions.
		public bool Executes
		{
			get
			{
				return this.opcode == ControlFlowOpcode.exec ||
					   this.opcode == ControlFlowOpcode.exece ||
					   this.opcode == ControlFlowOpcode.cexec ||
					   this.opcode == ControlFlowOpcode.cexece ||
					   this.opcode == ControlFlowOpcode.cexec_pred ||
					   this.opcode == ControlFlowOpcode.cexece_pred ||
					   this.opcode == ControlFlowOpcode.cexec_pred_clean ||
					   this.opcode == ControlFlowOpcode.cexece_pred_clean;
			}
		}

		// True if the given control flow opcode terminates the shader after
		// executing.
		public bool EndsShader
		{
			get
			{
				return this.opcode == ControlFlowOpcode.exece ||
					   this.opcode == ControlFlowOpcode.cexece ||
					   this.opcode == ControlFlowOpcode.cexece_pred ||
					   this.opcode == ControlFlowOpcode.cexece_pred_clean;
			}
		}

		// True if the given control flow opcode resets the predicate prior to
		// execution.
		public bool ResetsPredicate
		{
			get
			{
				return this.opcode == ControlFlowOpcode.cexec_pred_clean ||
					   this.opcode == ControlFlowOpcode.cexece_pred_clean;
			}
		}

		// This is used to determine if an `Executes()` instruction executes Fetch instructions.
		// index == index of instruction executed by `Executes()` instruction.
		public bool IsFetch(int index)
		{
			return (exec.serialize & (1 << (index * 2))) != 0;
		}
	}
}
