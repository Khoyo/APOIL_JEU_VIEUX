using UnityEngine;
using System.Collections;

public class CCreep : CElement 
{
	CScriptCreep m_ScriptCreep;
	CScriptCreepZone m_ScriptCreepZone;
	CSpriteSheet m_SpriteSheet;
	CGame m_Game;
	CPlayer m_PlayerParasitized;
	bool m_bIsOnPlayer;
	bool m_bCanParasitizedPlayer;
	bool m_bFollowPlayer;
	float m_fTimerParasite;
	float m_fVelocity;
	Vector3 m_posToGo;
	

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
			if(m_fTimerParasite > m_Game.m_fCreepTimerParasiteMin)
				m_bCanParasitizedPlayer = true;
			if(m_fTimerParasite > m_Game.m_fCreepTimerParasiteMax)
				LeavePlayer();
		}
		else  
		{
			Vector3 velocity = new Vector3(0,0,0);
			if(m_bFollowPlayer)
			{
				velocity = (m_posToGo - m_GameObject.transform.position).normalized;
			}
			else
			{
				Vector2 rand = Random.insideUnitCircle;
				velocity.x = rand.x;
				velocity.y = rand.y;
			}
			
			m_GameObject.transform.position += m_fVelocity * velocity * fDeltatime;
			
		}
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
		m_bFollowPlayer = true;
	}
	
	public void StopFollowPlayer()
	{
		m_posToGo = Vector3.zero;
		m_bFollowPlayer = false;		
	}
	
	public void SetPlayerParasitized(CPlayer player)
	{
		m_PlayerParasitized = player;
		m_PlayerParasitized.SetCreepOnPlayer(this);
		m_bIsOnPlayer = true;
		m_fTimerParasite = 0.0f;
		m_bCanParasitizedPlayer = false;
	}	
	
	public void LeavePlayer()
	{
		m_PlayerParasitized.CreepLeavePlayer();
		m_PlayerParasitized = null;		
		m_bIsOnPlayer = false;
		m_bCanParasitizedPlayer = true;
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
}
