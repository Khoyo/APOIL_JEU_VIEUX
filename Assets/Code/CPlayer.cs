using UnityEngine;
using System.Collections;

public class CPlayer : MonoBehaviour {

	public GameObject objetRender;
	public GameObject objetTorchLight;

	public enum EIdPlayer 
	{
		e_IdPlayer_Player1,
		e_IdPlayer_Player2,
		e_IdPlayer_Player3,
		e_IdPlayer_Player4,
	}

	enum EStateMove
	{
		e_Attente,
		e_Marche,
		e_Cours
	}

	enum EState
	{
		e_Normal,
		e_DieEat,
		e_DieHeadCut,
	}

	EIdPlayer m_eIdPlayer;
	EStateMove m_eStateMove;
	EState m_eState;
	SPlayerInput m_PlayerInput;
	Vector2 m_PositionInit;
	Vector2 m_PositionGravityMonster;
	Vector2 m_Move;
	Vector2 m_Direction;
	float m_fSpeed;
	float m_fAngleCone;
	float m_fAngleTorchLight;
	float m_fDistance;
	int m_nEnergieTorchLight;
	int m_nPrecision = 10;
	bool m_bActiveTorchLight;
	bool m_bGravity;
	bool m_bParazitized;


	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () 
	{
		SetPlayerInput ();
		m_fSpeed = 1.0f;
		m_eStateMove = EStateMove.e_Attente;
		m_nEnergieTorchLight = CGame.ms_nEnergieTorchLightMax;
		m_bActiveTorchLight = true;
		m_bGravity = false;
		m_bParazitized = false;
		m_fAngleCone = 0.0f;
		m_Direction = new Vector2 (1.0f, 0.0f);
		m_eState = EState.e_Normal;

		m_fAngleTorchLight = gameObject.transform.FindChild ("TorchLight").FindChild ("Spotlight").GetComponent<Light> ().spotAngle;

		m_fDistance = gameObject.transform.FindChild ("TorchLight").FindChild ("Spotlight").GetComponent<Light> ().range * 2.0f/3.0f;
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () 
	{
		SetPlayerInput ();
		SetStateMove ();

		switch(m_eState)
		{
			case EState.e_Normal:
			{
				Move ();
				ProcessTorchLight ();
				break;
			}
			case EState.e_DieEat:
			{
				gameObject.rigidbody2D.velocity = Vector2.zero;
				break;
			}
			case EState.e_DieHeadCut:
			{
				gameObject.rigidbody2D.velocity = Vector2.zero;
				//playAnimHeadCut
				Respawn();
				break;
			}

		}


	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void Move()
	{
		if(gameObject.rigidbody2D != null)
		{
			m_Move = Vector2.zero;
			m_Move = new Vector2(m_PlayerInput.MoveHorizontal, m_PlayerInput.MoveVertical);

			if(Mathf.Abs(m_PlayerInput.DirectionHorizontal) > 0.25f || Mathf.Abs(m_PlayerInput.DirectionVertical) > 0.25f)
			{
				m_Direction = new Vector2(m_PlayerInput.DirectionHorizontal, m_PlayerInput.DirectionVertical);
				m_Direction.Normalize();
			}

			CalculateVelocity();

			Vector2 directionGravity = Vector2.zero;
			if(m_bGravity)
			{
				Vector2 posPlayer = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
				directionGravity = m_PositionGravityMonster - posPlayer;
				directionGravity.Normalize();
			}

			gameObject.rigidbody2D.velocity = m_fSpeed * m_Move + directionGravity * CGame.ms_fGravityMonsterForce;
		}
	}

	void CalculateVelocity()
	{
		float fVelocityState = 1.0f;
		float fVelocityAttitude = 1.0f;
		float fCoeffDirection = 1.0f;

		switch(m_eStateMove)
		{
			case EStateMove.e_Attente:
			{
				fVelocityState = 0.0f;
				break;
			}
			case EStateMove.e_Marche:
			{
				fVelocityState = 1.0f;
				break;
			}
			case EStateMove.e_Cours:
			{
				fVelocityState = CGame.ms_fCoeffRun;
				break;
			}
		}

		fCoeffDirection = Vector2.Dot (m_Direction, m_Move);
		fCoeffDirection = CGame.ms_fCoeffReverseWalk + (1 - CGame.ms_fCoeffReverseWalk) * (fCoeffDirection + 1.0f) / 2.0f;

		float fCoeffParasitized = 1.0f;
		if(m_bParazitized)
			fCoeffParasitized = CGame.ms_fCoeffPararsitized;

		m_fSpeed = CGame.ms_fVelocityPlayer * fVelocityState * fVelocityAttitude * fCoeffDirection * fCoeffParasitized;
	}

	void ProcessTorchLight()
	{
		if(m_bActiveTorchLight && m_nEnergieTorchLight > 0)
		{
			float fAngleOld = m_fAngleCone;

			if(!objetTorchLight.activeSelf)
				objetTorchLight.SetActive(true);


			m_fAngleCone = CApoilMath.ConvertCartesianToPolar(m_Direction).y;

			objetTorchLight.transform.RotateAround(new Vector3(0, 0, 1),  m_fAngleCone - fAngleOld);

			ProcessColliderLight();

			//m_nEnergieTorchLight--;
		}
		else
		{
			if(objetTorchLight.activeSelf)
				objetTorchLight.SetActive(false);
		}
	}

	void ProcessColliderLight()
	{
		for(int i = 0 ; i < m_nPrecision ; ++i)
		{
			float fAngle = CApoilMath.InterpolationLinear(i, 0, m_nPrecision, -m_fAngleTorchLight/2.0f, m_fAngleTorchLight/2.0f);
			Matrix4x4 mat = Matrix4x4.TRS( Vector3.zero, Quaternion.Euler(0, 0, fAngle), Vector3.one);
			
			RaycastHit2D hit = Physics2D.Raycast(transform.position, mat * m_Direction, m_fDistance, CGame.ms_LayerMaskLight);
			//Debug.DrawRay(transform.position, m_fDistance * (mat*m_Direction));
			
			if(hit.collider != null)
			{
				//Debug.Log (hit.collider.name);
				if(hit.collider.CompareTag("GravityMonster"))
				{
					hit.collider.gameObject.GetComponent<CGravityMonster>().CollideWithLight();
				}
				
				if(hit.collider.CompareTag("Creep"))
				{
					hit.collider.gameObject.GetComponent<CCreep>().CollideWithLight();
				}
			}
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

	void SetStateMove()
	{
		if(Mathf.Abs(m_PlayerInput.MoveHorizontal) > 0.1 ||  Mathf.Abs(m_PlayerInput.MoveVertical) > 0.1)
		{
			m_eStateMove = EStateMove.e_Marche;
			if(m_PlayerInput.Run)
				m_eStateMove = EStateMove.e_Cours;
		}
		else
		{	
			m_eStateMove = EStateMove.e_Attente;
		}
	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
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

		//ordonner en Z pour eviter les chevauchements
		objetRender.renderer.sortingOrder = 50+nId;
	}

	public EIdPlayer GetIdPlayer()
	{
		return m_eIdPlayer;
	}

	public void Respawn()
	{
		SetPosition2D (m_PositionInit);
		m_eState = EState.e_Normal;
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

	public void SetEat()
	{
		m_eState = EState.e_DieEat;
	}

	public void SetGravityPosition(Vector2 pos)
	{
		m_PositionGravityMonster = pos;
		m_bGravity = true;
	}

	public void StopGravity()
	{
		m_PositionGravityMonster = Vector2.zero;
		m_bGravity = false;
	}

	public void setParasitized()
	{
		m_bParazitized = true;
	}

	public void setCreepDrop()
	{
		m_bParazitized = false;
	}
	
	public void DieHeadCut()
	{
		m_eState = EState.e_DieHeadCut;
	}

	public Vector2 GetPosition()
	{
		return new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
	}

	public GameObject GetGameObject()
	{
		return gameObject;
	}
}
