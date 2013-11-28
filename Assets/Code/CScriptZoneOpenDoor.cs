using UnityEngine;
using System.Collections;

public class CScriptZoneOpenDoor : MonoBehaviour 
{
	bool m_bNoPlayerInZone;
	bool m_bPowerOn; // si la zone est allimenté en energie
	bool m_bIsOnLight;
	float m_fTimerClose;
	
	CGame m_Game;
	// Use this for initialization
	void Start () 
	{
		m_bNoPlayerInZone = true;
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_bPowerOn = true;
		m_bIsOnLight = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bNoPlayerInZone)
		{
			if(m_fTimerClose < 0.0f)
				gameObject.transform.parent.gameObject.GetComponent<CMachinePorte>().StopBlockClose();
			else
				m_fTimerClose -= Time.deltaTime;
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		bool bCanOpen = false;	
		bool bIsOnLight = m_bIsOnLight;
		bool bIsOnLightOfPlayer = true;
		float fAngle = 75.0f;
		
		if(m_Game.getLevel() != null)
		{
			m_bNoPlayerInZone = true;
			
			for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
			{
				if(!m_bIsOnLight)
				{		
					if(m_Game.getLevel().getPlayer(i).HaveTorche())
					{
						Vector3 PosPlayer = m_Game.getLevel().getPlayer(i).GetGameObject().transform.position;
						Vector3 DirectionRegard = m_Game.getLevel().getPlayer(i).GetDirectionRegard();
						Vector3 DirectionDetecteur = gameObject.transform.parent.gameObject.transform.FindChild("Detecteur").position - PosPlayer;
						
						float fDotProduct = Vector3.Dot(DirectionRegard.normalized, DirectionDetecteur.normalized);
						
						if(fDotProduct < Mathf.Cos(CApoilMath.ConvertDegreeToRadian((fAngle/2.0f))))
						{
							bIsOnLightOfPlayer = false;
						}	
					}	
					else
						bIsOnLightOfPlayer = false;
				}
				else
					bIsOnLightOfPlayer = false;
				
				if(bIsOnLight || bIsOnLightOfPlayer)
					bCanOpen = true;

			
				if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
				{
					m_bNoPlayerInZone = false;
					m_fTimerClose = 1.0f;
					//gameObject.transform.parent.gameObject.GetComponent<CMachinePorte>().BlockClose();
					if(bCanOpen && m_bPowerOn)
					{
						gameObject.transform.parent.gameObject.GetComponent<CMachinePorte>().Open();	
						gameObject.transform.parent.gameObject.GetComponent<CMachinePorte>().BlockClose();	
					}
				}
			}
		}
	}
	
	public void SetPowerStatus(bool bOn)
	{
		m_bPowerOn = bOn;	
	}
}
