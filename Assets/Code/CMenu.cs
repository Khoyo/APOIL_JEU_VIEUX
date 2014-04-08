using UnityEngine;
using System.Collections;

public class CMenu : MonoBehaviour 
{
	public Texture m_Texture_Player1;
	public Texture m_Texture_Player2;
	public Texture m_Texture_Player3;
	public Texture m_Texture_Player4;

	public Texture m_Texture_Blue;
	public Texture m_Texture_Red;

	enum EState
	{
		e_InGame,
		e_Win
	}

	EState m_eState;
	Texture m_TextureWinPlayer;
	float m_fTimerEndLevel;
	float m_fDeltaTime;
	GameObject m_ObjectGame;


	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () 
	{
		m_eState = EState.e_InGame;
		m_TextureWinPlayer = m_Texture_Player1;
		m_fTimerEndLevel = 0.0f;
		m_fDeltaTime = Time.deltaTime;
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () 
	{
		Debug.Log (1/m_fDeltaTime);
	}

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnGUI()
	{
		switch(m_eState)
		{
			case EState.e_InGame:
			{
				break;
			}
			case EState.e_Win:
			{
				if(m_fTimerEndLevel > 0.0f)
				{
					m_fTimerEndLevel -= m_fDeltaTime;
					float fHeightFond = m_TextureWinPlayer.height * 5.0f/4.0f;
					GUI.DrawTexture(new Rect(0, Screen.height/2.0f - fHeightFond/2.0f, Screen.width, fHeightFond), m_Texture_Blue);
					GUI.DrawTexture(new Rect(Screen.width/2.0f - m_TextureWinPlayer.width/2.0f, Screen.height/2.0f - m_TextureWinPlayer.height/2.0f, m_TextureWinPlayer.width, m_TextureWinPlayer.height), m_TextureWinPlayer);
				}
				else
				{
					m_eState = EState.e_InGame;
					ResumeGame();
				m_ObjectGame.GetComponent<CGame>().GoToNextLevel();
				}
				break;
			}
		}
	}

	public void SetTextureWinPlayer(CPlayer.EIdPlayer eId)
	{
		switch(eId)
		{
			case CPlayer.EIdPlayer.e_Player1:
			{
				m_TextureWinPlayer = m_Texture_Player1;
				break;
			}
			case CPlayer.EIdPlayer.e_Player2:
			{
				m_TextureWinPlayer = m_Texture_Player2;
				break;
			}
			case CPlayer.EIdPlayer.e_Player3:
			{
				m_TextureWinPlayer = m_Texture_Player3;
				break;
			}
			case CPlayer.EIdPlayer.e_Player4:
			{
				m_TextureWinPlayer = m_Texture_Player4;
				break;
			}
		}
	}

	public void Win()
	{
		m_eState = EState.e_Win;
		m_fTimerEndLevel = CGame.ms_fMenuTimerEndLevel;
		PauseGame ();
	}

	public void SetObjectGame(GameObject obj)
	{
		m_ObjectGame = obj;
	}

	void PauseGame()
	{
		Time.timeScale = 0;
	}

	void ResumeGame()
	{
		Time.timeScale = 1;
	}
}
