using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CScriptZoneInGame : MonoBehaviour 
{
	List<GameObject> m_Lights;
	List<CScriptZoneOpenDoor> m_Portes;
	public bool m_bPowerLightOn = true;
	public bool m_bPowerDoorOn = true;
	// Use this for initialization
	void Start () 
	{
		m_Lights = new List<GameObject>();	
		m_Portes = new List<CScriptZoneOpenDoor>();
		SetObjectsInZone();
		TurnLight(m_bPowerLightOn);
		TurnDoor(m_bPowerDoorOn);
	}
	
	// Update is called once per frame
	void Update () 
	{
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
		m_bPowerLightOn = bOn;//pour debuger faudra l'enlever vu qu'on le fait deja dans SetPowerZone
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
	}
	
	public void SwitchPowerStateDoor()
	{
		if(m_bPowerDoorOn)
			TurnDoor(false);
		else
			TurnDoor(true);	
	}
}
