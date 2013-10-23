using UnityEngine;
using System.Collections;

public class CScriptPont : MonoBehaviour 
{
	CGame m_Game;
	CPont m_Pont;
	CSpriteSheet m_SpriteSheet;
	
	public Material m_material;
	
	// Use this for initialization
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_Game.getLevel().CreateElement<CPont>(gameObject);

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
 	 void OnTriggerEnter(Collider other) 
	{
		Debug.Log("yo men!");
		m_Pont.GetSpriteSheet().GoToNextFram();
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetPontElement(CPont obj)
	{
		m_Pont = obj;
	}
	
	public Material GetMaterial()
	{
		return m_material;	
	}
}
