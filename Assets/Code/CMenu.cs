using UnityEngine;
using System.Collections;

public class CMenu : MonoBehaviour{
	
	public enum EmenuState
	{
		e_menuState_splash,
		e_menuState_main,
		e_menuState_credits,
		e_menuState_movie,
		e_menuState_inGame,
		e_menuState_menuWinLoose,
	}
	
	EmenuState m_EState;
	
	public Texture m_Texture_Fond;
	public Texture m_Texture_ButtonPlay;
	public Texture m_Texture_ButtonCredit;
	public Texture m_Texture_ButtonMenu;
	public Texture m_Texture_ButtonQuit;
	public Texture m_Texture_ButtonPause;
	public Texture m_Texture_ButtonOk;
	public Texture m_Texture_Splash;
	public Texture m_Texture_Credit;
	public Texture m_Texture_Blue;
	public Texture m_Texture_Red;
	public Texture m_Texture_Lost;
	public MovieTexture m_Texture_movie_intro;
	Texture m_Texture_PlayerWin;
	
	float m_fTempsSplash;
	const float m_fTempsSplashInit = 2.0f;
	float m_fTempsVideoIntro;
	bool m_bGamePaused;
	CGame m_Game;
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	void PauseGame()
	{
		Time.timeScale = 0;
		m_bGamePaused = true;
		gameObject.GetComponent<CGame>().PauseGame();
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	void ResumeGame()
	{
		Time.timeScale = 1;
		m_bGamePaused = false;
		gameObject.GetComponent<CGame>().StartGame();
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	bool GameIsPaused()
	{
		return m_bGamePaused;
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void SetMenuState(EmenuState eState)
	{
		m_EState = eState;
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void SetTexturePlayerWin(Texture TexturePlayerWin)
	{
		m_Texture_PlayerWin = TexturePlayerWin;
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start()
	{
		m_Game = gameObject.GetComponent<CGame>();
		m_fTempsSplash = 0.0f;
		m_fTempsVideoIntro = 0.0f;
		m_bGamePaused = false;
		if(!m_Game.IsNotUseMasterGame())	
		{
			m_EState = EmenuState.e_menuState_splash;
			m_fTempsSplash = m_fTempsSplashInit;
		}
		else {
			m_EState = EmenuState.e_menuState_inGame;
		}
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update()
	{
		if(m_EState == EmenuState.e_menuState_splash && m_fTempsSplash>0.0f)
			m_fTempsSplash -= Time.deltaTime;
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnGUI() 
	{
		switch(m_EState)
		{
			case EmenuState.e_menuState_splash:
			{
				if(m_fTempsSplash > 0.0f)
				{
					float fCoeffScale = 1.0f + (m_fTempsSplashInit - m_fTempsSplash)/(10.0f*m_fTempsSplashInit);
					float fWidth = 1280 * fCoeffScale;
					float fHeight = 800 * fCoeffScale;
					GUI.DrawTexture(new Rect((1280 - fWidth)/2.0f, (800 - fHeight)/2.0f, fWidth, fHeight), m_Texture_Splash);
				}
				else
					m_EState = EmenuState.e_menuState_movie;
				break;
			}	
			
			case EmenuState.e_menuState_movie:
			{
				if(m_Texture_movie_intro != null)
					GUI.DrawTexture(new Rect(0, 0, 1280, 800), m_Texture_movie_intro);
				if(m_Texture_movie_intro != null && m_fTempsVideoIntro == 0.0f)
					m_Texture_movie_intro.Play();
				if(m_Texture_movie_intro != null && m_Texture_movie_intro.isPlaying)
				{
					m_fTempsVideoIntro += Time.deltaTime;
				}
				else 
				{
					//m_Texture_movie_intro.Stop();
					m_EState = EmenuState.e_menuState_main;
				}
				break;	
			}
			
			case EmenuState.e_menuState_main:
			{
				GUI.DrawTexture(new Rect(0, 0, 1280, 800), m_Texture_Fond);
			
				if (GUI.Button(new Rect(390, 100, 500, 150), m_Texture_ButtonPlay))
				{
		            m_Game.StartGame();
					ResumeGame();
					m_EState = EmenuState.e_menuState_inGame;
				}
				
				if (GUI.Button(new Rect(390, 400, 500, 150), m_Texture_ButtonCredit))
				{
					m_EState = EmenuState.e_menuState_credits;
				}
			
				if (GUI.Button(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu))
				{
					m_EState = EmenuState.e_menuState_main;
				}
			
				if (GUI.Button(new Rect(1160, 10, 60, 60), m_Texture_ButtonQuit))
				{
					Application.Quit();
				}
				break;
			}	
			
			case EmenuState.e_menuState_credits:
			{
				GUI.DrawTexture(new Rect(0, 0, 1280, 800), m_Texture_Credit);
			
				if (GUI.Button(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu))
				{
					m_EState = EmenuState.e_menuState_main;
				}
			
				if (GUI.Button(new Rect(1160, 10, 60, 60), m_Texture_ButtonQuit))
				{
					Application.Quit();
				}
				break;
			}	
			
			case EmenuState.e_menuState_inGame:
			{
				//if (GUI.Button(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu))
				if (GUI.Button(new Rect(10, 10, 200, 60), m_Texture_ButtonMenu))
				{
					m_EState = EmenuState.e_menuState_main;
					PauseGame();
				}
			
				if (GUI.Button(new Rect(1160, 10, 60, 60), m_Texture_ButtonQuit))
				{
					Application.Quit();
				}
			
				if (GUI.Button(new Rect(250, 10, 60, 60), m_Texture_ButtonPause))
				{
					if(!m_bGamePaused)
					{
						PauseGame();
					}
					else 
					{
						ResumeGame();
					}
				}
				break;
			}	
			case EmenuState.e_menuState_menuWinLoose:
			{
				PauseGame();
				float fPosX = m_Game.GetSizeScreen().x / 2.0f;
				float fPosY = m_Game.GetSizeScreen().y / 2.0f;
				float fWidth = 300.0f;
				float fSizeSprite = 200.0f;
				if(m_Game.IsWin())
				{
					GUI.DrawTexture(new Rect(0, fPosY - fWidth/2.0f, m_Game.GetSizeScreen().x, fWidth), m_Texture_Blue);
					GUI.DrawTexture(new Rect(fPosX - fSizeSprite/2.0f, fPosY - fSizeSprite/2.0f, fSizeSprite, fSizeSprite), m_Texture_PlayerWin);
					
				}
				else
				{
					GUI.DrawTexture(new Rect(0, fPosY - fWidth/2.0f, m_Game.GetSizeScreen().x, fWidth), m_Texture_Red);
					GUI.DrawTexture(new Rect(fPosX - fSizeSprite/2.0f, fPosY - fSizeSprite/2.0f, fSizeSprite, fSizeSprite), m_Texture_Lost);
				}
			
				if (GUI.Button(new Rect(390, 100, 500, 150), m_Texture_ButtonOk))
				{	
					m_EState = EmenuState.e_menuState_inGame;
					ResumeGame();
					m_Game.GoToNextLevelOrRestart();
				}
				break;
			}
		}
    }
}
