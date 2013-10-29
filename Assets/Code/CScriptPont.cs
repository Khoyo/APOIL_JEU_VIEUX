using UnityEngine;
using System.Collections;

public class CScriptPont : MonoBehaviour 
{
	public enum EState
	{
		e_NotBroken,
		e_Cracked,
		e_Broken	
	}
	
	CGame m_Game;
	CPont m_Pont;
	EState m_eState;
	
	public Material m_material;
	
	// Use this for initialization
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_Game.getLevel().CreateElement<CPont>(gameObject);
		m_eState = EState.e_NotBroken;
	}
	
	public void Reset()
	{
		m_eState = EState.e_NotBroken;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_eState == EState.e_Broken)
			m_Pont.GetSpriteSheet().SetVibration(false);
	}
	
 	void OnTriggerEnter(Collider other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).getGameObject())
			{
				m_Pont.GetSpriteSheet().GoToNextFram();
				switch(m_eState)
				{
					case EState.e_NotBroken :
					{
						m_eState = EState.e_Cracked;
						m_Pont.GetSpriteSheet().SetVibration(true);
						break;
					}
					case EState.e_Cracked :
					{
						m_eState = EState.e_Broken;
						break;
					}
				}
			}
		}
		
	}
	
	void OnTriggerStay(Collider other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).getGameObject())
			{
				Vector2 posOfDie = new Vector2(m_Game.getLevel().getPlayer(i).getGameObject().transform.position.x, m_Game.getLevel().getPlayer(i).getGameObject().transform.position.y);
				Vector2 posToDie = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

				if(m_eState == EState.e_Broken)
				{
					CPlayer player = m_Game.getLevel().getPlayer(i);
					m_Game.getLevel().getPlayer(i).DieFall(posOfDie, posToDie);
				}
			}
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetPontElement(CPont obj)
	{
		m_Pont = obj;
	}
	
	public Material GetMaterial()
	{
		return m_material;	
	}
	
	public void SetState(EState eState)
	{
		m_eState = eState;	
	}
	
	public EState GetState()
	{
		return m_eState;	
	}
}
