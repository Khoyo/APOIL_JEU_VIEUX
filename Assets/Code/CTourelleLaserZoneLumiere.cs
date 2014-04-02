using UnityEngine;
using System.Collections;

public class CTourelleLaserZoneLumiere : MonoBehaviour {

	void OnTriggerStay2D(Collider2D other)
	{
		CPlayer player = other.GetComponent<CPlayer>();
		
		if(player != null)
		{
			transform.parent.GetComponent<CTourelleLaser>().DetectedPlayer(player);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		CPlayer player = other.GetComponent<CPlayer>();
		
		if(player != null)
		{
			transform.parent.GetComponent<CTourelleLaser>().PlayerEnter(player);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		CPlayer player = other.GetComponent<CPlayer>();
		
		if(player != null)
		{
			transform.parent.GetComponent<CTourelleLaser>().PlayerExit(player);
		}
	}
}
