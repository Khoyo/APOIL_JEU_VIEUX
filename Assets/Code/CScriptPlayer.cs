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
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "ZoneLight") 
		{	
			Debug.Log ("on a bientot fini le jeu!");
		}	
	}
}
