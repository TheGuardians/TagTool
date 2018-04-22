using System.Runtime.InteropServices;

/* WARNING: DO NOT TOUCH THIS FILE UNLESS YOU KNOW WHAT YOU'RE DOING, AND ALSO MAKE THE APPROPRIATE
 CHANGES TO THE MATCHING FILE IN THE `TagToolUtilities` PROJECT */

namespace TagTool.ShaderDecompiler.UcodeDisassembler
{
	public enum Mask : uint
	{
		A_Vector = 0x01, // Vector destination mask
		B_Vector = 0x02, // Vector destination mask
		C_Vector = 0x04, // Vector destination mask
		D_Vector = 0x08, // Vector destination mask
	}
	public enum ScalarOpcode : uint
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
	public enum VectorOpcode : uint
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

	// ALU instruction data
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ALUInstruction
	{
		public uint vector_dest;
		public uint vector_dest_rel;
		public uint abs_constants;
		public uint scalar_dest;
		public uint scalar_dest_rel;
		public uint export_data;
		public Mask vector_write_mask;
		public Mask scalar_write_mask;
		public uint vector_clamp;
		public uint scalar_clamp;
		public ScalarOpcode scalar_opc;  // instr_scalar_opc_t

		public uint src3_swiz;
		public uint src2_swiz;
		public uint src1_swiz;
		public uint src3_reg_negate;
		public uint src2_reg_negate;
		public uint src1_reg_negate;
		public uint pred_condition;
		public uint is_predicated;
		public uint address_absolute;
		public uint const_1_rel_abs;
		public uint const_0_rel_abs;

		public uint src3_reg;
		public uint src2_reg;
		public uint src1_reg;
		public VectorOpcode vector_opc;  // instr_vector_opc_t
		public uint src3_sel;
		public uint src2_sel;
		public uint src1_sel;

		// Whether data is being exported (or written to local registers).
		public bool is_export { get => export_data != 0; }

		// Whether the instruction is predicated (or conditional).
		public bool Is_predicated { get => is_predicated != 0; }
		// Required condition value of the comparision (true or false).
		public bool Pred_condition { get => pred_condition != 0; }

		public bool Abs_constants { get => abs_constants == 1; }
		public bool Is_const_0_addressed { get => const_0_rel_abs != 0; }
		public bool Is_const_1_addressed { get => const_1_rel_abs != 0; }
		public bool Is_address_relative { get => address_absolute != 0; }

		// Whether the instruction operates on the vector ALU
		public bool Has_vector_op { get => vector_write_mask != 0 || is_export; }
		public bool Is_vector_dest_relative { get => vector_dest_rel != 0; }
		public bool Vector_clamp { get => vector_clamp != 0; }

		// Whether the instruction operates on the scalar ALU
		public bool Has_scalar_op { get => scalar_opc != ScalarOpcode.retain_prev || (!is_export && scalar_write_mask != 0); }
		public bool Is_scalar_dest_relative { get => scalar_dest_rel != 0; }
		public bool Scalar_clamp { get => scalar_clamp != 0; }

		// Gets the string representation of the Vector portion of the ALU instruction.
		public string GetVectorAsmString()
		{
			string asmString = "";
			asmString += $"{vector_opc}";
			return asmString;
		}

		// Gets the string representation of the Scalar portion of the ALU instruction.
		public string GetScalarAsmString()
		{
			string asmString = "";
			asmString += $"{scalar_opc}";
			return asmString;
		}

		// gets the string representation of the full dest operand
		public string GetDest_Operand()
		{
			return "";
		}
		// gets the string representation of just the dest register
		public string GetDest_Register()
		{
			return "";
		}

		// gets the string representation of the full src0 operand
		public string GetSrc0_Operand()
		{
			return "";
		}
		// gets the string representation of just the src0 register
		public string GetSrc0_Register()
		{
			return "";
		}

		// gets the string representation of the full src1 operand
		public string GetSrc1_Operand()
		{
			return "";
		}
		// gets the string representation of just the src1 register
		public string GetSrc1_Register()
		{
			return "";
		}

		// gets the string representation of the full src2 operand
		public string GetSrc2_Operand()
		{
			return "";
		}
		// gets the string representation of just the src2 register
		public string GetSrc2_Register()
		{
			return "";
		}
	}
}
