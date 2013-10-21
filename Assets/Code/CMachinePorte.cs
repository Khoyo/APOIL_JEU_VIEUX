using UnityEngine;
using System.Collections;

public class CMachinePorte : MonoBehaviour, IMachineAction 
{	
	bool m_bIsOpen;
	bool m_bChangeState;
	
	public void Activate(CPlayer player)
	{
		if(!m_bIsOpen)
		{
			gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().Reset();	
			gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().SetDirection(true);
		}
		else
		{
			//Vector3 posDeplacement = gameObject.transform.position - player.getGameObject().transform.position;
			//player.getGameObject().transform.Translate(new Vector3(posDeplacement.x, 0, posDeplacement.y));
			gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().ResetAtEnd();
			gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().SetDirection(false);
		}
		
		gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().AnimationStart();
		m_bChangeState = true;
	}
	
	public void Init()
	{
		m_bIsOpen = false;
		m_bChangeState = false;
		gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().setEndCondition(CSpriteSheet.EEndCondition.e_Stop);
		gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().AnimationStop();
	}
	
	public void Process()
	{
		if(gameObject.GetComponent<CScriptMachine>().GetMachine().GetSpriteSheet().IsEnd() && m_bChangeState)
		{
			m_bIsOpen = !m_bIsOpen;	
			m_bChangeState = false;
		}
		gameObject.collider.isTrigger = m_bIsOpen;
	}
}
