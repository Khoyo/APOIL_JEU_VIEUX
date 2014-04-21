using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CTourelleLaser : MonoBehaviour {
	
	CPlayer m_trackedPlayer;

	enum ETrackingState {
		NotTracking,
		PlayerInTrackingZone,
		PlayerInFiringZone
	};
	ETrackingState m_trackingState;

	public float m_angularSpeed;
	public float m_angularSpeedMax = 2;
	public float m_accelerationFactor;

	public float m_range;
	public float m_enterSoundRange;
	public float m_exitSoundRange;

	public float m_threshold = 1/10;

	public float m_firingWindows;

	// Use this for initialization
	void Start () {
		m_angularSpeed = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if(m_trackingState != ETrackingState.NotTracking) {
			Vector2 diff = m_trackedPlayer.transform.position - transform.position;

			float destAngle = Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) + 180;
			float sourceAngle = transform.rotation.eulerAngles.z;

			double crossProductSign = Vector3.Cross(transform.right * -1 /*"forward" of the object"*/, diff).z;

			float dist = Mathf.Abs(destAngle - sourceAngle);

			if((m_angularSpeed / dist) > m_threshold) {
					m_angularSpeed = m_threshold * dist;
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
		if(m_trackingState == ETrackingState.NotTracking){
			DetectPlayer();
		}
		else { //Check if the player is still in range
			float dist = (m_trackedPlayer.transform.position - transform.position).magnitude;
			if(dist > m_exitSoundRange)
			{
				DetectPlayer();
			}
		}
	}
	
	public void DetectPlayer()
	{
		float minDistance = Mathf.Infinity;
		CPlayer nearestPlayer = null;
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for(int i = 0; i < CGame.ms_nNbPlayer; i++){
			CPlayer player = players[i].GetComponent<CPlayer>();
			float dist = (player.transform.position - transform.position).magnitude;
			if(dist < minDistance){
				minDistance = dist;
				nearestPlayer = player;
			}
		}

		ProcessMinimalDistance(minDistance, nearestPlayer);

	}

	void StopTracking()
	{

	}

	void StartTracking(CPlayer player)
	{
		if(m_trackingState == ETrackingState.NotTracking)
			CSoundEngine.postEvent("TourelleZoneOn", gameObject);
		m_trackedPlayer = player;
	}
	
	//Get the nearest or tracked player distance and change tracking state if necessary
	void ProcessMinimalDistance(float minDistance, CPlayer nearestPlayer)
	{
		if(minDistance > m_exitSoundRange) //Out of sight
		{
			if(m_trackingState != ETrackingState.NotTracking)
			{
				CSoundEngine.postEvent("TourelleZoneOff", gameObject);
			}
			m_trackedPlayer = null;
			m_trackingState = ETrackingState.NotTracking;
		}
		else if(minDistance > m_enterSoundRange) //Player in outward zone
		{
			//We don't do anything here...
		}
		else if(minDistance > m_range) //Player in inward zone
		{
			StartTracking(nearestPlayer);

			m_trackingState = ETrackingState.PlayerInTrackingZone;
		}
		else //Player in firing range
		{
			StartTracking(nearestPlayer);

			m_trackingState = ETrackingState.PlayerInFiringZone;
		}
	}

	void OnDrawGizmos/*Selected*/() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, m_range);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, m_enterSoundRange);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, m_exitSoundRange);
		if(m_trackedPlayer)
			Gizmos.DrawRay(transform.position, m_trackedPlayer.transform.position);
	}

}

