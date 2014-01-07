using UnityEngine;
using System.Collections;

public class CScriptGravityMonsterZoneVision : MonoBehaviour 
{
	
	CGame m_Game;
	CGravityMonster m_GravityMonster;
	bool m_bHavePlayerInZone;
	
	// Use this for initialization
	void Start () 
	{
		m_bHavePlayerInZone = false;
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerStay(Collider other) 
	{
		bool bHavePlayerInZone = false;
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				bHavePlayerInZone = true;
			}
		}
		m_bHavePlayerInZone = bHavePlayerInZone;
	}

	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetGravityMonsterElement(CGravityMonster obj)
	{
		m_GravityMonster = obj;
	}
	
	public bool HavePlayerInZone()
	{
		return m_bHavePlayerInZone;	
	}
}
