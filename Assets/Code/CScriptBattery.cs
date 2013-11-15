 using UnityEngine;
using System.Collections;

public class CScriptBattery : MonoBehaviour {
	
	public float m_fChargeLevel = 100; //In percent
	float m_fTimeToDischarge;
	CGame m_Game;
	
	// Use this for initialization
	void Start () {
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_fTimeToDischarge = m_Game.m_fTimeToDischargeInSec;
	}
	
	// Update is called once per frame
	public void UseBattery() {
		m_fChargeLevel -= Time.deltaTime*100/m_fTimeToDischarge;
	}
}



