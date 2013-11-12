using UnityEngine;
using System.Collections;

public class CScie : CElement 
{
	public int m_number;
	public bool useTimer = false;
	public float m_fTimer = -1f;
	
	public float m_fFiredTime = 0f;
	
	public override void Init()
	{
		base.Init();
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_GameObject = GameObject.Instantiate(m_Game.m_PrefabScie) as GameObject;
		m_GameObject.GetComponent<CScriptScie>().SetScie(this);
		
		m_Game.getLevel().RegisterElement(this);		
	}
	
	public override void Process(float fDeltatime)
	{
		base.Process(fDeltatime);
		if(useTimer)
		{
			if(m_fFiredTime+m_fTimer < Time.time)
				m_GameObject.active = false;
		}
	}
	
	public override void Destroy()
	{
		base.Destroy();
		m_Game.getLevel().UnregisterElement(this);
	}
	
}
