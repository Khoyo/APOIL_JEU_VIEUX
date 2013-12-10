using UnityEngine;
using System.Collections;

public class CScriptMur : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		float fWidth = gameObject.renderer.material.GetTexture("_MainTex").width;
		float fHeight = gameObject.renderer.material.GetTexture("_MainTex").height;
		float fX = gameObject.transform.localScale.x;
		float fY = gameObject.transform.localScale.y;
		
		gameObject.renderer.material.SetTextureScale("_MainTex", new Vector2(fX / fWidth, fY / fHeight));	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
