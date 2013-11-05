using UnityEngine;
using System.Collections;

public class CScriptZoneOpenDoor : MonoBehaviour 
{
	bool m_bOpen;
	bool m_bNoPlayerInZone;
	
	CGame m_Game;
	// Use this for initialization
	void Start () {
		m_bOpen = false;
		m_bNoPlayerInZone = true;
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bNoPlayerInZone)
			gameObject.transform.parent.gameObject.GetComponent<CMachinePorte>().StopBlockClose();
	}
	
	void OnTriggerStay(Collider other)
	{
		bool bCanOpen = true;	
		m_bNoPlayerInZone = true;
		float fAngle = 75.0f;
		
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(!m_Game.LightIsOn())
			{
				Vector3 PosPlayer = m_Game.getLevel().getPlayer(i).GetGameObject().transform.position;
				Vector3 DirectionRegard = m_Game.getLevel().getPlayer(i).GetDirectionRegard();
				Vector3 DirectionDetecteur = gameObject.transform.parent.gameObject.transform.FindChild("Detecteur").position - PosPlayer;
				
				float fDotProduct = Vector3.Dot(DirectionRegard.normalized, DirectionDetecteur.normalized);
				
				if(fDotProduct < Mathf.Cos(CApoilMath.ConvertDegreeToRadian((fAngle/2.0f))))
				{
					bCanOpen = false;
				}
			}
			
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject() && bCanOpen)
			{
				gameObject.transform.parent.gameObject.GetComponent<CMachinePorte>().Open();	
				gameObject.transform.parent.gameObject.GetComponent<CMachinePorte>().BlockClose();
				m_bNoPlayerInZone = false;
			}
		}
	}
}
