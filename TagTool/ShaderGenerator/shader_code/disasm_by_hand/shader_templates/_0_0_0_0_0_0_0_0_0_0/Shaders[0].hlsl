//   Name             Reg   Size
//   ---------------- ----- ----
//   albedo_color     c58      1
//   base_map_xform   c59      1
//   detail_map_xform c60      1
//   debug_tint       c61      1
//   base_map         s0       1
//   detail_map       s2       1
uniform float4 albedo_color : register(c58);
uniform sampler2D base_map : register(s0);
uniform float4 base_map_xform : register(c59);
uniform float4 debug_tint : register(c61);
uniform sampler2D detail_map : register(s2);
uniform float4 detail_map_xform : register(c60);

struct VS_OUTPUT
{
	float4 TexCoord0 : TEXCOORD0;
	float4 TexCoord1 : TEXCOORD1;
};

struct PS_OUTPUT
{
	float4 Color0 : COLOR0;
	float4 Color1 : COLOR1;
	float4 Color2 : COLOR2;
};

PS_OUTPUT main(VS_OUTPUT In) : COLOR
{
	float4 r0, r1, r2;
	float4 c0 = float4(4.59478998, 0.00313080009, 12.9200001, 0.416666657); // def c0, 4.59478998, 0.00313080009, 12.9200001, 0.416666657
	float4 c1 = float4(1.05499995, -0.0549999997, 0.5, 0); // def c1, 1.05499995, -0.0549999997, 0.5, 0
	
	float2 texcoord0 = In.TexCoord0.xy; // dcl_texcoord v0.xy
	float4 texcoord1 = In.TexCoord1; // dcl_texcoord1 v1
	// dcl_2d s0
	// dcl_2d s2
	r0.xy = texcoord0 * base_map_xform + base_map_xform.zwzw; // mad r0.xy, v0, c59, c59.zwzw
	r0 = tex2D(base_map, r0); // texld r0, r0, s0
	r1.xy = texcoord0 * detail_map_xform + detail_map_xform.zwzw; // mad r1.xy, v0, c60, c60.zwzw
	r1 = tex2D(detail_map, r1); // texld r1, r1, s2
	r0 = r0 * r1; // mul r0, r0, r1
	r0 = r0 * albedo_color; // mul r0, r0, c58
	r1.xyz = r0 * c0.x; // mul r1.xyz, r0, c0.x
	r2.x = c0.x; // mov r2.x, c0.x
	r0.xyz = r0 * -r2.x + debug_tint; // mad r0.xyz, r0, -r2.x, c61
	r0.xyz = debug_tint.w * r0 + r1; // mad r0.xyz, c61.w, r0, r1
	r1.x = log2(r0.x); // log r1.x, r0.x
	r1.y = log2(r0.y); // log r1.y, r0.y
	r1.z = log2(r0.z); // log r1.z, r0.z
	r1.xyz = r1 * c0.w; // mul r1.xyz, r1, c0.w
	r2.x = exp2(r1.x); // exp r2.x, r1.x
	r2.y = exp2(r1.y); // exp r2.y, r1.y
	r2.z = exp2(r1.z); // exp r2.z, r1.z
	r1.xyz = r2 * c1.x + c1.y; // mad r1.xyz, r2, c1.x, c1.y
	r2.xyz = -r0 + c0.y; // add r2.xyz, -r0, c0.y
	r0.xyz = r0 * c0.z; // mul r0.xyz, r0, c0.z

	PS_OUTPUT output;
	output.Color0 = r2 >= 0 ? r0 : r1; // cmp oC0.xyz, r2, r0, r1
	output.Color0.w = r0.w; // mov oC0.w, r0.w
	output.Color1.w = r0.w; // mov oC1.w, r0.w
	output.Color1.xyz = texcoord1 * c1.z + c1.z; // mad oC1.xyz, v1, c1.z, c1.z
	output.Color2 = texcoord1.w; // mov oC2, v1.w
	return output;
}