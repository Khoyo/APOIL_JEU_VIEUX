using UnityEngine;
using System.Collections;

public class CTourelleLaser : MonoBehaviour {

	bool m_detectedThisFrame;
	float m_minDistance;
	CPlayer m_nearestPlayer;

	// Use this for initialization
	void Start () {
		ClearGun();
	}
	
	// Update is called once per frame
	void Update () {

		if(m_detectedThisFrame)
		{
			//Rotate towar target
			Vector2 diff = m_nearestPlayer.transform.position - transform.position;
			float destAngle = Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) +180;
			float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, destAngle, 0.1f);

			//Debug.Log(angle-transform.rotation.eulerAngles.z);
			transform.rotation = Quaternion.Euler(0, 0, angle);
		}

		ClearGun();
	}

	public void PlayerInDarkZone(CPlayer player)
	{
		DetectedPlayer(player);
	}

	public void PlayerInLightZone(CPlayer player)
	{
		if(true /*TODO : check if player is lighted*/){
			DetectedPlayer(player);
		}
	}

	void DetectedPlayer(CPlayer player)
	{
		Debug.Log("Detected "+player.name); 
		if((player.transform.position - transform.position).magnitude < m_minDistance) {
			m_detectedThisFrame = true;
			m_nearestPlayer = player;
			m_minDistance = (player.transform.position - transform.position).magnitude;
			Debug.Log("Locked "+player.name);
		}
	}

	void ClearGun()
	{
		m_detectedThisFrame = false;
		m_minDistance = Mathf.Infinity;
		m_nearestPlayer = null;
	}
}
