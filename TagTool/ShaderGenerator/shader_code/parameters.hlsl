#ifndef __UNIFORMS
#define __UNIFORMS

//uniform float4 albedo_color;
//uniform float4 debug_tint;
////uniform sampler2D base_map;
////uniform sampler2D bump_map;
//uniform float4 bump_map_xform : register(c58);
//uniform float fade : register(c32);


uniform float4 albedo_color : register(c58);
uniform float4 base_map_xform : register(c59);
uniform float4 detail_map_xform : register(c60);
uniform float4 debug_tint : register(c61);
uniform float4 bump_map_xform : register(c62);
uniform float4 bump_detail_map_xform : register(c63);
uniform float4 bump_detail_coefficient : register(c64);
uniform sampler base_map : register(s0);
uniform sampler detail_map : register(s1);
uniform sampler bump_map : register(s2);
uniform sampler bump_detail_map : register(s3);








#endif