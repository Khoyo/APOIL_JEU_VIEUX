using UnityEngine;
using System.Collections;

public class CGravityMonsterChoppe : MonoBehaviour {

	public GameObject objetGravity;
	
	CGravityMonster m_GravityMonster;

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () 
	{
		m_GravityMonster = objetGravity.GetComponent<CGravityMonster>();
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () 
	{
		
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnTriggerStay2D(Collider2D other) 
	{
		if(other.CompareTag("Player"))
		{
			m_GravityMonster.ChoppePlayer(other.gameObject);
		}
	}

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnTriggerExit2D(Collider2D other) 
	{
		if(other.CompareTag("Player"))
		{
			other.gameObject.GetComponent<CPlayer>().StopGravity();
			m_GravityMonster.SetState(CGravityMonster.EState.e_alerte);
		}
	}
}
