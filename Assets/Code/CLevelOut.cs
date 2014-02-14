using UnityEngine;
using System.Collections;

public class CLevelOut : MonoBehaviour {

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () {
	
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () {
	
	}

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnTriggerEnter(Collider other) 
	{
		Debug.Log ("trigger");
		if(other.CompareTag("Player"))
		{
			CPlayer player = other.gameObject.GetComponent<CPlayer>();
			GameObject.Find ("Game").GetComponent<CGame> ().WinLevel (player.GetIdPlayer());
		}
	}
	void OnCollisionEnter(Collision other) 
	{
		Debug.Log ("collider");
		if(other.collider.CompareTag("Player"))
		{
			CPlayer player = other.gameObject.GetComponent<CPlayer>();
			GameObject.Find ("Game").GetComponent<CGame> ().WinLevel (player.GetIdPlayer());
		}
	}

}
