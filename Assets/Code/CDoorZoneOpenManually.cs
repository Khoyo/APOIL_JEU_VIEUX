using UnityEngine;
using System.Collections;

public class CDoorZoneOpenManually : MonoBehaviour {

	public GameObject objetDoor;
	
	CDoor m_Door;

	// Use this for initialization
	void Start () 
	{
		m_Door = objetDoor.GetComponent<CDoor> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnTriggerStay2D(Collider2D other) 
	{
		if(other.CompareTag("Player"))
		{
			CPlayer playerInZone = other.gameObject.GetComponent<CPlayer>();
			if(playerInZone.GetPlayerInput().OpenDoor)
			{
				m_Door.Open();
			}
		}
	}
}
