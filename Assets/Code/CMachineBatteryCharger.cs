using UnityEngine;
using System.Collections;

public class CMachineBatteryCharger : MonoBehaviour, IMachineAction {

	// Use this for initialization
	public void Init() {
	
	}
	
	// Update is called once per frame
	public void Process ()
	{
		
	}
	
	public void Activate (CPlayer player)
	{
	}
	
	public void ActivateContinuous(CPlayer player)
	{
		CTakeElement takeElement = player.GetHeldElement();
		CScriptBattery battery;
		if((battery=takeElement.GetGameObject().GetComponent<CScriptBattery>() )!= null)
		{
			battery.m_fChargeLevel++;
		}
	}
}