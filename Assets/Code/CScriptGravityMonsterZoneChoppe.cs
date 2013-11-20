using UnityEngine;
using System.Collections;

public class CScriptGravityMonsterZoneChoppe : MonoBehaviour 
{

	CGame m_Game;
	CGravityMonster m_GravityMonster;
	bool m_bHavePlayerInZone;
	
	// Use this for initialization
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
	}
	
	public void Reset()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnTriggerEnter(Collider other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject() && m_GravityMonster.IsInState(CGravityMonster.EState.e_Alerte))
			{
				m_GravityMonster.CatchPlayer(m_Game.getLevel().getPlayer(i));
			}
		}
	}
	
	void OnTriggerExit(Collider other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				m_GravityMonster.DropPlayer(m_Game.getLevel().getPlayer(i));
			}
		}
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
