using UnityEngine;
using System.Collections;

public class CScriptZoneOpenDoor : MonoBehaviour 
{
	bool m_bNoPlayerInZone;
	bool m_bPowerOn; // si la zone est allimenté en energie
	bool m_bIsOnLight;
	bool m_bIsOnLightOfPlayer;
	float m_fTimerClose;
	
	CGame m_Game;
	// Use this for initialization
	void Start () 
	{
		m_bNoPlayerInZone = true;
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_bPowerOn = true;
		m_bIsOnLight = true;
		m_bIsOnLightOfPlayer = false;
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
		
		if(m_Game.getLevel() != null)
		{
			m_bNoPlayerInZone = true;
			
			for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
			{	
				m_bIsOnLightOfPlayer = m_Game.getLevel().getPlayer(i).TorchlightCollideWithElement(gameObject.transform.parent.gameObject.transform.FindChild("Detecteur").position);		
				
				if(bIsOnLight || m_bIsOnLightOfPlayer)
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
	
	public bool GetPowerStatus()
	{
		return m_bPowerOn;	
	}
	
	public bool DetecteurIsOn()
	{
		return (m_bPowerOn || m_bIsOnLightOfPlayer);
	}
}
