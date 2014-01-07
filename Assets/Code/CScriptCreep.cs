using UnityEngine;
using System.Collections;

public class CScriptCreep : MonoBehaviour 
{
	CGame m_Game;
	CCreep m_Creep;
	float m_fTimeToTurnLightOff;
	const float m_fTimeToTurnLightOffMax = 0.5f;
	
	public Material m_material;
	
	// Use this for initialization
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_Game.getLevel().CreateElement<CCreep>(gameObject);
		m_fTimeToTurnLightOff = 0.0f;
	}
	
	public void Reset()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!m_Creep.IsOnLight() && m_fTimeToTurnLightOff > 0.0f)
			m_Creep.SetLightStatus(true);
		
		else if(m_Creep.IsOnLight() && m_fTimeToTurnLightOff < 0.0f)
			m_Creep.SetLightStatus(false);
		
		m_fTimeToTurnLightOff -= Time.deltaTime;
	}
/*	
	void OnTriggerEnter(Collider other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				if(m_Creep.CanParasitizedPlayer() && m_Creep.GetParasitizedPlayer() != m_Game.getLevel().getPlayer(i) && !m_Creep.IsInState(CCreep.EState.e_Sleep) )
				{
					m_Creep.SetPlayerParasitized(m_Game.getLevel().getPlayer(i));
				}
			}
		}	
	}
*/
	
	void OnCollisionEnter(Collision other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				if(m_Creep.CanParasitizedPlayer() && m_Creep.GetParasitizedPlayer() != m_Game.getLevel().getPlayer(i) && m_Creep.CanTakePlayer() )
				{
					m_Creep.SetPlayerParasitized(m_Game.getLevel().getPlayer(i));
				}
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
	public void SetCreepElement(CCreep obj)
	{
		m_Creep = obj;
	}
	
	public Material GetMaterial()
	{
		return m_material;	
	}
}
