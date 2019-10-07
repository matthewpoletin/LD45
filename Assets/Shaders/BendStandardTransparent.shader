// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BendStandardTransparent"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		[Normal]_Normal("Normal", 2D) = "bump" {}
		_Emission("Emission", 2D) = "black" {}
		_MetallicSmoothness("MetallicSmoothness", 2D) = "black" {}
		[HDR]_EmissionColor("EmissionColor", Color) = (1,1,1,1)
		_Color("Color", Color) = (1,1,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float MaxChange;
		uniform float2 BendOrigin;
		uniform float3 BendAmount;
		uniform float MaxDist;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float4 _Color;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform float4 _EmissionColor;
		uniform sampler2D _MetallicSmoothness;
		uniform float4 _MetallicSmoothness_ST;


		float InverseLerp66( float T , float A , float B )
		{
			return (T - A)/(B - A);
		}


		float SinIn73( float t )
		{
			return sin((t - 1.0) *1.5707963267948966) + 1.0;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float T66 = distance( float3( BendOrigin ,  0.0 ) , ( ase_worldPos * BendAmount ) );
			float A66 = 0.0;
			float B66 = MaxDist;
			float localInverseLerp66 = InverseLerp66( T66 , A66 , B66 );
			float t73 = localInverseLerp66;
			float localSinIn73 = SinIn73( t73 );
			float lerpResult70 = lerp( 0.0 , MaxChange , localSinIn73);
			float4 appendResult52 = (float4(ase_vertex3Pos.x , ( ase_vertex3Pos.y - lerpResult70 ) , ase_vertex3Pos.z , 0.0));
			v.vertex.xyz = appendResult52.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode2 = tex2D( _Albedo, uv_Albedo );
			o.Albedo = ( _Color * tex2DNode2 ).rgb;
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			o.Emission = ( tex2D( _Emission, uv_Emission ) * _EmissionColor ).rgb;
			float2 uv_MetallicSmoothness = i.uv_texcoord * _MetallicSmoothness_ST.xy + _MetallicSmoothness_ST.zw;
			float4 tex2DNode7 = tex2D( _MetallicSmoothness, uv_MetallicSmoothness );
			o.Metallic = tex2DNode7.r;
			o.Smoothness = tex2DNode7.a;
			o.Alpha = ( _Color.a * tex2DNode2.a );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
542;73;1009;646;1832.26;-880.9393;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;58;-2356.425,1203.978;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;62;-2278.197,1423.922;Float;False;Global;BendAmount;BendAmount;6;0;Create;True;0;0;False;0;0,0,1;0,0,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-2068.456,1286.74;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector2Node;45;-2291.487,965.3133;Float;False;Global;BendOrigin;BendOrigin;5;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;69;-1857.197,1277.588;Float;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;46;-1943.694,1089.854;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1859.305,1373.747;Float;False;Global;MaxDist;MaxDist;8;0;Create;True;0;0;False;0;100;100;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;66;-1635.507,1258.021;Float;False;return (T - A)/(B - A)@;1;False;3;True;T;FLOAT;0;In;;Float;True;A;FLOAT;0;In;;Float;True;B;FLOAT;0;In;;Float;Inverse Lerp;True;False;0;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;73;-1402.097,1270.247;Float;False;return sin((t - 1.0) *1.5707963267948966) + 1.0@;1;False;1;True;t;FLOAT;0;In;;Float;Sin In;True;False;0;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-1254.034,1143.285;Float;False;Global;MaxChange;MaxChange;9;0;Create;True;0;0;False;0;30;30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-1191.787,1064.497;Float;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1367.438,-540.926;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PosVertexDataNode;54;-382.9727,747.9883;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;70;-968.0992,1192.392;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;6;-1375.783,-88.50533;Float;True;Property;_Emission;Emission;2;0;Create;True;0;0;False;0;None;None;False;black;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;8;-942.9944,352.2454;Float;True;Property;_MetallicSmoothness;MetallicSmoothness;3;0;Create;True;0;0;False;0;None;None;False;black;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;5;-1123.608,-88.08875;Float;True;Property;_TextureSample2;Texture Sample 2;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;10;-1027.56,163.2392;Float;False;Property;_EmissionColor;EmissionColor;4;1;[HDR];Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;49;-176.8196,973.3451;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-1365.865,-296.084;Float;True;Property;_Normal;Normal;1;1;[Normal];Create;True;0;0;False;0;None;None;False;bump;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ColorNode;63;-1058.499,-783.4722;Float;False;Property;_Color;Color;5;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-1130,-523;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-704.374,-396.3357;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-686.7414,347.3004;Float;True;Property;_TextureSample3;Texture Sample 3;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-1116.82,-296.7524;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;52;71.37366,895.4035;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-697.4479,-578.824;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-759.0818,85.92749;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomExpressionNode;67;-1896.474,883.9711;Float;False;return (T - A)/(B - A)@;1;False;3;True;T;FLOAT;0;In;;Float;True;A;FLOAT;0;In;;Float;True;B;FLOAT;0;In;;Float;Inverse Lerp;True;False;0;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;470.9572,11.78751;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;BendStandardTransparent;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Absolute;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;59;0;58;0
WireConnection;59;1;62;0
WireConnection;46;0;45;0
WireConnection;46;1;59;0
WireConnection;66;0;46;0
WireConnection;66;1;69;0
WireConnection;66;2;68;0
WireConnection;73;0;66;0
WireConnection;70;0;71;0
WireConnection;70;1;72;0
WireConnection;70;2;73;0
WireConnection;5;0;6;0
WireConnection;49;0;54;2
WireConnection;49;1;70;0
WireConnection;2;0;1;0
WireConnection;74;0;63;4
WireConnection;74;1;2;4
WireConnection;7;0;8;0
WireConnection;3;0;4;0
WireConnection;52;0;54;1
WireConnection;52;1;49;0
WireConnection;52;2;54;3
WireConnection;64;0;63;0
WireConnection;64;1;2;0
WireConnection;11;0;5;0
WireConnection;11;1;10;0
WireConnection;0;0;64;0
WireConnection;0;1;3;0
WireConnection;0;2;11;0
WireConnection;0;3;7;1
WireConnection;0;4;7;4
WireConnection;0;9;74;0
WireConnection;0;11;52;0
ASEEND*/
//CHKSM=2023E40AC42FBE31BFF9BB5CD74FA5C118B3CA4E