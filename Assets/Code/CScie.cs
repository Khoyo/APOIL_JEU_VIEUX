using UnityEngine;
using System.Collections;

public class CScie : CElement {
	
	CGame m_Game;
	
	public int m_number;
	
	public override void Init()
	{
		base.Init();
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_GameObject = GameObject.Instantiate(m_Game.m_PrefabScie) as GameObject;
		m_GameObject.GetComponent<CScriptScie>().SetScie(this);
		
	}
	
	
	
}
