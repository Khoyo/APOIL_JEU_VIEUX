using UnityEngine;
using System.Collections;

public class CCreep : CElement 
{
	enum EState
	{
		e_Nothing,
		e_FollowPlayer,
		e_TakePlayer,
		e_OnPlayer
	}
	
	CScriptCreep m_ScriptCreep;
	CScriptCreepZone m_ScriptCreepZone;
	CSpriteSheet m_SpriteSheet;
	CGame m_Game;
	CPlayer m_PlayerParasitized;
	bool m_bIsOnPlayer;
	bool m_bCanParasitizedPlayer;
	bool m_bIsOnLight;
	float m_fTimerParasite;
	float m_fTimerTakePlayer;
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
		
		CAnimation anim = new CAnimation(m_ScriptCreep.GetMaterial(), 2, 2, 1.0f);
		
		m_SpriteSheet = new CSpriteSheet(m_GameObject);
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
			
		m_SpriteSheet.Init();
		m_SpriteSheet.SetAnimation(anim);
		m_SpriteSheet.setEndCondition(CSpriteSheet.EEndCondition.e_Loop);
		m_SpriteSheet.AnimationStart();
		
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
				break;	
			}
			
			case EState.e_FollowPlayer:
			{
				velocity = (m_posToGo - m_GameObject.transform.position).normalized;
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
					m_eState = EState.e_OnPlayer;
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
					LeavePlayer();
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
	}
	
	public void StopFollowPlayer()
	{
		m_posToGo = Vector3.zero;
		m_eState = EState.e_Nothing;		
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
			m_eState = EState.e_Nothing;
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
}
