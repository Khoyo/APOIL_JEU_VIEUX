using UnityEngine;
using System.Collections;

public class CScriptButton : MonoBehaviour 
{
	CScriptZoneInGame m_Zone;
	CSpriteSheet m_SpriteSheet;
	CGame m_Game;
	
	// Use this for initialization
	void Start () 
	{
		m_Zone = gameObject.transform.parent.GetComponent<CScriptZoneInGame>();
		
		CAnimation anim = new CAnimation(gameObject.renderer.material, 2, 1, 60.0f);
		
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		
		m_SpriteSheet = new CSpriteSheet(gameObject);
			
		m_SpriteSheet.Init();
		m_SpriteSheet.SetAnimation(anim);
		m_SpriteSheet.setEndCondition(CSpriteSheet.EEndCondition.e_FramPerFram);
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_SpriteSheet.Process();
	}
	
	void OnTriggerStay(Collider other) 
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				if(m_Game.getLevel().getPlayer(i).GetPlayerInput().ClickButton)
				{
					Debug.Log("clic!!");
					m_Zone.SwitchPowerState();
				}
			}
			
		}
	}
}
