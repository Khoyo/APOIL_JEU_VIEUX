using UnityEngine;
using System.Collections;

public class CTourelleLaserZoneLumiere : MonoBehaviour {

	void OnTriggerStay2D(Collider2D other)
	{
		CPlayer player = other.GetComponent<CPlayer>();
		
		if(player != null)
		{
			transform.parent.GetComponent<CTourelleLaser>().PlayerInLightZone(player);
		}
	}
}
