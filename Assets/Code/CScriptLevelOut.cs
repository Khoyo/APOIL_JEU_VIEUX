using UnityEngine;
using System.Collections;

public class CScriptLevelOut : MonoBehaviour 
{
	
	CGame m_Game;	
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () {
	
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnTriggerEnter(Collider other)
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == m_Game.getLevel().getPlayer(i).GetGameObject())
			{
				CPlayer.EIdPlayer eIdPlayer = CPlayer.EIdPlayer.e_IdPlayer_Player1;
				switch(i)
				{
					case 0:
					{
						eIdPlayer = CPlayer.EIdPlayer.e_IdPlayer_Player1;
						break;	
					}
					case 1:
					{
						eIdPlayer = CPlayer.EIdPlayer.e_IdPlayer_Player2;
						break;	
					}
					case 2:
					{
						eIdPlayer = CPlayer.EIdPlayer.e_IdPlayer_Player3;
						break;	
					}
					case 3:
					{
						eIdPlayer = CPlayer.EIdPlayer.e_IdPlayer_Player4;
						break;	
					}
				}
				m_Game.FinishLevel(true, eIdPlayer);
			}
		}
	}
}
