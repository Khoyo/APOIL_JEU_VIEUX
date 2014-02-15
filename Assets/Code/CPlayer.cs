using UnityEngine;
using System.Collections;

public class CPlayer : MonoBehaviour {

	public enum EIdPlayer 
	{
		e_IdPlayer_Player1,
		e_IdPlayer_Player2,
		e_IdPlayer_Player3,
		e_IdPlayer_Player4,
	}

	EIdPlayer m_eIdPlayer;
	SPlayerInput m_PlayerInput;
	Vector2 m_PositionInit;
	Vector2 m_Direction;

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () 
	{
		SetPlayerInput ();
		m_PositionInit = new Vector2 (0, 0);
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () 
	{
		SetPlayerInput ();
		Move ();
	
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void Move()
	{
		if(gameObject.rigidbody2D != null)
		{
			Vector2 velocity = Vector2.zero;
			velocity = new Vector2(m_PlayerInput.MoveHorizontal, m_PlayerInput.MoveVertical);

			m_Direction = new Vector2(m_PlayerInput.DirectionHorizontal, m_PlayerInput.DirectionVertical);
			Debug.DrawRay(transform.position, 2 * new Vector3(m_Direction.x, m_Direction.y, 0));

			gameObject.rigidbody2D.velocity = CGame.ms_fVelocityPlayer * velocity;
		}
	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	void SetPlayerInput()
	{
		int idPlayer = 0;
		switch(m_eIdPlayer)
		{
			case EIdPlayer.e_IdPlayer_Player1:
				idPlayer = 0;
				break;
			case EIdPlayer.e_IdPlayer_Player2:
				idPlayer = 1;
				break;
			case EIdPlayer.e_IdPlayer_Player3:
				idPlayer = 2;
				break;
			case EIdPlayer.e_IdPlayer_Player4:
				idPlayer = 3;
				break;
		}
		m_PlayerInput = CApoilInput.InputPlayer[idPlayer];
	}

	public void SetIdPlayer(int nId)
	{
		switch(nId)
		{
			case 0:
				m_eIdPlayer = EIdPlayer.e_IdPlayer_Player1;
				break;
			case 1:
				m_eIdPlayer = EIdPlayer.e_IdPlayer_Player2;
				break;
			case 2:
				m_eIdPlayer = EIdPlayer.e_IdPlayer_Player3;
				break;
			case 3:
				m_eIdPlayer = EIdPlayer.e_IdPlayer_Player4;
				break;
		}
	}

	public EIdPlayer GetIdPlayer()
	{
		return m_eIdPlayer;
	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void SetPosition2D(Vector2 pos2D)
	{
		Vector3 pos3D = gameObject.transform.position;
		pos3D.x = pos2D.x;
		pos3D.y = pos2D.y;
		gameObject.transform.position = pos3D;
	}

	public void SetPositionInit(Vector2 pos2D)
	{
		m_PositionInit = pos2D;
	}
}
