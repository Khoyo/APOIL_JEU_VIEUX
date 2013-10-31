using UnityEngine;
using System.Collections;

public class CCaisse : CElement 
{

	CScriptCaisse m_ScriptCaisse;
	CSpriteSheet m_SpriteSheet;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CCaisse()
	{
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Init()
	{	
		base.Init();
		m_ScriptCaisse = m_GameObject.GetComponent<CScriptCaisse>();
		m_ScriptCaisse.SetCaisseElement(this);
		
		CAnimation anim = new CAnimation(m_ScriptCaisse.GetMaterial(), 1, 3, 60.0f);
		
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
		m_ScriptCaisse.Reset();
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
