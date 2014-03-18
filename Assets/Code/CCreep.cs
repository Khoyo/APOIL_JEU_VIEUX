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

	// Use this for initialization
	void Start () 
	{
		m_eState = EState.e_Nothing;
		m_LastPositionPlayer = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
		m_fVelocity = CGame.ms_fVelocityCreep;
	}
	
	// Update is called once per frame
	void Update () {
		switch(m_eState)
		{
			case EState.e_Nothing :
			{
				
				break;
			}
			case EState.e_FollowPlayer :
			{
				Vector2 position = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
				Vector2 direction = (m_LastPositionPlayer - position).normalized;
				
				gameObject.rigidbody2D.velocity = m_fVelocity * direction;
			
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

	public void SeePlayer(CPlayer player)
	{
		m_LastPositionPlayer = player.GetPosition();
		m_eState = EState.e_FollowPlayer;
	}
}
