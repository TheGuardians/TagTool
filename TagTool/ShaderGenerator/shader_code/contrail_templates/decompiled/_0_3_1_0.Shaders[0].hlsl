uniform float4 g_exposure : register(c0);
uniform sampler2D base_map : register(s0);

struct VS_OUTPUT
{
	float4 Color0 : COLOR0;
	float4 Color1 : COLOR1;
	float4 TexCoord0 : TEXCOORD0;
};

struct PS_OUTPUT
{
	float4 Color0 : COLOR0;
	float4 Color1 : COLOR1;
	float4 Color2 : COLOR2;
};

PS_OUTPUT main(VS_OUTPUT In) : COLOR
{
	float4 r0, r1, r2, oC0, oC1, oC2;
	
	// ps_3_0
	// def c1, 1, 0.5, 0, 0
	
	// Albedo.DiffuseOnly
	// Blend_Mode.Alpha_Mlend
	// Black_Point.On
	// Fog.Off
	r0.x = lerp(0.5f, 1.0f, -In.Color1.w);	// lrp r0.x, c1.y, c1.x, -v1.w
	r0.x = 1.0f / r0.x;						// rcp r0.x, r0.x
	r1 = tex2Dlod(base_map, In.TexCoord0);	// texld r1, v2, s0
	r0.y = r1.w + -In.Color1.w;				// add r0.y, r1.w, -v1.w
	r0.x = saturate(r0.x * r0.y);			// mul_sat r0.x, r0.x, r0.y
	r0.y = 1.0f + In.Color1.w;				// add r0.y, c1.x, v1.w
	r0.z = r0.y * 0.5f;						// mul r0.z, r0.y, c1.y
	r0.y = saturate((r0.y * -0.5f) + r1.w);	// mad_sat r0.y, r0.y, -c1.y, r1.w
	r2.xyz = In.Color1.xyz;					// mov r2.xyz, v1
	r1.xyz = ((r1 * In.Color0) + r2).xyz;	// mad r1.xyz, r1, v0, r2
	r0.x = (r0.z * r0.x) + r0.y;			// mad r0.x, r0.z, r0.x, r0.y
	r0.x = r0.x * In.Color0.w;				// mul r0.x, r0.x, v0.w
	oC0.w = r0.x * g_exposure.w;			// mul oC0.w, r0.x, c0.w
	oC1.w = r0.x * g_exposure.z;			// mul oC1.w, r0.x, c0.z
	r0.x = 1.0f / g_exposure.y;				// rcp r0.x, c0.y
	oC1.xyz = (r0.x * r1).xyz;				// mul oC1.xyz, r0.x, r1
	oC0.xyz = r1.xyz;						// mov oC0.xyz, r1
	oC2 = 0.0f;								// mov oC2, c1.z
	
	PS_OUTPUT Out;
	Out.Color0 = oC0;
	Out.Color1 = oC1;
	Out.Color2 = oC2;
	return Out;
}