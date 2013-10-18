using UnityEngine;
using System.Collections;

public class CScriptScie : MonoBehaviour {
	
	CGame m_Game;
	
	CScie m_parent;	
	
	public CScie GetScie()
	{
		return m_parent;
	}
	public void SetScie(CScie scie)
	{
		m_parent=scie;
	}
	
	//-------------------------------------------------------------------------------
	///	Unity
	//-------------------------------------------------------------------------------
	void Start() 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
	}
	
	//-------------------------------------------------------------------------------
	///	Unity
	//-------------------------------------------------------------------------------
	void Update () 
	{
	
	}
	
	//-------------------------------------------------------------------------------
	///	Unity
	//-------------------------------------------------------------------------------
	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject != null)
		{
			if(collision.gameObject.CompareTag("Solid"))
			{
				gameObject.active = false;	
			}
			for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
			{
				if(collision.gameObject == m_Game.getLevel().getPlayer(i).getGameObject())
				{
					gameObject.active = false;
					m_Game.getLevel().getPlayer(i).Die();
				}
			}
		}
	}
}
	