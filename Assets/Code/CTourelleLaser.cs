using UnityEngine;
using System.Collections;

public class CTourelleLaser : MonoBehaviour {
	
	bool m_detectedThisFrame;
	public int m_frameSinceLastDetection;
	public int m_frameSinceNotDetected;

	CPlayer m_trackedPlayer;
	bool m_tracking;

	public float m_angularSpeed;
	public float m_angularSpeedMax = 2;
	public float m_accelerationFactor;

	public float m_range;

	public float threshold = 1/10;

	// Use this for initialization
	void Start () {
		m_angularSpeed = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if(m_tracking) {
			Vector2 diff = m_trackedPlayer.transform.position - transform.position;

			float destAngle = Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) + 180;
			float sourceAngle = transform.rotation.eulerAngles.z;

			double crossProductSign = Vector3.Cross(transform.right * -1 /*"forward" of the object"*/, diff).z;

			float dist = Mathf.Abs(destAngle - sourceAngle);

			if((m_angularSpeed / dist) > threshold) {
					m_angularSpeed = threshold * dist;
			} else
					m_angularSpeed += m_accelerationFactor;

			float rotationAngle = Mathf.Min(m_angularSpeedMax, Mathf.Min(m_angularSpeed, dist));

			m_angularSpeed = rotationAngle;

			if(crossProductSign < 0)
					rotationAngle *= -1;

			//Debug.Log(Mathf.Abs(destAngle- sourceAngle));

			//angle *= angleDiff>180 ? -1 : 1;

			//angle += transform.rotation.eulerAngles.z;

			transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + rotationAngle);

			CSoundEngine.setRTPC("Engine_Speed", m_angularSpeed / m_angularSpeedMax, gameObject);
		}


		//Update tracking info
		if(!m_tracking){
			DetectPlayer();
			if(m_tracking)
				StartTracking();
		}
		else { //Check if the player is still in range
			float dist = (m_trackedPlayer.transform.position - transform.position).magnitude;
			if(dist > m_range)
			{
				m_trackedPlayer = null;
				m_tracking = false;
				DetectPlayer();
				if(!m_tracking)
					StopTracking();
			}
		}
	}
	
	public void DetectPlayer()
	{
		float minDistance = Mathf.Infinity;
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for(int i = 0; i < CGame.ms_nNbPlayer; i++){
			CPlayer player = players[i].GetComponent<CPlayer>();
			float dist = (player.transform.position - transform.position).magnitude;
			if(dist < minDistance && dist < m_range){
				m_trackedPlayer = player;
				minDistance = dist;
				m_tracking = true;
			}
		}
	}
	
	public void StartTracking()
	{
		CSoundEngine.postEvent("TourelleZoneOn", gameObject);
	}
	
	public void StopTracking()
	{
		CSoundEngine.postEvent("TourelleZoneOff", gameObject);
	}


	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, m_range);
		if(m_trackedPlayer)
			Gizmos.DrawRay(transform.position, m_trackedPlayer.transform.position);
	}

}

