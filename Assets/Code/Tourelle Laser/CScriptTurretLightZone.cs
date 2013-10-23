using UnityEngine;
using System.Collections;

public class CScriptTurretLightZone : MonoBehaviour {
	
	CLaserTurret m_Turret;
	
	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
		
	public void OnTriggerEnter(Collider c)
	{
		m_Turret.OnLightEnter(c);
	}
	
	public void OnTriggerExit(Collider c)
	{
		m_Turret.OnLightExit(c);
	}
	
	public void SetTurret(CLaserTurret turret)
	{
		m_Turret=turret;
	}
}
