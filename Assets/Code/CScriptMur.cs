using UnityEngine;
using System.Collections;

public class CScriptMur : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float fY = gameObject.transform.localScale.y / gameObject.transform.localScale.x;
		gameObject.renderer.material.SetTextureScale("_MainTex", new Vector2(1,fY));
	}
}
