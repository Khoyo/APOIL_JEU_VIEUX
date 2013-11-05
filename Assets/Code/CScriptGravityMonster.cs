using UnityEngine;
using System.Collections;

public class CScriptGravityMonster : MonoBehaviour 
{
	CGame m_Game;
	CGravityMonster m_GravityMonster;
	
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
				//gameObject.transform.Translate(new Vector3(10,10,0));
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
