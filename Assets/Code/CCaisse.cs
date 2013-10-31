using UnityEngine;
using System.Collections;

public class CCaisse : CElement 
{
	CScriptCaisse m_ScriptCaisse;
	CSpriteSheet m_SpriteSheet;
	CGame m_Game;
	int m_nNbImpact;
	bool m_bDestroy;

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CCaisse()
	{
		m_nNbImpact = 0;
		m_bDestroy = false;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Init()
	{	
		base.Init();
		m_ScriptCaisse = m_GameObject.GetComponent<CScriptCaisse>();
		m_ScriptCaisse.SetCaisseElement(this);
		
		CAnimation anim = new CAnimation(m_ScriptCaisse.GetMaterial(), 1, 6, 60.0f);
		
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
		m_ScriptCaisse.Reset();
		m_nNbImpact = 0;
		m_bDestroy = false;
		m_GameObject.active = true;
		m_GameObject.collider.enabled = true;
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
	
	public void Choc()
	{
		if(m_nNbImpact < m_Game.m_nNbImpactMaxCaisse)
		{
			++m_nNbImpact;
			m_SpriteSheet.GoToNextFram();
		}
		else if(!m_bDestroy)
		{
			m_bDestroy = true;
			m_GameObject.collider.enabled = false;
			m_SpriteSheet.GoToNextFram();
		}
	}
}
