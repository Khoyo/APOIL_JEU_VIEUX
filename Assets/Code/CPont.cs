using UnityEngine;
using System.Collections;

public class CPont : CElement 
{
	CScriptPont m_ScriptPont;
	CSpriteSheet m_SpriteSheet;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CPont()
	{
		
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Init()
	{	
		base.Init();
		m_ScriptPont = m_GameObject.GetComponent<CScriptPont>();
		m_ScriptPont.SetPontElement(this);
		
		CAnimation anim = new CAnimation(m_ScriptPont.GetMaterial(), 1, 3, 0.0f);
		
		m_SpriteSheet = new CSpriteSheet(m_GameObject);
		
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
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Process(float fDeltatime)
	{
		base.Process(fDeltatime);
		m_SpriteSheet.Process();
	}
	
	public CSpriteSheet GetSpriteSheet()
	{
		return m_SpriteSheet;	
	}
}
