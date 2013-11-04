using UnityEngine;
using System.Collections;

public class CCreep : CElement 
{
	CScriptCreep m_ScriptCreep;
	CSpriteSheet m_SpriteSheet;
	CGame m_Game;

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CCreep()
	{

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
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CSpriteSheet GetSpriteSheet()
	{
		return m_SpriteSheet;	
	}
}
