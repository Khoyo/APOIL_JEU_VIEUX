using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CScriptZoneInGame : MonoBehaviour 
{
	List<GameObject> m_Lights;
	List<CScriptZoneOpenDoor> m_Portes;
	List<CScriptButton> m_ButtonsLight;
	List<CScriptButton> m_ButtonsDoor;
	public bool m_bPowerLightOn = true;
	public bool m_bPowerDoorOn = true;
	bool m_bCheckGravityMonster;
	CGame m_Game;
	// Use this for initialization
	void Start () 
	{
		m_Lights = new List<GameObject>();	
		m_Portes = new List<CScriptZoneOpenDoor>();
		m_ButtonsDoor = new List<CScriptButton>();
		m_ButtonsLight = new List<CScriptButton>();
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		SetObjectsInZone();
		TurnLight(m_bPowerLightOn);
		TurnDoor(m_bPowerDoorOn);
		m_bCheckGravityMonster = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bCheckGravityMonster)
		{
			CheckGravityMonster();
			m_bCheckGravityMonster = true;
		}	
	}
	
	public void SetObjectsInZone()
	{
		GameObject[] ShipLight = GameObject.FindGameObjectsWithTag("ShipLight");	
		GameObject[] Portes = GameObject.FindGameObjectsWithTag("Porte");		
		GameObject[] Buttons = GameObject.FindGameObjectsWithTag("ButtonPower");
		
		foreach(GameObject currentLight in ShipLight)
		{
			if(currentLight.transform.parent != null)
			{
				if(currentLight.transform.parent.gameObject == gameObject)
					m_Lights.Add(currentLight);
			}
		}	
		
		foreach(GameObject currentPorte in Portes)
		{
			if(currentPorte.transform.parent != null)
			{
				if(currentPorte.transform.parent.gameObject == gameObject)
					m_Portes.Add(currentPorte.transform.GetComponentInChildren<CScriptZoneOpenDoor>());
			}
		}
		
		foreach(GameObject currentButton in Buttons)
		{
			if(currentButton.transform.parent != null)
			{
				if(currentButton.transform.parent.gameObject == gameObject)
				{
					CScriptButton ScriptButton = currentButton.GetComponent<CScriptButton>();
					if(ScriptButton.m_bIsForLight)
						m_ButtonsLight.Add(ScriptButton);
					else
						m_ButtonsDoor.Add(ScriptButton);
				}
			}
		}
		
	}
	
	public void TurnLight(bool bOn)
	{	
		m_bPowerLightOn = bOn;//pour debuger faudra l'enlever vu qu'on le fait deja dans SetPowerZone
		foreach(GameObject currentLight in m_Lights)
		{
			if(bOn)
			{
				currentLight.SetActiveRecursively(true);
				m_bCheckGravityMonster = true;
				CheckGravityMonster();
			}
			else
			{
				currentLight.SetActiveRecursively(false);
				currentLight.active = true;
				
			}
		}
	}
	
	public void TurnDoor(bool bOn)
	{	
		m_bPowerDoorOn = bOn; //pour debuger faudra l'enlever vu qu'on le fait deja dans SetPowerZone
		foreach(CScriptZoneOpenDoor currentPorte in m_Portes)
		{
			if(currentPorte != null)
				currentPorte.SetPowerStatus(bOn);
		}
	}
	
	public void SwitchPowerStateLight()
	{
		if(m_bPowerLightOn)
			TurnLight(false);
		else
			TurnLight(true);
		
		foreach(CScriptButton currentButton in m_ButtonsLight)
			currentButton.SetSprite();
	}
	
	public void SwitchPowerStateDoor()
	{
		if(m_bPowerDoorOn)
			TurnDoor(false);
		else
			TurnDoor(true);	
		
		foreach(CScriptButton currentButton in m_ButtonsDoor)
			currentButton.SetSprite();
	}
	
	void CheckGravityMonster()
	{	
		GameObject[] pGavityMonsterTab = m_Game.getLevel().GetGavityMonsterTab();
		foreach(GameObject GravityMonster in pGavityMonsterTab)
		{
			if(m_bCheckGravityMonster)
			{
				GravityMonster.collider.isTrigger = true;
			}
			else
				GravityMonster.collider.isTrigger = false;
		}
	}
}
