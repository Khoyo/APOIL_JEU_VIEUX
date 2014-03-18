using UnityEngine;
using System.Collections;

public class CCreepVision : MonoBehaviour {

	public GameObject objetCreep;

	CCreep m_Creep;

	// Use this for initialization
	void Start () 
	{
		m_Creep = objetCreep.GetComponent<CCreep> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D other) 
	{
		if(other.CompareTag("Player"))
		{
			m_Creep.SeePlayer(other.gameObject.GetComponent<CPlayer>());
		}
	}
}
