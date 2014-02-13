using UnityEngine;
using System.Collections;

public class CPlayer : MonoBehaviour {

	SPlayerInput m_PlayerInput;

	// Use this for initialization
	void Start () 
	{
		SetPlayerInput ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		SetPlayerInput ();
		Move ();
	
	}

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
			
			gameObject.rigidbody2D.velocity = CGame.m_fVelocityPlayer * velocity;
		}
	}

	void SetPlayerInput()
	{
		m_PlayerInput = CApoilInput.InputPlayer;
	}
}
