using UnityEngine;
using System.Collections;

public struct SAnimationCreep
{
	public CAnimation AnimVeille;
	public CAnimation AnimBouge;
	public CAnimation AnimChoppe;
	public CAnimation AnimDort;
	public CAnimation AnimEclaire;
}

public class CCreep : CElement 
{
	public enum EState
	{
		e_Nothing,
		e_FollowPlayer,
		e_TakePlayer,
		e_OnPlayer,
		e_Sleep,
		e_OnLight
	}
	
	CScriptCreep m_ScriptCreep;
	CScriptCreepZone m_ScriptCreepZone;
	CSpriteSheet m_SpriteSheet;
	SAnimationCreep m_AnimationCreep;
	CPlayer m_PlayerParasitized;
	bool m_bIsOnPlayer;
	bool m_bCanParasitizedPlayer;
	bool m_bIsOnLight;
	float m_fTimerParasite;
	float m_fTimerTakePlayer;
	float m_fTimerSleep;
	const float m_fTimerTakePlayerMax = 1.0f;
	float m_fVelocity;
	Vector3 m_posToGo;
	EState m_eState;

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CCreep()
	{
		m_PlayerParasitized = null;
		m_bIsOnPlayer = false;
		m_bCanParasitizedPlayer = true;
		m_fTimerParasite = 0.0f;
		m_fVelocity = 0.0f;
		m_fTimerTakePlayer = 0.0f;
		m_fTimerSleep = 0.0f;
		m_eState = EState.e_Nothing;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Init()
	{	
		base.Init();
		m_ScriptCreep = m_GameObject.GetComponent<CScriptCreep>();
		m_ScriptCreep.SetCreepElement(this);
		m_ScriptCreepZone = m_GameObject.GetComponentInChildren<CScriptCreepZone>();
		m_ScriptCreepZone.SetCreepElement(this);
				
		m_AnimationCreep.AnimBouge = new CAnimation(m_Game.m_materialMonterCreepBouge, 2, 2, 1.0f);
		m_AnimationCreep.AnimChoppe = new CAnimation(m_Game.m_materialMonterCreepChoppe, 2, 2, 1.0f);
		m_AnimationCreep.AnimDort = new CAnimation(m_Game.m_materialMonterCreepDort, 2, 2, 1.0f);
		m_AnimationCreep.AnimEclaire = new CAnimation(m_Game.m_materialMonterCreepEclaire, 2, 2, 1.0f);
		m_AnimationCreep.AnimVeille = new CAnimation(m_Game.m_materialMonterCreepVeille, 2, 2, 1.0f);
		
		m_SpriteSheet = new CSpriteSheet(m_GameObject);
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
			
		m_SpriteSheet.Init();
		m_SpriteSheet.SetAnimation(m_AnimationCreep.AnimVeille);
		m_SpriteSheet.setEndCondition(CSpriteSheet.EEndCondition.e_Loop);
		m_SpriteSheet.AnimationStart();
		SetAnimationState();
		
		m_fVelocity = m_Game.m_fCreepVelocity;
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public override void Reset()
	{
		base.Reset();
		m_SpriteSheet.Reset();
		m_ScriptCreep.Reset();
		m_ScriptCreepZone.Reset();
		m_bIsOnPlayer = false;
		m_bCanParasitizedPlayer = true;
		m_fTimerParasite = 0.0f;
		m_eState = EState.e_Nothing;
		SetAnimationState();
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Process(float fDeltatime)
	{
		base.Process(fDeltatime);
		m_SpriteSheet.Process();
		
		if(m_PlayerParasitized != null)
			if(m_PlayerParasitized.IsResetSubElements())
				Reset();
		
		Vector3 velocity = new Vector3(0,0,0);
		switch(m_eState)
		{
			case EState.e_Nothing:
			{
				Vector2 rand = Random.insideUnitCircle;
				velocity.x = rand.x;
				velocity.y = rand.y;
			
				if(m_bIsOnLight)
				{
					m_eState = EState.e_OnLight;
					SetAnimationState();
				}
			
				break;	
			}
			
			case EState.e_FollowPlayer:
			{
				velocity = (m_posToGo - m_GameObject.transform.position).normalized;
				if((m_posToGo - m_GameObject.transform.position).sqrMagnitude < 5.0f)
					StopFollowPlayer();
				break;	
			}
			
			case EState.e_TakePlayer:
			{
				m_fTimerTakePlayer += fDeltatime;
				
				Vector2 posCreep = new Vector2(m_GameObject.transform.position.x, m_GameObject.transform.position.y);
			
				float fX = CApoilMath.InterpolationLinear(m_fTimerTakePlayer, 0.0f, m_fTimerTakePlayerMax, posCreep.x, m_PlayerParasitized.GetPosition2D().x);
				float fY = CApoilMath.InterpolationLinear(m_fTimerTakePlayer, 0.0f, m_fTimerTakePlayerMax, posCreep.y, m_PlayerParasitized.GetPosition2D().y);
				
				SetPosition2D(new Vector2(fX, fY));
			
				if(m_fTimerTakePlayer > m_fTimerTakePlayerMax)
				{
					m_eState = EState.e_OnPlayer;
					SetAnimationState();
				}
			
				if(m_bIsOnLight)
				{
					m_eState = EState.e_OnLight;
					SetAnimationState();
				}
				break;	
			}
			
			case EState.e_OnPlayer:
			{
				m_fTimerParasite += fDeltatime;
				SetPosition2D(m_PlayerParasitized.GetPosition2D());
				if(m_fTimerParasite > m_Game.m_fCreepTimerParasiteMin)
					m_bCanParasitizedPlayer = true;
				if(m_fTimerParasite > m_Game.m_fCreepTimerParasiteMax)
					LeavePlayer();
				if(m_bIsOnLight)
				{
					LeavePlayer();
					m_eState = EState.e_OnLight;
					SetAnimationState();
				}
				break;	
			}
			
			case EState.e_Sleep:
			{
				if(m_fTimerSleep < m_Game.m_fCreepTimerSleep)
					m_fTimerSleep += fDeltatime;
				else
				{
					m_eState = EState.e_Nothing;
					SetAnimationState();
				}
				break;	
			}
			case EState.e_OnLight:
			{
				if(!m_bIsOnLight)
				{
					m_eState = EState.e_Nothing;
					SetAnimationState();
				}
				break;	
			}
			
			}
		
		m_GameObject.transform.position += m_fVelocity * velocity * fDeltatime;

	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CSpriteSheet GetSpriteSheet()
	{
		return m_SpriteSheet;	
	}
	
	public void SetPositionToGo(Vector2 pos)
	{
		m_posToGo = new Vector3(pos.x, pos.y, 0.0f);
		m_eState = EState.e_FollowPlayer;
		SetAnimationState();
	}
	
	public void StopFollowPlayer()
	{
		m_posToGo = Vector3.zero;
		m_eState = EState.e_Nothing;	
		SetAnimationState();
	}
	
	public void SetPlayerParasitized(CPlayer player)
	{
		LeavePlayer();
		m_PlayerParasitized = player;
		m_PlayerParasitized.SetCreepOnPlayer(this);
		m_bIsOnPlayer = true;
		m_fTimerParasite = 0.0f;
		m_bCanParasitizedPlayer = false;
		m_eState = EState.e_TakePlayer;
		SetAnimationState();
		m_fTimerTakePlayer = 0.0f;
	}	
	
	public void LeavePlayer()
	{
		if(m_PlayerParasitized != null)
		{
			m_PlayerParasitized.CreepLeavePlayer();
			m_PlayerParasitized = null;		
			m_bIsOnPlayer = false;
			m_bCanParasitizedPlayer = true;
			m_eState = EState.e_Sleep;
			SetAnimationState();
		}
	}
	
	void SetAnimationState()
	{
		switch(m_eState)
		{
			case EState.e_Nothing:
			{
				m_SpriteSheet.SetAnimation(m_AnimationCreep.AnimVeille);
				break;	
			}
			case EState.e_FollowPlayer:
			{
				m_SpriteSheet.SetAnimation(m_AnimationCreep.AnimBouge);
				break;	
			}
			case EState.e_OnLight:
			{
				m_SpriteSheet.SetAnimation(m_AnimationCreep.AnimEclaire);
				break;	
			}
			case EState.e_OnPlayer:
			{
				m_SpriteSheet.SetAnimation(m_AnimationCreep.AnimChoppe);
				break;	
			}
			case EState.e_Sleep:
			{
				m_SpriteSheet.SetAnimation(m_AnimationCreep.AnimDort);
				break;	
			}
			case EState.e_TakePlayer:
			{
				m_SpriteSheet.SetAnimation(m_AnimationCreep.AnimBouge);
				break;	
			}
		}	
	}
		
	public bool IsOnPlayer()
	{
		return m_bIsOnPlayer;
	}
	
	public bool CanParasitizedPlayer()
	{
		return m_bCanParasitizedPlayer;	
	}
	
	public CPlayer GetParasitizedPlayer()
	{
		return m_PlayerParasitized;
	}
	
	public void SetIsOnLight(bool bIsOnLight)
	{
		m_bIsOnLight = bIsOnLight;	
	}
	
	public bool IsInState(EState eState)
	{
		return (m_eState == eState);	
	}
	
}
