using UnityEngine;
using System.Collections;

public class CScriptTakeElement : MonoBehaviour 
{
	CGame m_Game;
	CTakeElement m_TakeElement;
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_Game.getLevel().CreateElement<CTakeElement>(gameObject);
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () 
	{
		if(CApoilInput.InputPlayer1.DropObject)
		{
			m_Game.getLevel().getPlayer(0).DropElement();
		}
		if(CApoilInput.InputPlayer2.DropObject)
		{
			m_Game.getLevel().getPlayer(1).DropElement();
		}
		if(CApoilInput.InputPlayer3.DropObject)
		{
			m_Game.getLevel().getPlayer(2).DropElement();
		}
		if(CApoilInput.InputPlayer4.DropObject)
		{
			m_Game.getLevel().getPlayer(3).DropElement();
		}
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnTriggerStay(Collider other)
	{	
		if(m_Game != null)
		{
			// ramasser un objet
			if(m_Game.m_nNbPlayer > 0 && other.gameObject == m_Game.getLevel().getPlayer(0).GetGameObject() && CApoilInput.InputPlayer1.PickUpObject)	
			{
				m_Game.getLevel().getPlayer(0).PickUpObject(m_TakeElement);
			}
			if(m_Game.m_nNbPlayer > 1 && other.gameObject == m_Game.getLevel().getPlayer(1).GetGameObject() && CApoilInput.InputPlayer2.PickUpObject)	
			{
				m_Game.getLevel().getPlayer(1).PickUpObject(m_TakeElement);
			}
			if(m_Game.m_nNbPlayer > 2 && other.gameObject == m_Game.getLevel().getPlayer(2).GetGameObject() && CApoilInput.InputPlayer3.PickUpObject)	
			{
				m_Game.getLevel().getPlayer(2).PickUpObject(m_TakeElement);
			}
			if(m_Game.m_nNbPlayer > 3 && other.gameObject == m_Game.getLevel().getPlayer(3).GetGameObject() && CApoilInput.InputPlayer4.PickUpObject)	
			{
				m_Game.getLevel().getPlayer(3).PickUpObject(m_TakeElement);
			}
		}

	}
	
	public CTakeElement GetTakeElement()
	{
		return m_TakeElement;
	}

	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetTakeElement(CTakeElement obj)
	{
		m_TakeElement = obj;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CTakeElement.ETypeObject GetTypeElement()
	{
		CTakeElement.ETypeObject type = CTakeElement.ETypeObject.e_TypeObject_NoTakeElement;
		switch(gameObject.name)
		{
			case "Batterie||Batterie(Clone)" :
			{
				type = CTakeElement.ETypeObject.e_TypeObject_Battery;
				break;	
			}
		}
		
		return type;
	}
}
