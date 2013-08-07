Shader "Custom/ShaderRendu" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,0.5)
    _MainTex ("Texture de la scene", 2D) = "white" { }
    _ConeTex ("Texture du Cone", 2D) = "white" { }
    _ForceTex ("Texture du rendu forcé", 2D) = "white" { }
}
SubShader {
    Pass {

	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	
	#include "UnityCG.cginc"
	
	float4 _Color;
	sampler2D _MainTex;
	sampler2D _ConeTex;
	sampler2D _ForceTex;
	
	struct v2f {
	    float4  pos : SV_POSITION;
	    float2  uv : TEXCOORD0;
	};
	
	float4 _MainTex_ST;
	
	v2f vert (appdata_base v)
	{
	    v2f o;
	    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
	    return o;
	}
	
	fixed4 frag (v2f i) : COLOR
	{
	    fixed4 texBlack = fixed4(0.0f, 0.0f, 0.0f, 1.0f);
	     fixed4 texWhite = fixed4(1.0f, 1.0f, 1.0f, 1.0f);
	     fixed4 texFondVert = fixed4(0.0f, 1.0f, 0.0f, 1.0f);
	    
	   	fixed4 texSceneColor = tex2D (_MainTex, i.uv)* _Color;
	    fixed4 texConeColor = tex2D (_ConeTex, i.uv)* _Color;
	    fixed4 texForceColor = tex2D (_ForceTex, i.uv)* _Color;
	    
	    fixed4 texReturn = texSceneColor - texConeColor;
	    
	    fixed4 texReturn2 = texForceColor;
	    if (texForceColor.x == texWhite.x && texForceColor.y == texWhite.y && texForceColor.z == texWhite.z)
	   		 texReturn2 = texReturn;
	  	//texReturn *= texForceColor
	   // return texAlpha;
	   return texReturn2;
	}
	ENDCG

    }
}
Fallback "VertexLit"
}