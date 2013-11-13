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
	
	public enum EMenuMain
	{
		e_Play,
		e_Credit,
		e_Menu,
		e_Quit
	}
	
	EmenuState m_EState;
	EMenuMain m_EMainState;
	
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
	
	float m_fTimerMenuNavigation;
	float m_fTimerMenuNavigationMax = 0.2f;
	
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
		m_fTimerMenuNavigation = 0.0f;
		if(!m_Game.IsNotUseMasterGame())	
		{
			m_EState = EmenuState.e_menuState_splash;
			//m_EState = EmenuState.e_menuState_main;
			m_fTempsSplash = m_fTempsSplashInit;
			m_EMainState = EMenuMain.e_Play;
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
			
				GestionMenuMain(m_Game.m_bPadXBox);
				
				break;
			}	
			
			case EmenuState.e_menuState_credits:
			{
				GUI.DrawTexture(new Rect(0, 0, 1280, 800), m_Texture_Credit);
			
				GestionMenuCredit(m_Game.m_bPadXBox);
				
				break;
			}	
			
			case EmenuState.e_menuState_inGame:
			{
				//if (GUI.Button(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu))
				
				GestionMenuInGame(m_Game.m_bPadXBox);
			
				break;
			}	
			case EmenuState.e_menuState_menuWinLoose:
			{
				PauseGame();
				
				GestionMenuWinLoose(m_Game.m_bPadXBox);
			
				break;
			}
		}
    }
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void GestionMenuMain(bool bManette)
	{
		bool bPlay = false;
		bool bCredit = false;
		bool bMenu = false;
		bool bQuit = false;
		
		if(!bManette)
		{
			if (GUI.Button(new Rect(390, 100, 500, 150), m_Texture_ButtonPlay))
			{
	            bPlay = true;
			}
			
			if (GUI.Button(new Rect(390, 400, 500, 150), m_Texture_ButtonCredit))
			{
				bCredit = true;
			}
		
			if (GUI.Button(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu))
			{
				bMenu = true;
			}
		
			if (GUI.Button(new Rect(1160, 10, 60, 60), m_Texture_ButtonQuit))
			{
				bQuit = true;
			}
		}
		else
		{
			
			if(m_fTimerMenuNavigation > m_fTimerMenuNavigationMax)
			{
				switch(m_EMainState)
				{
					case EMenuMain.e_Play :
					{
						if(CApoilInput.MenuValidate)
							bPlay = true;
						if(CApoilInput.MenuUp)
						{
							m_EMainState = EMenuMain.e_Menu;
							m_fTimerMenuNavigation = 0.0f;
						}
						else if (CApoilInput.MenuDown)
						{
							m_EMainState = EMenuMain.e_Credit;
							m_fTimerMenuNavigation = 0.0f;
						}
						break;	
					}
					
					case EMenuMain.e_Credit :
					{
						if(CApoilInput.MenuValidate)
							bCredit = true;
						if(CApoilInput.MenuUp)
						{
							m_EMainState = EMenuMain.e_Play;
							m_fTimerMenuNavigation = 0.0f;
						}
						else if (CApoilInput.MenuDown)
						{
							m_EMainState = EMenuMain.e_Credit;
							m_fTimerMenuNavigation = 0.0f;
						}
						break;	
					}
					
					case EMenuMain.e_Menu :
					{
						if(CApoilInput.MenuValidate)
							bMenu = true;
						if(CApoilInput.MenuUp)
						{
							m_EMainState = EMenuMain.e_Quit;
							m_fTimerMenuNavigation = 0.0f;
						}
						else if (CApoilInput.MenuDown)
						{
							m_EMainState = EMenuMain.e_Play;
							m_fTimerMenuNavigation = 0.0f;
						}
						break;	
					}
					
					case EMenuMain.e_Quit :
					{
						if(CApoilInput.MenuValidate)
							bQuit = true;
						if(CApoilInput.MenuUp)
						{
							m_EMainState = EMenuMain.e_Quit;
							m_fTimerMenuNavigation = 0.0f;
						}
						else if (CApoilInput.MenuDown)
						{
							m_EMainState = EMenuMain.e_Menu;
							m_fTimerMenuNavigation = 0.0f;
						}
						break;	
					}
					
					default:
						m_EMainState = EMenuMain.e_Menu;
						m_fTimerMenuNavigation = 0.0f;
						break;
				}
				
				if(CApoilInput.MenuMenu)
				{
					bPlay = true;
					m_fTimerMenuNavigation = 0.0f;
				}
				
			}
			else
			{
				m_fTimerMenuNavigation += 1/60.0f;
			}			
			
			float fOffsetPlay = (m_EMainState == EMenuMain.e_Play) ? 0.5f : 0.0f;
			float fOffsetCredit = (m_EMainState == EMenuMain.e_Credit) ? 0.5f : 0.0f;
			float fOffsetMenu = (m_EMainState == EMenuMain.e_Menu) ? 0.5f : 0.0f;
			float fOffsetQuit = (m_EMainState == EMenuMain.e_Quit) ? 0.5f : 0.0f;
			
			GUI.DrawTextureWithTexCoords(new Rect(390, 100, 500, 150), m_Texture_ButtonPlay, new Rect(fOffsetPlay, 0, 0.5f, 1));
			GUI.DrawTextureWithTexCoords(new Rect(390, 400, 500, 150), m_Texture_ButtonCredit, new Rect(fOffsetCredit, 0, 0.5f, 1));
			GUI.DrawTextureWithTexCoords(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu, new Rect(fOffsetMenu, 0, 0.5f, 1));
			GUI.DrawTextureWithTexCoords(new Rect(1160, 10, 60, 60), m_Texture_ButtonQuit, new Rect(fOffsetQuit, 0, 0.5f, 1));
		}
		
		NagigationMenuMain(bPlay, bCredit, bMenu, bQuit, false);
		
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void GestionMenuCredit(bool bManette)
	{
		bool bMenu = false;
		bool bQuit = false;
		
		if(!bManette)
		{
			if (GUI.Button(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu))
			{
				bMenu = true;
			}
		
			if (GUI.Button(new Rect(1160, 10, 60, 60), m_Texture_ButtonQuit))
			{
				bQuit = true;
			}
		}
		else
		{
			if(m_fTimerMenuNavigation > m_fTimerMenuNavigationMax)
			{
				switch(m_EMainState)
				{	
					case EMenuMain.e_Menu :
					{
						if(CApoilInput.MenuValidate)
							bMenu = true;
						if(CApoilInput.MenuUp)
						{
							m_EMainState = EMenuMain.e_Quit;
							m_fTimerMenuNavigation = 0.0f;
						}
						else if (CApoilInput.MenuDown)
						{
							m_EMainState = EMenuMain.e_Menu;
							m_fTimerMenuNavigation = 0.0f;
						}
						break;	
					}
					
					case EMenuMain.e_Quit :
					{
						if(CApoilInput.MenuValidate)
							bQuit = true;
						if(CApoilInput.MenuUp)
						{
							m_EMainState = EMenuMain.e_Quit;
							m_fTimerMenuNavigation = 0.0f;
						}
						else if (CApoilInput.MenuDown)
						{
							m_EMainState = EMenuMain.e_Menu;
							m_fTimerMenuNavigation = 0.0f;
						}
						break;	
					}
					default :
						m_EMainState = EMenuMain.e_Quit;
						break;
				}
			}
			else
			{
				m_fTimerMenuNavigation += 1/60.0f;
			}			
			
			float fOffsetMenu = (m_EMainState == EMenuMain.e_Menu) ? 0.5f : 0.0f;
			float fOffsetQuit = (m_EMainState == EMenuMain.e_Quit) ? 0.5f : 0.0f;
			
			GUI.DrawTextureWithTexCoords(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu, new Rect(fOffsetMenu, 0, 0.5f, 1));
			GUI.DrawTextureWithTexCoords(new Rect(1160, 10, 60, 60), m_Texture_ButtonQuit, new Rect(fOffsetQuit, 0, 0.5f, 1));	
		}
		NagigationMenuMain(false, false, bMenu, bQuit, false);
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void GestionMenuInGame(bool bManette)
	{
		bool bMenu = false;
		bool bQuit = false;
		bool bPause = false;
		
		if(!bManette)
		{
			if (GUI.Button(new Rect(10, 10, 200, 60), m_Texture_ButtonMenu))
			{
				bMenu = true;
				bPause = true;
			}
		
			if (GUI.Button(new Rect(250, 10, 60, 60), m_Texture_ButtonPause))
			{
				bPause = true;
			}	
		}
		else
		{
			if(m_fTimerMenuNavigation > m_fTimerMenuNavigationMax)
			{
				if(CApoilInput.MenuMenu)
				{
					bMenu = true;
					if(!m_bGamePaused)
						bPause = true;
					m_fTimerMenuNavigation = 0.0f;
				}
				if(CApoilInput.MenuPause)
				{
					Debug.Log ("pause" + m_bGamePaused);
					bPause = true;
					m_fTimerMenuNavigation = 0.0f;
				}
			}
			else
			{
				m_fTimerMenuNavigation += 1.0f/60.0f;	
			}
			
			float fOffsetPauseX = m_bGamePaused ? 0.5f : 0;
			
			GUI.DrawTextureWithTexCoords(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu, new Rect(0, 0, 0.5f, 1));
			GUI.DrawTextureWithTexCoords(new Rect(1160, 10, 60, 60), m_Texture_ButtonPause, new Rect(fOffsetPauseX, 0, 0.5f, 1));	
		}
		
		NagigationMenuMain(false, false, bMenu, bQuit, bPause);
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void GestionMenuWinLoose(bool bManette)
	{
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
		
		bool bClick = false;
		
		if(!bManette)
		{
			if (GUI.Button(new Rect(390, 100, 500, 150), m_Texture_ButtonOk))
			{	
				bClick = true;
			}
		}
		else
		{
			if(CApoilInput.MenuValidate)
				bClick = true;
			
			GUI.DrawTextureWithTexCoords(new Rect(390, 100, 500, 150), m_Texture_ButtonOk, new Rect(0, 0, 1, 1));
		}
		
		if(bClick)
		{
			m_EState = EmenuState.e_menuState_inGame;
			ResumeGame();
			m_Game.GoToNextLevelOrRestart(m_Game.IsWin());
			
		}
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void NagigationMenuMain(bool bPlay, bool bCredit, bool bMenu, bool bQuit, bool bPause)
	{
		if (bPlay)
		{
            m_Game.StartGame();
			ResumeGame();
			m_EState = EmenuState.e_menuState_inGame;
		}
		
		if (bCredit)
		{
			m_EState = EmenuState.e_menuState_credits;
		}
	
		if (bMenu)
		{
			m_EState = EmenuState.e_menuState_main;
			m_EMainState = EMenuMain.e_Menu;
		}
	
		if (bQuit)
		{
			Application.Quit();
		}
		
		if(bPause)
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
	}
}
