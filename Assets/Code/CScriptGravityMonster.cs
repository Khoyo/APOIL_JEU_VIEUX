using UnityEngine;
using System.Collections;

public class CScriptGravityMonster : MonoBehaviour 
{
	CGame m_Game;
	CGravityMonster m_GravityMonster;
	public float m_fForceMagnet = 5;
	float m_fTimeToTurnLightOff;
	const float m_fTimeToTurnLightOffMax = 0.5f;
	
	public Material m_material;
	
	// Use this for initialization
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_Game.getLevel().CreateElement<CGravityMonster>(gameObject);	
		m_fTimeToTurnLightOff = 0.0f;
	}
	
	public void Reset()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!m_GravityMonster.IsOnLight() && m_fTimeToTurnLightOff > 0.0f)
			m_GravityMonster.SetLightStatus(true);
		
		else if(m_GravityMonster.IsOnLight() && m_fTimeToTurnLightOff < 0.0f)
			m_GravityMonster.SetLightStatus(false);
		
		m_fTimeToTurnLightOff -= Time.deltaTime;
	}
	
	void OnTriggerEnter(Collider other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject() && m_GravityMonster.CanTakePlayer())
			{
				m_GravityMonster.EatPlayer(m_Game.getLevel().getPlayer(i));				
			}
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "ZoneLight") 
		{	
			m_fTimeToTurnLightOff = m_fTimeToTurnLightOffMax;	
		}	
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetGravityMonsterElement(CGravityMonster obj)
	{
		m_GravityMonster = obj;
	}
	
	public Material GetMaterial()
	{
		return m_material;	
	}
}
