using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CLevel
{
	CGame m_Game;
	CPlayer[] m_Players;
	float m_bTimerLightSwitch;
	GameObject m_ObjetLevel;
	List<CScriptZoneInGame> m_pObjetsZone;
	List<CElement> m_pElement;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CLevel()
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
				
		m_Players = new CPlayer[m_Game.m_nNbPlayer];

		m_bTimerLightSwitch = 0;
		
		m_pElement = new List<CElement>();		
		m_pObjetsZone = new List<CScriptZoneInGame>();;
		
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void Init()
	{	
		CreatePlayers();
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
			m_Players[i].Init();
		
		
		foreach(CElement elem in m_pElement)
			elem.Init();
		
		GameObject[] pObj = GameObject.FindGameObjectsWithTag("ZoneInGame");	
		foreach (GameObject currentObj in pObj)
		{
			m_pObjetsZone.Add(currentObj.transform.GetComponent<CScriptZoneInGame>());
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void Reset()
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			m_Players[i].Reset();
		}
		
		CElement[] pElement = new CElement[m_pElement.Count];
		m_pElement.CopyTo(pElement);
		
		foreach(CElement elem in pElement)
			elem.Reset();
		
		SetPlayerPosition();
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void Process(float fDeltatime)
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
			m_Players[i].Process(fDeltatime);
		
		GestionCameraFromPlayers();
		
		/*
		if(Input.GetKey(KeyCode.L) && m_bTimerLightSwitch <= 0)
		{
			m_bTimerLightSwitch = 10f;
			
			if(m_Game.m_bLightIsOn == false)
				TurnLight(true);
			else
				TurnLight(false);
		}*/
		
		if(Input.GetKey(KeyCode.L))
		{
			SetPlayerPosition();	
		}
		
		if(m_bTimerLightSwitch > 0.0f)
			m_bTimerLightSwitch -= 0.5f;
		
		CElement[] pElement = new CElement[m_pElement.Count];
		m_pElement.CopyTo(pElement);
		
		foreach(CElement elem in pElement)
			elem.Process(fDeltatime);
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void TurnLight(bool bOn)
	{
		m_Game.m_bLightIsOn = bOn;
		foreach(CScriptZoneInGame currentZone in m_pObjetsZone)
		{
			currentZone.TurnLight(bOn);
		}
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void CreateElement<ElemType>(GameObject obj) where ElemType : CElement, new()
	{	
		ElemType elem = new ElemType();
		elem.setGameObject(obj);
		elem.Init();
		m_pElement.Add(elem);
		
	}
	
	public void RegisterElement(CElement elem){
		m_pElement.Add(elem);
	}
	
	public void UnregisterElement(CElement elem)
	{
		m_pElement.Remove(elem);
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void CreatePlayers()
	{	
		SAnimationPlayer[] AnimPlayer = new SAnimationPlayer[4];
		
		for (int i = 0 ; i < 4 ; ++i)
		{
			float fFPS = 4.0f;
			AnimPlayer[i].AnimRepos = new CAnimation(m_Game.m_materialPlayer1Repos, 4, 1, fFPS);
			
			AnimPlayer[i].AnimUpUp = new CAnimation(	m_Game.m_materialPlayer1UpUp,	4, 1, fFPS);
			AnimPlayer[i].AnimUpDown = new CAnimation(	m_Game.m_materialPlayer1UpDown,	4, 1, fFPS);
			AnimPlayer[i].AnimUpLeft = new CAnimation(	m_Game.m_materialPlayer1UpLeft, 4, 1, fFPS);
			AnimPlayer[i].AnimUpRight= new CAnimation(	m_Game.m_materialPlayer1UpRight, 4, 1, fFPS);
			
			AnimPlayer[i].AnimDownUp = new CAnimation(m_Game.m_materialPlayer1DownUp, 4, 1, fFPS);
			AnimPlayer[i].AnimDownDown = new CAnimation(m_Game.m_materialPlayer1DownDown, 4, 1, fFPS);
			AnimPlayer[i].AnimDownLeft = new CAnimation(m_Game.m_materialPlayer1DownLeft, 4, 1, fFPS);
			AnimPlayer[i].AnimDownRight = new CAnimation(m_Game.m_materialPlayer1DownRight, 4, 1, fFPS);
			
			AnimPlayer[i].AnimLeftUp = new CAnimation(m_Game.m_materialPlayer1LeftUp, 4, 1, fFPS);
			AnimPlayer[i].AnimLeftDown = new CAnimation(m_Game.m_materialPlayer1LeftDown, 4, 1, fFPS);
			AnimPlayer[i].AnimLeftLeft = new CAnimation(m_Game.m_materialPlayer1LeftLeft, 4, 1, fFPS);
			AnimPlayer[i].AnimLeftRight = new CAnimation(m_Game.m_materialPlayer1LeftRight, 4, 1, fFPS);
			
			AnimPlayer[i].AnimRightUp = new CAnimation(m_Game.m_materialPlayer1RightUp, 4, 1, fFPS);
			AnimPlayer[i].AnimRightDown = new CAnimation(m_Game.m_materialPlayer1RightDown, 4, 1, fFPS);
			AnimPlayer[i].AnimRightLeft = new CAnimation(m_Game.m_materialPlayer1RightLeft, 4, 1, fFPS);
			AnimPlayer[i].AnimRightRight = new CAnimation(m_Game.m_materialPlayer1RightRight, 4, 1, fFPS);
			
			AnimPlayer[i].AnimDieHeadCut = new CAnimation(	m_Game.m_materialPlayer1DieHeadCut, 3, 2, 1.0f);
			AnimPlayer[i].AnimDieFall = new CAnimation(	m_Game.m_materialPlayer1DieFall, 3, 2, 3.0f);
			AnimPlayer[i].AnimDieGravity = new CAnimation(	m_Game.m_materialPlayer1DieGravity, 8, 1, 3.0f);
		}
		
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{	
			CPlayer.EIdPlayer eIdPlayer = SetIdPlayer(i);
			m_Players[i] = new CPlayer(new Vector2(0,0), eIdPlayer, AnimPlayer[i]);
		}
		
		SetPlayerPosition();
	}
	
	public void SetPlayerPosition()
	{
		Vector3 pos3D = m_ObjetLevel.transform.FindChild("LevelIn").position;
		float fSizePlayer = 100.0f;
		Vector2[] posPlayer = new Vector2[4];
		posPlayer[0] = new Vector2(pos3D.x - fSizePlayer / 2.0f, pos3D.y - fSizePlayer / 2.0f);
		posPlayer[1] = new Vector2(pos3D.x + fSizePlayer / 2.0f, pos3D.y - fSizePlayer / 2.0f);
		posPlayer[2] = new Vector2(pos3D.x + fSizePlayer / 2.0f, pos3D.y + fSizePlayer / 2.0f);
		posPlayer[3] = new Vector2(pos3D.x - fSizePlayer / 2.0f, pos3D.y + fSizePlayer / 2.0f);
		//Vector2 posInit = new Vector2 (0,0);
	
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			m_Players[i].SetPosition2D(posPlayer[i]);
			m_Players[i].ResetPosInit(posPlayer[i]);
		}
	}
	
	public void StartLevel()
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			m_Players[i].LaunchStargate();
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	CPlayer.EIdPlayer SetIdPlayer(int nId)
	{
		CPlayer.EIdPlayer eId = CPlayer.EIdPlayer.e_IdPlayer_Player1;
		switch(nId)
		{
			case 0:
				eId = CPlayer.EIdPlayer.e_IdPlayer_Player1;
				break;
			case 1:
				eId = CPlayer.EIdPlayer.e_IdPlayer_Player2;
				break;
			case 2:
				eId = CPlayer.EIdPlayer.e_IdPlayer_Player3;
				break;
			case 3:
				eId = CPlayer.EIdPlayer.e_IdPlayer_Player4;
				break;
		}
		return eId;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void GestionCameraFromPlayers()
	{
		if(m_Game.m_nNbPlayer == 1)
		{
			Vector2 posPlayer1 = new Vector2(m_Players[0].GetGameObject().transform.position.x, m_Players[0].GetGameObject().transform.position.y);
			m_Game.getCamera().SetPosition(posPlayer1);
			m_Game.getCamera().SetFactorScale(m_Game.m_fCameraDezoomMin);
		}
		
		if(m_Game.m_nNbPlayer == 2)
		{
			Vector2 posPlayer1 = new Vector2(m_Players[0].GetGameObject().transform.position.x, m_Players[0].GetGameObject().transform.position.y);
			Vector2 posPlayer2 = new Vector2(m_Players[1].GetGameObject().transform.position.x, m_Players[1].GetGameObject().transform.position.y);
			Vector2 posCenter = new Vector2(0,0);
			float fSize = 400.0f;
			if(m_Players[0].IsAlive() && m_Players[1].IsAlive()) 
			{
				posCenter = (posPlayer2 + posPlayer1)/2;
				fSize = (posPlayer1 - posPlayer2).magnitude;
			}
			else if(m_Players[0].IsAlive() && !m_Players[1].IsAlive())
			{
				posCenter = posPlayer1;
				fSize = 400.0f;
			}
			else if(!m_Players[0].IsAlive() && m_Players[1].IsAlive())
			{
				posCenter = posPlayer2;
				fSize = 400.0f;
			}
			m_Game.getCamera().SetPosition(posCenter);
			m_Game.getCamera().SetFactorScale(fSize);
		}

	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetObjetLevel(GameObject obj)
	{
		m_ObjetLevel = obj;
	}

	
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CPlayer getPlayer(int i)
	{
		return m_Players[i];
	}
	
	public void DeleteCElements()
	{
		m_pElement.Clear();
	}
	
}
