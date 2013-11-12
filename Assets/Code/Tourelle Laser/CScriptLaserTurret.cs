using UnityEngine;
using System.Collections;

public class CScriptLaserTurret : MonoBehaviour {
	
	public float m_fReloadTime = 2f;
	public float m_fShotSpeed = 500;
	public float m_fTimer = 200;
	
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
