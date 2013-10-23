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
		m_SpriteSheet = new CSpriteSheet(m_GameObject);
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Init()
	{	
		base.Init();
		m_ScriptPont = m_GameObject.GetComponent<CScriptPont>();
		m_ScriptPont.SetPontElement(this);
		
		m_SpriteSheet.Init();
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public override void Reset()
	{
		base.Reset();
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Process(float fDeltatime)
	{
		base.Process(fDeltatime);
	}	
}
