using UnityEngine;
using System.Collections;

public class CGravityMonster : MonoBehaviour {

	public enum EState
	{
		e_veille,
		e_alerte,
		e_actif,
		e_mange,
		e_eclaire
	}

	EState m_eState;
	bool m_bAlerte;
	bool m_bChoppe;
	bool m_bEat;
	bool m_bPlayerAreInZone;
	bool m_bDropPlayer;
	bool m_bLight;
	GameObject m_objetPlayerParasitized;
	GameObject m_objetPlayerEat;
	float m_fTimerStopAlerte;
	float m_fTimerStopActif;
	float m_fTimerEat;
	float m_fTimerStopLight;
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () 
	{
		SetState(EState.e_veille);
		m_bAlerte = false;
		m_bChoppe = false;
		m_bPlayerAreInZone = false;
		m_fTimerStopAlerte = 0.0f;
		m_fTimerStopActif = 0.0f;
		m_fTimerStopLight = 0.0f;
		m_bLight = false;
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () 
	{
		switch(m_eState)
		{
			case EState.e_veille :
			{
				if(m_bAlerte)
				{
					SetState(EState.e_alerte);
					m_bAlerte = false;
				}

				if(m_bLight)
				{
					SetState(EState.e_eclaire);
				}
				break;
			}
			case EState.e_alerte :
			{
				if(m_fTimerStopAlerte > 0.0f)
				{
					m_fTimerStopAlerte -= Time.deltaTime;
				}
				else
				{
					SetState(EState.e_veille);
				}

				if(m_bChoppe)
				{
					SetState(EState.e_actif);
					m_bChoppe = false;
					m_fTimerStopActif = CGame.ms_fMonsterTimerStopActif;
				}

				if(m_bLight)
				{
					SetState(EState.e_eclaire);
				}
				break;
			}
			case EState.e_actif :
			{
				if(!m_bPlayerAreInZone)
				{
					if(m_fTimerStopActif > 0.0f)
						m_fTimerStopActif -= Time.deltaTime;

					else
					{
						SetState(EState.e_alerte);
						m_fTimerStopAlerte = CGame.ms_fMonsterTimerStopAlerte;
						m_objetPlayerParasitized.GetComponent<CPlayer>().StopGravity();
					}

				}

				if(m_bEat)
				{
					SetState(EState.e_mange);
					m_objetPlayerParasitized.GetComponent<CPlayer>().StopGravity();
					m_fTimerEat = CGame.ms_fMonsterTimerEat + CGame.ms_fMonsterTimerStopEat;
					m_bEat = false;
					m_bDropPlayer = false;
					
				}

				if(m_bLight)
				{
					SetState(EState.e_eclaire);
					m_fTimerStopAlerte = CGame.ms_fMonsterTimerStopAlerte;
					m_objetPlayerParasitized.GetComponent<CPlayer>().StopGravity();
					m_bDropPlayer = false;
				}

				break;
			}
			case EState.e_mange :
			{
				if(m_fTimerEat > 0.0f)
				{
					if(m_fTimerEat > CGame.ms_fMonsterTimerStopEat) 	//entrain de manger
					{
						
					}
					else 	 											//entrain de digerer
					{
						if(!m_bDropPlayer)
						{
							m_objetPlayerEat.GetComponent<CPlayer>().Respawn();
							m_bDropPlayer = true;
						}
					}
					m_fTimerEat -= Time.deltaTime;
				}
				else
				{
					SetState(EState.e_veille);
				}
				break;
			}
			case EState.e_eclaire :
			{
				if(!m_bLight)
				{
					SetState(EState.e_veille);
				}
				break;
			}
		}

		m_fTimerStopLight += Time.deltaTime;
		if(m_fTimerStopLight > 1.0f)
		{
			m_fTimerStopLight = 0.0f;
			m_bLight = false;
		}
	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void SetState(EState eState)
	{
		m_eState = eState;

		switch(m_eState)
		{
			case EState.e_veille :
			{
				break;
			}
			case EState.e_alerte :
			{
				break;
			}
			case EState.e_actif :
			{
				break;
			}
			case EState.e_mange :
			{
				break;
			}
			case EState.e_eclaire :
			{
				break;
			}
		}
	}

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnTriggerStay2D(Collider2D other) 
	{
		if(other.CompareTag("Player"))
		{
			EatPlayer(other.gameObject);
		}
	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void SeePlayer()
	{
		m_bAlerte = true;
		m_fTimerStopAlerte = CGame.ms_fMonsterTimerStopAlerte;
	}

	public void ChoppePlayer(GameObject objetPlayer)
	{
		if(m_eState == EState.e_alerte)
		{
			m_bChoppe = true;
			m_objetPlayerParasitized = objetPlayer;
			m_objetPlayerParasitized.GetComponent<CPlayer>().SetGravityPosition(new Vector2(gameObject.gameObject.transform.position.x, gameObject.gameObject.transform.position.y));
		}
	}

	void EatPlayer(GameObject objetPlayer)
	{
		if(m_eState == EState.e_actif)
		{
			m_bEat = true;
			m_objetPlayerEat = objetPlayer;
			m_objetPlayerEat.GetComponent<CPlayer>().SetEat();
		}
	}

	public void SetPlayerAreInZone(bool bPlayerAreInZone)
	{
		m_bPlayerAreInZone = bPlayerAreInZone;
	}

	public void CollideWithLight()
	{
		m_bLight = true;
	}
}
