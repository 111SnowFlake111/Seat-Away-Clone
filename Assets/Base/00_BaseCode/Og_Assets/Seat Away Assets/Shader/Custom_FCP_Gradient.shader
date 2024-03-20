//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "Custom/FCP_Gradient" {
Properties {
[Enum(Horizontal,0,Vertical,1,Double,2,HueHorizontal,3,HueVertical,4)] _Mode ("Color mode", Float) = 0
_Color1 ("Color 1", Color) = (1,1,1,1)
_Color2 ("Color 2", Color) = (1,1,1,1)
[Enum(HS, 0, HV, 1, SH, 2, SV, 3, VH, 4, VS, 5)] _DoubleMode ("Double mode", Float) = 0
_HSV ("Complementing HSV values", Vector) = (0,0,0,1)
_HSV_MIN ("Min Range value for HSV", Vector) = (0,0,0,0)
_HSV_MAX ("Max Range value for HSV", Vector) = (1,1,1,1)
_StencilComp ("Stencil Comparison", Float) = 8
_Stencil ("Stencil ID", Float) = 0
_StencilOp ("Stencil Operation", Float) = 0
_StencilWriteMask ("Stencil Write Mask", Float) = 255
_StencilReadMask ("Stencil Read Mask", Float) = 255
_ColorMask ("Color Mask", Float) = 15
}
SubShader {
 Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
 Pass {
  Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
  ZWrite Off
  Stencil {
   ReadMask 0
   WriteMask 0
   Comp Disabled
   Pass Keep
   Fail Keep
   ZFail Keep
  }
  GpuProgramID 5651
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
uniform 	mediump vec4 _Color1;
uniform 	mediump vec4 _Color2;
uniform 	mediump vec4 _HSV;
uniform 	mediump vec4 _HSV_MIN;
uniform 	mediump vec4 _HSV_MAX;
uniform 	int _Mode;
uniform 	int _DoubleMode;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_COLOR0;
vec4 u_xlat0;
mediump vec4 u_xlat16_0;
vec4 u_xlat1;
mediump vec4 u_xlat16_2;
vec2 u_xlat4;
mediump float u_xlat16_5;
float u_xlat7;
int u_xlati7;
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
    switch(_Mode){
        case 1:
            u_xlat1 = (-_Color1) + _Color2;
            u_xlat0 = in_TEXCOORD0.yyyy * u_xlat1.yxzw + _Color1.yxzw;
            u_xlat16_0 = u_xlat0;
            break;
        case 2:
            switch(_DoubleMode){
                case 1:
                    u_xlat1.xy = (-_HSV_MIN.xz) + _HSV_MAX.xz;
                    u_xlat1.xy = in_TEXCOORD0.xy * u_xlat1.xy + _HSV_MIN.xz;
                    u_xlat16_0.x = _HSV.y;
                    u_xlat16_0.y = u_xlat1.y;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
                case 2:
                    u_xlat1.xy = (-_HSV_MIN.xy) + _HSV_MAX.xy;
                    u_xlat1.xy = in_TEXCOORD0.yx * u_xlat1.xy + _HSV_MIN.xy;
                    u_xlat16_0.x = u_xlat1.y;
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
                case 3:
                    u_xlat1.xy = (-_HSV_MIN.yz) + _HSV_MAX.yz;
                    u_xlat0.xy = in_TEXCOORD0.xy * u_xlat1.xy + _HSV_MIN.yz;
                    u_xlat16_0.xy = u_xlat0.xy;
                    u_xlat16_2.x = _HSV.x;
                    break;
                case 4:
                    u_xlat1.xy = (-_HSV_MIN.xz) + _HSV_MAX.xz;
                    u_xlat1.xy = in_TEXCOORD0.yx * u_xlat1.xy + _HSV_MIN.xz;
                    u_xlat16_0.x = _HSV.y;
                    u_xlat16_0.y = u_xlat1.y;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
                case 5:
                    u_xlat1.xy = (-_HSV_MIN.yz) + _HSV_MAX.yz;
                    u_xlat0.xy = in_TEXCOORD0.yx * u_xlat1.xy + _HSV_MIN.yz;
                    u_xlat16_0.xy = u_xlat0.xy;
                    u_xlat16_2.x = _HSV.x;
                    break;
                default:
                    u_xlat1.xy = (-_HSV_MIN.xy) + _HSV_MAX.xy;
                    u_xlat1.xy = in_TEXCOORD0.xy * u_xlat1.xy + _HSV_MIN.xy;
                    u_xlat16_0.x = u_xlat1.y;
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
            }
            u_xlat16_5 = u_xlat16_0.y * u_xlat16_0.x;
            u_xlat1.x = (-u_xlat16_0.x) * u_xlat16_0.y + u_xlat16_0.y;
            u_xlat4.xy = abs(u_xlat16_2.xx) * vec2(3.0, 1.0);
            u_xlat4.xy = fract(u_xlat4.xy);
            u_xlat4.x = u_xlat4.x * 2.0 + -1.0;
            u_xlat4.x = -abs(u_xlat4.x) + 1.0;
            u_xlat1.y = u_xlat16_5 * u_xlat4.x + u_xlat1.x;
            u_xlat7 = u_xlat4.y * 6.0;
            u_xlati7 = int(u_xlat7);
            switch(u_xlati7){
                case 0:
                    u_xlat16_0.xz = u_xlat1.yx;
                    break;
                case 1:
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.xyz = u_xlat16_0.yxz;
                    break;
                case 2:
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.xyz = u_xlat16_0.yxz;
                    break;
                case 3:
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.xyz = u_xlat16_0.zxy;
                    break;
                case 4:
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.xyz = u_xlat16_0.zxy;
                    break;
                case 5:
                    u_xlat16_0.xz = u_xlat1.xy;
                    break;
            }
            u_xlat16_0.w = 1.0;
            break;
        case 3:
            u_xlat1.x = (-_HSV_MIN.x) + _HSV_MAX.x;
            u_xlat1.x = in_TEXCOORD0.x * u_xlat1.x + _HSV_MIN.x;
            u_xlat16_2.x = _HSV.z * _HSV.y;
            u_xlat1.y = (-_HSV.y) * _HSV.z + _HSV.z;
            u_xlat1.xz = abs(u_xlat1.xx) * vec2(3.0, 1.0);
            u_xlat1.xz = fract(u_xlat1.xz);
            u_xlat1.x = u_xlat1.x * 2.0 + -1.0;
            u_xlat1.x = -abs(u_xlat1.x) + 1.0;
            u_xlat1.x = u_xlat16_2.x * u_xlat1.x + u_xlat1.y;
            u_xlat7 = u_xlat1.z * 6.0;
            u_xlati7 = int(u_xlat7);
            switch(u_xlati7){
                case 0:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 1:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 2:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                case 3:
                    u_xlat16_0.xy = u_xlat1.xy;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 4:
                    u_xlat16_0.xy = u_xlat1.yx;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 5:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                default:
                    u_xlat16_2.x = 0.0;
                    u_xlat16_2.w = _HSV.w;
                    u_xlat16_0 = u_xlat16_2.xxxw;
                    break;
            }
            break;
        case 4:
            u_xlat1.x = (-_HSV_MIN.x) + _HSV_MAX.x;
            u_xlat1.x = in_TEXCOORD0.y * u_xlat1.x + _HSV_MIN.x;
            u_xlat16_2.x = _HSV.z * _HSV.y;
            u_xlat1.y = (-_HSV.y) * _HSV.z + _HSV.z;
            u_xlat1.xz = abs(u_xlat1.xx) * vec2(3.0, 1.0);
            u_xlat1.xz = fract(u_xlat1.xz);
            u_xlat1.x = u_xlat1.x * 2.0 + -1.0;
            u_xlat1.x = -abs(u_xlat1.x) + 1.0;
            u_xlat1.x = u_xlat16_2.x * u_xlat1.x + u_xlat1.y;
            u_xlat7 = u_xlat1.z * 6.0;
            u_xlati7 = int(u_xlat7);
            switch(u_xlati7){
                case 0:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 1:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 2:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                case 3:
                    u_xlat16_0.xy = u_xlat1.xy;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 4:
                    u_xlat16_0.xy = u_xlat1.yx;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 5:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                default:
                    u_xlat16_2.x = 0.0;
                    u_xlat16_2.w = _HSV.w;
                    u_xlat16_0 = u_xlat16_2.xxxw;
                    break;
            }
            break;
        default:
            u_xlat1 = (-_Color1) + _Color2;
            u_xlat0 = in_TEXCOORD0.xxxx * u_xlat1.yxzw + _Color1.yxzw;
            u_xlat16_0 = u_xlat0;
            break;
    }
    vs_COLOR0 = u_xlat16_0.yxzw;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es

precision highp float;
precision highp int;
in mediump vec4 vs_COLOR0;
layout(location = 0) out highp vec4 SV_Target0;
void main()
{
    SV_Target0 = vs_COLOR0;
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
uniform 	mediump vec4 _Color1;
uniform 	mediump vec4 _Color2;
uniform 	mediump vec4 _HSV;
uniform 	mediump vec4 _HSV_MIN;
uniform 	mediump vec4 _HSV_MAX;
uniform 	int _Mode;
uniform 	int _DoubleMode;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_COLOR0;
vec4 u_xlat0;
mediump vec4 u_xlat16_0;
vec4 u_xlat1;
mediump vec4 u_xlat16_2;
vec2 u_xlat4;
mediump float u_xlat16_5;
float u_xlat7;
int u_xlati7;
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
    switch(_Mode){
        case 1:
            u_xlat1 = (-_Color1) + _Color2;
            u_xlat0 = in_TEXCOORD0.yyyy * u_xlat1.yxzw + _Color1.yxzw;
            u_xlat16_0 = u_xlat0;
            break;
        case 2:
            switch(_DoubleMode){
                case 1:
                    u_xlat1.xy = (-_HSV_MIN.xz) + _HSV_MAX.xz;
                    u_xlat1.xy = in_TEXCOORD0.xy * u_xlat1.xy + _HSV_MIN.xz;
                    u_xlat16_0.x = _HSV.y;
                    u_xlat16_0.y = u_xlat1.y;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
                case 2:
                    u_xlat1.xy = (-_HSV_MIN.xy) + _HSV_MAX.xy;
                    u_xlat1.xy = in_TEXCOORD0.yx * u_xlat1.xy + _HSV_MIN.xy;
                    u_xlat16_0.x = u_xlat1.y;
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
                case 3:
                    u_xlat1.xy = (-_HSV_MIN.yz) + _HSV_MAX.yz;
                    u_xlat0.xy = in_TEXCOORD0.xy * u_xlat1.xy + _HSV_MIN.yz;
                    u_xlat16_0.xy = u_xlat0.xy;
                    u_xlat16_2.x = _HSV.x;
                    break;
                case 4:
                    u_xlat1.xy = (-_HSV_MIN.xz) + _HSV_MAX.xz;
                    u_xlat1.xy = in_TEXCOORD0.yx * u_xlat1.xy + _HSV_MIN.xz;
                    u_xlat16_0.x = _HSV.y;
                    u_xlat16_0.y = u_xlat1.y;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
                case 5:
                    u_xlat1.xy = (-_HSV_MIN.yz) + _HSV_MAX.yz;
                    u_xlat0.xy = in_TEXCOORD0.yx * u_xlat1.xy + _HSV_MIN.yz;
                    u_xlat16_0.xy = u_xlat0.xy;
                    u_xlat16_2.x = _HSV.x;
                    break;
                default:
                    u_xlat1.xy = (-_HSV_MIN.xy) + _HSV_MAX.xy;
                    u_xlat1.xy = in_TEXCOORD0.xy * u_xlat1.xy + _HSV_MIN.xy;
                    u_xlat16_0.x = u_xlat1.y;
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
            }
            u_xlat16_5 = u_xlat16_0.y * u_xlat16_0.x;
            u_xlat1.x = (-u_xlat16_0.x) * u_xlat16_0.y + u_xlat16_0.y;
            u_xlat4.xy = abs(u_xlat16_2.xx) * vec2(3.0, 1.0);
            u_xlat4.xy = fract(u_xlat4.xy);
            u_xlat4.x = u_xlat4.x * 2.0 + -1.0;
            u_xlat4.x = -abs(u_xlat4.x) + 1.0;
            u_xlat1.y = u_xlat16_5 * u_xlat4.x + u_xlat1.x;
            u_xlat7 = u_xlat4.y * 6.0;
            u_xlati7 = int(u_xlat7);
            switch(u_xlati7){
                case 0:
                    u_xlat16_0.xz = u_xlat1.yx;
                    break;
                case 1:
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.xyz = u_xlat16_0.yxz;
                    break;
                case 2:
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.xyz = u_xlat16_0.yxz;
                    break;
                case 3:
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.xyz = u_xlat16_0.zxy;
                    break;
                case 4:
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.xyz = u_xlat16_0.zxy;
                    break;
                case 5:
                    u_xlat16_0.xz = u_xlat1.xy;
                    break;
            }
            u_xlat16_0.w = 1.0;
            break;
        case 3:
            u_xlat1.x = (-_HSV_MIN.x) + _HSV_MAX.x;
            u_xlat1.x = in_TEXCOORD0.x * u_xlat1.x + _HSV_MIN.x;
            u_xlat16_2.x = _HSV.z * _HSV.y;
            u_xlat1.y = (-_HSV.y) * _HSV.z + _HSV.z;
            u_xlat1.xz = abs(u_xlat1.xx) * vec2(3.0, 1.0);
            u_xlat1.xz = fract(u_xlat1.xz);
            u_xlat1.x = u_xlat1.x * 2.0 + -1.0;
            u_xlat1.x = -abs(u_xlat1.x) + 1.0;
            u_xlat1.x = u_xlat16_2.x * u_xlat1.x + u_xlat1.y;
            u_xlat7 = u_xlat1.z * 6.0;
            u_xlati7 = int(u_xlat7);
            switch(u_xlati7){
                case 0:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 1:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 2:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                case 3:
                    u_xlat16_0.xy = u_xlat1.xy;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 4:
                    u_xlat16_0.xy = u_xlat1.yx;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 5:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                default:
                    u_xlat16_2.x = 0.0;
                    u_xlat16_2.w = _HSV.w;
                    u_xlat16_0 = u_xlat16_2.xxxw;
                    break;
            }
            break;
        case 4:
            u_xlat1.x = (-_HSV_MIN.x) + _HSV_MAX.x;
            u_xlat1.x = in_TEXCOORD0.y * u_xlat1.x + _HSV_MIN.x;
            u_xlat16_2.x = _HSV.z * _HSV.y;
            u_xlat1.y = (-_HSV.y) * _HSV.z + _HSV.z;
            u_xlat1.xz = abs(u_xlat1.xx) * vec2(3.0, 1.0);
            u_xlat1.xz = fract(u_xlat1.xz);
            u_xlat1.x = u_xlat1.x * 2.0 + -1.0;
            u_xlat1.x = -abs(u_xlat1.x) + 1.0;
            u_xlat1.x = u_xlat16_2.x * u_xlat1.x + u_xlat1.y;
            u_xlat7 = u_xlat1.z * 6.0;
            u_xlati7 = int(u_xlat7);
            switch(u_xlati7){
                case 0:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 1:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 2:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                case 3:
                    u_xlat16_0.xy = u_xlat1.xy;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 4:
                    u_xlat16_0.xy = u_xlat1.yx;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 5:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                default:
                    u_xlat16_2.x = 0.0;
                    u_xlat16_2.w = _HSV.w;
                    u_xlat16_0 = u_xlat16_2.xxxw;
                    break;
            }
            break;
        default:
            u_xlat1 = (-_Color1) + _Color2;
            u_xlat0 = in_TEXCOORD0.xxxx * u_xlat1.yxzw + _Color1.yxzw;
            u_xlat16_0 = u_xlat0;
            break;
    }
    vs_COLOR0 = u_xlat16_0.yxzw;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es

precision highp float;
precision highp int;
in mediump vec4 vs_COLOR0;
layout(location = 0) out highp vec4 SV_Target0;
void main()
{
    SV_Target0 = vs_COLOR0;
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
uniform 	mediump vec4 _Color1;
uniform 	mediump vec4 _Color2;
uniform 	mediump vec4 _HSV;
uniform 	mediump vec4 _HSV_MIN;
uniform 	mediump vec4 _HSV_MAX;
uniform 	int _Mode;
uniform 	int _DoubleMode;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_COLOR0;
vec4 u_xlat0;
mediump vec4 u_xlat16_0;
vec4 u_xlat1;
mediump vec4 u_xlat16_2;
vec2 u_xlat4;
mediump float u_xlat16_5;
float u_xlat7;
int u_xlati7;
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
    switch(_Mode){
        case 1:
            u_xlat1 = (-_Color1) + _Color2;
            u_xlat0 = in_TEXCOORD0.yyyy * u_xlat1.yxzw + _Color1.yxzw;
            u_xlat16_0 = u_xlat0;
            break;
        case 2:
            switch(_DoubleMode){
                case 1:
                    u_xlat1.xy = (-_HSV_MIN.xz) + _HSV_MAX.xz;
                    u_xlat1.xy = in_TEXCOORD0.xy * u_xlat1.xy + _HSV_MIN.xz;
                    u_xlat16_0.x = _HSV.y;
                    u_xlat16_0.y = u_xlat1.y;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
                case 2:
                    u_xlat1.xy = (-_HSV_MIN.xy) + _HSV_MAX.xy;
                    u_xlat1.xy = in_TEXCOORD0.yx * u_xlat1.xy + _HSV_MIN.xy;
                    u_xlat16_0.x = u_xlat1.y;
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
                case 3:
                    u_xlat1.xy = (-_HSV_MIN.yz) + _HSV_MAX.yz;
                    u_xlat0.xy = in_TEXCOORD0.xy * u_xlat1.xy + _HSV_MIN.yz;
                    u_xlat16_0.xy = u_xlat0.xy;
                    u_xlat16_2.x = _HSV.x;
                    break;
                case 4:
                    u_xlat1.xy = (-_HSV_MIN.xz) + _HSV_MAX.xz;
                    u_xlat1.xy = in_TEXCOORD0.yx * u_xlat1.xy + _HSV_MIN.xz;
                    u_xlat16_0.x = _HSV.y;
                    u_xlat16_0.y = u_xlat1.y;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
                case 5:
                    u_xlat1.xy = (-_HSV_MIN.yz) + _HSV_MAX.yz;
                    u_xlat0.xy = in_TEXCOORD0.yx * u_xlat1.xy + _HSV_MIN.yz;
                    u_xlat16_0.xy = u_xlat0.xy;
                    u_xlat16_2.x = _HSV.x;
                    break;
                default:
                    u_xlat1.xy = (-_HSV_MIN.xy) + _HSV_MAX.xy;
                    u_xlat1.xy = in_TEXCOORD0.xy * u_xlat1.xy + _HSV_MIN.xy;
                    u_xlat16_0.x = u_xlat1.y;
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_2.x = u_xlat1.x;
                    break;
            }
            u_xlat16_5 = u_xlat16_0.y * u_xlat16_0.x;
            u_xlat1.x = (-u_xlat16_0.x) * u_xlat16_0.y + u_xlat16_0.y;
            u_xlat4.xy = abs(u_xlat16_2.xx) * vec2(3.0, 1.0);
            u_xlat4.xy = fract(u_xlat4.xy);
            u_xlat4.x = u_xlat4.x * 2.0 + -1.0;
            u_xlat4.x = -abs(u_xlat4.x) + 1.0;
            u_xlat1.y = u_xlat16_5 * u_xlat4.x + u_xlat1.x;
            u_xlat7 = u_xlat4.y * 6.0;
            u_xlati7 = int(u_xlat7);
            switch(u_xlati7){
                case 0:
                    u_xlat16_0.xz = u_xlat1.yx;
                    break;
                case 1:
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.xyz = u_xlat16_0.yxz;
                    break;
                case 2:
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.xyz = u_xlat16_0.yxz;
                    break;
                case 3:
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.xyz = u_xlat16_0.zxy;
                    break;
                case 4:
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.xyz = u_xlat16_0.zxy;
                    break;
                case 5:
                    u_xlat16_0.xz = u_xlat1.xy;
                    break;
            }
            u_xlat16_0.w = 1.0;
            break;
        case 3:
            u_xlat1.x = (-_HSV_MIN.x) + _HSV_MAX.x;
            u_xlat1.x = in_TEXCOORD0.x * u_xlat1.x + _HSV_MIN.x;
            u_xlat16_2.x = _HSV.z * _HSV.y;
            u_xlat1.y = (-_HSV.y) * _HSV.z + _HSV.z;
            u_xlat1.xz = abs(u_xlat1.xx) * vec2(3.0, 1.0);
            u_xlat1.xz = fract(u_xlat1.xz);
            u_xlat1.x = u_xlat1.x * 2.0 + -1.0;
            u_xlat1.x = -abs(u_xlat1.x) + 1.0;
            u_xlat1.x = u_xlat16_2.x * u_xlat1.x + u_xlat1.y;
            u_xlat7 = u_xlat1.z * 6.0;
            u_xlati7 = int(u_xlat7);
            switch(u_xlati7){
                case 0:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 1:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 2:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                case 3:
                    u_xlat16_0.xy = u_xlat1.xy;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 4:
                    u_xlat16_0.xy = u_xlat1.yx;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 5:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                default:
                    u_xlat16_2.x = 0.0;
                    u_xlat16_2.w = _HSV.w;
                    u_xlat16_0 = u_xlat16_2.xxxw;
                    break;
            }
            break;
        case 4:
            u_xlat1.x = (-_HSV_MIN.x) + _HSV_MAX.x;
            u_xlat1.x = in_TEXCOORD0.y * u_xlat1.x + _HSV_MIN.x;
            u_xlat16_2.x = _HSV.z * _HSV.y;
            u_xlat1.y = (-_HSV.y) * _HSV.z + _HSV.z;
            u_xlat1.xz = abs(u_xlat1.xx) * vec2(3.0, 1.0);
            u_xlat1.xz = fract(u_xlat1.xz);
            u_xlat1.x = u_xlat1.x * 2.0 + -1.0;
            u_xlat1.x = -abs(u_xlat1.x) + 1.0;
            u_xlat1.x = u_xlat16_2.x * u_xlat1.x + u_xlat1.y;
            u_xlat7 = u_xlat1.z * 6.0;
            u_xlati7 = int(u_xlat7);
            switch(u_xlati7){
                case 0:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 1:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.xy;
                    u_xlat16_0.w = 1.0;
                    break;
                case 2:
                    u_xlat16_0.x = _HSV.z;
                    u_xlat16_0.yz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                case 3:
                    u_xlat16_0.xy = u_xlat1.xy;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 4:
                    u_xlat16_0.xy = u_xlat1.yx;
                    u_xlat16_0.z = _HSV.z;
                    u_xlat16_0.w = 1.0;
                    break;
                case 5:
                    u_xlat16_0.y = _HSV.z;
                    u_xlat16_0.xz = u_xlat1.yx;
                    u_xlat16_0.w = 1.0;
                    break;
                default:
                    u_xlat16_2.x = 0.0;
                    u_xlat16_2.w = _HSV.w;
                    u_xlat16_0 = u_xlat16_2.xxxw;
                    break;
            }
            break;
        default:
            u_xlat1 = (-_Color1) + _Color2;
            u_xlat0 = in_TEXCOORD0.xxxx * u_xlat1.yxzw + _Color1.yxzw;
            u_xlat16_0 = u_xlat0;
            break;
    }
    vs_COLOR0 = u_xlat16_0.yxzw;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es

precision highp float;
precision highp int;
in mediump vec4 vs_COLOR0;
layout(location = 0) out highp vec4 SV_Target0;
void main()
{
    SV_Target0 = vs_COLOR0;
    return;
}

#endif
"
}
}
}
}
}