using UnityEngine;
using System.Collections;

public class CTakeElement : CElement {
	
	public enum ETypeObject
	{
		e_TypeObject_NoTakeElement,
		e_TypeObject_Battery
	}
	
	ETypeObject m_eTypeObject;
	CScriptTakeElement m_ScriptTakeElement;
	public bool m_bHeld;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CTakeElement()
	{
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public override void Init()
	{	
		base.Init();
		m_ScriptTakeElement = m_GameObject.GetComponent<CScriptTakeElement>();
		m_ScriptTakeElement.SetTakeElement(this);
		m_eTypeObject = m_GameObject.GetComponent<CScriptTakeElement>().GetTypeElement();
		
		// appel a la main des script de l'objet
		//m_ScriptTakeElement.Init();	
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
		
		// appel a la main des script de l'objet
		//m_ScriptTakeElement.Process();
	}
	
	public void Held()
	{
		m_bHeld = true;
	}
	
	public void Drop()
	{
		m_bHeld = false;
	}
	
	public ETypeObject GetTypeElement()
	{
		return m_eTypeObject;
	}
}
