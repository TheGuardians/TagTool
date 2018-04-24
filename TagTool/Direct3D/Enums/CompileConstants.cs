using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Direct3D.Enums
{
	/// <summary>
	/// Docs: https://msdn.microsoft.com/en-us/library/windows/desktop/gg615083(v=vs.85).aspx
	/// </summary>
	[Flags]
	public enum CompileConstants
	{
		// Directs the compiler to insert debug file/line/type/symbol information into the output code.
		DEBUG = (1 << 0),

		// Directs the compiler not to validate the generated code against known capabilities and constraints. 
		// We recommend that you use this constant only with shaders that have been successfully compiled in the past. 
		// DirectX always validates shaders before it sets them to a device.
		SKIP_VALIDATION = (1 << 1),

		// Directs the compiler to skip optimization steps during code generation. 
		// We recommend that you set this constant for debug purposes only.
		SKIP_OPTIMIZATION = (1 << 2),

		// Directs the compiler to pack matrices in row-major order on input and output from the shader.
		PACK_MATRIX_ROW_MAJOR = (1 << 3),

		// Directs the compiler to pack matrices in column-major order on input and output from the shader. 
		// This type of packing is generally more efficient because a series of dot-products can then perform 
		// vector-matrix multiplication.
		PACK_MATRIX_COLUMN_MAJOR = (1 << 4),

		// Directs the compiler to perform all computations with partial precision. 
		// If you set this constant, the compiled code might run faster on some hardware.
		PARTIAL_PRECISION = (1 << 5),

		// Directs the compiler to compile a vertex shader for the next highest shader profile. 
		// This constant turns debugging on and optimizations off.
		FORCE_VS_SOFTWARE_NO_OPT = (1 << 6),

		// Directs the compiler to compile a pixel shader for the next highest shader profile.
		// This constant also turns debugging on and optimizations off.
		FORCE_PS_SOFTWARE_NO_OPT = (1 << 7),

		// Directs the compiler to disable Preshaders. 
		// If you set this constant, the compiler does not pull out static expression for evaluation.
		NO_PRESHADER = (1 << 8),

		// Directs the compiler to not use flow-control constructs where possible.
		AVOID_FLOW_CONTROL = (1 << 9),

		// Directs the compiler to use flow-control constructs where possible.
		PREFER_FLOW_CONTROL = (1 << 10),

		// Forces strict compile, which might not allow for legacy syntax.
		// By default, the compiler disables strictness on deprecated syntax.
		ENABLE_STRICTNESS = (1 << 11),

		// Directs the compiler to enable older shaders to compile to newer targets.
		ENABLE_BACKWARDS_COMPATIBILITY = (1 << 12),

		// Forces the IEEE strict compile.
		IEEE_STRICTNESS = (1 << 13),

		// Directs the compiler to use the second lowest optimization level.
		OPTIMIZATION_LEVEL1 = (0),

		// Directs the compiler to use the second highest optimization level.
		OPTIMIZATION_LEVEL2 = ((1 << 14) | (1 << 15)),

		// Directs the compiler to use the highest optimization level.
		// If you set this constant, the compiler produces the best possible code but might 
		// take significantly longer to do so. 
		// Set this constant for final builds of an application when performance is the most important factor.
		OPTIMIZATION_LEVEL3 = (1 << 15),

		// Directs the compiler to treat all warnings as errors when it compiles the shader code. 
		// We recommend that you use this constant for new shader code, so that you can resolve 
		// all warnings and lower the number of hard-to-find code defects.
		WARNINGS_ARE_ERRORS = (1 << 18),

		// Directs the compiler to assume that unordered access views (UAVs) and 
		// shader resource views (SRVs) may alias for cs_5_0.
		// *NOTE* This compiler constant is new starting with the D3dcompiler_47.dll.
		RESOURCES_MAY_ALIAS = (1 << 19),

		// Directs the compiler to enable unbounded descriptor tables.
		// *NOTE* This compiler constant is new starting with the D3dcompiler_47.dll.
		ENABLE_UNBOUNDED_DESCRIPTOR_TABLES = (1 << 20),

		// Directs the compiler to ensure all resources are bound.
		// *NOTE* This compiler constant is new starting with the D3dcompiler_47.dll.
		ALL_RESOURCES_BOUND = (1 << 21)
	}
}
