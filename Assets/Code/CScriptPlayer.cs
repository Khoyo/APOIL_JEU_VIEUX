using UnityEngine;
using System.Collections;

public class CScriptPlayer : MonoBehaviour 
{
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "player") 
		{	
			Physics.IgnoreCollision(other.collider, collider); 	
		}
		
		
	}
}
