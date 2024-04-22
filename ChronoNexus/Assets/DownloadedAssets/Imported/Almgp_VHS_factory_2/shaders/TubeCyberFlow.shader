// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TubeCyberFlow"
{
	Properties
	{
		_speed2("speed2", Range( -4 , 4)) = 0.33
		_speed("speed", Range( -1 , 1)) = 0.33
		_Size2("Size2", Range( 0 , 8)) = 2.8
		_Size("Size", Range( 0 , 8)) = 2.8
		_Main("Main", 2D) = "white" {}
		[HDR]_Color("Color", Color) = (1,1,1,0)
		_BackColor("BackColor", Color) = (0,0,0,0)
		_Gloss("Gloss", Range( 0 , 1)) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _BackColor;
		uniform sampler2D _Main;
		uniform float _Size2;
		uniform float _speed2;
		uniform float _Size;
		uniform float _speed;
		uniform float4 _Color;
		uniform float _Gloss;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _BackColor.rgb;
			float2 appendResult13 = (float2(i.uv_texcoord.x , ( ( _Size2 * i.uv_texcoord.y ) + ( _Time.y * _speed2 ) )));
			float2 appendResult7 = (float2(i.uv_texcoord.x , ( ( _Size * i.uv_texcoord.y ) + ( _Time.y * _speed ) )));
			o.Emission = ( ( tex2D( _Main, appendResult13 ) * tex2D( _Main, appendResult7 ) ) * _Color ).rgb;
			o.Metallic = 0.0;
			o.Smoothness = _Gloss;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
212;618;1446;996;-733.9736;691.1921;1.3;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-163.7529,-394.9449;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-198.729,286.2358;Float;False;Property;_speed2;speed2;0;0;Create;True;0;0;False;0;0.33;0;-4;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-283.4636,116.3009;Float;False;Property;_speed;speed;1;0;Create;True;0;0;False;0;0.33;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;215.1687,-569.6583;Float;False;Property;_Size2;Size2;2;0;Create;True;0;0;False;0;2.8;0;0;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-116.7959,-724.4102;Float;False;Property;_Size;Size;3;0;Create;True;0;0;False;0;2.8;0;0;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;1;-470,-110;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;197.6551,-302.1633;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;126.0139,257.1266;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;639.5391,-42.32988;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-69.3,-138.4;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;440.1141,291.3232;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;379.8295,-28.3365;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;7;562.8997,-577.1998;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;13;1066.637,252.4137;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;11;642.0696,-322.3442;Float;True;Property;_Main;Main;4;0;Create;True;0;0;False;0;0231c8187e15e9240bbc993da962f605;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;8;908.6999,-547.1;Float;True;Property;_grad;grad;1;0;Create;True;0;0;False;0;7ec655a86e46fe2499f4777db4ee0c46;7ec655a86e46fe2499f4777db4ee0c46;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;999.7216,-221.3799;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;7ec655a86e46fe2499f4777db4ee0c46;7ec655a86e46fe2499f4777db4ee0c46;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;1346.413,-410.7595;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;20;1453.192,-599.6145;Float;False;Property;_Color;Color;5;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;1690.774,-701.5919;Float;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;1820.774,-528.6921;Float;False;Property;_Gloss;Gloss;7;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;1690.138,-210.2876;Float;False;Property;_BackColor;BackColor;6;0;Create;True;0;0;False;0;0,0,0,0;0.3867925,0.3867925,0.3867925,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;1648.388,-420.6392;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2091.655,-526.0403;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;TubeCyberFlow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;10;0
WireConnection;9;1;4;2
WireConnection;18;0;1;2
WireConnection;18;1;17;0
WireConnection;14;0;15;0
WireConnection;14;1;4;2
WireConnection;2;0;1;2
WireConnection;2;1;3;0
WireConnection;19;0;14;0
WireConnection;19;1;18;0
WireConnection;6;0;9;0
WireConnection;6;1;2;0
WireConnection;7;0;4;1
WireConnection;7;1;6;0
WireConnection;13;0;4;1
WireConnection;13;1;19;0
WireConnection;8;0;11;0
WireConnection;8;1;7;0
WireConnection;12;0;11;0
WireConnection;12;1;13;0
WireConnection;16;0;12;0
WireConnection;16;1;8;0
WireConnection;21;0;16;0
WireConnection;21;1;20;0
WireConnection;0;0;22;0
WireConnection;0;2;21;0
WireConnection;0;3;24;0
WireConnection;0;4;23;0
ASEEND*/
//CHKSM=5E3EFE8BF1EB5AD7B5E54BCD502B8D00AADE5E50