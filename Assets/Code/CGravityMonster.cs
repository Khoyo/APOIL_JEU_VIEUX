using UnityEngine;
using System.Collections;

public struct SAnimationMonster
{
	public CAnimation AnimVeille;
	public CAnimation AnimAlerte;
	public CAnimation AnimActif;
	public CAnimation AnimMange;
	public CAnimation AnimEclaire;
}

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
	CScriptGravityMonsterZoneChoppe m_ScriptGravityMonsterZoneChoppe;
	CScriptGravityMonsterZoneVision m_ScriptGravityMonsterZoneVision;
	CSpriteSheet m_SpriteSheet;
	SAnimationMonster m_AnimationMonster;
	CPlayer m_PlayerAttracted;
	bool m_bIsOnLight;
	float m_fVeilleTimer; //t1 dans le shema (temps avant que le monstre "oublie" le player)
	float m_fEatTimer; //t2 dans le shema


	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CGravityMonster()
	{
		m_PlayerAttracted = null;
		m_fEatTimer = 0.0f;
		m_fVeilleTimer = 0.0f;
		m_eState = EState.e_Veille;
		m_bIsOnLight = false;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Init()
	{	
		base.Init();
		m_ScriptGravityMonster = m_GameObject.GetComponent<CScriptGravityMonster>();
		m_ScriptGravityMonster.SetGravityMonsterElement(this);
		m_ScriptGravityMonsterZoneChoppe = m_GameObject.GetComponentInChildren<CScriptGravityMonsterZoneChoppe>();
		m_ScriptGravityMonsterZoneChoppe.SetGravityMonsterElement(this);
		m_ScriptGravityMonsterZoneVision = m_GameObject.GetComponentInChildren<CScriptGravityMonsterZoneVision>();
		m_ScriptGravityMonsterZoneVision.SetGravityMonsterElement(this);
		
		m_AnimationMonster.AnimActif = new CAnimation(m_Game.m_materialMonterGravityActif, 3, 2, 1.0f);
		m_AnimationMonster.AnimAlerte = new CAnimation(m_Game.m_materialMonterGravityAlerte, 3, 2, 1.0f);
		m_AnimationMonster.AnimEclaire = new CAnimation(m_Game.m_materialMonterGravityEclaire, 3, 2, 1.0f);
		m_AnimationMonster.AnimMange = new CAnimation(m_Game.m_materialMonterGravityMange, 3, 2, 1.0f);
		m_AnimationMonster.AnimVeille = new CAnimation(m_Game.m_materialMonterGravityVeille, 3, 2, 1.0f);
		
		m_SpriteSheet = new CSpriteSheet(m_GameObject);
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
			
		m_SpriteSheet.Init();
		m_SpriteSheet.SetAnimation(m_AnimationMonster.AnimVeille);
		m_SpriteSheet.setEndCondition(CSpriteSheet.EEndCondition.e_Loop);
		m_SpriteSheet.AnimationStart();
		SetAnimationState();
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public override void Reset()
	{
		base.Reset();
		m_SpriteSheet.Reset();
		m_ScriptGravityMonster.Reset();
		m_ScriptGravityMonsterZoneChoppe.Reset();
		m_fEatTimer = 0.0f;
		m_eState = EState.e_Veille;
		SetAnimationState();
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
				if(m_bIsOnLight)
				{
					m_eState = EState.e_Eclaire;
					SetAnimationState();
				}
				else if(m_ScriptGravityMonsterZoneVision.HavePlayerInZone())
				{
					m_eState = EState.e_Alerte;
					m_fVeilleTimer = 0.0f;
					SetAnimationState();
				}
				
				break;	
			}
			case EState.e_Alerte:
			{
				if(m_fVeilleTimer < m_Game.m_fGravityVeilleTimerMax)
					m_fVeilleTimer += fDeltatime;
				else
				{
					m_eState = EState.e_Veille;
					SetAnimationState();
				}	
				if(m_ScriptGravityMonsterZoneVision.HavePlayerInZone())
				{
					m_fVeilleTimer = 0.0f;
				}
				break;	
			}
			case EState.e_Actif:
			{
				
				break;	
			}
			case EState.e_Mange:
			{
				if(m_fEatTimer < m_Game.m_fGravityEatTimerMax)
					m_fEatTimer += fDeltatime;
				else
				{
					m_PlayerAttracted.SetParalyse(false);
					m_PlayerAttracted.Respawn();
					m_PlayerAttracted.ResetSprite();
					DropPlayer(m_PlayerAttracted);
					m_eState = EState.e_Veille;
					SetAnimationState();
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
		SetAnimationState();
	}
	
	public void SetLightStatus(bool bIsLight)
	{
		m_bIsOnLight = bIsLight;	
	}
	
	void SetAnimationState()
	{
		switch(m_eState)
		{
			case EState.e_Veille:
			{
				m_SpriteSheet.SetAnimation(m_AnimationMonster.AnimVeille);
				break;	
			}
			case EState.e_Alerte:
			{
				m_SpriteSheet.SetAnimation(m_AnimationMonster.AnimAlerte);
				break;	
			}
			case EState.e_Actif:
			{
				m_SpriteSheet.SetAnimation(m_AnimationMonster.AnimActif);
				break;	
			}
			case EState.e_Mange:
			{
				m_SpriteSheet.SetAnimation(m_AnimationMonster.AnimMange);
				break;	
			}
			case EState.e_Eclaire:
			{
				m_SpriteSheet.SetAnimation(m_AnimationMonster.AnimEclaire);
				break;	
			}
		}	
	}
}
