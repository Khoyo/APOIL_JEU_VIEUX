using UnityEngine;
using System.Collections;

public class CScriptCaisse : MonoBehaviour {

	CGame m_Game;
	CCaisse m_Caisse;
	
	public Material m_material;
	
	// Use this for initialization
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_Game.getLevel().CreateElement<CCaisse>(gameObject);		
	}
	
	public void Reset()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetCaisseElement(CCaisse obj)
	{
		m_Caisse = obj;
	}
	
	public Material GetMaterial()
	{
		return m_material;	
	}
}
