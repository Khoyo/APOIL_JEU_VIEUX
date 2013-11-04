using UnityEngine;
using System.Collections;

public class CCreep : CElement 
{
	CScriptCreep m_ScriptCreep;
	CSpriteSheet m_SpriteSheet;
	CGame m_Game;
	CPlayer m_PlayerParasitized;
	bool m_bIsOnPlayer;
	float m_fTimerParasite;

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CCreep()
	{
		m_PlayerParasitized = null;
		m_bIsOnPlayer = false;
		m_fTimerParasite = 0.0f;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Init()
	{	
		base.Init();
		m_ScriptCreep = m_GameObject.GetComponent<CScriptCreep>();
		m_ScriptCreep.SetCreepElement(this);
		
		CAnimation anim = new CAnimation(m_ScriptCreep.GetMaterial(), 2, 2, 1.0f);
		
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
		m_ScriptCreep.Reset();
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Process(float fDeltatime)
	{
		base.Process(fDeltatime);
		m_SpriteSheet.Process();
		if(m_bIsOnPlayer)
		{
			m_fTimerParasite += fDeltatime;
			SetPosition2D(m_PlayerParasitized.GetPosition2D());
			if(m_fTimerParasite > m_Game.m_fCreepTimerParasiteMax)
				LeavePlayer();
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CSpriteSheet GetSpriteSheet()
	{
		return m_SpriteSheet;	
	}
	
	public void SetPlayerParasitized(CPlayer player)
	{
		m_PlayerParasitized = player;
		m_PlayerParasitized.SetCreepOnPlayer(this);
		m_bIsOnPlayer = true;
		m_fTimerParasite = 0.0f;
	}	
	
	public void LeavePlayer()
	{
		m_PlayerParasitized.CreepLeavePlayer();
		m_PlayerParasitized = null;		
		m_bIsOnPlayer = false;
	}
		
	public bool IsOnPlayer()
	{
		return m_bIsOnPlayer;
	}
}
