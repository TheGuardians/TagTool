
uniform bool k_is_lightmap_exist : register(b0);
uniform bool k_is_water_interaction : register(b1);
uniform bool no_dynamic_lights : register(b2);

uniform float4 g_exposure : register(c0);
uniform float4 wave_displacement_array_xform : register(c11);
uniform float4 time_warp : register(c13);
uniform float4 simple_light_count : register(c17);
uniform float4 simple_lights[40] : register(c18);
uniform float4 wave_slope_array_xform : register(c58);
uniform float4 time_warp_aux : register(c59);
uniform float4 foam_texture_xform : register(c60);
uniform float4 foam_texture_detail_xform : register(c61);
uniform float4 foam_height : register(c62);
uniform float4 foam_pow : register(c63);
uniform float4 slope_range_x : register(c64);
uniform float4 slope_range_y : register(c65);
uniform float4 detail_slope_scale_x : register(c66);
uniform float4 detail_slope_scale_y : register(c67);
uniform float4 detail_slope_scale_z : register(c68);
uniform float4 detail_slope_steepness : register(c69);
uniform float4 refraction_texcoord_shift : register(c70);
uniform float4 refraction_extinct_distance : register(c71);
uniform float4 minimal_wave_disturbance : register(c72);
uniform float4 refraction_depth_dominant_ratio : register(c73);
uniform float4 reflection_coefficient : register(c74);
uniform float4 sunspot_cut : register(c75);
uniform float4 shadow_intensity_mark : register(c76);
uniform float4 fresnel_coefficient : register(c77);
uniform float4 water_color_pure : register(c78);
uniform float4 water_diffuse : register(c79);
uniform float4 water_murkiness : register(c80);
uniform float4 p_lightmap_compress_constant_0 : register(c210);
uniform float4 k_ps_water_view_xform_inverse[4] : register(c213);
uniform float4 k_ps_water_player_view_constant : register(c218);

uniform sampler tex_ripple_buffer_slope_height : register(s1);
uniform sampler lightprobe_texture_array : register(s2);
uniform sampler scene_ldr_texture : register(s3);
uniform sampler depth_buffer : register(s4);
uniform sampler wave_slope_array : register(s5);
uniform sampler watercolor_texture : register(s6);
uniform sampler global_shape_texture : register(s7);
uniform sampler environment_map : register(s8);
uniform sampler foam_texture : register(s9);
uniform sampler foam_texture_detail : register(s10);


struct VS_OUTPUT
{
    float4 TexCoord : TEXCOORD;
    float4 TexCoord1 : TEXCOORD1; // Unused?
    float4 TexCoord2 : TEXCOORD2;
    float4 TexCoord3 : TEXCOORD3;
    float4 TexCoord4 : TEXCOORD4;
    float4 TexCoord5 : TEXCOORD5;
    float4 TexCoord6 : TEXCOORD6;
    float4 TexCoord7 : TEXCOORD7; // Unused?
    float4 TexCoord8 : TEXCOORD8;
    float4 TexCoord9 : TEXCOORD9;
    float4 TexCoord10 : TEXCOORD10;
    float4 TexCoord11 : TEXCOORD11;
};

struct PS_OUTPUT
{
    float4 Diffuse;
    float4 Normal;
    float4 Unknown;
};

PS_OUTPUT main(VS_OUTPUT input) : COLOR
{
    //ps_3_0
    float4 c1 = float4(1, 0, -5, 6); //def c1, 1, 0, -0.5, 6
    float4 c2 = float4(0.300000012, 2.0999999, 2.00787401, -1.00787401); //def c2, 0.300000012, 2.0999999, 2.00787401, -1.00787401
    float4 c3 = float4(1, 0, 0.0625, 0.1875); //def c3, 1, 0, 0.0625, 0.1875
    float4 c4 = float4(2, -2, 3, 0.00100000005); //def c4, 2, -2, 3, 0.00100000005
    float4 c5 = float4(2, -1, 1.44269502, 0.00200000009); //def c5, 2, -1, 1.44269502, 0.00200000009
    float4 c6 = float4(9.99999975e-005, 0.0500000007, 20, 4); //def c6, 9.99999975e-005, 0.0500000007, 20, 4
    float4 c7 = float4(5, 7, 2.5, 0); //def c7, 5, 7, 2.5, 0

    float3 dcl_texcoord = input.TexCoord.xyz; //dcl_texcoord v0.xyw
    // Appears that 1 is unused
    float4 dcl_texcoord2 = input.TexCoord2; //dcl_texcoord2 v1
    float4 dcl_texcoord3 = input.TexCoord3; //dcl_texcoord3 v2
    float4 dcl_texcoord4 = input.TexCoord4; //dcl_texcoord4 v3
    float4 dcl_texcoord5 = input.TexCoord5; //dcl_texcoord5 v4
    float3 dcl_texcoord6 = input.TexCoord6.xyz; //dcl_texcoord6 v5.xyz
    // Appears that 7 is unused...
    float4 dcl_texcoord8 = input.TexCoord; //dcl_texcoord8 v6
    float4 dcl_texcoord9 = input.TexCoord; //dcl_texcoord9 v7
    float3 dcl_texcoord10 = input.TexCoord.xyz; //dcl_texcoord10 v8.xyz
    float3 dcl_texcoord11 = input.TexCoord.xyz; //dcl_texcoord11 v9.xyz

    float3 v0 = dcl_texcoord;
    float4 v1 = dcl_texcoord2;
    float4 v2 = dcl_texcoord3;
    float4 v3 = dcl_texcoord4;
    float4 v4 = dcl_texcoord5;
    float3 v5 = dcl_texcoord6;
    float4 v6 = dcl_texcoord8;
    float4 v7 = dcl_texcoord9;
    float3 v8 = dcl_texcoord10;
    float3 v9 = dcl_texcoord11;

    sampler2D s1 = tex_ripple_buffer_slope_height; //dcl_2d s1
    sampler3D s2 = lightprobe_texture_array; //dcl_volume s2
    sampler2D s3 = scene_ldr_texture; //dcl_2d s3
    sampler2D s4 = depth_buffer; //dcl_2d s4
    sampler3D s5 = wave_slope_array; //dcl_volume s5
    sampler2D s6 = watercolor_texture; //dcl_2d s6
    sampler2D s7 = global_shape_texture; //dcl_2d s7
    sampler3D s8 = environment_map; //dcl_cube s8
    sampler2D s9 = foam_texture; //dcl_2d s9
    sampler2D s10 = foam_texture_detail; //dcl_2d s10

    float4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11, r12, r13, r14, r15, r16;

    if (k_is_water_interaction) //if b1
    {
        r0 = c1.xxyy * v7.zwzz; //  mul r0, c1.xxyy, v7.zwzz
        r0 = tex2Dlod(tex_ripple_buffer_slope_height, r0); //  texldl r0, r0, s1
        r0.xy = (r0.yzzw * c1.z).xy; //  add r0.xy, r0.yzzw, c1.z
        r0.xy = (r0 * c1.w).xy; //  mul r0.xy, r0, c1.w
        r0.xyz = (r0.xyww).xyz; //  mov r0.xyz, r0.xyww
    } 
    else
    {
        r0.xyz = float3(c1.y); //  mov r0.xyz, c1.y
    }


    r0.w = r0.y + r0.x; //add r0.w, r0.y, r0.x
    r0.w = r0.w + c1.x; //add r0.w, r0.w, c1.x
    r1.x = max(c2.x, r0.w); //max r1.x, c2.x, r0.w
    r0.w = min(r1.x, c2.y); //min r0.w, r1.x, c2.y
    r1.x = slope_range_x.x; //mov r1.x, c64.x
    r1.y = slope_range_y.x; //mov r1.y, c65.x
    r2.xy = wave_displacement_array_xform.xy; //mov r2.xy, c11
    r3.x = r2.x * detail_slope_scale_x.x; //mul r3.x, r2.x, c66.x
    r3.y = r2.y * detail_slope_scale_y.x; //mul r3.y, r2.y, c67.x
    r1.zw = wave_displacement_array_xform.xy; //mov r1.zw, c11
    r2.xy = v0.xy * r3.xy + r1.zw; //mad r2.xy, v0, r3, r1.zwzw
    r3.x = time_warp.x; //mov r3.x, c13.x
    r2.z = r3.x * detail_slope_scale_z.x; //mul r2.z, r3.x, c68.x
    r2 = tex3D(wave_slope_array, r2.xyz); //texld r2, r2, s5
    r1.zw = r2.xy * c2.z + c2.w; //mad r1.zw, r2.xyxy, c2.z, c2.w
    r2.xy = r1.xy * c1.z; //mul r2.xy, r1, c1.z
    r1.zw = r1.xy * r1.xy + r2.xy; //mad r1.zw, r1, r1.xyxy, r2.xyxy
    r2.z = 1.0 / r0.w; //rcp r2.z, r0.w
    r2.w = r2.z * detail_slope_steepness.x; //mul r2.w, r2.z, c69.x
    r1.zw = r1.xy * r2.w; //mul r1.zw, r1, r2.w
    //mul r2.w, r2.z, v2.w
    //mov r3.z, c59.x
    //mad r3.xy, v0, c58, c58.zwzw
    //texld r3, r3, s5
    //mad r3.xy, r3, c2.z, c2.w
    //mad r3.xy, r3, r1, r2
    //mad r1.zw, r3.xyxy, r2.w, r1
    //mul r2.z, r2.z, v1.w
    //max r3.x, r2.z, c72.x
    //mov r4.z, c13.x
    //mad r4.xy, v0, c11, c11.zwzw
    //texld r4, r4, s5
    //mad r3.yz, r4.xxyw, c2.z, c2.w
    //mad r1.xy, r3.yzzw, r1, r2
    //mad r2.xy, r1, r3.x, r1.zwzw
    //add r2.xy, r0, r2
    //mul r2.xy, r2, v4.yxzw
    //mul r2.xy, r2, c70.x
    //add r2.w, -c1.x, v0.w
    //rcp r3.x, v0.w
    //cmp r2.w, r2.w, r3.x, c1.x
    //mad r1.xy, r1, r2.z, r1.zwzw
    //mad r1.xy, r1, r2.w, r0
    //mov r1.z, c1.x
    //nrm r3.xyz, r1
    //mul r1.xyz, r3.y, v2
    //mad r1.xyz, r3.x, v1, r1
    //mov r4.xyz, v2
    //mul r3.xyw, r4.zxzy, v1.yzzx
    //mad r3.xyw, r4.yzzx, v1.zxzy, -r3
    //nrm r4.xyz, r3.xyww
    //mad r1.xyz, r3.z, -r4, r1
    //rcp r0.x, v3.w
    //mul r2.zw, r0.x, v3.xyxy
    //mad r3.xy, r2.zwzw, -c1.z, -c1.z
    //add r3.z, -r3.y, c1.x
    //mad r3.xy, r3.xzzw, c218.zwzw, c218
    //texld r4, r3, s4
    //mov r4.z, r4.x
    //mad r4.xyw, r2.zwzz, c1.xxzy, c1.yyzx
    //dp4 r0.y, r4, c216
    //rcp r0.y, r0.y
    //dp4 r5.x, r4, c213
    //dp4 r5.y, r4, c214
    //dp4 r5.z, r4, c215
    //mad r4.xyz, r5, r0.y, -v5
    //dp3 r0.y, r4, r4
    //rsq r0.y, r0.y
    //rcp r0.y, r0.y
    //mul_sat r0.y, r0.y, c4.z
    //mul r2.xy, r0.y, r2
    //rcp r0.y, v4.w
    //add_sat r0.y, r0.y, r0.y
    //mul r2.xy, r0.y, r2
    //mul r2.xy, r2, c218.zwzw
    //mad r2.xy, r2, r0.w, r3
    //mov r4.xzw, c4
    //add r2.zw, r4.w, c218.xyxy
    //max r3.zw, r2.xyxy, r2
    //add r2.xy, c218.zwzw, c218
    //add r2.xy, r2, -c4.w
    //min r4.yw, r2.xxzy, r3.xzzw
    //texld r2, r4.ywzw, s4
    //mad r0.x, v3.z, r0.x, -r2.x
    //cmp r2.xy, r0.x, r3, r4.ywzw
    //add r2.z, -r2.y, c1.x
    //mad r3.xy, r2.xzzw, c5.x, c5.y
    //texld r5, r2, s4
    //mad r3.zw, r5.x, c1.xyxy, c1.xyyx
    //dp4 r0.x, r3, c216
    //nrm r5.xyz, r1
    //if b0
    //  mad r1.xyz, v7.xyxw, c3.xxyw, c3.yyzw
    //  texld r1, r1, s2
    //  mad r6.xyz, v7.xyxw, c3.xxyw, c3.yyww
    //  texld r6, r6, s2
    //  add r1.xyz, r1, r6
    //  mul r0.y, r1.w, r6.w
    //  mul r0.y, r0.y, c210.x
    //  mad r1.xyz, r1, c4.x, c4.y
    //  mul_sat r1.xyz, r0.y, r1
    //else
    //  mov r1.xyz, c1.x
    //endif
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
    //texld r3, v6, s6
    //add r2.zw, -c62.x, v6.xywz
    //mov_sat r2.z, r2.z
    //rcp r0.y, r2.z
    //mul_sat r0.y, r0.y, r2.w
    //pow r1.w, r0.y, c63.x
    //texld r6, v6, s7
    //max r0.y, r1.w, r6.z
    //max r6.w, r0.z, r0.y
    //mul r3.xyz, r1, c78
    //texld r2, r2, s3
    //mad r7.xyz, r3, -r0.w, r2
    //mul r0.yzw, r0.w, r3.xxyz
    //mad r0.yzw, r0.x, r7.xxyz, r0
    //add r1.w, -r6.w, c5.w
    //mad r3.xy, v0, c60, c60.zwzw
    //texld r7, r3, s9
    //mad r3.xy, v0, c61, c61.zwzw
    //texld r8, r3, s10
    //mul r7, r7, r8
    //mul r7.w, r6.w, r7.w
    //mov r6.xyz, c1.y
    //cmp r6, r1.w, r6, r7
    //dp3 r1.w, -v4, r5
    //add r1.w, r1.w, r1.w
    //mov_sat r2.w, r5.z
    //add_sat r3.xyz, r1, -c76.x
    //dp3 r3.x, r3, r3
    //mad r7.xyz, r5, -r1.w, -v4
    //abs r7.w, r7.z
    //texld r7, r7.xyww, s8
    //min r1.w, c75.x, r7.w
    //add r3.y, r7.w, -c75.x
    //mov_sat r3.y, r3.y
    //mad r1.w, r3.y, r3.x, r1.w
    //mul r3.xyz, r1.w, r7
    //mul r3.xyz, r3, c74.x
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
    //        max r7.w, r2.w, c1.y
    //        pow r2.w, r7.w, c6.z
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
    //              max r7.w, r2.w, c1.y
    //              pow r2.w, r7.w, c6.z
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
    //                max r7.w, r2.w, c1.y
    //                pow r2.w, r7.w, c6.z
    //                add r4.x, -r4.x, c7.y
    //                add r8.xyz, c53, -v5
    //                dp3 r7.w, r8, r8
    //                rsq r8.w, r7.w
    //                mul r8.xyz, r8.w, r8
    //                dp3 r8.w, r8, -v4
    //                add r9.w, r7.w, c53.w
    //                rcp r10.x, r9.w
    //                dp3 r10.y, r8, c54
    //                mad r10.xy, r10, c56, c56.zwzw
    //                max r12.xy, c6.x, r10
    //                pow r9.w, r12.y, c55.w
    //                add_sat r9.w, r9.w, c54.w
    //                mov_sat r12.x, r12.x
    //                mul r9.w, r9.w, r12.x
    //                max r10.x, r8.w, c1.y
    //                pow r8.w, r10.x, c6.z
    //                add r1.w, r1.w, -c52.x
    //                mul r10.xyz, r5.w, c50
    //                mad r12.xyz, r10, r2.w, r11
    //                cmp r12.xyz, r1.w, r11, r12
    //                add r2.w, r7.w, -c57.x
    //                mul r13.xyz, r9.w, c55
    //                mad r14.xyz, r13, r8.w, r12
    //                cmp r14.xyz, r2.w, r12, r14
    //                cmp r11.xyz, r4.x, r12, r14
    //                dp3 r4.y, r5, r4.yzww
    //                max r5.w, c6.y, r4.y
    //                dp3 r4.y, r5, r8
    //                max r7.w, c6.y, r4.y
    //                mad r4.yzw, r10.xxyz, r5.w, r9.xxyz
    //                cmp r4.yzw, r1.w, r9.xxyz, r4
    //                mad r8.xyz, r13, r7.w, r4.yzww
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
    //  mad r3.xyz, r11, c6.z, r3
    //endif
    //dp3_sat r1.w, v4, r5
    //add r1.w, -r1.w, c1.x
    //pow r2.w, r1.w, c7.z
    //mov r4.x, c1.x
    //lrp r1.w, r2.w, r4.x, c77.x
    //lrp r4.xyz, r1.w, r3, r0.yzww
    //add r0.yzw, r7.xxyz, r4.xxyz
    //mov_sat r2.w, r3.w
    //lrp r3.xyz, r2.w, r0.yzww, r2
    //mad r0.yzw, r6.xxyz, r1.xxyz, -r3.xxyz
    //mad r0.yzw, r6.w, r0, r3.xxyz
    //add r1.x, -r1.w, c1.x
    //mul r0.x, r0.x, r1.x
    //add r1.x, -r2.w, c1.x
    //mad r0.x, r0.x, r2.w, r1.x
    //add r1.x, -r6.w, c1.x
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
}