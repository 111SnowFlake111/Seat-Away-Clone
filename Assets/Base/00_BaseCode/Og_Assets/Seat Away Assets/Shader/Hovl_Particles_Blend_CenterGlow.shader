//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "Hovl/Particles/Blend_CenterGlow" {
Properties {
_MainTex ("MainTex", 2D) = "white" { }
_Noise ("Noise", 2D) = "white" { }
_Flow ("Flow", 2D) = "white" { }
_Mask ("Mask", 2D) = "white" { }
_SpeedMainTexUVNoiseZW ("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
_DistortionSpeedXYPowerZ ("Distortion Speed XY Power Z", Vector) = (0,0,0,0)
_Emission ("Emission", Float) = 2
_Color ("Color", Color) = (0.5,0.5,0.5,1)
_Opacity ("Opacity", Range(0, 3)) = 1
[Toggle] _Usecenterglow ("Use center glow?", Float) = 0
[MaterialToggle] _Usedepth ("Use depth?", Float) = 0
_Depthpower ("Depth power", Float) = 1
[Enum(Cull Off,0, Cull Front,1, Cull Back,2)] _CullMode ("Culling", Float) = 0
_texcoord ("", 2D) = "white" { }
}
SubShader {
 Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
 Pass {
  Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
  ColorMask RGB 0
  ZWrite Off
  Cull Off
  GpuProgramID 12051
Program "vp" {
SubProgram "gles3 hw_tier00 " {
"#ifdef VERTEX
#version 300 es

#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_COLOR0;
out highp vec4 vs_TEXCOORD0;
vec4 u_xlat0;
vec4 u_xlat1;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
    vs_COLOR0 = in_COLOR0;
    vs_TEXCOORD0 = in_TEXCOORD0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es

precision highp float;
precision highp int;
#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 _Time;
uniform 	vec4 _MainTex_ST;
uniform 	float _Usecenterglow;
uniform 	vec4 _SpeedMainTexUVNoiseZW;
uniform 	vec4 _DistortionSpeedXYPowerZ;
uniform 	vec4 _Flow_ST;
uniform 	vec4 _Mask_ST;
uniform 	vec4 _Noise_ST;
uniform 	vec4 _Color;
uniform 	float _Emission;
uniform 	float _Opacity;
UNITY_LOCATION(0) uniform mediump sampler2D _Mask;
UNITY_LOCATION(1) uniform mediump sampler2D _Flow;
UNITY_LOCATION(2) uniform mediump sampler2D _MainTex;
UNITY_LOCATION(3) uniform mediump sampler2D _Noise;
in mediump vec4 vs_COLOR0;
in highp vec4 vs_TEXCOORD0;
layout(location = 0) out mediump vec4 SV_Target0;
vec4 u_xlat0;
mediump vec4 u_xlat16_0;
vec4 u_xlat1;
mediump vec4 u_xlat16_1;
vec2 u_xlat2;
mediump vec3 u_xlat16_2;
vec2 u_xlat7;
mediump vec2 u_xlat16_7;
void main()
{
    u_xlat0.xy = vs_TEXCOORD0.xy * _Noise_ST.xy + _Noise_ST.zw;
    u_xlat0.xy = _Time.yy * _SpeedMainTexUVNoiseZW.zw + u_xlat0.xy;
    u_xlat16_0 = texture(_Noise, u_xlat0.xy);
    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    u_xlat1.xy = _Time.yy * _SpeedMainTexUVNoiseZW.xy + u_xlat1.xy;
    u_xlat7.xy = vs_TEXCOORD0.xy * _Flow_ST.xy + _Flow_ST.zw;
    u_xlat7.xy = _Time.yy * _DistortionSpeedXYPowerZ.xy + u_xlat7.xy;
    u_xlat16_7.xy = texture(_Flow, u_xlat7.xy).xy;
    u_xlat2.xy = vs_TEXCOORD0.xy * _Mask_ST.xy + _Mask_ST.zw;
    u_xlat16_2.xyz = texture(_Mask, u_xlat2.xy).xyz;
    u_xlat7.xy = u_xlat16_7.xy * u_xlat16_2.xy;
    u_xlat1.xy = (-u_xlat7.xy) * _DistortionSpeedXYPowerZ.zz + u_xlat1.xy;
    u_xlat16_1 = texture(_MainTex, u_xlat1.xy);
    u_xlat0 = u_xlat16_0 * u_xlat16_1;
    u_xlat0 = u_xlat0 * _Color;
    u_xlat0 = u_xlat0 * vs_COLOR0;
    u_xlat1.x = (-vs_TEXCOORD0.z) + 1.0;
    u_xlat1.xyz = (-u_xlat1.xxx) + u_xlat16_2.xyz;
    u_xlat1.xyz = u_xlat1.xyz * u_xlat16_2.xyz;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xyz = min(max(u_xlat1.xyz, 0.0), 1.0);
#else
    u_xlat1.xyz = clamp(u_xlat1.xyz, 0.0, 1.0);
#endif
    u_xlat1.xyz = u_xlat0.xyz * u_xlat1.xyz + (-u_xlat0.xyz);
    u_xlat0.xyz = vec3(_Usecenterglow) * u_xlat1.xyz + u_xlat0.xyz;
    u_xlat1 = u_xlat0 * vec4(_Emission, _Emission, _Emission, _Opacity);
    SV_Target0 = u_xlat1;
    return;
}

#endif
"
}
SubProgram "gles3 hw_tier01 " {
"#ifdef VERTEX
#version 300 es

#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_COLOR0;
out highp vec4 vs_TEXCOORD0;
vec4 u_xlat0;
vec4 u_xlat1;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
    vs_COLOR0 = in_COLOR0;
    vs_TEXCOORD0 = in_TEXCOORD0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es

precision highp float;
precision highp int;
#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 _Time;
uniform 	vec4 _MainTex_ST;
uniform 	float _Usecenterglow;
uniform 	vec4 _SpeedMainTexUVNoiseZW;
uniform 	vec4 _DistortionSpeedXYPowerZ;
uniform 	vec4 _Flow_ST;
uniform 	vec4 _Mask_ST;
uniform 	vec4 _Noise_ST;
uniform 	vec4 _Color;
uniform 	float _Emission;
uniform 	float _Opacity;
UNITY_LOCATION(0) uniform mediump sampler2D _Mask;
UNITY_LOCATION(1) uniform mediump sampler2D _Flow;
UNITY_LOCATION(2) uniform mediump sampler2D _MainTex;
UNITY_LOCATION(3) uniform mediump sampler2D _Noise;
in mediump vec4 vs_COLOR0;
in highp vec4 vs_TEXCOORD0;
layout(location = 0) out mediump vec4 SV_Target0;
vec4 u_xlat0;
mediump vec4 u_xlat16_0;
vec4 u_xlat1;
mediump vec4 u_xlat16_1;
vec2 u_xlat2;
mediump vec3 u_xlat16_2;
vec2 u_xlat7;
mediump vec2 u_xlat16_7;
void main()
{
    u_xlat0.xy = vs_TEXCOORD0.xy * _Noise_ST.xy + _Noise_ST.zw;
    u_xlat0.xy = _Time.yy * _SpeedMainTexUVNoiseZW.zw + u_xlat0.xy;
    u_xlat16_0 = texture(_Noise, u_xlat0.xy);
    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    u_xlat1.xy = _Time.yy * _SpeedMainTexUVNoiseZW.xy + u_xlat1.xy;
    u_xlat7.xy = vs_TEXCOORD0.xy * _Flow_ST.xy + _Flow_ST.zw;
    u_xlat7.xy = _Time.yy * _DistortionSpeedXYPowerZ.xy + u_xlat7.xy;
    u_xlat16_7.xy = texture(_Flow, u_xlat7.xy).xy;
    u_xlat2.xy = vs_TEXCOORD0.xy * _Mask_ST.xy + _Mask_ST.zw;
    u_xlat16_2.xyz = texture(_Mask, u_xlat2.xy).xyz;
    u_xlat7.xy = u_xlat16_7.xy * u_xlat16_2.xy;
    u_xlat1.xy = (-u_xlat7.xy) * _DistortionSpeedXYPowerZ.zz + u_xlat1.xy;
    u_xlat16_1 = texture(_MainTex, u_xlat1.xy);
    u_xlat0 = u_xlat16_0 * u_xlat16_1;
    u_xlat0 = u_xlat0 * _Color;
    u_xlat0 = u_xlat0 * vs_COLOR0;
    u_xlat1.x = (-vs_TEXCOORD0.z) + 1.0;
    u_xlat1.xyz = (-u_xlat1.xxx) + u_xlat16_2.xyz;
    u_xlat1.xyz = u_xlat1.xyz * u_xlat16_2.xyz;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xyz = min(max(u_xlat1.xyz, 0.0), 1.0);
#else
    u_xlat1.xyz = clamp(u_xlat1.xyz, 0.0, 1.0);
#endif
    u_xlat1.xyz = u_xlat0.xyz * u_xlat1.xyz + (-u_xlat0.xyz);
    u_xlat0.xyz = vec3(_Usecenterglow) * u_xlat1.xyz + u_xlat0.xyz;
    u_xlat1 = u_xlat0 * vec4(_Emission, _Emission, _Emission, _Opacity);
    SV_Target0 = u_xlat1;
    return;
}

#endif
"
}
SubProgram "gles3 hw_tier02 " {
"#ifdef VERTEX
#version 300 es

#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_COLOR0;
out highp vec4 vs_TEXCOORD0;
vec4 u_xlat0;
vec4 u_xlat1;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
    vs_COLOR0 = in_COLOR0;
    vs_TEXCOORD0 = in_TEXCOORD0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es

precision highp float;
precision highp int;
#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 _Time;
uniform 	vec4 _MainTex_ST;
uniform 	float _Usecenterglow;
uniform 	vec4 _SpeedMainTexUVNoiseZW;
uniform 	vec4 _DistortionSpeedXYPowerZ;
uniform 	vec4 _Flow_ST;
uniform 	vec4 _Mask_ST;
uniform 	vec4 _Noise_ST;
uniform 	vec4 _Color;
uniform 	float _Emission;
uniform 	float _Opacity;
UNITY_LOCATION(0) uniform mediump sampler2D _Mask;
UNITY_LOCATION(1) uniform mediump sampler2D _Flow;
UNITY_LOCATION(2) uniform mediump sampler2D _MainTex;
UNITY_LOCATION(3) uniform mediump sampler2D _Noise;
in mediump vec4 vs_COLOR0;
in highp vec4 vs_TEXCOORD0;
layout(location = 0) out mediump vec4 SV_Target0;
vec4 u_xlat0;
mediump vec4 u_xlat16_0;
vec4 u_xlat1;
mediump vec4 u_xlat16_1;
vec2 u_xlat2;
mediump vec3 u_xlat16_2;
vec2 u_xlat7;
mediump vec2 u_xlat16_7;
void main()
{
    u_xlat0.xy = vs_TEXCOORD0.xy * _Noise_ST.xy + _Noise_ST.zw;
    u_xlat0.xy = _Time.yy * _SpeedMainTexUVNoiseZW.zw + u_xlat0.xy;
    u_xlat16_0 = texture(_Noise, u_xlat0.xy);
    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    u_xlat1.xy = _Time.yy * _SpeedMainTexUVNoiseZW.xy + u_xlat1.xy;
    u_xlat7.xy = vs_TEXCOORD0.xy * _Flow_ST.xy + _Flow_ST.zw;
    u_xlat7.xy = _Time.yy * _DistortionSpeedXYPowerZ.xy + u_xlat7.xy;
    u_xlat16_7.xy = texture(_Flow, u_xlat7.xy).xy;
    u_xlat2.xy = vs_TEXCOORD0.xy * _Mask_ST.xy + _Mask_ST.zw;
    u_xlat16_2.xyz = texture(_Mask, u_xlat2.xy).xyz;
    u_xlat7.xy = u_xlat16_7.xy * u_xlat16_2.xy;
    u_xlat1.xy = (-u_xlat7.xy) * _DistortionSpeedXYPowerZ.zz + u_xlat1.xy;
    u_xlat16_1 = texture(_MainTex, u_xlat1.xy);
    u_xlat0 = u_xlat16_0 * u_xlat16_1;
    u_xlat0 = u_xlat0 * _Color;
    u_xlat0 = u_xlat0 * vs_COLOR0;
    u_xlat1.x = (-vs_TEXCOORD0.z) + 1.0;
    u_xlat1.xyz = (-u_xlat1.xxx) + u_xlat16_2.xyz;
    u_xlat1.xyz = u_xlat1.xyz * u_xlat16_2.xyz;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xyz = min(max(u_xlat1.xyz, 0.0), 1.0);
#else
    u_xlat1.xyz = clamp(u_xlat1.xyz, 0.0, 1.0);
#endif
    u_xlat1.xyz = u_xlat0.xyz * u_xlat1.xyz + (-u_xlat0.xyz);
    u_xlat0.xyz = vec3(_Usecenterglow) * u_xlat1.xyz + u_xlat0.xyz;
    u_xlat1 = u_xlat0 * vec4(_Emission, _Emission, _Emission, _Opacity);
    SV_Target0 = u_xlat1;
    return;
}

#endif
"
}
SubProgram "gles3 hw_tier00 " {
Keywords { "SOFTPARTICLES_ON" }
"#ifdef VERTEX
#version 300 es

#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 _ProjectionParams;
uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
uniform 	vec4 hlslcc_mtx4x4unity_MatrixV[4];
uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_COLOR0;
out highp vec4 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD2;
vec4 u_xlat0;
vec4 u_xlat1;
float u_xlat2;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
    gl_Position = u_xlat1;
    vs_COLOR0 = in_COLOR0;
    vs_TEXCOORD0 = in_TEXCOORD0;
    u_xlat2 = u_xlat0.y * hlslcc_mtx4x4unity_MatrixV[1].z;
    u_xlat0.x = hlslcc_mtx4x4unity_MatrixV[0].z * u_xlat0.x + u_xlat2;
    u_xlat0.x = hlslcc_mtx4x4unity_MatrixV[2].z * u_xlat0.z + u_xlat0.x;
    u_xlat0.x = hlslcc_mtx4x4unity_MatrixV[3].z * u_xlat0.w + u_xlat0.x;
    vs_TEXCOORD2.z = (-u_xlat0.x);
    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
    u_xlat0.w = u_xlat0.x * 0.5;
    u_xlat0.xz = u_xlat1.xw * vec2(0.5, 0.5);
    vs_TEXCOORD2.w = u_xlat1.w;
    vs_TEXCOORD2.xy = u_xlat0.zz + u_xlat0.xw;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es

precision highp float;
precision highp int;
#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 _Time;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 _MainTex_ST;
uniform 	float _Usecenterglow;
uniform 	vec4 _SpeedMainTexUVNoiseZW;
uniform 	vec4 _DistortionSpeedXYPowerZ;
uniform 	vec4 _Flow_ST;
uniform 	vec4 _Mask_ST;
uniform 	vec4 _Noise_ST;
uniform 	vec4 _Color;
uniform 	float _Emission;
uniform 	float _Opacity;
uniform 	mediump float _Usedepth;
uniform 	float _Depthpower;
UNITY_LOCATION(0) uniform highp sampler2D _CameraDepthTexture;
UNITY_LOCATION(1) uniform mediump sampler2D _Mask;
UNITY_LOCATION(2) uniform mediump sampler2D _Flow;
UNITY_LOCATION(3) uniform mediump sampler2D _MainTex;
UNITY_LOCATION(4) uniform mediump sampler2D _Noise;
in mediump vec4 vs_COLOR0;
in highp vec4 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD2;
layout(location = 0) out mediump vec4 SV_Target0;
vec4 u_xlat0;
mediump vec4 u_xlat16_0;
vec4 u_xlat1;
mediump vec4 u_xlat16_1;
vec2 u_xlat2;
mediump vec3 u_xlat16_2;
vec2 u_xlat7;
mediump vec2 u_xlat16_7;
void main()
{
    u_xlat0.xy = vs_TEXCOORD0.xy * _Noise_ST.xy + _Noise_ST.zw;
    u_xlat0.xy = _Time.yy * _SpeedMainTexUVNoiseZW.zw + u_xlat0.xy;
    u_xlat16_0 = texture(_Noise, u_xlat0.xy);
    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    u_xlat1.xy = _Time.yy * _SpeedMainTexUVNoiseZW.xy + u_xlat1.xy;
    u_xlat7.xy = vs_TEXCOORD0.xy * _Flow_ST.xy + _Flow_ST.zw;
    u_xlat7.xy = _Time.yy * _DistortionSpeedXYPowerZ.xy + u_xlat7.xy;
    u_xlat16_7.xy = texture(_Flow, u_xlat7.xy).xy;
    u_xlat2.xy = vs_TEXCOORD0.xy * _Mask_ST.xy + _Mask_ST.zw;
    u_xlat16_2.xyz = texture(_Mask, u_xlat2.xy).xyz;
    u_xlat7.xy = u_xlat16_7.xy * u_xlat16_2.xy;
    u_xlat1.xy = (-u_xlat7.xy) * _DistortionSpeedXYPowerZ.zz + u_xlat1.xy;
    u_xlat16_1 = texture(_MainTex, u_xlat1.xy);
    u_xlat0 = u_xlat16_0 * u_xlat16_1;
    u_xlat0 = u_xlat0 * _Color;
    u_xlat0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
    u_xlat1.x = (-vs_TEXCOORD0.z) + 1.0;
    u_xlat1.xyz = (-u_xlat1.xxx) + u_xlat16_2.xyz;
    u_xlat1.xyz = u_xlat1.xyz * u_xlat16_2.xyz;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xyz = min(max(u_xlat1.xyz, 0.0), 1.0);
#else
    u_xlat1.xyz = clamp(u_xlat1.xyz, 0.0, 1.0);
#endif
    u_xlat1.xyz = u_xlat0.xyz * u_xlat1.xyz + (-u_xlat0.xyz);
    u_xlat0.xyz = vec3(_Usecenterglow) * u_xlat1.xyz + u_xlat0.xyz;
    u_xlat1.xyz = u_xlat0.xyz * vec3(_Emission);
    u_xlat0.xy = vs_TEXCOORD2.xy / vs_TEXCOORD2.ww;
    u_xlat0.x = texture(_CameraDepthTexture, u_xlat0.xy).x;
    u_xlat0.x = _ZBufferParams.z * u_xlat0.x + _ZBufferParams.w;
    u_xlat0.x = float(1.0) / u_xlat0.x;
    u_xlat0.x = u_xlat0.x + (-vs_TEXCOORD2.z);
    u_xlat0.x = u_xlat0.x / _Depthpower;
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    u_xlat0.x = u_xlat0.x + -1.0;
    u_xlat0.x = _Usedepth * u_xlat0.x + 1.0;
    u_xlat0.x = u_xlat0.x * vs_COLOR0.w;
    u_xlat0.x = u_xlat0.x * u_xlat0.w;
    u_xlat1.w = u_xlat0.x * _Opacity;
    SV_Target0 = u_xlat1;
    return;
}

#endif
"
}
SubProgram "gles3 hw_tier01 " {
Keywords { "SOFTPARTICLES_ON" }
"#ifdef VERTEX
#version 300 es

#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 _ProjectionParams;
uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
uniform 	vec4 hlslcc_mtx4x4unity_MatrixV[4];
uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_COLOR0;
out highp vec4 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD2;
vec4 u_xlat0;
vec4 u_xlat1;
float u_xlat2;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
    gl_Position = u_xlat1;
    vs_COLOR0 = in_COLOR0;
    vs_TEXCOORD0 = in_TEXCOORD0;
    u_xlat2 = u_xlat0.y * hlslcc_mtx4x4unity_MatrixV[1].z;
    u_xlat0.x = hlslcc_mtx4x4unity_MatrixV[0].z * u_xlat0.x + u_xlat2;
    u_xlat0.x = hlslcc_mtx4x4unity_MatrixV[2].z * u_xlat0.z + u_xlat0.x;
    u_xlat0.x = hlslcc_mtx4x4unity_MatrixV[3].z * u_xlat0.w + u_xlat0.x;
    vs_TEXCOORD2.z = (-u_xlat0.x);
    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
    u_xlat0.w = u_xlat0.x * 0.5;
    u_xlat0.xz = u_xlat1.xw * vec2(0.5, 0.5);
    vs_TEXCOORD2.w = u_xlat1.w;
    vs_TEXCOORD2.xy = u_xlat0.zz + u_xlat0.xw;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es

precision highp float;
precision highp int;
#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 _Time;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 _MainTex_ST;
uniform 	float _Usecenterglow;
uniform 	vec4 _SpeedMainTexUVNoiseZW;
uniform 	vec4 _DistortionSpeedXYPowerZ;
uniform 	vec4 _Flow_ST;
uniform 	vec4 _Mask_ST;
uniform 	vec4 _Noise_ST;
uniform 	vec4 _Color;
uniform 	float _Emission;
uniform 	float _Opacity;
uniform 	mediump float _Usedepth;
uniform 	float _Depthpower;
UNITY_LOCATION(0) uniform highp sampler2D _CameraDepthTexture;
UNITY_LOCATION(1) uniform mediump sampler2D _Mask;
UNITY_LOCATION(2) uniform mediump sampler2D _Flow;
UNITY_LOCATION(3) uniform mediump sampler2D _MainTex;
UNITY_LOCATION(4) uniform mediump sampler2D _Noise;
in mediump vec4 vs_COLOR0;
in highp vec4 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD2;
layout(location = 0) out mediump vec4 SV_Target0;
vec4 u_xlat0;
mediump vec4 u_xlat16_0;
vec4 u_xlat1;
mediump vec4 u_xlat16_1;
vec2 u_xlat2;
mediump vec3 u_xlat16_2;
vec2 u_xlat7;
mediump vec2 u_xlat16_7;
void main()
{
    u_xlat0.xy = vs_TEXCOORD0.xy * _Noise_ST.xy + _Noise_ST.zw;
    u_xlat0.xy = _Time.yy * _SpeedMainTexUVNoiseZW.zw + u_xlat0.xy;
    u_xlat16_0 = texture(_Noise, u_xlat0.xy);
    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    u_xlat1.xy = _Time.yy * _SpeedMainTexUVNoiseZW.xy + u_xlat1.xy;
    u_xlat7.xy = vs_TEXCOORD0.xy * _Flow_ST.xy + _Flow_ST.zw;
    u_xlat7.xy = _Time.yy * _DistortionSpeedXYPowerZ.xy + u_xlat7.xy;
    u_xlat16_7.xy = texture(_Flow, u_xlat7.xy).xy;
    u_xlat2.xy = vs_TEXCOORD0.xy * _Mask_ST.xy + _Mask_ST.zw;
    u_xlat16_2.xyz = texture(_Mask, u_xlat2.xy).xyz;
    u_xlat7.xy = u_xlat16_7.xy * u_xlat16_2.xy;
    u_xlat1.xy = (-u_xlat7.xy) * _DistortionSpeedXYPowerZ.zz + u_xlat1.xy;
    u_xlat16_1 = texture(_MainTex, u_xlat1.xy);
    u_xlat0 = u_xlat16_0 * u_xlat16_1;
    u_xlat0 = u_xlat0 * _Color;
    u_xlat0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
    u_xlat1.x = (-vs_TEXCOORD0.z) + 1.0;
    u_xlat1.xyz = (-u_xlat1.xxx) + u_xlat16_2.xyz;
    u_xlat1.xyz = u_xlat1.xyz * u_xlat16_2.xyz;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xyz = min(max(u_xlat1.xyz, 0.0), 1.0);
#else
    u_xlat1.xyz = clamp(u_xlat1.xyz, 0.0, 1.0);
#endif
    u_xlat1.xyz = u_xlat0.xyz * u_xlat1.xyz + (-u_xlat0.xyz);
    u_xlat0.xyz = vec3(_Usecenterglow) * u_xlat1.xyz + u_xlat0.xyz;
    u_xlat1.xyz = u_xlat0.xyz * vec3(_Emission);
    u_xlat0.xy = vs_TEXCOORD2.xy / vs_TEXCOORD2.ww;
    u_xlat0.x = texture(_CameraDepthTexture, u_xlat0.xy).x;
    u_xlat0.x = _ZBufferParams.z * u_xlat0.x + _ZBufferParams.w;
    u_xlat0.x = float(1.0) / u_xlat0.x;
    u_xlat0.x = u_xlat0.x + (-vs_TEXCOORD2.z);
    u_xlat0.x = u_xlat0.x / _Depthpower;
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    u_xlat0.x = u_xlat0.x + -1.0;
    u_xlat0.x = _Usedepth * u_xlat0.x + 1.0;
    u_xlat0.x = u_xlat0.x * vs_COLOR0.w;
    u_xlat0.x = u_xlat0.x * u_xlat0.w;
    u_xlat1.w = u_xlat0.x * _Opacity;
    SV_Target0 = u_xlat1;
    return;
}

#endif
"
}
SubProgram "gles3 hw_tier02 " {
Keywords { "SOFTPARTICLES_ON" }
"#ifdef VERTEX
#version 300 es

#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 _ProjectionParams;
uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
uniform 	vec4 hlslcc_mtx4x4unity_MatrixV[4];
uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_COLOR0;
out highp vec4 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD2;
vec4 u_xlat0;
vec4 u_xlat1;
float u_xlat2;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
    gl_Position = u_xlat1;
    vs_COLOR0 = in_COLOR0;
    vs_TEXCOORD0 = in_TEXCOORD0;
    u_xlat2 = u_xlat0.y * hlslcc_mtx4x4unity_MatrixV[1].z;
    u_xlat0.x = hlslcc_mtx4x4unity_MatrixV[0].z * u_xlat0.x + u_xlat2;
    u_xlat0.x = hlslcc_mtx4x4unity_MatrixV[2].z * u_xlat0.z + u_xlat0.x;
    u_xlat0.x = hlslcc_mtx4x4unity_MatrixV[3].z * u_xlat0.w + u_xlat0.x;
    vs_TEXCOORD2.z = (-u_xlat0.x);
    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
    u_xlat0.w = u_xlat0.x * 0.5;
    u_xlat0.xz = u_xlat1.xw * vec2(0.5, 0.5);
    vs_TEXCOORD2.w = u_xlat1.w;
    vs_TEXCOORD2.xy = u_xlat0.zz + u_xlat0.xw;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es

precision highp float;
precision highp int;
#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
#if HLSLCC_ENABLE_UNIFORM_BUFFERS
#define UNITY_UNIFORM
#else
#define UNITY_UNIFORM uniform
#endif
#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
#if UNITY_SUPPORTS_UNIFORM_LOCATION
#define UNITY_LOCATION(x) layout(location = x)
#define UNITY_BINDING(x) layout(binding = x, std140)
#else
#define UNITY_LOCATION(x)
#define UNITY_BINDING(x) layout(std140)
#endif
uniform 	vec4 _Time;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 _MainTex_ST;
uniform 	float _Usecenterglow;
uniform 	vec4 _SpeedMainTexUVNoiseZW;
uniform 	vec4 _DistortionSpeedXYPowerZ;
uniform 	vec4 _Flow_ST;
uniform 	vec4 _Mask_ST;
uniform 	vec4 _Noise_ST;
uniform 	vec4 _Color;
uniform 	float _Emission;
uniform 	float _Opacity;
uniform 	mediump float _Usedepth;
uniform 	float _Depthpower;
UNITY_LOCATION(0) uniform highp sampler2D _CameraDepthTexture;
UNITY_LOCATION(1) uniform mediump sampler2D _Mask;
UNITY_LOCATION(2) uniform mediump sampler2D _Flow;
UNITY_LOCATION(3) uniform mediump sampler2D _MainTex;
UNITY_LOCATION(4) uniform mediump sampler2D _Noise;
in mediump vec4 vs_COLOR0;
in highp vec4 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD2;
layout(location = 0) out mediump vec4 SV_Target0;
vec4 u_xlat0;
mediump vec4 u_xlat16_0;
vec4 u_xlat1;
mediump vec4 u_xlat16_1;
vec2 u_xlat2;
mediump vec3 u_xlat16_2;
vec2 u_xlat7;
mediump vec2 u_xlat16_7;
void main()
{
    u_xlat0.xy = vs_TEXCOORD0.xy * _Noise_ST.xy + _Noise_ST.zw;
    u_xlat0.xy = _Time.yy * _SpeedMainTexUVNoiseZW.zw + u_xlat0.xy;
    u_xlat16_0 = texture(_Noise, u_xlat0.xy);
    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    u_xlat1.xy = _Time.yy * _SpeedMainTexUVNoiseZW.xy + u_xlat1.xy;
    u_xlat7.xy = vs_TEXCOORD0.xy * _Flow_ST.xy + _Flow_ST.zw;
    u_xlat7.xy = _Time.yy * _DistortionSpeedXYPowerZ.xy + u_xlat7.xy;
    u_xlat16_7.xy = texture(_Flow, u_xlat7.xy).xy;
    u_xlat2.xy = vs_TEXCOORD0.xy * _Mask_ST.xy + _Mask_ST.zw;
    u_xlat16_2.xyz = texture(_Mask, u_xlat2.xy).xyz;
    u_xlat7.xy = u_xlat16_7.xy * u_xlat16_2.xy;
    u_xlat1.xy = (-u_xlat7.xy) * _DistortionSpeedXYPowerZ.zz + u_xlat1.xy;
    u_xlat16_1 = texture(_MainTex, u_xlat1.xy);
    u_xlat0 = u_xlat16_0 * u_xlat16_1;
    u_xlat0 = u_xlat0 * _Color;
    u_xlat0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
    u_xlat1.x = (-vs_TEXCOORD0.z) + 1.0;
    u_xlat1.xyz = (-u_xlat1.xxx) + u_xlat16_2.xyz;
    u_xlat1.xyz = u_xlat1.xyz * u_xlat16_2.xyz;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xyz = min(max(u_xlat1.xyz, 0.0), 1.0);
#else
    u_xlat1.xyz = clamp(u_xlat1.xyz, 0.0, 1.0);
#endif
    u_xlat1.xyz = u_xlat0.xyz * u_xlat1.xyz + (-u_xlat0.xyz);
    u_xlat0.xyz = vec3(_Usecenterglow) * u_xlat1.xyz + u_xlat0.xyz;
    u_xlat1.xyz = u_xlat0.xyz * vec3(_Emission);
    u_xlat0.xy = vs_TEXCOORD2.xy / vs_TEXCOORD2.ww;
    u_xlat0.x = texture(_CameraDepthTexture, u_xlat0.xy).x;
    u_xlat0.x = _ZBufferParams.z * u_xlat0.x + _ZBufferParams.w;
    u_xlat0.x = float(1.0) / u_xlat0.x;
    u_xlat0.x = u_xlat0.x + (-vs_TEXCOORD2.z);
    u_xlat0.x = u_xlat0.x / _Depthpower;
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    u_xlat0.x = u_xlat0.x + -1.0;
    u_xlat0.x = _Usedepth * u_xlat0.x + 1.0;
    u_xlat0.x = u_xlat0.x * vs_COLOR0.w;
    u_xlat0.x = u_xlat0.x * u_xlat0.w;
    u_xlat1.w = u_xlat0.x * _Opacity;
    SV_Target0 = u_xlat1;
    return;
}

#endif
"
}
}
}
}
}