using UnityEngine;
using System.Collections;

public class CMachineChargeurLampeTorche : MonoBehaviour, IMachineAction {

	public float m_chargeRate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Activate(CPlayer player)
	{
	}

	public void ActivateContinuous(CPlayer player)
	{
		player.RechargeLight(m_chargeRate * Time.deltaTime);
	}
}
