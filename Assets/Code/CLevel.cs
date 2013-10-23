using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CLevel
{
	CGame m_Game;
	CPlayer[] m_Players;
	float m_bTimerLightSwitch;
	GameObject m_ObjetLevel;
	
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
		
		foreach(CElement elem in m_pElement)
			elem.Reset();
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void Process(float fDeltatime)
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
			m_Players[i].Process(fDeltatime);
		
		GestionCameraTwoPlayers();
		
		if(Input.GetKey(KeyCode.L) && m_bTimerLightSwitch <= 0){
			m_Game.m_bLightIsOn = !m_Game.m_bLightIsOn;
			m_bTimerLightSwitch = 10f;
		}
		m_bTimerLightSwitch -= 0.5f;
		if(m_Game.m_bLightIsOn == false)
			TurnLight(false);
		else
			TurnLight(true);
		
		foreach(CElement elem in m_pElement)
			elem.Process(fDeltatime);
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void TurnLight(bool bOn)
	{
		GameObject[] ShipLight;
		ShipLight = GameObject.FindGameObjectsWithTag("ShipLight");
		
		foreach(GameObject currentLight in ShipLight)
		{
			if(bOn)
			{
				currentLight.SetActiveRecursively(true);
			}
			else
			{
				currentLight.SetActiveRecursively(false);
				currentLight.active = true;
			}
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
	
	public void UnregisterElement(CElement elem)
	{
		m_pElement.Remove(elem);
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void CreatePlayers()
	{
		Vector3 pos3D = m_ObjetLevel.transform.FindChild("LevelIn").position;
		Vector2 posInit = new Vector2 (pos3D.x, pos3D.y);
		float fSizePlayer = 100.0f;
		Vector2[] posPlayer = new Vector2[4];
		posPlayer[0] = new Vector2(pos3D.x - fSizePlayer / 2.0f, pos3D.y - fSizePlayer / 2.0f);
		posPlayer[1] = new Vector2(pos3D.x + fSizePlayer / 2.0f, pos3D.y - fSizePlayer / 2.0f);
		posPlayer[2] = new Vector2(pos3D.x + fSizePlayer / 2.0f, pos3D.y + fSizePlayer / 2.0f);
		posPlayer[3] = new Vector2(pos3D.x - fSizePlayer / 2.0f, pos3D.y + fSizePlayer / 2.0f);
		//Vector2 posInit = new Vector2 (0,0);
		int nNbPlayer = m_Game.m_nNbPlayer;
		
		
		SAnimationPlayer[] AnimPlayer = new SAnimationPlayer[4];
		
		AnimPlayer[0].AnimRepos = new CAnimation(m_Game.m_materialPlayer1Repos, 1, 1, 1.0f);
		AnimPlayer[0].AnimHorizontal = new CAnimation(m_Game.m_materialPlayer1Horizontal, 7, 4, 0.5f);
		AnimPlayer[0].AnimVertical = new CAnimation(m_Game.m_materialPlayer1Vertical, 6, 1, 2.0f);
		AnimPlayer[0].AnimDie = new CAnimation(m_Game.m_materialPlayer1Die, 3, 2, 1.0f);
		
		AnimPlayer[1].AnimRepos = new CAnimation(m_Game.m_materialPlayer2Repos, 1, 1, 1.0f);
		AnimPlayer[1].AnimHorizontal = new CAnimation(m_Game.m_materialPlayer2Horizontal, 7, 4, 0.5f);
		AnimPlayer[1].AnimVertical = new CAnimation(m_Game.m_materialPlayer2Vertical, 6, 1, 2.0f);
		AnimPlayer[1].AnimDie = new CAnimation(m_Game.m_materialPlayer2Die, 3, 2, 1.0f);
		
		AnimPlayer[2].AnimRepos = new CAnimation(m_Game.m_materialPlayer3Repos, 1, 1, 1.0f);
		AnimPlayer[2].AnimHorizontal = new CAnimation(m_Game.m_materialPlayer3Horizontal, 7, 4, 0.5f);
		AnimPlayer[2].AnimVertical = new CAnimation(m_Game.m_materialPlayer3Vertical, 6, 1, 2.0f);
		AnimPlayer[2].AnimDie = new CAnimation(m_Game.m_materialPlayer3Die, 3, 2, 1.0f);
		
		AnimPlayer[3].AnimRepos = new CAnimation(m_Game.m_materialPlayer4Repos, 1, 1, 1.0f);
		AnimPlayer[3].AnimHorizontal = new CAnimation(m_Game.m_materialPlayer4Horizontal, 7, 4, 0.5f);
		AnimPlayer[3].AnimVertical = new CAnimation(m_Game.m_materialPlayer4Vertical, 6, 1, 2.0f);
		AnimPlayer[3].AnimDie = new CAnimation(m_Game.m_materialPlayer4Die, 3, 2, 1.0f);
		
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			
			CPlayer.EIdPlayer eIdPlayer = SetIdPlayer(i);
			m_Players[i] = new CPlayer(posPlayer[i], eIdPlayer, AnimPlayer[i]);
		}
		
	}
	
	public void SetPlayerPosition()
	{
		Vector3 pos3D = m_ObjetLevel.transform.FindChild("LevelIn").position;
		Vector2 posInit = new Vector2 (pos3D.x, pos3D.y);
		float fSizePlayer = 100.0f;
		Vector2[] posPlayer = new Vector2[4];
		posPlayer[0] = new Vector2(pos3D.x - fSizePlayer / 2.0f, pos3D.y - fSizePlayer / 2.0f);
		posPlayer[1] = new Vector2(pos3D.x + fSizePlayer / 2.0f, pos3D.y - fSizePlayer / 2.0f);
		posPlayer[2] = new Vector2(pos3D.x + fSizePlayer / 2.0f, pos3D.y + fSizePlayer / 2.0f);
		posPlayer[3] = new Vector2(pos3D.x - fSizePlayer / 2.0f, pos3D.y + fSizePlayer / 2.0f);
		//Vector2 posInit = new Vector2 (0,0);
		int nNbPlayer = m_Game.m_nNbPlayer;
		
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			m_Players[i].SetPosition2D(posPlayer[i]);
		}
	}
	
	public void StartLevel()
	{
		SetPlayerPosition();
		
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
	public void GestionCameraTwoPlayers()
	{
		Vector2 posPlayer1 = new Vector2(m_Players[0].getGameObject().transform.position.x, m_Players[0].getGameObject().transform.position.y);
		Vector2 posPlayer2 = new Vector2(m_Players[1].getGameObject().transform.position.x, m_Players[1].getGameObject().transform.position.y);
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
	
}
