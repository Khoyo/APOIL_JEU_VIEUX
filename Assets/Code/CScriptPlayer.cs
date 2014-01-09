using UnityEngine;
using System.Collections;

public class CScriptPlayer : MonoBehaviour 
{
	
	bool m_bIsOnLight;
	float m_fTimeToTurnLightOff;
	const float m_fTimeToTurnLightOffMax = 0.5f;
	
	// Use this for initialization
	void Start () 
	{
		m_bIsOnLight = false;
		m_fTimeToTurnLightOff = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!m_bIsOnLight && m_fTimeToTurnLightOff > 0.0f)
			m_bIsOnLight = true;
		
		else if(m_bIsOnLight && m_fTimeToTurnLightOff < 0.0f)
			m_bIsOnLight = false;
		
		m_fTimeToTurnLightOff -= Time.deltaTime;
	}
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "player") 
		{	
			Physics.IgnoreCollision(other.collider, collider); 	
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "ZoneLight") 
		{	
			m_fTimeToTurnLightOff = m_fTimeToTurnLightOffMax;	
			Debug.Log("player eclairé");
		}
	}
	
}
