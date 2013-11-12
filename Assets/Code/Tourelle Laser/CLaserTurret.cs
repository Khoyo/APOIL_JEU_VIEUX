using UnityEngine;
using System.Collections;

public class CLaserTurret : CElement
{	
	GameObject m_Base, m_LightZone, m_DarkZone;
	GameObject tracked = null;
	CScriptLaserTurret m_script;
	float m_fLastFired = 0f;
	
	public override void Init()
	{
		base.Init();
		m_script =	m_GameObject.GetComponent<CScriptLaserTurret>();
		m_script.SetTurret(this);
		
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
		
		if(tracked != null && Time.time>(m_fLastFired+m_script.m_fReloadTime))
		{
			//First, fire in it's direction ^^
			CScie saw = new CScie();
			saw.Init();
			saw.GetGameObject().active = true;
			saw.GetGameObject().transform.position = m_GameObject.transform.position; // + new Vector3(0,0, 10);
			saw.GetGameObject().rigidbody.velocity = (tracked.transform.position - m_GameObject.transform.position).normalized * m_script.m_fShotSpeed;
			saw.useTimer = true;
			saw.m_fTimer = m_script.m_fTimer;
			saw.m_fFiredTime = m_fLastFired = Time.time;
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
