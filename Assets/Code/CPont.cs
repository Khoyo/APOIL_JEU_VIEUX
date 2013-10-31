using UnityEngine;
using System.Collections;

public class CPont : CElement 
{
	CScriptPont m_ScriptPont;
	CSpriteSheet m_SpriteSheet;
	GameObject m_SubObject1;
	GameObject m_SubObject2;
	
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
		
		CAnimation anim = new CAnimation(m_ScriptPont.GetMaterial(), 1, 3, 60.0f);
		
		m_SpriteSheet = new CSpriteSheet(m_GameObject);
		
		m_SpriteSheet.Init();
		m_SpriteSheet.SetAnimation(anim);
		m_SpriteSheet.setEndCondition(CSpriteSheet.EEndCondition.e_FramPerFram);
		
		SetSubObjects(m_GameObject.transform.FindChild("Border1").gameObject, m_GameObject.transform.FindChild("Border2").gameObject);
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public override void Reset()
	{
		base.Reset();
		m_SpriteSheet.Reset();
		m_ScriptPont.Reset();
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
	
	public void SetSubObjects(GameObject sub1, GameObject sub2)
	{
		m_SubObject1 = sub1;
		m_SubObject2 = sub2;
	}
	
	public GameObject GetSubObject1()
	{
		return m_SubObject1;
	}
	
	public GameObject GetSubObject2()
	{
		return m_SubObject2;
	}
}
