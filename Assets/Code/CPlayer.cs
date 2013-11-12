using UnityEngine;
using System.Collections;

public struct SAnimationPlayer //AnimAB A = mouvement de deplacement, B = mouvement du regard
{
	public CAnimation AnimRepos;
	
	public CAnimation AnimUpUp;
	public CAnimation AnimUpDown;
	public CAnimation AnimUpLeft;
	public CAnimation AnimUpRight;
	
	public CAnimation AnimDownUp;
	public CAnimation AnimDownDown;
	public CAnimation AnimDownLeft;
	public CAnimation AnimDownRight;
	
	public CAnimation AnimLeftUp;
	public CAnimation AnimLeftDown;
	public CAnimation AnimLeftLeft;
	public CAnimation AnimLeftRight;
	
	public CAnimation AnimRightUp;
	public CAnimation AnimRightDown;
	public CAnimation AnimRightLeft;
	public CAnimation AnimRightRight;
	
	public CAnimation AnimDieHeadCut;
	public CAnimation AnimDieFall;
}

public class CPlayer : CCharacter 
{
	//CGame m_game = GameObject.Find("_Game").GetComponent<CGame>();
	float m_fSpeed;
	float m_fAngleCone;
	float m_fTimerDead;
	float m_fForceMagnet;
	const float m_fTimerDeadMax = 2.0f;
	CSpriteSheet m_spriteSheet;
	SAnimationPlayer m_AnimPlayer;
	CConeVision m_ConeVision;
	GameObject m_Torche;
	
	Camera m_CameraCone;
	Vector2 m_posInit;
	Vector2 m_posRespawn;
	Vector2 m_posGoToDie;
	Vector2 m_posOfDie;
	Vector2 m_DirectionRegard;
	Vector2 m_DirectionDeplacement;
	Vector2 m_PosMagnet; //Yeah Bitch! Magnets!
	bool m_bMainCharacter;
	bool m_bHaveObject;
	bool m_bIsAlive;
	bool m_bHaveDirectionToDie;
	bool m_bIsRespawn;
	bool m_bResetSubElements;
	bool m_bMagnet;
	bool m_bParalyse;
	
	bool m_bLookLeft;
	bool m_bLookRight;
	bool m_bLookUp;
	bool m_bLookDown;
	
	CCercleDiscretion m_CercleDiscretion;
	CTakeElement m_YounesSuceDesBites;
	SPlayerInput m_PlayerInput;
	int m_nNbCreepOnPlayer;
	
	public enum EMoveModState // mode de deplacement
	{
		e_MoveModState_attente,
		e_MoveModState_discret,
		e_MoveModState_marche,
		e_MoveModState_cours
	}
	
	public enum EState // etat de l'avatar
	{
		e_Normal,
		e_Paralyse,
		
		e_State_nbState
	}
	
	public enum EIdPlayer // commentaire
	{
		e_IdPlayer_Player1,
		e_IdPlayer_Player2,
		e_IdPlayer_Player3,
		e_IdPlayer_Player4,
	}
	
	EMoveModState m_eMoveModState;
	EState m_eState;
	EIdPlayer m_eIdPlayer;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CPlayer(Vector2 posInit, EIdPlayer eIdPlayer, SAnimationPlayer AnimPlayer)
	{
		Debug.Log ("CPLAYER");
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		GameObject prefab = m_Game.prefabPlayer;
		m_GameObject = GameObject.Instantiate(prefab) as GameObject;
		SetPosition2D(posInit);
		m_posInit = posInit;
		
		//m_ConeVision = m_GameObject.GetComponent<CConeVision>();
		//m_CameraCone = m_Game.m_CameraCone;
		
		m_fSpeed = m_Game.m_fSpeedPlayer;
		m_spriteSheet = new CSpriteSheet(m_GameObject); //m_GameObject.GetComponent<CSpriteSheet>();	
				
		m_AnimPlayer = AnimPlayer;
		
		m_eMoveModState = EMoveModState.e_MoveModState_marche;
		m_eIdPlayer = eIdPlayer;
		
		SetPlayerInput();
		
		m_YounesSuceDesBites = null;
		m_bHaveObject = false;
		m_bIsRespawn = false;
		m_bIsAlive = true;
		m_bResetSubElements = false;
		m_bParalyse = false;
		m_fTimerDead = 0.0f;
		m_bLookLeft = false;
		m_bLookRight = false;
		m_bLookUp = false;
		m_bLookDown = false;
		
		m_Torche = m_GameObject.transform.FindChild("Torche").gameObject;
		
		m_CercleDiscretion = m_GameObject.transform.FindChild("CercleDiscretion").GetComponent<CCercleDiscretion>();
		
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public new void Init()
	{	
		base.Init();
		SetPosition2D(m_posInit);
		m_posRespawn = m_posInit;
		m_posGoToDie = new Vector2(0,0);
		m_posOfDie = new Vector2(0,0);
		//Appel a la main des scripts du gameObject
		m_spriteSheet.Init();
		//m_ConeVision.Init();
		//m_spriteSheet.SetAnimation(m_AnimRepos);
		m_CercleDiscretion.Init(this);

		PrepareStargate();
		m_Game.getSoundEngine().setSwitch("Sol", "Metal02", m_GameObject);

	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public new void Reset()
	{
		base.Reset();
		SetPosition2D(m_posInit);
		m_GameObject.active = true;
		m_bIsAlive = true;
		m_bResetSubElements = false;
		StopMagnet();
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public new void Process(float fDeltatime)
	{
		if(m_bIsAlive)
		{
			base.Process(fDeltatime);
			SetPlayerInput();
			
			if(!m_bParalyse)
				MovePlayer(fDeltatime);
			else
			{
				m_spriteSheet.SetAnimation(m_AnimPlayer.AnimRepos);
				m_spriteSheet.AnimationStart();
				m_eMoveModState = EMoveModState.e_MoveModState_attente;
				m_GameObject.rigidbody.velocity = Vector3.zero;	
			}
			GestionTorche(fDeltatime);
			
			if(m_Game.IsDebug())
			{
				if(Input.GetKeyDown(KeyCode.A))
				{
					m_eState = (m_eState + 1);
					
					if (m_eState >= EState.e_State_nbState)
						m_eState = EState.e_Normal;

				}
			}
			
			//gestion de la lampe torche
			if(m_Game.m_bLightIsOn == false)
			{
				m_Torche.SetActiveRecursively(true);
			}
			else
			{
				m_Torche.SetActiveRecursively(false);
			}
			
			//gestion si on tiens un objet
			if(m_bHaveObject)
			{
				m_YounesSuceDesBites.SetPosition2D(m_GameObject.transform.position);
			}
		
			//Appel a la main des scripts du gameObject
			m_spriteSheet.Process();
		
			/*if(m_bMainCharacter)
				m_ConeVision.Process();*/
			
			m_CercleDiscretion.Process();
			if(m_bIsRespawn)
				m_bIsRespawn = false;
		}
		else 
			GestionDie(fDeltatime);
	}
	
	void PrepareStargate()
	{
		Object.DontDestroyOnLoad(m_GameObject);
	}
	
	
	//Prepare player for a new level
	public void LaunchStargate()
	{
		m_bIsAlive = true;
		m_YounesSuceDesBites = null;
		m_bHaveObject = false;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void GestionDie(float fDeltatime)
	{
		if (m_fTimerDead < m_fTimerDeadMax)
		{
			m_spriteSheet.Process();
			m_fTimerDead += fDeltatime;
			if(m_bHaveDirectionToDie)
			{
				float fX;
				float fY;
				if(m_fTimerDead < m_fTimerDeadMax / 2.0f)
				{
					fX = CApoilMath.InterpolationLinear(m_fTimerDead, 0.0f, m_fTimerDeadMax / 2.0f, m_posOfDie.x, m_posGoToDie.x);
					fY = CApoilMath.InterpolationLinear(m_fTimerDead, 0.0f, m_fTimerDeadMax / 2.0f, m_posOfDie.y, m_posGoToDie.y);
				}
				else
				{
					fX = m_posGoToDie.x;
					fY = m_posGoToDie.y;
				}
				SetPosition2D(new Vector2(fX, fY)); 
			}
		}
		else 
		{
			Respawn();
		}	
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	void CalculateSpeed()
	{
		float fVitesseEtat = 1.0f;
		float fVitesseAttitude;
		switch(m_eMoveModState)
		{
			case EMoveModState.e_MoveModState_attente:
			{
				fVitesseAttitude = 0.0f;
				break;
			}
			case EMoveModState.e_MoveModState_discret:
			{
				fVitesseAttitude = m_Game.m_fCoeffSlowWalk;
				m_spriteSheet.SetCoeffVelocity(1.5f);
				break;
			}
			case EMoveModState.e_MoveModState_marche:
			{
				fVitesseAttitude = m_Game.m_fCoeffNormalWalk;
				m_spriteSheet.SetCoeffVelocity(1.0f);
				break;
			}
			case EMoveModState.e_MoveModState_cours	:
			{
				fVitesseAttitude = m_Game.m_fCoeffRunWalk;
				m_spriteSheet.SetCoeffVelocity(0.5f);
				break;
			}
			default: fVitesseAttitude = 1.0f; break;
		}
		
		float fCoeffDirection = Vector2.Dot(m_DirectionRegard, m_DirectionDeplacement);
		fCoeffDirection = m_Game.m_fCoeffReverseWalk + (1.0f - m_Game.m_fCoeffReverseWalk)*(fCoeffDirection + 1)/2;
		
		float fCoeffVelocityCreep = m_Game.m_fCreepCoeffRalentissement * (1.0f/(float)m_nNbCreepOnPlayer);
		if(m_nNbCreepOnPlayer == 0)
			fCoeffVelocityCreep = 1.0f;
				
		m_fSpeed = m_Game.m_fSpeedPlayer * fVitesseEtat * fVitesseAttitude * fCoeffDirection * fCoeffVelocityCreep;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void PickUpObject(CTakeElement obj)
	{
		m_YounesSuceDesBites = obj;
		m_bHaveObject = true;
	}
	
	public CTakeElement GetHeldElement()
	{
		return m_YounesSuceDesBites;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void DropElement()
	{
		m_YounesSuceDesBites = null;
		m_bHaveObject = false;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void flipRight ()
	{
		if (m_GameObject.transform.localScale.x < 0)
		{
			Vector3 nTrans = m_GameObject.transform.localScale;
			nTrans.x *= -1;
			m_GameObject.transform.localScale = nTrans;
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void flipLeft ()
	{
		if (m_GameObject.transform.localScale.x > 0)
		{
			Vector3 nTrans = m_GameObject.transform.localScale;
			nTrans.x *= -1;
			m_GameObject.transform.localScale = nTrans;
		}
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void MovePlayer(float fDeltatime)
	{	
		if (m_GameObject.rigidbody != null)
		{			
			Vector3 velocity = Vector3.zero;
			if (m_PlayerInput.MoveUp) 
			{ 
				velocity += new Vector3(0,1,0); 
				
				m_eMoveModState = EMoveModState.e_MoveModState_marche;
				
				if(m_bLookDown)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimUpDown);
				else if(m_bLookLeft)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimUpLeft);
				else if(m_bLookRight)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimUpRight);
				else
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimUpUp);
				m_spriteSheet.AnimationStart();
				
			}
			else if (m_PlayerInput.MoveDown) 
			{ 
				velocity += new Vector3(0,-1,0); 
				m_eMoveModState = EMoveModState.e_MoveModState_marche;
				
				if(m_bLookUp)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimDownUp);
				else if(m_bLookLeft)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimDownLeft);
				else if(m_bLookRight)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimDownRight);
				else
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimDownDown);
				
				m_spriteSheet.AnimationStart();
				
			}
			if (m_PlayerInput.MoveLeft) 
			{
				velocity += new Vector3(-1,0,0); 
				m_eMoveModState = EMoveModState.e_MoveModState_marche;
				
				if(m_bLookUp)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimLeftUp);
				else if(m_bLookDown)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimLeftDown);
				else if(m_bLookRight)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimLeftRight);
				else
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimLeftLeft);
				
				m_spriteSheet.AnimationStart();		
			}
			else if (m_PlayerInput.MoveRight) 
			{ 
				velocity += new Vector3(1,0,0); 
				m_eMoveModState = EMoveModState.e_MoveModState_marche;
				
				if(m_bLookUp)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimRightUp);
				else if(m_bLookDown)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimRightDown);
				else if(m_bLookLeft)
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimRightLeft);
				else
					m_spriteSheet.SetAnimation(m_AnimPlayer.AnimRightRight);
				
				m_spriteSheet.AnimationStart();
				
			}
			if(!m_PlayerInput.MoveUp && !m_PlayerInput.MoveDown && !m_PlayerInput.MoveLeft && !m_PlayerInput.MoveRight) 
			{
				m_spriteSheet.SetAnimation(m_AnimPlayer.AnimRepos);
				m_spriteSheet.AnimationStart();
				m_eMoveModState = EMoveModState.e_MoveModState_attente;
				m_GameObject.rigidbody.velocity = Vector3.zero;
			}
			
			velocity.Normalize();
			m_DirectionDeplacement = velocity;
			
			if(m_PlayerInput.WalkFast)
			{
				m_eMoveModState = EMoveModState.e_MoveModState_cours;
			}	
			if(m_PlayerInput.WalkSlow)
			{
				m_eMoveModState = EMoveModState.e_MoveModState_discret;	
			}
			
			CalculateSpeed();
			
			Vector3 MagnetDeviation = Vector3.zero;
			if(m_bMagnet)
			{
				Vector2 Deviation = (m_PosMagnet - GetPosition2D()).normalized;
				MagnetDeviation.x = Deviation.x;
				MagnetDeviation.y = Deviation.y;
			}
				
			m_GameObject.transform.position += m_fSpeed * velocity * fDeltatime + m_fForceMagnet * MagnetDeviation;
		}
		else
		{
			Debug.LogError("Pas de rigid body sur "+m_GameObject.name);
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void GestionTorche(float fDeltatime)
	{
		float fAngleOld = m_fAngleCone;
		
		if(m_Game.IsPadXBoxMod())
		{
			float fPadY = m_PlayerInput.DirectionHorizontal;
			float fPadX = m_PlayerInput.DirectionVertical;
			float fTolerance = 0.05f;
			if(Mathf.Abs(fPadX) > fTolerance || Mathf.Abs(fPadY) > fTolerance)
			{
				m_DirectionRegard = (new Vector2(fPadX, -fPadY)).normalized;
			
				m_bLookUp = m_PlayerInput.LookUp;
				m_bLookDown = m_PlayerInput.LookDown;
				m_bLookLeft = m_PlayerInput.LookLeft;
				m_bLookRight = m_PlayerInput.LookRight;
			}
			
			/*
			Vector3 debugVec = new Vector3 (m_DirectionRegard.x, m_DirectionRegard.y, 0.0f);
           // Debug.DrawLine(m_GameObject.transform.position, m_GameObject.transform.position + 100*debugVec);
			Debug.DrawRay(m_GameObject.transform.position, 100*debugVec);*/
			
			m_fAngleCone = CApoilMath.ConvertCartesianToPolar(new Vector2(m_DirectionRegard.x, m_DirectionRegard.y)).y;
		}
		else
		{
			Vector3 posPlayerTmp = m_GameObject.transform.position;
			Vector2 posMouse = CApoilInput.MousePosition;
			Vector2 posPlayer = new Vector2(posPlayerTmp.x, posPlayerTmp.y);
			m_DirectionRegard = (posMouse - posPlayer).normalized;
			m_fAngleCone = Mathf.Acos(Vector2.Dot(m_DirectionRegard, new Vector2(1,0)));
			
			if(posMouse.y < posPlayer.y)
			{
				m_fAngleCone *=-1;
			}		
			//float fAngleVise = -m_fAngleCone*180/3.14159f - 90 - 75/2;
			//m_ConeVision.setAngleVise(fAngleVise);  		
		}
		
		m_fAngleCone += 3.14159f/2.0f;
		m_Torche.transform.RotateAround(new Vector3(0,0,1),  m_fAngleCone - fAngleOld);
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void SetPlayerInput()
	{
		switch(m_eIdPlayer)
		{
			case EIdPlayer.e_IdPlayer_Player1:
				m_PlayerInput = CApoilInput.InputPlayer1;
				break;
			case EIdPlayer.e_IdPlayer_Player2:
				m_PlayerInput = CApoilInput.InputPlayer2;
				break;
			case EIdPlayer.e_IdPlayer_Player3:
				m_PlayerInput = CApoilInput.InputPlayer3;
				break;
			case EIdPlayer.e_IdPlayer_Player4:
				m_PlayerInput = CApoilInput.InputPlayer4;
				break;
		}
			
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void DieHeadCut()
	{
		if(m_bIsAlive)
		{
			DropElement();
			m_bIsAlive = false;
			m_spriteSheet.SetAnimation(m_AnimPlayer.AnimDieHeadCut);
			m_spriteSheet.setEndCondition(CSpriteSheet.EEndCondition.e_Stop);
			m_spriteSheet.Reset();
			m_spriteSheet.AnimationStart();
			m_fTimerDead = 0.0f;
			m_bResetSubElements = true;
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void DieFall(Vector2 posOfDie, Vector2 posToDie, Vector2 posRespawn)
	{
		if(m_bIsAlive && !m_bIsRespawn)
		{
			DropElement();
			m_bIsAlive = false;
			m_spriteSheet.SetAnimation(m_AnimPlayer.AnimDieFall);
			m_spriteSheet.setEndCondition(CSpriteSheet.EEndCondition.e_Stop);
			m_spriteSheet.Reset();
			m_spriteSheet.AnimationStart();
			m_fTimerDead = 0.0f;
			m_bHaveDirectionToDie = true;
			m_posGoToDie = posToDie;
			m_posOfDie 	 = posOfDie;
			SetPosRespawn(posRespawn);
			m_bResetSubElements = true;
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void Respawn()
	{
		SetPosition2D(m_posRespawn);
		m_bIsAlive = true;	
		m_spriteSheet.setEndCondition(CSpriteSheet.EEndCondition.e_Loop);
		m_spriteSheet.Reset();
		m_spriteSheet.AnimationStart();
		m_bHaveDirectionToDie = false;
		m_bIsRespawn = true;
		m_bResetSubElements = false;
		m_nNbCreepOnPlayer = 0;
	}
	
	public void ResetPosInit(Vector2 posInit)
	{
		m_posInit = posInit;	
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public EState GetState()
	{
		return m_eState;	
	}
	
	public Vector2 GetDirectionDeplacement()
	{
		return m_DirectionDeplacement;	
	}
	
	public Vector3 GetDirectionRegard()
	{
		return m_DirectionRegard;	
	}
	
	public EMoveModState GetMoveModState(){
		return m_eMoveModState;
	}
	
	public float GetSpeed()
	{
		return m_fSpeed;
	}
	
	public bool HaveObject()
	{
		return m_bHaveObject;	
	}
	
	public bool IsAlive()
	{
		return (m_bIsAlive || m_GameObject.active);
	}	
	
	public void SetPosRespawn(Vector2 pos)
	{
		m_posRespawn = pos;
	}
	
	public void SetCreepOnPlayer(CCreep creep)
	{
		++m_nNbCreepOnPlayer;		
	}
	
	public void CreepLeavePlayer()
	{
		if(m_nNbCreepOnPlayer > 0)
			--m_nNbCreepOnPlayer;
	}
	
	public bool IsResetSubElements()
	{
		return m_bResetSubElements;	
	}
	
	public void SetMagnetData(Vector3 pos, float fForce)
	{
		m_bMagnet = true;
		m_PosMagnet = new Vector2(pos.x, pos.y);
		m_fForceMagnet = fForce;
	}	
	
	public void StopMagnet()
	{
		m_bMagnet = false;
		m_PosMagnet = Vector2.zero;
		m_fForceMagnet = 0.0f;
	}
	
	public void SetParalyse(bool bState)
	{
		m_bParalyse = bState;	
	}
	
	public SPlayerInput GetPlayerInput()
	{
		return m_PlayerInput;	
	}
}
