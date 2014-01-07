using UnityEngine;
using System.Collections;

public class CScriptGravityMonsterZoneVision : MonoBehaviour 
{
	
	CGame m_Game;
	CGravityMonster m_GravityMonster;
	bool m_bHavePlayerInZone;
	float m_fTimeToTurnOff;
	const float m_fTimeToTurnOffMax = 0.5f;
	
	// Use this for initialization
	void Start () 
	{
		m_bHavePlayerInZone = false;
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_fTimeToTurnOff = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_fTimeToTurnOff < 0.0f)
			m_bHavePlayerInZone = false;
		else
			m_bHavePlayerInZone = true;
		
		m_fTimeToTurnOff -= Time.deltaTime;
	}
	
	void OnTriggerStay(Collider other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				m_fTimeToTurnOff = m_fTimeToTurnOffMax;		
			}
		}	
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
