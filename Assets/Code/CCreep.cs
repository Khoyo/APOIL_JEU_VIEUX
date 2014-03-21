using UnityEngine;
using System.Collections;

public class CCreep : MonoBehaviour {

	enum EState
	{
		e_Nothing,
		e_FollowPlayer,
		e_TakePlayer,
		e_OnPlayer,
		e_Sleep,
		e_OnLight
	}
	
	EState m_eState;
	Vector2 m_LastPositionPlayer;
	float m_fVelocity;
	GameObject m_objPlayerParasitized;
	float m_fTimerTakePlayer;
	float m_fTimerSleep;
	float m_fTimerWakeUp;
	bool m_bFollowPlayer;
	bool m_bTakePlayer;
	bool m_bIsOnPlayer;



	// Use this for initialization
	void Start () 
	{
		m_eState = EState.e_Nothing;
		m_LastPositionPlayer = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
		m_fVelocity = CGame.ms_fVelocityCreep;
		m_objPlayerParasitized = null;
		m_fTimerTakePlayer = 0.0f;
		m_fTimerSleep = 0.0f;
		m_fTimerWakeUp = 0.0f;
		m_bIsOnPlayer = false;
		m_bFollowPlayer = false;
		m_bTakePlayer = false;
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log (m_eState);

		switch(m_eState)
		{
			case EState.e_Nothing :
			{
				if(m_bFollowPlayer)
				{
					setState(EState.e_FollowPlayer);
					m_bFollowPlayer = false;
				}
				break;
			}
			case EState.e_FollowPlayer :
			{
				Vector2 position = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
				Vector2 diff = m_LastPositionPlayer - position;
				Vector2 direction = diff.normalized;
				
				gameObject.rigidbody2D.velocity = m_fVelocity * direction;

				if(diff.magnitude < 0.1f)
				{
					setState(EState.e_Nothing);
					gameObject.rigidbody2D.velocity = Vector3.zero;
				}

				if(m_bTakePlayer)
				{
					setState(EState.e_TakePlayer);
					m_fTimerTakePlayer = CGame.ms_fCreepTimerTake;
					m_bTakePlayer = false;
				}

				break;
			}
			case EState.e_TakePlayer :
			{
				if(m_fTimerTakePlayer > 0)
				{
					Vector2 position = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
					Vector2 destination = new Vector2 (m_objPlayerParasitized.transform.position.x, m_objPlayerParasitized.transform.position.y);

					float fX = CApoilMath.InterpolationLinear(CGame.ms_fCreepTimerTake - m_fTimerTakePlayer, 0.0f, CGame.ms_fCreepTimerTake, position.x , destination.x);
					float fY = CApoilMath.InterpolationLinear(CGame.ms_fCreepTimerTake - m_fTimerTakePlayer, 0.0f, CGame.ms_fCreepTimerTake, position.y , destination.y);

					Vector3 newPos = gameObject.transform.position;
					newPos.x = fX;
					newPos.y = fY;
					gameObject.transform.position = newPos;

					m_fTimerTakePlayer -= Time.deltaTime; 
				}
				else
				{
					setState(EState.e_OnPlayer);
					m_fTimerSleep = CGame.ms_fCreepTimerYeute + CGame.ms_fCreepTimerSleep;
				}
				break;
			}
			case EState.e_OnPlayer :
			{
				gameObject.transform.position = m_objPlayerParasitized.transform.position;
				if(m_fTimerSleep > 0.0f)
				{
					if(m_fTimerSleep < CGame.ms_fCreepTimerSleep) //le monstre yeute ailleur
					{
						if(m_bTakePlayer)
						{
							setState(EState.e_TakePlayer);
							m_fTimerTakePlayer = 1.0f;
							m_bTakePlayer = false;
						}
					}
					else //le monstre est bien accroché
					{

					}
					m_fTimerSleep -= Time.deltaTime;
				}
				else
				{
					setState(EState.e_Sleep);
					DropPlayer();
					m_fTimerWakeUp = CGame.ms_fCreepTimerWakeUp;
					gameObject.rigidbody2D.velocity = Vector3.zero;
				}
				break;
			}
			case EState.e_Sleep :
			{
				if(m_fTimerWakeUp > 0.0f)
				{
					m_fTimerWakeUp -= Time.deltaTime;
				}
				else
				{
					setState(EState.e_Nothing);
					
				}
				break;
			}
			case EState.e_OnLight :
			{
				break;
			}
		}

	}

	void setState(EState eState)
	{
		m_eState = eState;

		switch(m_eState)
		{
			case EState.e_Nothing :
			{
				break;
			}
			case EState.e_FollowPlayer :
			{
				break;
			}
			case EState.e_TakePlayer :
			{
				break;
			}
			case EState.e_OnPlayer :
			{
				break;
			}
			case EState.e_Sleep :
			{
				break;
			}
			case EState.e_OnLight :
			{
				break;
			}
		}
	}

	bool canTakePlayer()
	{
		return (m_eState == EState.e_Nothing) || (m_eState == EState.e_FollowPlayer && m_fTimerSleep < CGame.ms_fCreepTimerSleep);
	}

	public void SeePlayer(CPlayer player)
	{
		if(m_eState == EState.e_Nothing)
			m_bFollowPlayer = true;
		m_LastPositionPlayer = player.GetPosition();
	}

	public void StopFollowPlayer()
	{
		setState(EState.e_Nothing);	
	}

	public void TakePlayer(CPlayer player)
	{
		if(canTakePlayer()/* && m_objPlayerParasitized != player.GetGameObject()*/)
		{
			m_bTakePlayer = true;
			m_objPlayerParasitized = player.GetGameObject();
		}
	}

	public void DropPlayer()
	{
		m_objPlayerParasitized = null;
	}

	void OnTriggerStay2D(Collider2D other) 
	{
		if(other.CompareTag("Player"))
		{
			TakePlayer(other.gameObject.GetComponent<CPlayer>());
		}
	}
}
