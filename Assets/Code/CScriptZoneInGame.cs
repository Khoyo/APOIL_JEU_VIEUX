using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CScriptZoneInGame : MonoBehaviour 
{
	List<GameObject> m_Lights;
	bool m_bNoPower;
	// Use this for initialization
	void Start () 
	{
		m_Lights = new List<GameObject>();	
		SetObjectsInZone();
		m_bNoPower = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(CApoilInput.DebugF10)
		{
			if(!m_bNoPower)
				TurnLight(false);
			else
				TurnLight(true);
		}
	}
	
	public void SetObjectsInZone()
	{
		GameObject[] ShipLight;
		ShipLight = GameObject.FindGameObjectsWithTag("ShipLight");
		
		foreach(GameObject currentLight in ShipLight)
		{
			GameObject papa = currentLight.transform.parent.gameObject;
			int k = 0;
			
			if(currentLight.transform.parent != null)
			{
				if(currentLight.transform.parent.gameObject == gameObject)
					m_Lights.Add(currentLight);
			}
		}	
	}
	
	public void TurnLight(bool bOn)
	{	
		m_bNoPower = !bOn;
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
}
