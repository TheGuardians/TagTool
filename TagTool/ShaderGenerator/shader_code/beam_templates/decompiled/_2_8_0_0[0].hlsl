// Registers:
//
//   Name         Reg   Size
//   ------------ ----- ----
//   g_exposure   c0       1
//   base_map     s0       1
//   palette      s1       1
//   alpha_map    s2       1

uniform float4 g_exposure : register(c0);
uniform sampler2D base_map : register(s0);
uniform sampler2D palette : register(s1);
uniform sampler2D alpha_map : register(s2);

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
	// ps_3_0
	float4 r0, r1, oC0, oC1, oC2;
	float4 c1 = float4(0, 0, 0, 0);// def c1, 0, 0, 0, 0
	
	// Albedo.Palettized_Plus_Alpha
	// Blend_Mode.Inv_Alpha_Blend
	// BlackPoint.Off
	// Fog.Off
	r0 = tex2Dlod(alpha_map, In.TexCoord0); // texld r0, v2, s2					   
	r0.x = r0.w * In.Color0.w; // mul r0.x, r0.w, v0.w
	oC0.w = r0.x * g_exposure.w; // mul oC0.w, r0.x, c0.w
	oC1.w = r0.x * g_exposure.z; // mul oC1.w, r0.x, c0.z
	r0 = tex2Dlod(base_map, In.TexCoord0); // texld r0, v2, s0
	r0.y = In.TexCoord0.z; // mov r0.y, v2.z
	r0 = tex2Dlod(palette, r0); // texld r0, r0, s1
	r1.xyz = In.Color0.xyz; // mov r1.xyz, v0
	r0.xyz = ((r0 * r1) + In.Color1).xyz;// mad r0.xyz, r0, r1, v1
	r0.w = 1.0f / g_exposure.y; // rcp r0.w, c0.y
	oC1.xyz = (r0.w * r0).xyz; // mul oC1.xyz, r0.w, r0
	oC0.xyz = r0.xyz; // mov oC0.xyz, r0
	oC2 = c1.xxxx; // mov oC2, c1.x
	
	PS_OUTPUT Out;
	Out.Color0 = oC0;
	Out.Color1 = oC1;
	Out.Color2 = oC2;
	return Out;
}