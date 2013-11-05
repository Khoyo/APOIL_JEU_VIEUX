using UnityEngine;
using System.Collections;

public class CGravityMonster : CElement 
{
	
	CScriptGravityMonster m_ScriptGravityMonster;
	CSpriteSheet m_SpriteSheet;
	CGame m_Game;


	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CGravityMonster()
	{
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Init()
	{	
		base.Init();
		m_ScriptGravityMonster = m_GameObject.GetComponent<CScriptGravityMonster>();
		m_ScriptGravityMonster.SetGravityMonsterElement(this);
		
		CAnimation anim = new CAnimation(m_ScriptGravityMonster.GetMaterial(), 1, 6, 60.0f);
		
		m_SpriteSheet = new CSpriteSheet(m_GameObject);
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
			
		m_SpriteSheet.Init();
		m_SpriteSheet.SetAnimation(anim);
		m_SpriteSheet.setEndCondition(CSpriteSheet.EEndCondition.e_FramPerFram);
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public override void Reset()
	{
		base.Reset();
		m_SpriteSheet.Reset();
		m_ScriptGravityMonster.Reset();
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Process(float fDeltatime)
	{
		base.Process(fDeltatime);
		m_SpriteSheet.Process();
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CSpriteSheet GetSpriteSheet()
	{
		return m_SpriteSheet;	
	}
}
