using UnityEngine;
using System.Collections;

public class CScriptCreepZone : MonoBehaviour 
{

	CGame m_Game;
	CCreep m_Creep;
	
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
	
	void OnTriggerStay(Collider other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				if(!m_Creep.IsOnPlayer())
				{
					m_Creep.SetPositionToGo(m_Game.getLevel().getPlayer(i).GetPosition2D());
				}
			}
			
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetCreepElement(CCreep obj)
	{
		m_Creep = obj;
	}
}
