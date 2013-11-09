using UnityEngine;
using System.Collections;

public class CScriptPont : MonoBehaviour 
{
	public bool m_bIsHole = false;
	public enum EState
	{
		e_NotBroken,
		e_Cracked,
		e_Broken	
	}
	
	CGame m_Game;
	CPont m_Pont;
	EState m_eState;
	float m_fTimerDestruction;
	
	public Material m_material;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_Game.getLevel().CreateElement<CPont>(gameObject);
		if(m_bIsHole)
			m_eState = EState.e_Broken;
		else
			m_eState = EState.e_NotBroken;
		m_fTimerDestruction = 0.0f;
		SetSpriteSheetState();
	}
	
	public void Reset()
	{
		if(m_bIsHole)
			m_eState = EState.e_Broken;
		else
			m_eState = EState.e_NotBroken;
		m_fTimerDestruction = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_eState == EState.e_Cracked)
			if(m_fTimerDestruction < m_Game.m_fTimerDestructionPont)
				m_fTimerDestruction += Time.deltaTime;
			else
			{
				m_eState = EState.e_Broken;
				SetSpriteSheetState();
			}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void SetSpriteSheetState()
	{
		switch(m_eState)
		{
			case EState.e_NotBroken :
			{
				m_Pont.GetSpriteSheet().Reset();
				m_Pont.GetSpriteSheet().SetVibration(false);
				break;
			}
			case EState.e_Cracked :
			{
				m_Pont.GetSpriteSheet().GoToFram(1);
				m_Pont.GetSpriteSheet().SetVibration(true);
				break;
			}
			case EState.e_Broken :
			{
				m_Pont.GetSpriteSheet().ResetAtEnd();
				m_Pont.GetSpriteSheet().SetVibration(false);
				break;
			}
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
 	void OnTriggerEnter(Collider other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{				
				switch(m_eState)
				{
					case EState.e_NotBroken :
					{
						m_eState = EState.e_Cracked;
						break;
					}
					case EState.e_Cracked :
					{
						m_eState = EState.e_Broken;
						break;
					}
				}
				SetSpriteSheetState();
			}
		}
		
	}
	
	void OnTriggerStay(Collider other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject() && m_Game.getLevel().getPlayer(i).IsAlive())
			{
				Vector2 posOfDie = new Vector2(m_Game.getLevel().getPlayer(i).GetGameObject().transform.position.x, m_Game.getLevel().getPlayer(i).GetGameObject().transform.position.y);
				Vector2 posToDie = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
				
				if(m_eState == EState.e_Broken)
				{
					float fDistToBorder1 = (m_Game.getLevel().getPlayer(i).GetGameObject().transform.position - m_Pont.GetSubObject1().transform.position).magnitude;
					float fDistToBorder2 = (m_Game.getLevel().getPlayer(i).GetGameObject().transform.position - m_Pont.GetSubObject2().transform.position).magnitude;
					
					Vector2 posRespawn;
					if(fDistToBorder1 < fDistToBorder2)
						posRespawn = new Vector2(m_Pont.GetSubObject1().transform.position.x, m_Pont.GetSubObject1().transform.position.y);
					else
						posRespawn = new Vector2(m_Pont.GetSubObject2().transform.position.x, m_Pont.GetSubObject2().transform.position.y);
					
					m_Game.getLevel().getPlayer(i).DieFall(posOfDie, posToDie, posRespawn);
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
