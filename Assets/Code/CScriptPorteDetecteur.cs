using UnityEngine;
using System.Collections;

public class CScriptPorteDetecteur : MonoBehaviour 
{
	CScriptZoneOpenDoor m_ZoneOpenDoor;
	float m_fTimeToTurnLightOff;
	const float m_fTimeToTurnLightOffMax = 0.5f;
	
	// Use this for initialization
	void Start () 
	{
		m_fTimeToTurnLightOff = 0.0f;
		//m_ZoneOpenDoor = gameObject.transform.parent.FindChild<>()
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		if(!m_ZoneOpenDoor.GetPowerStatus() && m_fTimeToTurnLightOff > 0.0f)
			m_ZoneOpenDoor.SetPowerStatus(true);
		
		else if(m_ZoneOpenDoor.GetPowerStatus() && m_fTimeToTurnLightOff < 0.0f)
			m_ZoneOpenDoor.SetPowerStatus(false);
		
		m_fTimeToTurnLightOff -= Time.deltaTime;*/
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "ZoneLight") 
		{	
			m_fTimeToTurnLightOff = m_fTimeToTurnLightOffMax;	
		}
	}
}
