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
			if (m_PlayerInput.MoveUp)
			{
				velocity += new Vector2(0,1);
			}
			else if (m_PlayerInput.MoveDown)
			{
				velocity += new Vector2(0,-1);
			}
			else if (m_PlayerInput.MoveLeft)
			{
				velocity += new Vector2(-1,0);
			}
			else if (m_PlayerInput.MoveRight)
			{
				velocity += new Vector2(1,0);
			}
			
			gameObject.rigidbody2D.velocity = CGame.ms_fVelocityPlayer * velocity;
		}
	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	void SetPlayerInput()
	{
		m_PlayerInput = CApoilInput.InputPlayer;
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

	EIdPlayer GetIdPlayer()
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
