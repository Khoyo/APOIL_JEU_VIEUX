using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CScriptZoneInGame : MonoBehaviour 
{
	List<GameObject> m_Lights;
	List<CScriptZoneOpenDoor> m_Portes;
	public bool m_bPowerOn = true;
	// Use this for initialization
	void Start () 
	{
		m_Lights = new List<GameObject>();	
		m_Portes = new List<CScriptZoneOpenDoor>();
		SetObjectsInZone();
		SetPowerZone(m_bPowerOn);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(CApoilInput.DebugF10)
		{
			SwitchPowerState();
		}
	}
	
	public void SetObjectsInZone()
	{
		GameObject[] ShipLight = GameObject.FindGameObjectsWithTag("ShipLight");	
		GameObject[] Portes = GameObject.FindGameObjectsWithTag("Porte");
		
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
	}
	
	public void TurnLight(bool bOn)
	{	
		m_bPowerOn = bOn;//pour debuger faudra l'enlever vu qu'on le fait deja dans SetPowerZone
		foreach(GameObject currentLight in m_Lights)
		{
			if(bOn)
			{
				currentLight.SetActiveRecursively(true);
			}
			else
			{
				currentLight.SetActiveRecursively(false);
				currentLight.active = true;
			}
		}
	}
	
	public void TurnDoors(bool bOn)
	{	
		m_bPowerOn = bOn; //pour debuger faudra l'enlever vu qu'on le fait deja dans SetPowerZone
		foreach(CScriptZoneOpenDoor currentPorte in m_Portes)
		{
			if(currentPorte != null)
				currentPorte.SetPowerStatus(bOn);
		}
	}
		
	public void SetPowerZone(bool bOn)
	{
		m_bPowerOn = bOn;
		TurnLight(bOn);
		TurnDoors(bOn);
	}
	
	public void SwitchPowerState()
	{
		if(m_bPowerOn)
			SetPowerZone(false);
		else
			SetPowerZone(true);	
	}
}
