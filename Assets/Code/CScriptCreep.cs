using UnityEngine;
using System.Collections;

public class CScriptCreep : MonoBehaviour 
{
	CGame m_Game;
	CCreep m_Creep;
	
	public Material m_material;
	
	// Use this for initialization
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_Game.getLevel().CreateElement<CCreep>(gameObject);		
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
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				if(m_Creep.CanParasitizedPlayer() && m_Creep.GetParasitizedPlayer() != m_Game.getLevel().getPlayer(i) && !m_Creep.IsInState(CCreep.EState.e_Sleep) )
				{
					m_Creep.SetPlayerParasitized(m_Game.getLevel().getPlayer(i));
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
	
	public Material GetMaterial()
	{
		return m_material;	
	}
}
