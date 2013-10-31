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
	
	void OnCollisionEnter(Collision other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).getGameObject())
			{
				gameObject.transform.Translate(new Vector3(10,10,0));
			}
			else if(other.gameObject.CompareTag("Laser"))
			{
				m_Caisse.Choc();	
			}
			
		}
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
