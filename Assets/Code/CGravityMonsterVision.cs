using UnityEngine;
using System.Collections;

public class CGravityMonsterVision : MonoBehaviour {

	public GameObject objetGravity;

	CGravityMonster m_GravityMonster;
	bool m_bPlayerAreInZone;

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () 
	{
		m_GravityMonster = objetGravity.GetComponent<CGravityMonster>();
		m_bPlayerAreInZone = false;
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () 
	{
		
	}

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnTriggerStay2D(Collider2D other) 
	{
		m_bPlayerAreInZone = false;
		if(other.CompareTag("Player"))
		{
			m_GravityMonster.SeePlayer();
			m_bPlayerAreInZone = true;
		}
		m_GravityMonster.SetPlayerAreInZone (m_bPlayerAreInZone);
	}
}
