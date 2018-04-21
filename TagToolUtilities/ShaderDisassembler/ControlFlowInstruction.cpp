#include <cstdint>

#pragma pack(push, 1)
// Instruction data for ControlFlowOpcode::kExec and kExecEnd.
struct PackedExec {
	// Word 0: (32 bits)
	uint32_t address : 12;
	uint32_t count : 3;
	uint32_t is_yeild : 1;
	uint32_t serialize : 12;
	uint32_t vc_hi : 4;  // Vertex cache?

	// Word 1: (16 bits)
	uint32_t vc_lo : 2;
	uint32_t reserved0 : 7;
	uint32_t clean : 1;
	uint32_t reserved1 : 1;
	uint32_t address_mode : 1;
	uint32_t opcode : 4;
};
struct Exec {
	uint32_t address;
	uint32_t count;
	uint32_t is_yeild;
	uint32_t serialize;
	uint32_t vc_hi;  // Vertex cache?

	uint32_t vc_lo;
	uint32_t reserved0;
	uint32_t clean;
	uint32_t reserved1;
	uint32_t address_mode;
	uint32_t opcode;
};

// Instruction data for ControlFlowOpcode::kCondExec and kCondExecEnd.
struct PackedCondExec {
	// Word 0: (32 bits)
	uint32_t address : 12;
	uint32_t count : 3;
	uint32_t is_yeild : 1;
	uint32_t serialize : 12;
	uint32_t vc_hi : 4;  // Vertex cache?

	// Word 1: (16 bits)
	uint32_t vc_lo : 2;
	uint32_t bool_address : 8;
	uint32_t condition : 1;
	uint32_t address_mode : 1;
	uint32_t opcode : 4;
}; 
struct CondExec {
	uint32_t address;
	uint32_t count;
	uint32_t is_yeild;
	uint32_t serialize;
	uint32_t vc_hi;  // Vertex cache?

	uint32_t vc_lo;
	uint32_t bool_address;
	uint32_t condition;
	uint32_t address_mode;
	uint32_t opcode;
};

// Instruction data for ControlFlowOpcode::kCondExecPred, kCondExecPredEnd,
// kCondExecPredClean, kCondExecPredCleanEnd.
struct PackedCondExecPred {
	// Word 0: (32 bits)
	uint32_t address : 12;
	uint32_t count : 3;
	uint32_t is_yeild : 1;
	uint32_t serialize : 12;
	uint32_t vc_hi : 4;  // Vertex cache?

	// Word 1: (16 bits)
	uint32_t vc_lo : 2;
	uint32_t reserved0 : 7;
	uint32_t clean : 1;
	uint32_t condition : 1;
	uint32_t address_mode : 1;
	uint32_t opcode : 4;
};
struct CondExecPred {
	uint32_t address;
	uint32_t count;
	uint32_t is_yeild;
	uint32_t serialize;
	uint32_t vc_hi;  // Vertex cache?

	uint32_t vc_lo;
	uint32_t reserved0;
	uint32_t clean;
	uint32_t condition;
	uint32_t address_mode;
	uint32_t opcode;
};

// Instruction data for ControlFlowOpcode::kLoopStart.
struct PackedLoopStart {
	// Word 0: (32 bits)
	uint32_t address : 13;
	uint32_t is_repeat : 1;
	uint32_t reserved0 : 2;
	uint32_t loop_id : 5;
	uint32_t reserved1 : 11;

	// Word 1: (16 bits)
	uint32_t reserved2 : 11;
	uint32_t address_mode : 1;
	uint32_t opcode : 4;
};
struct LoopStart {
	uint32_t address;
	uint32_t is_repeat;
	uint32_t reserved0;
	uint32_t loop_id;
	uint32_t reserved1;

	uint32_t reserved2;
	uint32_t address_mode;
	uint32_t opcode;
};

// Instruction data for ControlFlowOpcode::kLoopEnd.
struct PackedLoopEnd {
	// Word 0: (32 bits)
	uint32_t address : 13;
	uint32_t reserved0 : 3;
	uint32_t loop_id : 5;
	uint32_t is_predicated_break : 1;
	uint32_t reserved1 : 10;

	// Word 1: (16 bits)
	uint32_t reserved2 : 10;
	uint32_t condition : 1;
	uint32_t address_mode : 1;
	uint32_t opcode : 4;
};
struct LoopEnd {
	uint32_t address;
	uint32_t reserved0;
	uint32_t loop_id;
	uint32_t is_predicated_break;
	uint32_t reserved1;

	uint32_t reserved2;
	uint32_t condition;
	uint32_t address_mode;
	uint32_t opcode;
};

// Instruction data for ControlFlowOpcode::kCondCall.
struct PackedCondCall {
	// Word 0: (32 bits)
	uint32_t address : 13;
	uint32_t is_unconditional : 1;
	uint32_t is_predicated : 1;
	uint32_t reserved0 : 17;

	// Word 1: (16 bits)
	uint32_t reserved1 : 2;
	uint32_t bool_address : 8;
	uint32_t condition : 1;
	uint32_t address_mode : 1;
	uint32_t opcode : 4;
};
struct CondCall {
	uint32_t address;
	uint32_t is_unconditional;
	uint32_t is_predicated;
	uint32_t reserved0;

	uint32_t reserved1;
	uint32_t bool_address;
	uint32_t condition;
	uint32_t address_mode;
	uint32_t opcode;
};

// Instruction data for ControlFlowOpcode::kReturn.
struct PackedReturn {
	// Word 0: (32 bits)
	uint32_t reserved0 : 32;

	// Word 1: (16 bits)
	uint32_t reserved1 : 11;
	uint32_t address_mode : 1;
	uint32_t opcode : 4;
};
struct Return {
	uint32_t reserved0;

	uint32_t reserved1;
	uint32_t address_mode;
	uint32_t opcode;
};

// Instruction data for ControlFlowOpcode::kCondJmp.
struct PackedCondJmp {
	// Word 0: (32 bits)
	uint32_t address : 13;
	uint32_t is_unconditional : 1;
	uint32_t is_predicated : 1;
	uint32_t reserved0 : 17;

	// Word 1: (16 bits)
	uint32_t reserved1 : 1;
	uint32_t direction : 1;
	uint32_t bool_address : 8;
	uint32_t condition : 1;
	uint32_t address_mode : 1;
	uint32_t opcode : 4;
};
struct CondJmp {
	uint32_t address;
	uint32_t is_unconditional;
	uint32_t is_predicated;
	uint32_t reserved0;

	uint32_t reserved1;
	uint32_t direction;
	uint32_t bool_address;
	uint32_t condition;
	uint32_t address_mode;
	uint32_t opcode;
};

// Instruction data for ControlFlowOpcode::kAlloc.
struct PackedAlloc {
	// Word 0: (32 bits)
	uint32_t size : 3;
	uint32_t reserved0 : 29;

	// Word 1: (16 bits)
	uint32_t reserved1 : 8;
	uint32_t is_unserialized : 1;
	uint32_t alloc_type : 2;
	uint32_t reserved2: 1;
	uint32_t opcode : 4;
};
struct Alloc {
	uint32_t size;
	uint32_t reserved0;

	uint32_t reserved1;
	uint32_t is_unserialized;
	uint32_t alloc_type;
	uint32_t reserved2;
	uint32_t opcode;
};

union CFUnion {

	PackedExec exec;                    // kExec*
	PackedCondExec cond_exec;           // kCondExec*
	PackedCondExecPred cond_exec_pred;  // kCondExecPred*
	PackedLoopStart loop_start;         // kLoopStart
	PackedLoopEnd loop_end;             // kLoopEnd
	PackedCondCall cond_call;           // kCondCall
	PackedReturn ret;                   // kReturn
	PackedCondJmp cond_jmp;             // kCondJmp
	PackedAlloc alloc;                  // kAlloc
	
	struct {
		uint32_t unused_0 : 32;
		uint32_t unused_1 : 12;
		uint32_t opcode_value : 4;
	};
	struct {
		uint32_t dword;
		uint32_t word;
	};
};

struct CFInstruction {
	Exec exec;
	CondExec cond_exec;
	CondExecPred cond_exec_pred;
	LoopStart loop_start;
	LoopEnd loop_end;
	CondCall cond_call;
	Return ret;
	CondJmp cond_jmp;
	Alloc alloc;
	uint32_t opcode;
};

#pragma pack(pop)

extern "C"
{
	__declspec(dllexport) void decodeCF(uint32_t dword0, uint32_t dword1, CFInstruction *cfi)
	{
		CFUnion cfu;
		cfu.dword = dword0;
		cfu.word = dword1;

		cfi->opcode = cfu.opcode_value;

		cfi->exec.address = cfu.exec.address;
		cfi->exec.address_mode = cfu.exec.address_mode;
		cfi->exec.clean = cfu.exec.clean;
		cfi->exec.count = cfu.exec.count;
		cfi->exec.is_yeild = cfu.exec.is_yeild;
		cfi->exec.opcode = cfu.exec.opcode;
		cfi->exec.reserved0 = cfu.exec.reserved0;
		cfi->exec.reserved1 = cfu.exec.reserved1;
		cfi->exec.serialize = cfu.exec.serialize;
		cfi->exec.vc_hi = cfu.exec.vc_hi;
		cfi->exec.vc_lo = cfu.exec.vc_lo;

		cfi->cond_exec.address = cfu.cond_exec.address;
		cfi->cond_exec.address_mode = cfu.cond_exec.address_mode;
		cfi->cond_exec.bool_address = cfu.cond_exec.bool_address;
		cfi->cond_exec.condition = cfu.cond_exec.condition;
		cfi->cond_exec.count = cfu.cond_exec.count;
		cfi->cond_exec.is_yeild = cfu.cond_exec.is_yeild;
		cfi->cond_exec.opcode = cfu.cond_exec.opcode;
		cfi->cond_exec.serialize = cfu.cond_exec.serialize;
		cfi->cond_exec.vc_hi = cfu.cond_exec.vc_hi;
		cfi->cond_exec.vc_lo = cfu.cond_exec.vc_lo;

		cfi->cond_exec_pred.address = cfu.cond_exec_pred.address;
		cfi->cond_exec_pred.address_mode = cfu.cond_exec_pred.address_mode;
		cfi->cond_exec_pred.clean = cfu.cond_exec_pred.clean;
		cfi->cond_exec_pred.condition = cfu.cond_exec_pred.condition;
		cfi->cond_exec_pred.count = cfu.cond_exec_pred.count;
		cfi->cond_exec_pred.is_yeild = cfu.cond_exec_pred.is_yeild;
		cfi->cond_exec_pred.opcode = cfu.cond_exec_pred.opcode;
		cfi->cond_exec_pred.reserved0 = cfu.cond_exec_pred.reserved0;
		cfi->cond_exec_pred.serialize = cfu.cond_exec_pred.serialize;
		cfi->cond_exec_pred.vc_hi = cfu.cond_exec_pred.vc_hi;
		cfi->cond_exec_pred.vc_lo = cfu.cond_exec_pred.vc_lo;

		cfi->loop_start.address = cfu.loop_start.address;
		cfi->loop_start.address_mode = cfu.loop_start.address_mode;
		cfi->loop_start.is_repeat = cfu.loop_start.is_repeat;
		cfi->loop_start.loop_id = cfu.loop_start.loop_id;
		cfi->loop_start.opcode = cfu.loop_start.opcode;
		cfi->loop_start.reserved0 = cfu.loop_start.reserved0;
		cfi->loop_start.reserved1 = cfu.loop_start.reserved1;
		cfi->loop_start.reserved2 = cfu.loop_start.reserved2;

		cfi->loop_end.address = cfu.loop_end.address;
		cfi->loop_end.address_mode = cfu.loop_end.address_mode;
		cfi->loop_end.condition = cfu.loop_end.condition;
		cfi->loop_end.is_predicated_break = cfu.loop_end.is_predicated_break;
		cfi->loop_end.loop_id = cfu.loop_end.loop_id;
		cfi->loop_end.opcode = cfu.loop_end.opcode;
		cfi->loop_end.reserved0 = cfu.loop_end.reserved0;
		cfi->loop_end.reserved1 = cfu.loop_end.reserved1;
		cfi->loop_end.reserved2 = cfu.loop_end.reserved2;

		cfi->cond_call.address = cfu.cond_call.address;
		cfi->cond_call.address_mode = cfu.cond_call.address_mode;
		cfi->cond_call.bool_address = cfu.cond_call.bool_address;
		cfi->cond_call.condition = cfu.cond_call.condition;
		cfi->cond_call.is_predicated = cfu.cond_call.is_predicated;
		cfi->cond_call.is_unconditional = cfu.cond_call.is_unconditional;
		cfi->cond_call.opcode = cfu.cond_call.opcode;
		cfi->cond_call.reserved0 = cfu.cond_call.reserved0;
		cfi->cond_call.reserved1 = cfu.cond_call.reserved1;

		cfi->ret.address_mode = cfu.ret.address_mode;
		cfi->ret.opcode = cfu.ret.opcode;
		cfi->ret.reserved0 = cfu.ret.reserved0;
		cfi->ret.reserved1 = cfu.ret.reserved1;

		cfi->cond_jmp.address = cfu.cond_jmp.address;
		cfi->cond_jmp.address_mode = cfu.cond_jmp.address_mode;
		cfi->cond_jmp.bool_address = cfu.cond_jmp.bool_address;
		cfi->cond_jmp.condition = cfu.cond_jmp.condition;
		cfi->cond_jmp.direction = cfu.cond_jmp.direction;
		cfi->cond_jmp.is_predicated = cfu.cond_jmp.is_predicated;
		cfi->cond_jmp.is_unconditional = cfu.cond_jmp.is_unconditional;
		cfi->cond_jmp.opcode = cfu.cond_jmp.opcode;
		cfi->cond_jmp.reserved0 = cfu.cond_jmp.reserved0;
		cfi->cond_jmp.reserved1 = cfu.cond_jmp.reserved1;

		cfi->alloc.alloc_type = cfu.alloc.alloc_type;
		cfi->alloc.is_unserialized = cfu.alloc.is_unserialized;
		cfi->alloc.opcode = cfu.alloc.opcode;
		cfi->alloc.reserved0 = cfu.alloc.reserved0;
		cfi->alloc.reserved1 = cfu.alloc.reserved1;
		cfi->alloc.reserved2 = cfu.alloc.reserved2;
		cfi->alloc.size = cfu.alloc.size;
	}
}