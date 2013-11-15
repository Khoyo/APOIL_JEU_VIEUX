using UnityEngine;
using System.Collections;

public class CGravityMonster : CElement 
{
	enum EState
	{
		e_Veille,
		e_Alerte,
		e_Actif,
		e_Mange,
		e_Eclaire
	}
	
	EState m_eState;
	CScriptGravityMonster m_ScriptGravityMonster;
	CScriptGravityMonsterZone m_ScriptGravityMonsterZone;
	CSpriteSheet m_SpriteSheet;
	CPlayer m_PlayerAttracted;
	float m_fEatTimer;


	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CGravityMonster()
	{
		m_PlayerAttracted = null;
		m_fEatTimer = 0.0f;
		m_eState = EState.e_Veille;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Init()
	{	
		base.Init();
		m_ScriptGravityMonster = m_GameObject.GetComponent<CScriptGravityMonster>();
		m_ScriptGravityMonster.SetGravityMonsterElement(this);
		m_ScriptGravityMonsterZone = m_GameObject.GetComponentInChildren<CScriptGravityMonsterZone>();
		m_ScriptGravityMonsterZone.SetGravityMonsterElement(this);
		
		CAnimation anim = new CAnimation(m_ScriptGravityMonster.GetMaterial(), 3, 2, 1.0f);
		
		m_SpriteSheet = new CSpriteSheet(m_GameObject);
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
			
		m_SpriteSheet.Init();
		m_SpriteSheet.SetAnimation(anim);
		m_SpriteSheet.setEndCondition(CSpriteSheet.EEndCondition.e_Loop);
		m_SpriteSheet.AnimationStart();
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public override void Reset()
	{
		base.Reset();
		m_SpriteSheet.Reset();
		m_ScriptGravityMonster.Reset();
		m_ScriptGravityMonsterZone.Reset();
		m_fEatTimer = 0.0f;
		m_eState = EState.e_Veille;
	}
	

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Process(float fDeltatime)
	{
		base.Process(fDeltatime);
		m_SpriteSheet.Process();
		
		switch(m_eState)
		{
			case EState.e_Veille:
			{
				break;	
			}
			case EState.e_Alerte:
			{
				break;	
			}
			case EState.e_Actif:
			{
				
				break;	
			}
			case EState.e_Mange:
			{
				if(m_fEatTimer < m_Game.m_fGravityTimerPrisonMax)
					m_fEatTimer += fDeltatime;
				else
				{
					m_PlayerAttracted.SetParalyse(false);	
					DropPlayer(m_PlayerAttracted);
					m_eState = EState.e_Veille;
				}
				break;	
			}
			case EState.e_Eclaire:
			{
				DropPlayer(m_PlayerAttracted);
				break;	
			}
		}	
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CSpriteSheet GetSpriteSheet()
	{
		return m_SpriteSheet;	
	}
	
	public void CatchPlayer(CPlayer player)
	{
		if(m_PlayerAttracted == null)
		{
			m_PlayerAttracted = player;
			m_PlayerAttracted.SetMagnetData(m_GameObject.transform.position, m_ScriptGravityMonster.m_fForceMagnet);	
		}
	}
	
	public void DropPlayer(CPlayer player)
	{
		if(m_PlayerAttracted == player)
		{
			m_PlayerAttracted.StopMagnet();
			m_PlayerAttracted.Respawn();
			m_PlayerAttracted.ResetSprite();
			m_PlayerAttracted = null;
		}
	}
	
	public void EatPlayer(CPlayer player)
	{
		//DropPlayer(player);
		CatchPlayer(player);
		m_fEatTimer = 0.0f;
		m_eState = EState.e_Mange;
		m_PlayerAttracted.SetSpriteDieGravity();
		m_PlayerAttracted.SetParalyse(true);
	}
}
