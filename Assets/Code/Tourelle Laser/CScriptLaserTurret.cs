using UnityEngine;
using System.Collections;

public class CScriptLaserTurret : MonoBehaviour {
	
	CGame m_Game;
	CLaserTurret m_Turret;
	
	// Use this for initialization
	void Start()
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_Game.getLevel().CreateElement<CLaserTurret>(gameObject);
	}
	
	// Update is called once per frame
	void Update() 
	{
	
	}
	
	public void SetTurret(CLaserTurret turret)
	{
		m_Turret=turret;
	}
}
