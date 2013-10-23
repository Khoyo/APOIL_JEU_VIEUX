using UnityEngine;
using System.Collections;

public class CScriptTurretDarkZone : MonoBehaviour {
	
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
		m_Turret.OnDarkEnter(c);
	}
	
	public void OnTriggerExit(Collider c)
	{
		m_Turret.OnDarkExit(c);
	}
	
	public void SetTurret(CLaserTurret turret)
	{
		m_Turret=turret;
	}
}
