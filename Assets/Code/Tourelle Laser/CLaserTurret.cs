using UnityEngine;
using System.Collections;

public class CLaserTurret : CElement
{
	GameObject m_Base, m_LightZone, m_DarkZone;
	GameObject tracked = null;
	float m_fLastFired = 0f;
	public override void Init()
	{
		base.Init();
		m_GameObject.GetComponent<CScriptLaserTurret>().SetTurret(this);
		
		CScriptTurretLightZone light = m_GameObject.GetComponentInChildren<CScriptTurretLightZone>();
		m_LightZone = light.gameObject;
		light.SetTurret(this);
		
		CScriptTurretDarkZone dark = m_GameObject.GetComponentInChildren<CScriptTurretDarkZone>();
		m_DarkZone = dark.gameObject;
		dark.SetTurret(this);
		
	}
	
	public override void Process(float fDeltatime)
	{
		base.Process(fDeltatime);
		
		if(tracked != null && Time.time>(m_fLastFired+2f))
		{
			//First, fire in it's direction ^^
			CScie saw = new CScie();
			saw.Init();
			saw.GetGameObject().active = true;
			saw.GetGameObject().transform.position = m_GameObject.transform.position;
			saw.GetGameObject().rigidbody.velocity = (tracked.transform.position - m_GameObject.transform.position)*2 ;  
			m_fLastFired = Time.time;
		}
		
	}
	
	public void OnLightEnter(Collider other)
	{
		if(tracked != null)
			return; //We don't change target while tracking
		
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject ==  m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				tracked = other.gameObject;
				Debug.Log ("Started tracking");
			}
		}
	}
	
	public void OnDarkEnter(Collider other)
	{
	
	}
	
	public void OnDarkExit(Collider other)
	{
	}
	
	public void OnLightExit(Collider other)
	{
		if(other.gameObject == tracked)
			tracked = null;
	}
	
}
