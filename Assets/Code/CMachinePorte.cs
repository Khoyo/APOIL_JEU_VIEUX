using UnityEngine;
using System.Collections;

public class CMachinePorte : MonoBehaviour, IMachineAction 
{	
	bool m_bIsOpen;			// statut en cours	
	bool m_bTargetIsOpen;	// statut vers lequel tend la porte
	bool m_bBlockClose;
	bool m_bChangeState;
	float m_fTimer;
	const float m_fTimerMax = 3.0f;
	
	public void Activate(CPlayer player)
	{
		if(!m_bIsOpen)
			Open();
		else
			Close();
	}
	
	public void Init()
	{
		m_bIsOpen = false;
		m_bChangeState = false;
		m_bTargetIsOpen = false;
		m_bBlockClose = false;
		CSpriteSheet sprite = gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet();
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
}
