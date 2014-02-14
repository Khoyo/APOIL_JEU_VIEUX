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
	void OnTriggerEnter2D(Collider2D other) 
	{
		Debug.Log ("trigger");
		if(other.CompareTag("Player"))
		{
			CPlayer player = other.gameObject.GetComponent<CPlayer>();
			GameObject.Find ("Game").GetComponent<CGame> ().WinLevel (player.GetIdPlayer());
		}
	}
}
