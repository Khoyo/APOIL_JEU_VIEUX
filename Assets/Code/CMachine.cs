using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CMachine : CElement 
{
	CSpriteSheet m_SpriteSheet;
	
	
	CScriptMachine m_ScriptMachine;
	CMachineActiveZone m_ActiveZone;	
	

	public CMachine()
	{
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Init()
	{	
		base.Init();
		
		//m_GameObject = obj;
		m_ScriptMachine = m_GameObject.GetComponent<CScriptMachine>();
		m_ScriptMachine.SetMachine(this);
		m_ActiveZone = m_GameObject.transform.GetComponentInChildren<CMachineActiveZone>();
		m_ActiveZone.Init(this);
		
		m_SpriteSheet = new CSpriteSheet(m_GameObject);
		m_SpriteSheet.Init();
		m_SpriteSheet.SetAnimation(m_ScriptMachine.GetAnimation());
		
		Component[] components = m_GameObject.GetComponents<Component>();
		foreach(Component component in components){
			IMachineAction action = component as IMachineAction;
			if(action != null)
				action.Init();
		}
		
		// appel a la main des script de l'objet
		//m_ScriptMachine.Init();
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public override  void Reset()
	{
		base.Reset();
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Process(float fDeltatime)
	{
		if(!m_GameObject.active)
			return;
		
		base.Process(fDeltatime);
		m_SpriteSheet.Process();
		
		Component[] components = m_GameObject.GetComponents<Component>();
		foreach(Component component in components){
			IMachineAction action = component as IMachineAction;
			if(action != null)
				action.Process();
		}
		
		// appel a la main des script de l'objet
		//m_ScriptMachine.Process();
		m_ActiveZone.Process();
	}
	
	public void Activate(CPlayer player){
		Component[] components = m_GameObject.GetComponents<Component>();
		
		foreach(Component component in components){
			IMachineAction action = component as IMachineAction;
			if(action != null)
				action.Activate(player);
		}
	}
	
	public CSpriteSheet GetSpriteSheet()
	{
		return m_SpriteSheet;	
	}
	

	
}

