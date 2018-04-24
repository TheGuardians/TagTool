using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderDecompiler.UcodeDisassembler
{
	public enum Swizzle
	{
		xyzw = 0x00,
		yyzw = 0x01,
		zyzw = 0x02,
		wyzw = 0x03,
		xzzw = 0x04,
		yzzw = 0x05,
		zzzw = 0x06,
		wzzw = 0x07,
		xwzw = 0x08,
		ywzw = 0x09,
		zwzw = 0x0A,
		wwzw = 0x0B,
		xxzw = 0x0C,
		yxzw = 0x0D,
		zxzw = 0x0E,
		wxzw = 0x0F,
		xyww = 0x10,
		yyww = 0x11,
		zyww = 0x12,
		wyww = 0x13,
		xzww = 0x14,
		yzww = 0x15,
		zzww = 0x16,
		wzww = 0x17,
		xwww = 0x18,
		ywww = 0x19,
		zwww = 0x1A,
		wwww = 0x1B,
		xxww = 0x1C,
		yxww = 0x1D,
		zxww = 0x1E,
		wxww = 0x1F,
		xyxw = 0x20,
		yyxw = 0x21,
		zyxw = 0x22,
		wyxw = 0x23,
		xzxw = 0x24,
		yzxw = 0x25,
		zzxw = 0x26,
		wzxw = 0x27,
		xwxw = 0x28,
		ywxw = 0x29,
		zwxw = 0x2A,
		wwxw = 0x2B,
		xxxw = 0x2C,
		yxxw = 0x2D,
		zxxw = 0x2E,
		wxxw = 0x2F,
		xyyw = 0x30,
		yyyw = 0x31,
		zyyw = 0x32,
		wyyw = 0x33,
		xzyw = 0x34,
		yzyw = 0x35,
		zzyw = 0x36,
		wzyw = 0x37,
		xwyw = 0x38,
		ywyw = 0x39,
		zwyw = 0x3A,
		wwyw = 0x3B,
		xxyw = 0x3C,
		yxyw = 0x3D,
		zxyw = 0x3E,
		wxyw = 0x3F,
		xyzx = 0x40,
		yyzx = 0x41,
		zyzx = 0x42,
		wyzx = 0x43,
		xzzx = 0x44,
		yzzx = 0x45,
		zzzx = 0x46,
		wzzx = 0x47,
		xwzx = 0x48,
		ywzx = 0x49,
		zwzx = 0x4A,
		wwzx = 0x4B,
		xxzx = 0x4C,
		yxzx = 0x4D,
		zxzx = 0x4E,
		wxzx = 0x4F,
		xywx = 0x50,
		yywx = 0x51,
		zywx = 0x52,
		wywx = 0x53,
		xzwx = 0x54,
		yzwx = 0x55,
		zzwx = 0x56,
		wzwx = 0x57,
		xwwx = 0x58,
		ywwx = 0x59,
		zwwx = 0x5A,
		wwwx = 0x5B,
		xxwx = 0x5C,
		yxwx = 0x5D,
		zxwx = 0x5E,
		wxwx = 0x5F,
		xyxx = 0x60,
		yyxx = 0x61,
		zyxx = 0x62,
		wyxx = 0x63,
		xzxx = 0x64,
		yzxx = 0x65,
		zzxx = 0x66,
		wzxx = 0x67,
		xwxx = 0x68,
		ywxx = 0x69,
		zwxx = 0x6A,
		wwxx = 0x6B,
		xxxx = 0x6C,
		yxxx = 0x6D,
		zxxx = 0x6E,
		wxxx = 0x6F,
		xyyx = 0x70,
		yyyx = 0x71,
		zyyx = 0x72,
		wyyx = 0x73,
		xzyx = 0x74,
		yzyx = 0x75,
		zzyx = 0x76,
		wzyx = 0x77,
		xwyx = 0x78,
		ywyx = 0x79,
		zwyx = 0x7A,
		wwyx = 0x7B,
		xxyx = 0x7C,
		yxyx = 0x7D,
		zxyx = 0x7E,
		wxyx = 0x7F,
		xyzy = 0x80,
		yyzy = 0x81,
		zyzy = 0x82,
		wyzy = 0x83,
		xzzy = 0x84,
		yzzy = 0x85,
		zzzy = 0x86,
		wzzy = 0x87,
		xwzy = 0x88,
		ywzy = 0x89,
		zwzy = 0x8A,
		wwzy = 0x8B,
		xxzy = 0x8C,
		yxzy = 0x8D,
		zxzy = 0x8E,
		wxzy = 0x8F,
		xywy = 0x90,
		yywy = 0x91,
		zywy = 0x92,
		wywy = 0x93,
		xzwy = 0x94,
		yzwy = 0x95,
		zzwy = 0x96,
		wzwy = 0x97,
		xwwy = 0x98,
		ywwy = 0x99,
		zwwy = 0x9A,
		wwwy = 0x9B,
		xxwy = 0x9C,
		yxwy = 0x9D,
		zxwy = 0x9E,
		wxwy = 0x9F,
		xyxy = 0xA0,
		yyxy = 0xA1,
		zyxy = 0xA2,
		wyxy = 0xA3,
		xzxy = 0xA4,
		yzxy = 0xA5,
		zzxy = 0xA6,
		wzxy = 0xA7,
		xwxy = 0xA8,
		ywxy = 0xA9,
		zwxy = 0xAA,
		wwxy = 0xAB,
		xxxy = 0xAC,
		yxxy = 0xAD,
		zxxy = 0xAE,
		wxxy = 0xAF,
		xyyy = 0xB0,
		yyyy = 0xB1,
		zyyy = 0xB2,
		wyyy = 0xB3,
		xzyy = 0xB4,
		yzyy = 0xB5,
		zzyy = 0xB6,
		wzyy = 0xB7,
		xwyy = 0xB8,
		ywyy = 0xB9,
		zwyy = 0xBA,
		wwyy = 0xBB,
		xxyy = 0xBC,
		yxyy = 0xBD,
		zxyy = 0xBE,
		wxyy = 0xBF,
		xyzz = 0xC0,
		yyzz = 0xC1,
		zyzz = 0xC2,
		wyzz = 0xC3,
		xzzz = 0xC4,
		yzzz = 0xC5,
		zzzz = 0xC6,
		wzzz = 0xC7,
		xwzz = 0xC8,
		ywzz = 0xC9,
		zwzz = 0xCA,
		wwzz = 0xCB,
		xxzz = 0xCC,
		yxzz = 0xCD,
		zxzz = 0xCE,
		wxzz = 0xCF,
		xywz = 0xD0,
		yywz = 0xD1,
		zywz = 0xD2,
		wywz = 0xD3,
		xzwz = 0xD4,
		yzwz = 0xD5,
		zzwz = 0xD6,
		wzwz = 0xD7,
		xwwz = 0xD8,
		ywwz = 0xD9,
		zwwz = 0xDA,
		wwwz = 0xDB,
		xxwz = 0xDC,
		yxwz = 0xDD,
		zxwz = 0xDE,
		wxwz = 0xDF,
		xyxz = 0xE0,
		yyxz = 0xE1,
		zyxz = 0xE2,
		wyxz = 0xE3,
		xzxz = 0xE4,
		yzxz = 0xE5,
		zzxz = 0xE6,
		wzxz = 0xE7,
		xwxz = 0xE8,
		ywxz = 0xE9,
		zwxz = 0xEA,
		wwxz = 0xEB,
		xxxz = 0xEC,
		yxxz = 0xED,
		zxxz = 0xEE,
		wxxz = 0xEF,
		xyyz = 0xF0,
		yyyz = 0xF1,
		zyyz = 0xF2,
		wyyz = 0xF3,
		xzyz = 0xF4,
		yzyz = 0xF5,
		zzyz = 0xF6,
		wzyz = 0xF7,
		xwyz = 0xF8,
		ywyz = 0xF9,
		zwyz = 0xFA,
		wwyz = 0xFB,
		xxyz = 0xFC,
		yxyz = 0xFD,
		zxyz = 0xFE,
		wxyz = 0xFF,
	}
	public enum Mask
	{
		A_Vector = 0x01, // Vector destination mask
		B_Vector = 0x02, // Vector destination mask
		C_Vector = 0x04, // Vector destination mask
		D_Vector = 0x08, // Vector destination mask
	}

	public enum ControlFlowOpcode : uint
	{
		// No-op - used to fill space.
		cnop = 0,

		// Executes up to six fetch or ALU instructions.
		exec = 1,

		// Executes up to six fetch or ALU instructions then ends execution.
		exece = 2,

		// Conditionally executes up to six fetch or ALU instructions based on a bool const.
		cexec = 3,

		// Conditionally executes up to six fetch or ALU instructions based on a bool const then ends execution.
		cexece = 4,

		// Conditionally executes up to six fetch or ALU instructions based on the current predicate.
		cexec_pred = 5,

		// Conditionally executes up to six fetch or ALU instructions based on the current predicate then ends execution.
		cexece_pred = 6,

		// Starts a loop that must be terminated with 'endloop'.
		loop = 7,

		// Continues or breaks out of a loop started with 'loop'.
		endloop = 8,

		// Conditionally calls a function.  A return address is pushed to the stack to be used by a 'ret' instruction.
		ccall = 9,

		// Returns from the current function as called by 'ccall'. This is a no-op if not in a function.
		ret = 10,

		// Conditionally jumps to an arbitrary address based on a bool const.
		cjmp = 11,

		// Reserves space in the internal buffers that store the shader's output values. 
		// This instruction must appear before the corresponding output values are written. 
		alloc = 12,

		// Conditionally executes based on the current predicate. Optionally resets the predicate value.
		cexec_pred_clean = 13,

		// Conditionally executes based on the current predicate then ends execution. Optionally resets the predicate value.
		cexece_pred_clean = 14,

		// Hints that no more vertex fetches will be performed.
		vfetche = 15
	}
	public enum VectorOpcode
	{
		// Per-Component Floating-Point Add
		// add dest, src0, src1
		//     dest.x = src0.x + src1.x;
		//     dest.y = src0.y + src1.y;
		//     dest.z = src0.z + src1.z;
		//     dest.w = src0.w + src1.w;
		add = 0,

		// Per-Component Floating-Point Multiply
		// mul dest, src0, src1
		//     dest.x = src0.x * src1.x;
		//     dest.y = src0.y * src1.y;
		//     dest.z = src0.z * src1.z;
		//     dest.w = src0.w * src1.w;
		mul = 1,

		// Per-Component Floating-Point Maximum
		// max dest, src0, src1
		//     dest.x = src0.x >= src1.x ? src0.x : src1.x;
		//     dest.y = src0.x >= src1.y ? src0.y : src1.y;
		//     dest.z = src0.x >= src1.z ? src0.z : src1.z;
		//     dest.w = src0.x >= src1.w ? src0.w : src1.w;
		max = 2,

		// Per-Component Floating-Point Minimum
		// min dest, src0, src1
		//     dest.x = src0.x < src1.x ? src0.x : src1.x;
		//     dest.y = src0.x < src1.y ? src0.y : src1.y;
		//     dest.z = src0.x < src1.z ? src0.z : src1.z;
		//     dest.w = src0.x < src1.w ? src0.w : src1.w;
		min = 3,

		// Per-Component Floating-Point Set If Equal
		// seq dest, src0, src1
		//     dest.x = src0.x == src1.x ? 1.0 : 0.0;
		//     dest.y = src0.y == src1.y ? 1.0 : 0.0;
		//     dest.z = src0.z == src1.z ? 1.0 : 0.0;
		//     dest.w = src0.w == src1.w ? 1.0 : 0.0;
		seq = 4,

		// Per-Component Floating-Point Set If Greater Than
		// sgt dest, src0, src1
		//     dest.x = src0.x > src1.x ? 1.0 : 0.0;
		//     dest.y = src0.y > src1.y ? 1.0 : 0.0;
		//     dest.z = src0.z > src1.z ? 1.0 : 0.0;
		//     dest.w = src0.w > src1.w ? 1.0 : 0.0;
		sgt = 5,

		// Per-Component Floating-Point Set If Greater Than Or Equal
		// sge dest, src0, src1
		//     dest.x = src0.x >= src1.x ? 1.0 : 0.0;
		//     dest.y = src0.y >= src1.y ? 1.0 : 0.0;
		//     dest.z = src0.z >= src1.z ? 1.0 : 0.0;
		//     dest.w = src0.w >= src1.w ? 1.0 : 0.0;
		sge = 6,

		// Per-Component Floating-Point Set If Not Equal
		// sne dest, src0, src1
		//     dest.x = src0.x != src1.x ? 1.0 : 0.0;
		//     dest.y = src0.y != src1.y ? 1.0 : 0.0;
		//     dest.z = src0.z != src1.z ? 1.0 : 0.0;
		//     dest.w = src0.w != src1.w ? 1.0 : 0.0;
		sne = 7,

		// Per-Component Floating-Point Fractional
		// frc dest, src0
		//     dest.x = src0.x - floor(src0.x);
		//     dest.y = src0.y - floor(src0.y);
		//     dest.z = src0.z - floor(src0.z);
		//     dest.w = src0.w - floor(src0.w);
		frc = 8,

		// Per-Component Floating-Point Truncate
		// trunc dest, src0
		//     dest.x = src0.x >= 0 ? floor(src0.x) : -floor(-src0.x);
		//     dest.y = src0.y >= 0 ? floor(src0.y) : -floor(-src0.y);
		//     dest.z = src0.z >= 0 ? floor(src0.z) : -floor(-src0.z);
		//     dest.w = src0.w >= 0 ? floor(src0.w) : -floor(-src0.w);
		trunc = 9,

		// Per-Component Floating-Point Floor
		// floor dest, src0
		//     dest.x = floor(src0.x);
		//     dest.y = floor(src0.y);
		//     dest.z = floor(src0.z);
		//     dest.w = floor(src0.w);
		floor = 10,

		// Per-Component Floating-Point Multiply-Add
		// mad dest, src0, src1, src2
		//     dest.x = src0.x * src1.x + src2.x;
		//     dest.y = src0.y * src1.y + src2.y;
		//     dest.z = src0.z * src1.z + src2.z;
		//     dest.w = src0.w * src1.w + src2.w;
		mad = 11,

		// Per-Component Floating-Point Conditional Move If Equal
		// cndeq dest, src0, src1, src2
		//     dest.x = src0.x == 0.0 ? src1.x : src2.x;
		//     dest.y = src0.y == 0.0 ? src1.y : src2.y;
		//     dest.z = src0.z == 0.0 ? src1.z : src2.z;
		//     dest.w = src0.w == 0.0 ? src1.w : src2.w;
		cndeq = 12,

		// Per-Component Floating-Point Conditional Move If Greater Than Or Equal
		// cndge dest, src0, src1, src2
		//     dest.x = src0.x >= 0.0 ? src1.x : src2.x;
		//     dest.y = src0.y >= 0.0 ? src1.y : src2.y;
		//     dest.z = src0.z >= 0.0 ? src1.z : src2.z;
		//     dest.w = src0.w >= 0.0 ? src1.w : src2.w;
		cndge = 13,

		// Per-Component Floating-Point Conditional Move If Greater Than
		// cndgt dest, src0, src1, src2
		//     dest.x = src0.x > 0.0 ? src1.x : src2.x;
		//     dest.y = src0.y > 0.0 ? src1.y : src2.y;
		//     dest.z = src0.z > 0.0 ? src1.z : src2.z;
		//     dest.w = src0.w > 0.0 ? src1.w : src2.w;
		cndgt = 14,

		// Four-Element Dot Product
		// dp4 dest, src0, src1
		//     dest.xyzw = src0.x * src1.x + src0.y * src1.y + src0.z * src1.z +
		//                 src0.w * src1.w;
		// Note: only pv.x contains the value.
		dp4 = 15,

		// Three-Element Dot Product
		// dp3 dest, src0, src1
		//     dest.xyzw = src0.x * src1.x + src0.y * src1.y + src0.z * src1.z;
		// Note: only pv.x contains the value.
		dp3 = 16,

		// Two-Element Dot Product and Add
		// dp2add dest, src0, src1, src2
		//     dest.xyzw = src0.x * src1.x + src0.y * src1.y + src2.x;
		// Note: only pv.x contains the value.
		dp2add = 17,

		// Cube Map
		// cube dest, src0, src1
		//     dest.x = T cube coordinate;
		//     dest.y = S cube coordinate;
		//     dest.z = 2.0 * MajorAxis;
		//     dest.w = FaceID;
		// Expects src0.zzxy and src1.yxzz swizzles.
		// FaceID is D3DCUBEMAP_FACES:
		// https://msdn.microsoft.com/en-us/library/windows/desktop/bb172528(v=vs.85).aspx
		cube = 18,

		// Four-Element Maximum
		// max4 dest, src0
		//     dest.xyzw = max(src0.x, src0.y, src0.z, src0.w);
		// Note: only pv.x contains the value.
		max4 = 19,

		// Floating-Point Predicate Counter Increment If Equal
		// setp_eq_push dest, src0, src1
		//     if (src0.w == 0.0 && src1.w == 0.0) {
		//       p0 = 1;
		//     } else {
		//       p0 = 0;
		//     }
		//     if (src0.x == 0.0 && src1.x == 0.0) {
		//       dest.xyzw = 0.0;
		//     } else {
		//       dest.xyzw = src0.x + 1.0;
		//     }
		setp_eq_push = 20,

		// Floating-Point Predicate Counter Increment If Not Equal
		// setp_ne_push dest, src0, src1
		//     if (src0.w == 0.0 && src1.w != 0.0) {
		//       p0 = 1;
		//     } else {
		//       p0 = 0;
		//     }
		//     if (src0.x == 0.0 && src1.x != 0.0) {
		//       dest.xyzw = 0.0;
		//     } else {
		//       dest.xyzw = src0.x + 1.0;
		//     }
		setp_ne_push = 21,

		// Floating-Point Predicate Counter Increment If Greater Than
		// setp_gt_push dest, src0, src1
		//     if (src0.w == 0.0 && src1.w > 0.0) {
		//       p0 = 1;
		//     } else {
		//       p0 = 0;
		//     }
		//     if (src0.x == 0.0 && src1.x > 0.0) {
		//       dest.xyzw = 0.0;
		//     } else {
		//       dest.xyzw = src0.x + 1.0;
		//     }
		setp_gt_push = 22,

		// Floating-Point Predicate Counter Increment If Greater Than Or Equal
		// setp_ge_push dest, src0, src1
		//     if (src0.w == 0.0 && src1.w >= 0.0) {
		//       p0 = 1;
		//     } else {
		//       p0 = 0;
		//     }
		//     if (src0.x == 0.0 && src1.x >= 0.0) {
		//       dest.xyzw = 0.0;
		//     } else {
		//       dest.xyzw = src0.x + 1.0;
		//     }
		setp_ge_push = 23,

		// Floating-Point Pixel Kill If Equal
		// kill_eq dest, src0, src1
		//     if (src0.x == src1.x ||
		//         src0.y == src1.y ||
		//         src0.z == src1.z ||
		//         src0.w == src1.w) {
		//       dest.xyzw = 1.0;
		//       discard;
		//     } else {
		//       dest.xyzw = 0.0;
		//     }
		kill_eq = 24,

		// Floating-Point Pixel Kill If Greater Than
		// kill_gt dest, src0, src1
		//     if (src0.x > src1.x ||
		//         src0.y > src1.y ||
		//         src0.z > src1.z ||
		//         src0.w > src1.w) {
		//       dest.xyzw = 1.0;
		//       discard;
		//     } else {
		//       dest.xyzw = 0.0;
		//     }
		kill_gt = 25,

		// Floating-Point Pixel Kill If Greater-Or-Equal
		// kill_ge dest, src0, src1
		//     if (src0.x >= src1.x ||
		//         src0.y >= src1.y ||
		//         src0.z >= src1.z ||
		//         src0.w >= src1.w) {
		//       dest.xyzw = 1.0;
		//       discard;
		//     } else {
		//       dest.xyzw = 0.0;
		//     }
		kill_ge = 26,

		// Floating-Point Pixel Kill If Not-Equal
		// kill_ne dest, src0, src1
		//     if (src0.x != src1.x ||
		//         src0.y != src1.y ||
		//         src0.z != src1.z ||
		//         src0.w != src1.w) {
		//       dest.xyzw = 1.0;
		//       discard;
		//     } else {
		//       dest.xyzw = 0.0;
		//     }
		kill_ne = 27,

		// Floating-Point Distance Vector Computation
		// dst dest, src0, src1
		//     dest.x = 1.0;
		//     dest.y = src0.y * src1.y;
		//     dest.z = src0.z;
		//     dest.w = src1.w;
		dst = 28,

		// Per-Component Floating-Point Maximum with Copy To Integer in Address Register (a0)
		// maxa dest, src0, src1
		// This is a combined max + mova.
		//     int result = (int)floor(src0.w + 0.5);
		//     a0 = clamp(result, -256, 255);
		//     dest.x = src0.x >= src1.x ? src0.x : src1.x;
		//     dest.y = src0.x >= src1.y ? src0.y : src1.y;
		//     dest.z = src0.x >= src1.z ? src0.z : src1.z;
		//     dest.w = src0.x >= src1.w ? src0.w : src1.w;
		maxa = 29,

		// disassembler calls it this, probably unused garbage.
		opcode_30 = 30,
		// disassembler calls it this, probably unused garbage.
		opcode_31 = 31,
	}
	public enum ScalarOpcode
	{
		// Floating-Point Add
		// adds dest, src0.ab
		//     dest.xyzw = src0.a + src0.b;
		adds = 0,

		// Floating-Point Add (with Previous)
		// adds_prev dest, src0.a
		//     dest.xyzw = src0.a + ps;
		adds_prev = 1,

		// Floating-Point Multiply
		// muls dest, src0.ab
		//     dest.xyzw = src0.a * src0.b;
		muls = 2,

		// Floating-Point Multiply (with Previous)
		// muls_prev dest, src0.a
		//     dest.xyzw = src0.a * ps;
		muls_prev = 3,

		// Scalar Multiply Emulating LIT Operation
		// muls_prev2 dest, src0.ab
		//    dest.xyzw =
		//        ps == -FLT_MAX || !isfinite(ps) || !isfinite(src0.b) || src0.b <= 0
		//        ? -FLT_MAX : src0.a * ps;
		muls_prev2 = 4,

		// Floating-Point Maximum
		// maxs dest, src0.ab
		//     dest.xyzw = src0.a >= src0.b ? src0.a : src0.b;
		maxs = 5,

		// Floating-Point Minimum
		// mins dest, src0.ab
		//     dest.xyzw = src0.a < src0.b ? src0.a : src0.b;
		mins = 6,

		// Floating-Point Set If Equal
		// seqs dest, src0.a
		//     dest.xyzw = src0.a == 0.0 ? 1.0 : 0.0;
		seqs = 7,

		// Floating-Point Set If Greater Than
		// sgts dest, src0.a
		//     dest.xyzw = src0.a > 0.0 ? 1.0 : 0.0;
		sgts = 8,

		// Floating-Point Set If Greater Than Or Equal
		// sges dest, src0.a
		//     dest.xyzw = src0.a >= 0.0 ? 1.0 : 0.0;
		sges = 9,

		// Floating-Point Set If Not Equal
		// snes dest, src0.a
		//     dest.xyzw = src0.a != 0.0 ? 1.0 : 0.0;
		snes = 10,

		// Floating-Point Fractional
		// frcs dest, src0.a
		//     dest.xyzw = src0.a - floor(src0.a);
		frcs = 11,

		// Floating-Point Truncate
		// truncs dest, src0.a
		//     dest.xyzw = src0.a >= 0 ? floor(src0.a) : -floor(-src0.a);
		truncs = 12,

		// Floating-Point Floor
		// floors dest, src0.a
		//     dest.xyzw = floor(src0.a);
		floors = 13,

		// Scalar Base-2 Exponent, IEEE
		// exp dest, src0.a
		//     dest.xyzw = src0.a == 0.0 ? 1.0 : pow(2, src0.a);
		exp = 14,

		// Scalar Base-2 Log
		// logc dest, src0.a
		//     float t = src0.a == 1.0 ? 0.0 : log(src0.a) / log(2.0);
		//     dest.xyzw = t == -INF ? -FLT_MAX : t;
		logc = 15,

		// Scalar Base-2 IEEE Log
		// log dest, src0.a
		//     dest.xyzw = src0.a == 1.0 ? 0.0 : log(src0.a) / log(2.0);
		log = 16,

		// Scalar Reciprocal, Clamp to Maximum
		// rcpc dest, src0.a
		//     float t = src0.a == 1.0 ? 1.0 : 1.0 / src0.a;
		//     if (t == -INF) t = -FLT_MAX;
		//     else if (t == INF) t = FLT_MAX;
		//     dest.xyzw = t;
		rcpc = 17,

		// Scalar Reciprocal, Clamp to Zero
		// rcpf dest, src0.a
		//     float t = src0.a == 1.0 ? 1.0 : 1.0 / src0.a;
		//     if (t == -INF) t = -0.0;
		//     else if (t == INF) t = 0.0;
		//     dest.xyzw = t;
		rcpf = 18,

		// Scalar Reciprocal, IEEE Approximation
		// rcp dest, src0.a
		//     dest.xyzw = src0.a == 1.0 ? 1.0 : 1.0 / src0.a;
		rcp = 19,

		// Scalar Reciprocal Square Root, Clamp to Maximum
		// rsqc dest, src0.a
		//     float t = src0.a == 1.0 ? 1.0 : 1.0 / sqrt(src0.a);
		//     if (t == -INF) t = -FLT_MAX;
		//     else if (t == INF) t = FLT_MAX;
		//     dest.xyzw = t;
		rsqc = 20,

		// Scalar Reciprocal Square Root, Clamp to Zero
		// rsqc dest, src0.a
		//     float t = src0.a == 1.0 ? 1.0 : 1.0 / sqrt(src0.a);
		//     if (t == -INF) t = -0.0;
		//     else if (t == INF) t = 0.0;
		//     dest.xyzw = t;
		rsqf = 21,

		// Scalar Reciprocal Square Root, IEEE Approximation
		// rsq dest, src0.a
		//     dest.xyzw = src0.a == 1.0 ? 1.0 : 1.0 / sqrt(src0.a);
		rsq = 22,

		// Floating-Point Maximum with Copy To Integer in AR
		// maxas dest, src0.ab
		// movas dest, src0.aa
		//     int result = (int)floor(src0.a + 0.5);
		//     a0 = clamp(result, -256, 255);
		//     dest.xyzw = src0.a >= src0.b ? src0.a : src0.b;
		maxas = 23,

		// Floating-Point Maximum with Copy Truncated To Integer in AR
		// maxasf dest, src0.ab
		// movasf dest, src0.aa
		//     int result = (int)floor(src0.a);
		//     a0 = clamp(result, -256, 255);
		//     dest.xyzw = src0.a >= src0.b ? src0.a : src0.b;
		maxasf = 24,

		// Floating-Point Subtract
		// subs dest, src0.ab
		//     dest.xyzw = src0.a - src0.b;
		subs = 25,

		// Floating-Point Subtract (with Previous)
		// subs_prev dest, src0.a
		//     dest.xyzw = src0.a - ps;
		subs_prev = 26,

		// Floating-Point Predicate Set If Equal
		// setp_eq dest, src0.a
		//     if (src0.a == 0.0) {
		//       dest.xyzw = 0.0;
		//       p0 = 1;
		//     } else {
		//       dest.xyzw = 1.0;
		//       p0 = 0;
		//     }
		setpeq = 27,

		// Floating-Point Predicate Set If Not Equal
		// setp_ne dest, src0.a
		//     if (src0.a != 0.0) {
		//       dest.xyzw = 0.0;
		//       p0 = 1;
		//     } else {
		//       dest.xyzw = 1.0;
		//       p0 = 0;
		//     }
		setpne = 28,

		// Floating-Point Predicate Set If Greater Than
		// setp_gt dest, src0.a
		//     if (src0.a > 0.0) {
		//       dest.xyzw = 0.0;
		//       p0 = 1;
		//     } else {
		//       dest.xyzw = 1.0;
		//       p0 = 0;
		//     }
		setpgt = 29,

		// Floating-Point Predicate Set If Greater Than Or Equal
		// setp_ge dest, src0.a
		//     if (src0.a >= 0.0) {
		//       dest.xyzw = 0.0;
		//       p0 = 1;
		//     } else {
		//       dest.xyzw = 1.0;
		//       p0 = 0;
		//     }
		setpge = 30,

		// Predicate Counter Invert
		// setp_inv dest, src0.a
		//     if (src0.a == 1.0) {
		//       dest.xyzw = 0.0;
		//       p0 = 1;
		//     } else {
		//       if (src0.a == 0.0) {
		//         dest.xyzw = 1.0;
		//       } else {
		//         dest.xyzw = src0.a;
		//       }
		//       p0 = 0;
		//     }
		setpinv = 31,

		// Predicate Counter Pop
		// setp_pop dest, src0.a
		//     if (src0.a - 1.0 <= 0.0) {
		//       dest.xyzw = 0.0;
		//       p0 = 1;
		//     } else {
		//       dest.xyzw = src0.a - 1.0;
		//       p0 = 0;
		//     }
		setppop = 32,

		// Predicate Counter Clear
		// setp_clr dest
		//     dest.xyzw = FLT_MAX;
		//     p0 = 0;
		setpclr = 33,

		// Predicate Counter Restore
		// setp_rstr dest, src0.a
		//     if (src0.a == 0.0) {
		//       dest.xyzw = 0.0;
		//       p0 = 1;
		//     } else {
		//       dest.xyzw = src0.a;
		//       p0 = 0;
		//     }
		setprstr = 34,

		// Floating-Point Pixel Kill If Equal
		// kills_eq dest, src0.a
		//     if (src0.a == 0.0) {
		//       dest.xyzw = 1.0;
		//       discard;
		//     } else {
		//       dest.xyzw = 0.0;
		//     }
		killseq = 35,

		// Floating-Point Pixel Kill If Greater Than
		// kills_gt dest, src0.a
		//     if (src0.a > 0.0) {
		//       dest.xyzw = 1.0;
		//       discard;
		//     } else {
		//       dest.xyzw = 0.0;
		//     }
		killsgt = 36,

		// Floating-Point Pixel Kill If Greater Than Or Equal
		// kills_ge dest, src0.a
		//     if (src0.a >= 0.0) {
		//       dest.xyzw = 1.0;
		//       discard;
		//     } else {
		//       dest.xyzw = 0.0;
		//     }
		killsge = 37,

		// Floating-Point Pixel Kill If Not Equal
		// kills_ne dest, src0.a
		//     if (src0.a != 0.0) {
		//       dest.xyzw = 1.0;
		//       discard;
		//     } else {
		//       dest.xyzw = 0.0;
		//     }
		killsne = 38,

		// Floating-Point Pixel Kill If One
		// kills_one dest, src0.a
		//     if (src0.a == 1.0) {
		//       dest.xyzw = 1.0;
		//       discard;
		//     } else {
		//       dest.xyzw = 0.0;
		//     }
		killsone = 39,

		// Scalar Square Root, IEEE Aproximation
		// sqrt dest, src0.a
		//     dest.xyzw = sqrt(src0.a);
		sqrt = 40,

		// disassembler calls it this, probably unused garbage.
		opcode_41 = 41,

		// Floating-Point Multiply - src0 must be a (c#) register, and the src1 must be a (r#) register. 
		// muls dest, src0.a, src1.b
		//     dest.xyzw = src0.a * src1.b;
		mulsc0 = 42,
		// same as mulsc0.
		mulsc1 = 43,

		// Floating-Point Add - src0 must be a (c#) register, and the src1 must be a (r#) register. 
		// addsc dest, src0.a, src1.b
		//     dest.xyzw = src0.a + src1.b;
		addsc0 = 44,
		// same as addsc0.
		addsc1 = 45,

		// Floating-Point Subtract - src0 must be a (c#) register, and the src1 must be a (r#) register. 
		// addsc dest, src0.a, src1.b
		//     dest.xyzw = src0.a - src1.b;
		subsc0 = 46,
		// same as subsc0.
		subsc1 = 47,

		// Scalar Sin
		// sin dest, src0.a
		//     dest.xyzw = sin(src0.a);
		sin = 48,

		// Scalar Cos
		// cos dest, src0.a
		//     dest.xyzw = cos(src0.a);
		cos = 49,

		// retain_prev dest
		//     dest.xyzw = ps;
		retain_prev = 50,

		// disassembler calls it this, probably unused garbage.
		opcode_51 = 51,
		// disassembler calls it this, probably unused garbage.
		opcode_52 = 52,
		// disassembler calls it this, probably unused garbage.
		opcode_53 = 53,
		// disassembler calls it this, probably unused garbage.
		opcode_54 = 54,
		// disassembler calls it this, probably unused garbage.
		opcode_55 = 55,
		// disassembler calls it this, probably unused garbage.
		opcode_56 = 56,
		// disassembler calls it this, probably unused garbage.
		opcode_57 = 57,
		// disassembler calls it this, probably unused garbage.
		opcode_58 = 58,
		// disassembler calls it this, probably unused garbage.
		opcode_59 = 59,
		// disassembler calls it this, probably unused garbage.
		opcode_60 = 60,
		// disassembler calls it this, probably unused garbage.
		opcode_61 = 61,
		// disassembler calls it this, probably unused garbage.
		opcode_62 = 62,
		// disassembler calls it this, probably unused garbage.
		opcode_63 = 63
	}
	public enum FetchOpCode
	{
		// Fetches data from a vertex buffer using a semantic. 
		vfetch = 0,

		// Fetches sample data from a texture. 
		tfetch = 1,

		//Gets the fraction of border color that would be blended into the texture data 
		// (retrieved using a texture-fetch) at the specified coordinates. 
		getBCF = 16,

		// For textures, gets the LOD for all of the pixels in the quad at the specified coordinates. 
		getCompTexLOD = 17,

		// Gets the gradients of the first source, relative to the screen horizontal and screen vertical. 
		getGradients = 18,

		// Gets the weights used in a bilinear fetch from textures. 
		getWeights = 0x13,

		// Sets the level of detail. 
		setTexLOD = 24,

		// Sets the horizontal gradients. 
		setGradientH = 25,

		// Sets the vertical gradients. 
		setGradientV = 26,

		// Converts xbox360 shaders to PC format! ecks dee.
		UnknownTextureOp = 27
	}

	public enum TextureFilter
	{
		Point = 0x00,
		Linear = 0x01,
		BaseMap = 0x02,  // Only applicable for mip-filter.
		UseFetchConst = 0x03,
	}
	public enum AnisoFilter
	{
		Disabled = 0x00,
		Max_1_1 = 0x01,
		Max_2_1 = 0x02,
		Max_4_1 = 0x03,
		Max_8_1 = 0x04,
		Max_16_1 = 0x05,
		UseFetchConst = 0x07,
	}
	public enum ArbitraryFilter
	{
		ARBITRARY_FILTER_2X4_SYM = 0x00,
		ARBITRARY_FILTER_2X4_ASYM = 0x01,
		ARBITRARY_FILTER_4X2_SYM = 0x02,
		ARBITRARY_FILTER_4X2_ASYM = 0x03,
		ARBITRARY_FILTER_4X4_SYM = 0x04,
		ARBITRARY_FILTER_4X4_ASYM = 0x05,
		ARBITRARY_FILTER_USE_FETCH_CONST = 0x07,
	}

	public enum SampleLocation
	{
		Centroid = 0x00,
		Center = 0x01,
	}
	public enum SurfaceFormat
	{
		FMT_1_REVERSE = 0x00,
		FMT_1 = 0x01,
		FMT_8 = 0x02,
		FMT_1_5_5_5 = 0x03,
		FMT_5_6_5 = 0x04,
		FMT_6_5_5 = 0x05,
		FMT_8_8_8_8 = 0x06,
		FMT_2_10_10_10 = 0x07,
		FMT_8_A = 0x08,
		FMT_8_B = 0x09,
		FMT_8_8 = 0x0A,
		FMT_Cr_Y1_Cb_Y0 = 0x0B,
		FMT_Y1_Cr_Y0_Cb = 0x0C,
		FMT_5_5_5_1 = 0x0D,
		FMT_8_8_8_8_A = 0x0E,
		FMT_4_4_4_4 = 0x0F,
		FMT_10_11_11 = 0x10,
		FMT_11_11_10 = 0x11,
		FMT_DXT1 = 0x12,
		FMT_DXT2_3 = 0x13,
		FMT_DXT4_5 = 0x14,
		FMT_24_8 = 0x16,
		FMT_24_8_FLOAT = 0x17,
		FMT_16 = 0x18,
		FMT_16_16 = 0x19,
		FMT_16_16_16_16 = 0x1A,
		FMT_16_EXPAND = 0x1B,
		FMT_16_16_EXPAND = 0x1C,
		FMT_16_16_16_16_EXPAND = 0x1D,
		FMT_16_FLOAT = 0x1E,
		FMT_16_16_FLOAT = 0x1F,
		FMT_16_16_16_16_FLOAT = 0x20,
		FMT_32 = 0x21,
		FMT_32_32 = 0x22,
		FMT_32_32_32_32 = 0x23,
		FMT_32_FLOAT = 0x24,
		FMT_32_32_FLOAT = 0x25,
		FMT_32_32_32_32_FLOAT = 0x26,
		FMT_32_AS_8 = 0x27,
		FMT_32_AS_8_8 = 0x28,
		FMT_16_MPEG = 0x29,
		FMT_16_16_MPEG = 0x2A,
		FMT_8_INTERLACED = 0x2B,
		FMT_32_AS_8_INTERLACED = 0x2C,
		FMT_32_AS_8_8_INTERLACED = 0x2D,
		FMT_16_INTERLACED = 0x2E,
		FMT_16_MPEG_INTERLACED = 0x2F,
		FMT_16_16_MPEG_INTERLACED = 0x30,
		FMT_DXN = 0x31,
		FMT_8_8_8_8_AS_16_16_16_16 = 0x32,
		FMT_DXT1_AS_16_16_16_16 = 0x33,
		FMT_DXT2_3_AS_16_16_16_16 = 0x34,
		FMT_DXT4_5_AS_16_16_16_16 = 0x35,
		FMT_2_10_10_10_AS_16_16_16_16 = 0x36,
		FMT_10_11_11_AS_16_16_16_16 = 0x37,
		FMT_11_11_10_AS_16_16_16_16 = 0x38,
		FMT_32_32_32_FLOAT = 0x39,
		FMT_DXT3A = 0x3A,
		FMT_DXT5A = 0x3B,
		FMT_CTX1 = 0x3C,
		FMT_DXT3A_AS_1_1_1_1 = 0x3D,
	}
	public enum VertexFormat
	{
		kUndefined = 0x00,
		k_8_8_8_8 = 0x06,
		k_2_10_10_10 = 0x07,
		k_10_11_11 = 0x10,
		k_11_11_10 = 0x11,
		k_16_16 = 0x19,
		k_16_16_16_16 = 0x1A,
		k_16_16_FLOAT = 0x1F,
		k_16_16_16_16_FLOAT = 0x20,
		k_32 = 0x21,
		k_32_32 = 0x22,
		k_32_32_32_32 = 0x23,
		k_32_FLOAT = 0x24,
		k_32_32_FLOAT = 0x25,
		k_32_32_32_32_FLOAT = 0x26,
		k_32_32_32_FLOAT = 0x39,
	}
	public enum TextureDimension
	{
		One = 0x00,
		Two = 0x01,
		Three = 0x02,
		Cube = 0x03,
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
}
