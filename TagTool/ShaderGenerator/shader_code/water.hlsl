#define water_template
#define _debug_color (float4(0, 0, 170, 255) / 255);
#include "parameters.hlsl"

bool k_is_lightmap_exist : register(b0);
bool k_is_water_interaction : register(b1);
bool no_dynamic_lights : register(b2);
float4 g_exposure : register(c0);
float4 wave_displacement_array_xform : register(c11);
float4 time_warp : register(c13);
float4 simple_light_count : register(c17);
float4 simple_lights : register(c18);
float4 wave_slope_array_xform : register(c58);
float4 time_warp_aux : register(c59);
float4 foam_texture_xform : register(c60);
float4 foam_texture_detail_xform : register(c61);
float4 foam_height : register(c62);
float4 foam_pow : register(c63);
float4 slope_range_x : register(c64);
float4 slope_range_y : register(c65);
float4 detail_slope_scale_x : register(c66);
float4 detail_slope_scale_y : register(c67);
float4 detail_slope_scale_z : register(c68);
float4 detail_slope_steepness : register(c69);
float4 refraction_texcoord_shift : register(c70);
float4 refraction_extinct_distance : register(c71);
float4 minimal_wave_disturbance : register(c72);
float4 refraction_depth_dominant_ratio : register(c73);
float4 reflection_coefficient : register(c74);
float4 sunspot_cut : register(c75);
float4 shadow_intensity_mark : register(c76);
float4 fresnel_coefficient : register(c77);
float4 watercolor_coefficient : register(c78);
float4 water_diffuse : register(c79);
float4 water_murkiness : register(c80);
float4 bankalpha_infuence_depth : register(c81);
float4 p_lightmap_compress_constant_0 : register(c210);
float4 k_ps_water_view_xform_inverse : register(c213);
float4 k_ps_water_view_depth_constant : register(c217);
float4 k_ps_water_player_view_constant : register(c218);
sampler tex_ripple_buffer_slope_height : register(s1);
sampler lightprobe_texture_array : register(s2);
sampler scene_ldr_texture : register(s3);
sampler depth_buffer : register(s4);
sampler wave_slope_array : register(s5);
sampler watercolor_texture : register(s6);
sampler global_shape_texture : register(s7);
sampler environment_map : register(s8);
sampler foam_texture : register(s9);
sampler foam_texture_detail : register(s10);

#include "lightmap_sampling.hlsl"
#include "texture_xform.hlsl"

//dcl_texcoord v0.xyw
//dcl_texcoord2 v1
//dcl_texcoord3 v2
//dcl_texcoord4 v3
//dcl_texcoord5 v4
//dcl_texcoord6 v5
//dcl_texcoord8 v6
//dcl_texcoord9 v7
//dcl_texcoord10 v8.xyz
//dcl_texcoord11 v9.xyz


struct VS_OUTPUT
{
	float4 TexCoord : TEXCOORD;
	float4 TexCoord1 : TEXCOORD1;
	float4 TexCoord2 : TEXCOORD2;
	float4 TexCoord3 : TEXCOORD3;
	float4 TexCoord4 : TEXCOORD4;
	float4 TexCoord5 : TEXCOORD5;
	float4 TexCoord6 : TEXCOORD6;
	float4 TexCoord7 : TEXCOORD7;
	float4 TexCoord8 : TEXCOORD8;
	float4 TexCoord9 : TEXCOORD9;
	float4 TexCoord10 : TEXCOORD10;
	float4 TexCoord11 : TEXCOORD11;
};

struct PS_OUTPUT
{
	float4 LDR;
	float4 HDR;
	//float4 Unknown;
};

PS_OUTPUT main(VS_OUTPUT input) : COLOR
{
// Registers:
//
//   Name                            Reg   Size
//   ------------------------------- ----- ----
//   k_is_lightmap_exist             b0       1
//   k_is_water_interaction          b1       1
//   no_dynamic_lights               b2       1
//   g_exposure                      c0       1
//   wave_displacement_array_xform   c11      1
//   time_warp                       c13      1
//   simple_light_count              c17      1
//   simple_lights                   c18     40
//   wave_slope_array_xform          c58      1
//   time_warp_aux                   c59      1
//   foam_texture_xform              c60      1
//   foam_texture_detail_xform       c61      1
//   foam_height                     c62      1
//   foam_pow                        c63      1
//   slope_range_x                   c64      1
//   slope_range_y                   c65      1
//   detail_slope_scale_x            c66      1
//   detail_slope_scale_y            c67      1
//   detail_slope_scale_z            c68      1
//   detail_slope_steepness          c69      1
//   refraction_texcoord_shift       c70      1
//   refraction_extinct_distance     c71      1
//   minimal_wave_disturbance        c72      1
//   refraction_depth_dominant_ratio c73      1
//   reflection_coefficient          c74      1
//   sunspot_cut                     c75      1
//   shadow_intensity_mark           c76      1
//   fresnel_coefficient             c77      1
//   watercolor_coefficient          c78      1
//   water_diffuse                   c79      1
//   water_murkiness                 c80      1
//   bankalpha_infuence_depth        c81      1
//   p_lightmap_compress_constant_0  c210     1
//   k_ps_water_view_xform_inverse   c213     4
//   k_ps_water_view_depth_constant  c217     1
//   k_ps_water_player_view_constant c218     1
//   tex_ripple_buffer_slope_height  s1       1
//   lightprobe_texture_array        s2       1
//   scene_ldr_texture               s3       1
//   depth_buffer                    s4       1
//   wave_slope_array                s5       1
//   watercolor_texture              s6       1
//   global_shape_texture            s7       1
//   environment_map                 s8       1
//   foam_texture                    s9       1
//   foam_texture_detail             s10      1
//
	bool b0 = k_is_lightmap_exist;
	bool b1 = k_is_water_interaction;
	bool b2 = no_dynamic_lights;
	float4 c0 = g_exposure;
	float4 c11 = wave_displacement_array_xform;
	float4 c13 = time_warp;
	float4 c17 = simple_light_count;
	float4 c18 = simple_lights;
	float4 c58 = wave_slope_array_xform;
	float4 c59 = time_warp_aux;
	float4 c60 = foam_texture_xform;
	float4 c61 = foam_texture_detail_xform;
	float4 c62 = foam_height;
	float4 c63 = foam_pow;
	float4 c64 = slope_range_x;
	float4 c65 = slope_range_y;
	float4 c66 = detail_slope_scale_x;
	float4 c67 = detail_slope_scale_y;
	float4 c68 = detail_slope_scale_z;
	float4 c69 = detail_slope_steepness;
	float4 c70 = refraction_texcoord_shift;
	float4 c71 = refraction_extinct_distance;
	float4 c72 = minimal_wave_disturbance;
	float4 c73 = refraction_depth_dominant_ratio;
	float4 c74 = reflection_coefficient;
	float4 c75 = sunspot_cut;
	float4 c76 = shadow_intensity_mark;
	float4 c77 = fresnel_coefficient;
	float4 c78 = watercolor_coefficient;
	float4 c79 = water_diffuse;
	float4 c80 = water_murkiness;
	float4 c81 = bankalpha_infuence_depth;
	float4 c210 = p_lightmap_compress_constant_0;
	float4 c213 = k_ps_water_view_xform_inverse[0];
	float4 c214 = k_ps_water_view_xform_inverse[1];
	float4 c215 = k_ps_water_view_xform_inverse[2];
	float4 c216 = k_ps_water_view_xform_inverse[3];
	float4 c217 = k_ps_water_view_depth_constant;
	float4 c218 = k_ps_water_player_view_constant;

	sampler2D s1 = tex_ripple_buffer_slope_height;
	sampler3D s2 = lightprobe_texture_array;
	sampler2D s3 = scene_ldr_texture;
	sampler2D s4 = depth_buffer;
	sampler3D s5 = wave_slope_array;
	sampler2D s6 = watercolor_texture;
	sampler2D s7 = global_shape_texture;
	samplerCUBE s8 = environment_map;
	sampler2D s9 = foam_texture;
	sampler2D s10 = foam_texture_detail;

	//ps_3_0
	float4 c1 = float4(1, 0, -0.5, 6); //def c1, 1, 0, -0.5, 6
	float4 c2 = float4(0.3, 2.1, 2.00787401, -1.00787401); //def c2, 0.3, 2.1, 2.00787401, -1.00787401
	float4 c3 = float4(1, 0, 0.0625, 0.1875); //def c3, 1, 0, 0.0625, 0.1875
	float4 c4 = float4(2, -2, 3, 0.001); //def c4, 2, -2, 3, 0.001
	float4 c5 = float4(2, -1, 1.44269502, 0.002); //def c5, 2, -1, 1.44269502, 0.002
	float4 c6 = float4(0.0001, 0.05, 20, 4); //def c6, 0.0001, 0.05, 20, 4
	float4 c7 = float4(5, 7, 2.5, 0); //def c7, 5, 7, 2.5, 0
    //dcl_texcoord v0.xyw
    //dcl_texcoord2 v1
    //dcl_texcoord3 v2
    //dcl_texcoord4 v3
    //dcl_texcoord5 v4
    //dcl_texcoord6 v5
    //dcl_texcoord8 v6
    //dcl_texcoord9 v7
    //dcl_texcoord10 v8.xyz
    //dcl_texcoord11 v9.xyz

	float4 v0 = input.TexCoord;
	// UNUSED   input.TexCoord1;
	float4 v1 = input.TexCoord2;
	float4 v2 = input.TexCoord3;
	float4 v3 = input.TexCoord4;
	float4 v4 = input.TexCoord5;
	float4 v5 = input.TexCoord6;
	// UNUSED   input.TexCoord7;
	float4 v6 = input.TexCoord8;
	float4 v7 = input.TexCoord9;
	float3 v8 = input.TexCoord.xyz;
	float3 v9 = input.TexCoord.xyz;

	float4 oC0 = float4(0, 0, 0, 0);
	float4 oC1 = float4(0, 0, 0, 0);
	float4 oc2 = float4(0, 0, 0, 0);

	float4 r0 = float4(0, 0, 0, 0);
	float4 r1 = float4(0, 0, 0, 0);
	float4 r2 = float4(0, 0, 0, 0);
	float4 r3 = float4(0, 0, 0, 0);
	float4 r4 = float4(0, 0, 0, 0);
	float4 r5 = float4(0, 0, 0, 0);
	float4 r6 = float4(0, 0, 0, 0);
	float4 r7 = float4(0, 0, 0, 0);
	float4 r8 = float4(0, 0, 0, 0);
	float4 r9 = float4(0, 0, 0, 0);
	float4 r10 = float4(0, 0, 0, 0);
	float4 r11 = float4(0, 0, 0, 0);
	float4 r12 = float4(0, 0, 0, 0);
	float4 r13 = float4(0, 0, 0, 0);
	float4 r14 = float4(0, 0, 0, 0);

    //dcl_2d s1
    //dcl_volume s2
    //dcl_2d s3
    //dcl_2d s4
    //dcl_volume s5
    //dcl_2d s6
    //dcl_2d s7
    //dcl_cube s8
    //dcl_2d s9
    //dcl_2d s10

	//NOTE: This is water interaction below
	//if b1
	//  mul r0, c1.xxyy, v7.zwzz
	//  texldl r0, r0, s1
	//  add r0.xy, r0.yzzw, c1.z
	//  mul r0.xy, r0, c1.w
	//  mov r0.xyz, r0.xyww
	//else
	//  mov r0.xyz, c1.y
	//endif

	float3 water_interaction = float3(0, 0, 0);
	if (k_is_water_interaction)
	{
		float4 water_interaction_texcoord = float4(v7.zw, 0, 0);
		float4 water_interaction_sample = tex2Dlod(s1, water_interaction_texcoord);

		water_interaction.xy = water_interaction_sample.yz + c1.z;
		water_interaction.xy = water_interaction.xy * c1.w;
		water_interaction.z = water_interaction_sample.w;
	}
	r0.xyz = water_interaction;

	r0.w = r0.y + r0.x; //add r0.w, r0.y, r0.x
	r0.w = r0.w + c1.x;	//add r0.w, r0.w, c1.x
	r1.x = max(r0.w, c2.x);  //max r1.x, c2.x, r0.w
	r0.w = min(r1.x, c2.y);	 //min r0.w, r1.x, c2.y

	{
		//NOTE: I think this section handles the wave displacement for the slope of the water surface

		//mov r1.x, c64.x
		//mov r1.y, c65.x
		r1.xy = float2(slope_range_x.x, slope_range_y.x);
		r2.xy = wave_displacement_array_xform.xy; //mov r2.xy, c11
											   //mul r3.x, r2.x, c66.x
											   //mul r3.y, r2.y, c67.x
		r3.xy = r2.xy * float2(detail_slope_scale_x.x, detail_slope_scale_y.y);
		r1.zw = wave_displacement_array_xform.xy; //mov r1.zw, c11
		r2.xy = v0.xy * r3.xy * r1.zw; //mad r2.xy, v0, r3, r1.zwzw
		r3.x = time_warp.x; //mov r3.x, c13.x
		r2.z = r3.x * detail_slope_scale_z.x; //mul r2.z, r3.x, c68.x
		r2 = tex3D(s5, r2.xyz); //texld r2, r2, s5

	}

	{
		//NOTE: I think this section handles the wave displacement for the slope steepness

		r1.zw = r2.xy * c2.z + c2.w; //mad r1.zw, r2.xyxy, c2.z, c2.w
		r2.xy = r1.xy * c1.x; //mul r2.xy, r1, c1.z
		r1.zw = (r1 * r1.xyxy + r2.xyxy).xy; //mad r1.zw, r1, r1.xyxy, r2.xyxy
		r1.z = 1.0 / r0.w; //rcp r2.z, r0.w
		r2.w = r2.z * detail_slope_steepness.x; //mul r2.w, r2.z, c69.x
		r1.zw = (r1 * r2.w).xy; //mul r1.zw, r1, r2.w
		r2.w = r2.z * v2.w; //mul r2.w, r2.z, v2.w
		r2.z = time_warp_aux.x; //mov r3.z, c59.x
		r3.xy = apply_xform(v0.xy, wave_slope_array_xform); //mad r3.xy, v0, c58, c58.zwzw
		r3 = tex3D(s5, r3.xyz); //texld r3, r3, s5
	}

	{
		r3.xy = r3.xy * c2.z + c2.w; //mad r3.xy, r3, c2.z, c2.w
		r3.xy = r3.xy * r1.xy + r2.xy; //mad r3.xy, r3, r1, r2
		r1.zw = r3.xy * r2.ww + r1.xy; //mad r1.zw, r3.xyxy, r2.w, r1
		r2.z = r2.x * v1.w; //mul r2.z, r2.z, v1.w
		r3.x = max(r2.z, minimal_wave_disturbance.x); //max r3.x, r2.z, c72.x
		r4.z = time_warp.x; //mov r4.z, c13.x
		r4.xy = v0.xy * wave_displacement_array_xform.xy + wave_displacement_array_xform.zw; //mad r4.xy, v0, c11, c11.zwzw
		r4 = tex3D(s5, r4.xyz); //texld r4, r4, s5
	}

	r3.yz = r4.xx * c2.z + c2.w; //mad r3.yz, r4.xxyw, c2.z, c2.w
	r1.xy = r3.yz * r1.xy + r2.xy; //mad r1.xy, r3.yzzw, r1, r2
	r2.xy = r1.xy * r3.x + r1.zw; //mad r2.xy, r1, r3.x, r1.zwzw
	r2.xy = r0.xy + r2.xy; //add r2.xy, r0, r2
	r2.xy = r2.xy * v4.yx; //mul r2.xy, r2, v4.yxzw
	r2.xy = r2.xy * c70.x; //mul r2.xy, r2, c70.x

	oC0.xyz = float3(r1.xyz);

	r2.w = v0.w - c1.x; //add r2.w, -c1.x, v0.w
	r3.x = 1.0 / v0.w; //rcp r3.x, v0.w
	r2.w = r2.w >= 0 ? r3.x : c1.x; //cmp r2.w, r2.w, r3.x, c1.x
	r1.xy = (r1 * r2.z + r1.zwzw).xy; //mad r1.xy, r1, r2.z, r1.zwzw
	r1.xy = (r1 * r2.w + r0).xy; //mad r1.xy, r1, r2.w, r0
	r1.z = c1.x; //mov r1.z, c1.x
	r3.xyz = normalize(r1.xyz); //nrm r3.xyz, r1
	r1.xyz = v2.xyz * r3.y; //mul r1.xyz, r3.y, v2
	r1.xyz = (v1 * r3.x + r1).xyz; //mad r1.xyz, r3.x, v1, r1
	r4.xyz = v2.xyz; //mov r4.xyz, v2
	r3.xyw = (r4.zxzy * v1.yzzx).xyz; //mul r3.xyw, r4.zxzy, v1.yzzx
	r3.xyw = (r4.yzzx * v1.zxzy - r3).xyz; //mad r3.xyw, r4.yzzx, v1.zxzy, -r3
	r4.xyz = normalize((r3.xyww).xyz); //nrm r4.xyz, r3.xyww



	r1.xyz = r3.z, -r4, r1; //mad r1.xyz, r3.z, -r4, r1
	r0.x = 1.0 / v3.w; //rcp r0.x, v3.w
	r2.zw = (r0.x * v3.xyxy).xy; //mul r2.zw, r0.x, v3.xyxy
	r3.xy = (r2.zwzw * (-c1.z) - c1.z).xy; //mad r3.xy, r2.zwzw, -c1.z, -c1.z
	r3.z = c1.x - r3.y; //add r3.z, -r3.y, c1.x
	r3.xy = (r3.xzzw * k_ps_water_player_view_constant.zwzw + k_ps_water_player_view_constant).xy; //mad r3.xy, r3.xzzw, c218.zwzw, c218
	r4 = tex2D(s4, r3.xy); //texld r4, r3, s4

	
	// CLEAN
	r0.y = 1.0 / r4.x; //rcp r0.y, r4.x
	r4.z = c217.x * r0.y + c217.y; //mad r4.z, c217.x, r0.y, c217.y
	r4.xyw = (r2.zwzz * c1.xxzy + c1.yyzx).xyz; //mad r4.xyw, r2.zwzz, c1.xxzy, c1.yyzx
	r0.y = dot(r4, c216); //dp4 r0.y, r4, c216
	r0.y = 1.0 / r0.y; //rcp r0.y, r0.y
	r5.x = dot(r4, c213); //dp4 r5.x, r4, c213
	r5.y = dot(r4, c214); //dp4 r5.y, r4, c214
	r5.z = dot(r4, c215); //dp4 r5.z, r4, c215
	r4.xyz = r5 * r0.y - v5; //mad r4.xyz, r5, r0.y, -v5
	// END CLEAN

	r0.y = dot(r4.xyz, r4.xyz); //dp3 r0.y, r4, r4
    //rsq r0.y, r0.y
    //rcp r0.y, r0.y
	r0.y = sqrt(r0.y);
    //mul_sat r0.y, r0.y, c4.z
	r0.y = saturate(r0.y * c4.z);
	r2.xy = r2.xy * r0.y; //mul r2.xy, r0.y, r2
	r0.y = 1.0 / v4.w; //rcp r0.y, v4.w
	r0.y = saturate(r0.y + r0.y); //add_sat r0.y, r0.y, r0.y
	r2.xy = r2.xy * r0.y; //mul r2.xy, r0.y, r2
	r2.xy = r2.xy * k_ps_water_player_view_constant.zw; //mul r2.xy, r2, c218.zwzw
	r2.xy = (r2 * r0.w + r3).xy; //mad r2.xy, r2, r0.w, r3
	r4.xzw = c4.xyz; //mov r4.xzw, c4
	r2.zw = c218.xy + r4.w; //add r2.zw, r4.w, c218.xyxy
	r3.zw = max(r2.xyxy, r2).xy; //max r3.zw, r2.xyxy, r2
	r2.xy = (c218.zwzw + c218).xy; //add r2.xy, c218.zwzw, c218
	r2.xy = (r2 - c4.w).xy; //add r2.xy, r2, -c4.w
	r4.yw = min(r2.xxzy, r3.xzzw).xy;//min r4.yw, r2.xxzy, r3.xzzw
	r2 = tex2D(s4, r4.ywzw); //texld r2, r4.ywzw, s4

	

	r0.y = 1.0 / r2.x; //rcp r0.y, r2.x
	r0.y = r0.y * c217.x + c217.y; //mad r0.y, c217.x, r0.y, c217.y
	r0.x = v3.z * (-r0.x) + r0.y; //mad r0.x, v3.z, -r0.x, r0.y
	r2.xy = (r0.xxxx >= 0 ? r3 : r4.ywzw).xy; //cmp r2.xy, r0.x, r3, r4.ywzw
	r2.z = (-r2.y) + c1.x; //add r2.z, -r2.y, c1.x
	r3.xy = r2.xzzw * c5.x + c5.y; //mad r3.xy, r2.xzzw, c5.x, c5.y
	r5 = tex2D(s4, r2); //texld r5, r2, s4
	r0.x = 1.0 / r5.x; //rcp r0.x, r5.x
	r3.z = r0.x * c217.x + c217.y; //mad r3.z, c217.x, r0.x, c217.y
	r3.w = c1.x; //mov r3.w, c1.x
	r0.x = dot(r3, c216); //dp4 r0.x, r3, c216
	r5.xyz = normalize(r1.xyz); //nrm r5.xyz, r1

	//NOTE: This is global illumination below
	//if b0
	//	mad r1.xyz, v7.xyxw, c3.xxyw, c3.yyzw
	//	texld r1, r1, s2
	//	mad r6.xyz, v7.xyxw, c3.xxyw, c3.yyww
	//	texld r6, r6, s2
	//	add r1.xyz, r1, r6
	//	mul r0.y, r1.w, r6.w
	//	mul r0.y, r0.y, c210.x
	//	mad r1.xyz, r1, c4.x, c4.y
	//	mul_sat r1.xyz, r0.y, r1
	//else
	//	mov r1.xyz, c1.x
	//endif

	r1.xyz = sample_lightprobe_texture_array(lightprobe_texture_array, v7.xy);

	

    //rcp r0.x, r0.x
    //dp4 r6.x, r3, c213
    //dp4 r6.y, r3, c214
    //dp4 r6.z, r3, c215
    //mad r3, r6.zxyz, r0.x, -v5.zxyz
    //dp3 r0.x, r3.yzww, r3.yzww
    //rsq r0.x, r0.x
    //rcp r0.x, r0.x
    //lrp r1.w, c73.x, r3_abs.x, r0.x
    //mul r0.x, r1.w, c80.x
    //mul r0.x, r0.x, c5.z
    //exp r0.x, r0.x
    //rcp_sat r0.x, r0.x
    //add r0.x, -r0.x, c1.x
    //mad r0.x, r0.x, -r0.w, c1.x
    //rcp r0.y, c71.x
    //mad_sat r0.y, v4.w, -r0.y, c1.x
    //mul r0.y, r0.y, r0.x
    //cmp r0.x, r0.x, r0.y, c1.y
    //add r2.zw, -c62.x, v6.xywz
    //mov_sat r2.z, r2.z
    //rcp r0.y, r2.z
    //mul_sat r0.y, r0.y, r2.w
    //pow r1.w, r0.y, c63.x
    //texld r3, v6, s7
    //max r0.y, r1.w, r3.z
    //max r3.w, r0.z, r0.y
    //texld r6, v6, s6
    //mul r6.xyz, r6, c78.x
    //mul r6.xyz, r1, r6
    //texld r2, r2, s3
    //mad r7.xyz, r6, -r0.w, r2
    //mul r0.yzw, r0.w, r6.xxyz
    //mad r0.yzw, r0.x, r7.xxyz, r0
    //add r1.w, -r3.w, c5.w
    //mad r4.yw, v0.xxzy, c60.xxzy, c60.xzzw
    //texld r6, r4.ywzw, s9
    //mad r4.yw, v0.xxzy, c61.xxzy, c61.xzzw
    //texld r7, r4.ywzw, s10
    //mul r6, r6, r7
    //mul r6.w, r3.w, r6.w
    //mov r3.xyz, c1.y
    //cmp r3, r1.w, r3, r6
    //dp3 r1.w, -v4, r5
    //add r1.w, r1.w, r1.w
    //mov_sat r2.w, r5.z
    //add_sat r6.xyz, r1, -c76.x
    //dp3 r4.y, r6, r6
    //mad r6.xyz, r5, -r1.w, -v4
    //abs r6.w, r6.z
    //texld r6, r6.xyww, s8
    //min r1.w, c75.x, r6.w
    //add r4.w, r6.w, -c75.x
    //mov_sat r4.w, r4.w
    //mad r1.w, r4.w, r4.y, r1.w
    //mul r6.xyz, r1.w, r6
    //mul r6.xyz, r6, c74.x
    //mul r7.xyz, r2.w, c79
    //mul r7.xyz, r1, r7


    //if !b2
    //  mov r8.xyw, c1
    //  if_lt -c17.x, r8.y
    //    add r9.xyz, c18, -v5
    //    dp3 r1.w, r9, r9
    //    rsq r2.w, r1.w
    //    mul r9.xyz, r2.w, r9
    //    dp3 r2.w, r9, -v4
    //    add r4.y, r1.w, c18.w
    //    rcp r10.x, r4.y
    //    dp3 r10.y, r9, c19
    //    mad r4.yw, r10.xxzy, c21.xxzy, c21.xzzw
    //    max r8.yz, c6.x, r4.xyww
    //    pow r4.y, r8.z, c20.w
    //    add_sat r4.y, r4.y, c19.w
    //    mov_sat r8.y, r8.y
    //    mul r4.y, r4.y, r8.y
    //    max r4.w, r2.w, c1.y
    //    pow r2.w, r4.w, c6.z
    //    add r1.w, r1.w, -c22.x
    //    mul r10.xyz, r4.y, c20
    //    mul r11.xyz, r2.w, r10
    //    cmp r11.xyz, r1.w, c1.y, r11
    //    dp3 r2.w, r5, r9
    //    max r4.y, c6.y, r2.w
    //    mul r9.xyz, r4.y, r10
    //    cmp r9.xyz, r1.w, c1.y, r9
    //    if_lt r8.x, c17.x
    //      add r8.xyz, c23, -v5
    //      dp3 r1.w, r8, r8
    //      rsq r2.w, r1.w
    //      mul r8.xyz, r2.w, r8
    //      dp3 r2.w, r8, -v4
    //      add r4.y, r1.w, c23.w
    //      rcp r10.x, r4.y
    //      dp3 r10.y, r8, c24
    //      mad r4.yw, r10.xxzy, c26.xxzy, c26.xzzw
    //      max r10.xy, c6.x, r4.ywzw
    //      pow r4.y, r10.y, c25.w
    //      add_sat r4.y, r4.y, c24.w
    //      mov_sat r10.x, r10.x
    //      mul r4.y, r4.y, r10.x
    //      max r4.w, r2.w, c1.y
    //      pow r2.w, r4.w, c6.z
    //      add r1.w, r1.w, -c27.x
    //      mul r10.xyz, r4.y, c25
    //      mad r12.xyz, r10, r2.w, r11
    //      cmp r11.xyz, r1.w, r11, r12
    //      dp3 r2.w, r5, r8
    //      max r4.y, c6.y, r2.w
    //      mad r8.xyz, r10, r4.y, r9
    //      cmp r9.xyz, r1.w, r9, r8
    //      if_lt r4.x, c17.x
    //        add r4.xyw, c28.xyzz, -v5.xyzz
    //        dp3 r1.w, r4.xyww, r4.xyww
    //        rsq r2.w, r1.w
    //        mul r4.xyw, r2.w, r4
    //        dp3 r2.w, r4.xyww, -v4
    //        add r5.w, r1.w, c28.w
    //        rcp r8.x, r5.w
    //        dp3 r8.y, r4.xyww, c29
    //        mad r8.xy, r8, c31, c31.zwzw
    //        max r10.xy, c6.x, r8
    //        pow r5.w, r10.y, c30.w
    //        add_sat r5.w, r5.w, c29.w
    //        mov_sat r10.x, r10.x
    //        mul r5.w, r5.w, r10.x
    //        max r6.w, r2.w, c1.y
    //        pow r2.w, r6.w, c6.z
    //        add r1.w, r1.w, -c32.x
    //        mul r8.xyz, r5.w, c30
    //        mad r10.xyz, r8, r2.w, r11
    //        cmp r11.xyz, r1.w, r11, r10
    //        dp3 r2.w, r5, r4.xyww
    //        max r4.x, c6.y, r2.w
    //        mad r4.xyw, r8.xyzz, r4.x, r9.xyzz
    //        cmp r9.xyz, r1.w, r9, r4.xyww
    //        if_lt r4.z, c17.x
    //          add r4.xyz, c33, -v5
    //          dp3 r1.w, r4, r4
    //          rsq r2.w, r1.w
    //          mul r4.xyz, r2.w, r4
    //          dp3 r2.w, r4, -v4
    //          add r4.w, r1.w, c33.w
    //          rcp r8.x, r4.w
    //          dp3 r8.y, r4, c34
    //          mad r8.xy, r8, c36, c36.zwzw
    //          max r10.xy, c6.x, r8
    //          pow r4.w, r10.y, c35.w
    //          add_sat r4.w, r4.w, c34.w
    //          mov_sat r10.x, r10.x
    //          mul r4.w, r4.w, r10.x
    //          max r5.w, r2.w, c1.y
    //          pow r2.w, r5.w, c6.z
    //          add r1.w, r1.w, -c37.x
    //          mul r8.xyz, r4.w, c35
    //          mad r10.xyz, r8, r2.w, r11
    //          cmp r11.xyz, r1.w, r11, r10
    //          dp3 r2.w, r5, r4
    //          max r4.x, c6.y, r2.w
    //          mad r4.xyz, r8, r4.x, r9
    //          cmp r9.xyz, r1.w, r9, r4
    //          mov r1.w, c6.w
    //          if_lt r1.w, c17.x
    //            add r4.xyz, c38, -v5
    //            dp3 r1.w, r4, r4
    //            rsq r2.w, r1.w
    //            mul r4.xyz, r2.w, r4
    //            dp3 r2.w, r4, -v4
    //            add r4.w, r1.w, c38.w
    //            rcp r8.x, r4.w
    //            dp3 r8.y, r4, c39
    //            mad r8.xy, r8, c41, c41.zwzw
    //            max r10.xy, c6.x, r8
    //            pow r4.w, r10.y, c40.w
    //            add_sat r4.w, r4.w, c39.w
    //            mov_sat r10.x, r10.x
    //            mul r4.w, r4.w, r10.x
    //            max r5.w, r2.w, c1.y
    //            pow r2.w, r5.w, c6.z
    //            add r1.w, r1.w, -c42.x
    //            mul r8.xyz, r4.w, c40
    //            mad r10.xyz, r8, r2.w, r11
    //            cmp r11.xyz, r1.w, r11, r10
    //            dp3 r2.w, r5, r4
    //            max r4.x, c6.y, r2.w
    //            mad r4.xyz, r8, r4.x, r9
    //            cmp r9.xyz, r1.w, r9, r4
    //            mov r4.x, c17.x
    //            if_lt c7.x, r4.x
    //              add r4.yzw, c43.xxyz, -v5.xxyz
    //              dp3 r1.w, r4.yzww, r4.yzww
    //              rsq r2.w, r1.w
    //              mul r4.yzw, r2.w, r4
    //              dp3 r2.w, r4.yzww, -v4
    //              add r5.w, r1.w, c43.w
    //              rcp r8.x, r5.w
    //              dp3 r8.y, r4.yzww, c44
    //              mad r8.xy, r8, c46, c46.zwzw
    //              max r10.xy, c6.x, r8
    //              pow r5.w, r10.y, c45.w
    //              add_sat r5.w, r5.w, c44.w
    //              mov_sat r10.x, r10.x
    //              mul r5.w, r5.w, r10.x
    //              max r6.w, r2.w, c1.y
    //              pow r2.w, r6.w, c6.z
    //              add r1.w, r1.w, -c47.x
    //              mul r8.xyz, r5.w, c45
    //              mad r10.xyz, r8, r2.w, r11
    //              cmp r11.xyz, r1.w, r11, r10
    //              dp3 r2.w, r5, r4.yzww
    //              max r4.y, c6.y, r2.w
    //              mad r4.yzw, r8.xxyz, r4.y, r9.xxyz
    //              cmp r9.xyz, r1.w, r9, r4.yzww
    //              if_lt r8.w, c17.x
    //                add r4.yzw, c48.xxyz, -v5.xxyz
    //                dp3 r1.w, r4.yzww, r4.yzww
    //                rsq r2.w, r1.w
    //                mul r4.yzw, r2.w, r4
    //                dp3 r2.w, r4.yzww, -v4
    //                add r5.w, r1.w, c48.w
    //                rcp r8.x, r5.w
    //                dp3 r8.y, r4.yzww, c49
    //                mad r8.xy, r8, c51, c51.zwzw
    //                max r10.xy, c6.x, r8
    //                pow r5.w, r10.y, c50.w
    //                add_sat r5.w, r5.w, c49.w
    //                mov_sat r10.x, r10.x
    //                mul r5.w, r5.w, r10.x
    //                max r6.w, r2.w, c1.y
    //                pow r2.w, r6.w, c6.z
    //                add r4.x, -r4.x, c7.y
    //                add r8.xyz, c53, -v5
    //                dp3 r6.w, r8, r8
    //                rsq r7.w, r6.w
    //                mul r8.xyz, r7.w, r8
    //                dp3 r7.w, r8, -v4
    //                add r8.w, r6.w, c53.w
    //                rcp r10.x, r8.w
    //                dp3 r10.y, r8, c54
    //                mad r10.xy, r10, c56, c56.zwzw
    //                max r12.xy, c6.x, r10
    //                pow r8.w, r12.y, c55.w
    //                add_sat r8.w, r8.w, c54.w
    //                mov_sat r12.x, r12.x
    //                mul r8.w, r8.w, r12.x
    //                max r9.w, r7.w, c1.y
    //                pow r7.w, r9.w, c6.z
    //                add r1.w, r1.w, -c52.x
    //                mul r10.xyz, r5.w, c50
    //                mad r12.xyz, r10, r2.w, r11
    //                cmp r12.xyz, r1.w, r11, r12
    //                add r2.w, r6.w, -c57.x
    //                mul r13.xyz, r8.w, c55
    //                mad r14.xyz, r13, r7.w, r12
    //                cmp r14.xyz, r2.w, r12, r14
    //                cmp r11.xyz, r4.x, r12, r14
    //                dp3 r4.y, r5, r4.yzww
    //                max r5.w, c6.y, r4.y
    //                dp3 r4.y, r5, r8
    //                max r6.w, c6.y, r4.y
    //                mad r4.yzw, r10.xxyz, r5.w, r9.xxyz
    //                cmp r4.yzw, r1.w, r9.xxyz, r4
    //                mad r8.xyz, r13, r6.w, r4.yzww
    //                cmp r8.xyz, r2.w, r4.yzww, r8
    //                cmp r9.xyz, r4.x, r4.yzww, r8
    //              endif
    //            endif
    //          endif
    //        endif
    //      endif
    //    endif
    //  else
    //    mov r9.xyz, c1.y
    //    mov r11.xyz, c1.y
    //  endif
    //  mad r7.xyz, r9, c79, r7
    //  mad r6.xyz, r11, c6.z, r6
    //endif


    //dp3_sat r1.w, v4, r5
    //add r1.w, -r1.w, c1.x
    //pow r2.w, r1.w, c7.z
    //mov r4.x, c1.x
    //lrp r1.w, r2.w, r4.x, c77.x
    //lrp r4.xyz, r1.w, r6, r0.yzww
    //add r0.yzw, r7.xxyz, r4.xxyz
    //rcp r2.w, c81.x
    //mul_sat r2.w, r2.w, v5.w
    //lrp r4.xyz, r2.w, r0.yzww, r2
    //mad r0.yzw, r3.xxyz, r1.xxyz, -r4.xxyz
    //mad r0.yzw, r3.w, r0, r4.xxyz
    //add r1.x, -r1.w, c1.x
    //mul r0.x, r0.x, r1.x
    //add r1.x, -r2.w, c1.x
    //mad r0.x, r0.x, r2.w, r1.x
    //add r1.x, -r3.w, c1.x
    //mul r1.y, r0.x, r1.x
    //mad r0.yzw, r2.xxyz, -r1.y, r0
    //mad r0.x, r0.x, -r1.x, c1.x
    //mul r1.xzw, r0.x, v9.xyyz
    //mad r0.xyz, r0.yzww, v8, r1.xzww
    //mul r1.xyz, r1.y, r2
    //mad r0.xyz, r0, c0.x, r1
    //max r1.xyz, r0, c1.y
    //rcp r0.x, c0.y
    //mul oC1.xyz, r0.x, r1
    //mov oC0.xyz, r1
    //mov oC0.w, c0.w
    //mov oC1.w, c0.z
    //mov oC2, c1.y

	// approximately 490 instruction slots used (16 texture, 474 arithmetic)

	PS_OUTPUT output;
	output.LDR = oC0;
	output.HDR = oC1;

	oC0.w = c0.w;
	oC1.w = c0.z;


	//output.Unknown = 0.0;
	return output;

}

#ifdef entry_static_per_pixel_ps
PS_OUTPUT static_per_pixel_ps(VS_OUTPUT input) : COLOR
{
	return main(input);
}
#endif
#ifdef entry_static_per_vertex_ps
PS_OUTPUT static_per_vertex_ps(VS_OUTPUT input) : COLOR
{
	return main(input);
}
#endif
