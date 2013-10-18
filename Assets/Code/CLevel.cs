using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CLevel
{
	CGame m_Game;
	CPlayer[] m_Players;
	CMonster m_Monster;
	float m_bTimerLightSwitch;
	
	List<CElement> m_pElement;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CLevel()
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		
		Vector2 posInitM = new Vector2(100.0f, 0.0f);
		
		m_Players = new CPlayer[m_Game.m_nNbPlayer];
		CreatePlayers();

		m_Monster = new CMonster(posInitM);
		m_bTimerLightSwitch = 0;
		
		m_pElement = new List<CElement>();
		
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void Init()
	{	
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
			m_Players[i].Init();
		
		m_Monster.Init();
		
		foreach(CElement elem in m_pElement)
			elem.Init();
		
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void Reset()
	{
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
			m_Players[i].Reset();
		m_Monster.Reset();
		
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
		m_Monster.Process(fDeltatime);
		
		SetCameraPosition();
		
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
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void CreatePlayers()
	{
		Vector2 posInit = new Vector2(0.0f, 0.0f);
		int nNbPlayer = m_Game.m_nNbPlayer;
		
		SAnimationPlayer[] AnimPlayer = new SAnimationPlayer[4];
		
		AnimPlayer[0].AnimRepos = new CAnimation(m_Game.m_materialPlayer1Repos, 1, 1, 1.0f);
		AnimPlayer[0].AnimHorizontal = new CAnimation(m_Game.m_materialPlayer1Horizontal, 7, 4, 0.5f);
		AnimPlayer[0].AnimVertical = new CAnimation(m_Game.m_materialPlayer1Vertical, 6, 1, 2.0f);
		
		AnimPlayer[1].AnimRepos = new CAnimation(m_Game.m_materialPlayer2Repos, 1, 1, 1.0f);
		AnimPlayer[1].AnimHorizontal = new CAnimation(m_Game.m_materialPlayer2Horizontal, 7, 4, 0.5f);
		AnimPlayer[1].AnimVertical = new CAnimation(m_Game.m_materialPlayer2Vertical, 6, 1, 2.0f);
		
		AnimPlayer[2].AnimRepos = new CAnimation(m_Game.m_materialPlayer3Repos, 1, 1, 1.0f);
		AnimPlayer[2].AnimHorizontal = new CAnimation(m_Game.m_materialPlayer3Horizontal, 7, 4, 0.5f);
		AnimPlayer[2].AnimVertical = new CAnimation(m_Game.m_materialPlayer3Vertical, 6, 1, 2.0f);
		
		AnimPlayer[3].AnimRepos = new CAnimation(m_Game.m_materialPlayer4Repos, 1, 1, 1.0f);
		AnimPlayer[3].AnimHorizontal = new CAnimation(m_Game.m_materialPlayer4Horizontal, 7, 4, 0.5f);
		AnimPlayer[3].AnimVertical = new CAnimation(m_Game.m_materialPlayer4Vertical, 6, 1, 2.0f);
		
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			CPlayer.EIdPlayer eIdPlayer = SetIdPlayer(i);
			m_Players[i] = new CPlayer(posInit, eIdPlayer, AnimPlayer[i]);
			posInit[1] += 150.0f;
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
	public void SetCameraPosition()
	{
		Vector2 posPlayer1 = new Vector2(m_Players[0].getGameObject().transform.position.x, m_Players[0].getGameObject().transform.position.y);
		Vector2 posPlayer2 = new Vector2(m_Players[1].getGameObject().transform.position.x, m_Players[1].getGameObject().transform.position.y);
		Vector2 posCenter = (posPlayer2 + posPlayer1)/2;
		m_Game.getCamera().SetPosition(posCenter);
		m_Game.getCamera().SetFactorScale((posPlayer1 - posPlayer2).magnitude);
		
		//Debug
	//	Debug.DrawLine (m_Players[0].getGameObject().transform.position, m_Players[1].getGameObject().transform.position);
		Debug.DrawLine(m_Players[0].getGameObject().transform.position, new Vector3(posCenter.x, posCenter.y, 0));
	}
	

	
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CPlayer getPlayer(int i)
	{
		return m_Players[i];
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CMonster getMonster()
	{
		return m_Monster;
	}
}
