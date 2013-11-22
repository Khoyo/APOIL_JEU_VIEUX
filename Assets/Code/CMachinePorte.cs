using UnityEngine;
using System.Collections;

public class CMachinePorte : MonoBehaviour, IMachineAction 
{	
	bool m_bIsOpen;			// statut en cours	
	bool m_bTargetIsOpen;	// statut vers lequel tend la porte
	bool m_bBlockClose;
	bool m_bForceBlockClose; //quand le player essaye de fermer mais qu'il devrait pas pouvoir
	bool m_bChangeState;
	float m_fTimer;
	const float m_fTimerMax = 3.0f;
	CGame m_Game;
	
	public void Activate(CPlayer player)
	{
		if(!m_bIsOpen)
			Open();
		else if(!m_bForceBlockClose)
			Close();
	}
	
	public void Init()
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_bIsOpen = false;
		m_bChangeState = false;
		m_bTargetIsOpen = false;
		m_bBlockClose = false;
		m_bForceBlockClose = false;
		gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().setEndCondition(CSpriteSheet.EEndCondition.e_Stop);
		gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().AnimationStop();
		m_fTimer = 0.0f;
	}
	
	public void Process()
	{
		if(gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().IsEnd() && m_bChangeState)
		{
			m_bIsOpen = m_bTargetIsOpen;	
			m_bChangeState = false;
		}
		
		gameObject.collider.isTrigger = m_bIsOpen;
		
		if(m_bIsOpen && !m_bChangeState)
		{
			if(m_fTimer < m_fTimerMax)
				m_fTimer += Time.deltaTime;
			else
				Close();
		}
		
		if(m_bTargetIsOpen)	
			gameObject.transform.FindChild("Detecteur").FindChild("light").light.enabled = true;
		else
			gameObject.transform.FindChild("Detecteur").FindChild("light").light.enabled = false;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void BlockClose()
	{
		m_bBlockClose = true;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void StopBlockClose()
	{
		m_bBlockClose = false;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void Open()
	{
		if(!m_bTargetIsOpen)
		{
			gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().Reset();	
			gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().SetDirection(true);
			m_bChangeState = true;
			gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().AnimationStart();	
			m_fTimer = 0.0f;
			m_bTargetIsOpen = true;
		}
	}
	
	public void Close()
	{
		if(m_bTargetIsOpen && !m_bBlockClose)
		{
			gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().ResetAtEnd();
			gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().SetDirection(false);
			m_bChangeState = true;
			gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().AnimationStart();	
			m_fTimer = 0.0f;
			m_bTargetIsOpen = false;
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void OnTriggerStay(Collider other) 
	{
		bool bForce = false;
		
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				BlockClose();
				bForce = true;
			}	
		}
		
		m_bForceBlockClose = bForce;
	}
	
}
