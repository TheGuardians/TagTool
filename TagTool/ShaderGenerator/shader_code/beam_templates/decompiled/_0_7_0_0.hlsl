uniform float4 g_exposure : register(c0);
uniform sampler2D base_map : register(s0);

struct VS_OUTPUT
{
	float4 Color : COLOR;
	float4 Color1 : COLOR1;
	float4 TexCoord : TEXCOORD;
};

struct PS_OUTPUT
{
	float4 Color : COLOR;
	float4 Color1 : COLOR1;
};

PS_OUTPUT main(VS_OUTPUT In) : COLOR
{
	float4 r0, r1, oC0, oC1;

	// Albedo.DiffuseOnly
	// Blend_Mode.Add_Src_Times_DstAlpha
	// BlackPoint.Off
	// Fog.Off
	r0 = tex2Dlod(base_map, In.TexCoord); // texld r0, v2, s0							   
	r0.w = r0.w * In.Color.w; // mul r0.w, r0.w, v0.w
	r1.xyz = In.Color.xyz; // mov r1.xyz, v0
	r0.xyz = ((r0 * r1) + In.Color1).xyz; // mad r0.xyz, r0, r1, v1
	oC0.w = r0.w * g_exposure.w; // mul oC0.w, r0.w, c0.w
	oC1.w = r0.w * g_exposure.z; // mul oC1.w, r0.w, c0.z
	r0.w = 1.0f / g_exposure.y; // rcp r0.w, c0.y
	oC1.xyz = (r0.wwww * r0.xyzw).xyz; // mul oC1.xyz, r0.w, r0
	oC0.xyz = r0.xyz; // mov oC0.xyz, r0

	PS_OUTPUT Out;
	Out.Color = oC0;
	Out.Color1 = oC1;
	return Out;
}