﻿using UnityEngine;
using System.Collections;

public class CCreepVision : MonoBehaviour {

	public GameObject objetCreep;

	CCreep m_Creep;

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () 
	{
		m_Creep = objetCreep.GetComponent<CCreep> ();
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () {
	
	}

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnTriggerStay2D(Collider2D other) 
	{
		if(other.CompareTag("Player"))
		{
			m_Creep.SeePlayer(other.gameObject.GetComponent<CPlayer>());
		}
	}
}
