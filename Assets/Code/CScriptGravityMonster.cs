using UnityEngine;
using System.Collections;

public class CScriptGravityMonster : MonoBehaviour 
{
	CGame m_Game;
	CGravityMonster m_GravityMonster;
	public float m_fForceMagnet = 5;
	
	public Material m_material;
	
	// Use this for initialization
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_Game.getLevel().CreateElement<CGravityMonster>(gameObject);		
	}
	
	public void Reset()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnCollisionEnter(Collision other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				m_GravityMonster.JailPlayer(m_Game.getLevel().getPlayer(i));				
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
	
	public Material GetMaterial()
	{
		return m_material;	
	}
}
