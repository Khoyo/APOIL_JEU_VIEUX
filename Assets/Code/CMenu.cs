using UnityEngine;
using System.Collections;

public class CMenu : MonoBehaviour{
	
	public enum EmenuState
	{
		e_menuState_splash,
		e_menuState_main,
		e_menuState_credits,
		e_menuState_Bonus,
		e_menuState_movie,
		e_menuState_selectLevel,
		e_menuState_inGame,
		e_menuState_menuWinLoose,
	}
	
	public enum EMenuMain
	{
		e_Play,
		e_Credit,
		e_Bonus,
		e_Menu,
		e_Quit
	}
	
	EmenuState m_EState;
	EMenuMain m_EMainState;
	
	public Texture m_Texture_Fond;
	public Texture m_Texture_ButtonPlay;
	public Texture m_Texture_ButtonCredit;
	public Texture m_Texture_ButtonBonus;
	public Texture m_Texture_ButtonMenu;
	public Texture m_Texture_ButtonQuit;
	public Texture m_Texture_ButtonPause;
	public Texture m_Texture_ButtonOk;
	public Texture m_Texture_Splash;
	public Texture m_Texture_Credit;
	public Texture m_Texture_Blue;
	public Texture m_Texture_Red;
	public Texture m_Texture_Lost;
	public Texture m_Texture_LoadSave;
	public MovieTexture m_Texture_movie_intro;
	public MovieTexture m_Texture_movie_Bonus;
	//public AudioClip m_audio_Bonus;
	Texture m_Texture_PlayerWin;
	
	const float m_fDeltatime = 1/60.0f;
	float m_fTempsSplash;
	const float m_fTempsSplashInit = 2.0f;
	float m_fTempsVideoIntro;
	float m_fTempsVideoBonus;
	float m_fTempsErrorMessage;
	float m_fWaitingTimerVideoBonus;
	bool m_bGamePaused;
	CGame m_Game;
	int m_nLevelToLoad;
	
	float m_fTimerMenuNavigation;
	float m_fTimerMenuNavigationMax = 0.4f;
	
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
		m_fTempsVideoBonus = 0.0f;
		m_bGamePaused = false;
		m_fTimerMenuNavigation = 0.0f;
		m_fWaitingTimerVideoBonus = 0.0f;
		m_fTempsErrorMessage = 0.0f;
		m_nLevelToLoad = 0;
		if(!m_Game.IsNotUseMasterGame())	
		{
			//m_EState = EmenuState.e_menuState_splash;
			m_EState = EmenuState.e_menuState_main;
			m_fTempsSplash = m_fTempsSplashInit;
			m_EMainState = EMenuMain.e_Play;
		}
		else 
		{
			m_EState = EmenuState.e_menuState_inGame;
		}
		m_Texture_movie_Bonus.Stop();
		m_Texture_movie_intro.Stop();
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
				GestionSave();
				break;
			}	
			
			case EmenuState.e_menuState_selectLevel:
			{
				GUI.DrawTexture(new Rect(0, 0, 1280, 800), m_Texture_Fond);
			
				GestionSelectLevel(m_Game.m_bPadXBox);
				
				break;
			}
			
			case EmenuState.e_menuState_credits:
			{
				GUI.DrawTexture(new Rect(0, 0, 1280, 800), m_Texture_Credit);
			
				GestionMenuCredit(m_Game.m_bPadXBox);
				
				break;
			}	
			
			case EmenuState.e_menuState_Bonus:
			{
				if(m_Texture_movie_Bonus != null)
					GUI.DrawTexture(new Rect(0, 0, 1280, 800), m_Texture_movie_Bonus);
				if(m_Texture_movie_Bonus != null && m_fTempsVideoBonus == 0.0f)
				{
					m_Texture_movie_Bonus.Play();
				}	
				if(m_Texture_movie_Bonus != null && m_Texture_movie_Bonus.isPlaying)
				{
					m_fTempsVideoBonus += Time.deltaTime;
				}
				else 
				{
					//m_Texture_movie_Bonus.Stop();
					m_Texture_movie_Bonus.Stop();
					m_EState = EmenuState.e_menuState_main;
					//m_fTempsVideoBonus = 0.0f;
				}
				
				if(m_fWaitingTimerVideoBonus < 1.0f)
					m_fWaitingTimerVideoBonus += m_fDeltatime;
				else if(CApoilInput.MenuValidate)
				{
					m_Texture_movie_Bonus.Stop();
					m_EState = EmenuState.e_menuState_main;
					m_fTimerMenuNavigation = 0.0f;
					m_fWaitingTimerVideoBonus = 0.0f;
				}
			
				break;	
			}
			
			case EmenuState.e_menuState_inGame:
			{
				//if (GUI.Button(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu))
				
				GestionMenuInGame(m_Game.m_bPadXBox);
				GestionSave();
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
		bool bBonus = false;
		bool bMenu = false;
		bool bQuit = false;
		bool bResume = false;
		
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
						{
							bPlay = true;
							m_fTimerMenuNavigation = 0.0f;
						}
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
							m_EMainState = EMenuMain.e_Bonus;
							m_fTimerMenuNavigation = 0.0f;
						}
						break;	
					}
					
					case EMenuMain.e_Bonus :
					{
						if(CApoilInput.MenuValidate)
							bBonus = true;
						if(CApoilInput.MenuUp)
						{
							m_EMainState = EMenuMain.e_Credit;
							m_fTimerMenuNavigation = 0.0f;
						}
						else if (CApoilInput.MenuDown)
						{
							m_EMainState = EMenuMain.e_Bonus;
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
					bResume = true;
					m_fTimerMenuNavigation = 0.0f;
				}
				
			}
			else
			{
				m_fTimerMenuNavigation += m_fDeltatime;
			}			
			
			float fOffsetPlay = (m_EMainState == EMenuMain.e_Play) ? 0.5f : 0.0f;
			float fOffsetCredit = (m_EMainState == EMenuMain.e_Credit) ? 0.5f : 0.0f;
			float fOffsetBonus = (m_EMainState == EMenuMain.e_Bonus) ? 0.5f : 0.0f;
			float fOffsetMenu = (m_EMainState == EMenuMain.e_Menu) ? 0.5f : 0.0f;
			float fOffsetQuit = (m_EMainState == EMenuMain.e_Quit) ? 0.5f : 0.0f;
			
			GUI.DrawTextureWithTexCoords(new Rect(390, 100, 500, 150), m_Texture_ButtonPlay, new Rect(fOffsetPlay, 0, 0.5f, 1));
			GUI.DrawTextureWithTexCoords(new Rect(390, 300, 500, 150), m_Texture_ButtonCredit, new Rect(fOffsetCredit, 0, 0.5f, 1));
			GUI.DrawTextureWithTexCoords(new Rect(390, 500, 500, 150), m_Texture_ButtonBonus, new Rect(fOffsetBonus, 0, 0.5f, 1));
			GUI.DrawTextureWithTexCoords(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu, new Rect(fOffsetMenu, 0, 0.5f, 1));
			GUI.DrawTextureWithTexCoords(new Rect(1160, 10, 60, 60), m_Texture_ButtonQuit, new Rect(fOffsetQuit, 0, 0.5f, 1));
		}
		
		NagigationMenuMain(bPlay, bCredit, bBonus, bMenu, bQuit, false, false, bResume);
		
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
				
				//alternate Back
				if(CApoilInput.MenuBack)
				{
					bMenu = true;
					m_fTimerMenuNavigation = 0.0f;
				}
			}
			else
			{
				m_fTimerMenuNavigation += m_fDeltatime;
			}			
			
			float fOffsetMenu = (m_EMainState == EMenuMain.e_Menu) ? 0.5f : 0.0f;
			float fOffsetQuit = (m_EMainState == EMenuMain.e_Quit) ? 0.5f : 0.0f;
			
			GUI.DrawTextureWithTexCoords(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu, new Rect(fOffsetMenu, 0, 0.5f, 1));
			GUI.DrawTextureWithTexCoords(new Rect(1160, 10, 60, 60), m_Texture_ButtonQuit, new Rect(fOffsetQuit, 0, 0.5f, 1));	
		}
		NagigationMenuMain(false, false, false, bMenu, bQuit, false, false, false);
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
				m_fTimerMenuNavigation += m_fDeltatime;	
			}
			
			float fOffsetPauseX = m_bGamePaused ? 0.5f : 0;
			
			GUI.DrawTextureWithTexCoords(new Rect(940, 10, 200, 60), m_Texture_ButtonMenu, new Rect(0, 0, 0.5f, 1));
			GUI.DrawTextureWithTexCoords(new Rect(1160, 10, 60, 60), m_Texture_ButtonPause, new Rect(fOffsetPauseX, 0, 0.5f, 1));	
		}
		
		NagigationMenuMain(false, false, false, bMenu, bQuit, bPause, false, false);
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
	void GestionSelectLevel(bool bManette)
	{
		bool bSelect = false;
		bool bMenu = false;
		if(!bManette)
		{

		}
		else
		{
			if(m_fTimerMenuNavigation > m_fTimerMenuNavigationMax)
			{
				if(CApoilInput.MenuValidate)
				{
					bSelect = true;
					m_fTimerMenuNavigation = 0.0f;
				}
				
				if(CApoilInput.MenuBack)
				{
					bMenu = true;
					m_fTimerMenuNavigation = 0.0f;
				}
				
				if(CApoilInput.MenuLeft)
				{
					if(m_nLevelToLoad > 0)
						--m_nLevelToLoad;
					m_fTimerMenuNavigation = 0.0f;
				}
				
				if(CApoilInput.MenuRight)
				{
					if(m_nLevelToLoad < Application.levelCount-1 && m_nLevelToLoad < m_Game.GetSaveManager().GetSaveData().m_nLastLevelUnlock-1)
						++m_nLevelToLoad;
					m_fTimerMenuNavigation = 0.0f;
				}			
				
			}
			else
			{
				m_fTimerMenuNavigation += m_fDeltatime;
			}
		}
		
		string texturePath = "_ScreenLevel/level_"+System.Convert.ToString(m_nLevelToLoad+1);
		Texture textLevel = (Texture)Resources.Load(texturePath, typeof(Texture)); 

		GUI.DrawTexture(new Rect(390, 100, 480, 360), textLevel);
		
		GUI.skin.label.font = m_Game.m_Font;
		GUI.Label(new Rect(390, 100, 500, 150), System.Convert.ToString(m_nLevelToLoad+1));
		
		NagigationMenuMain(false, false, false, bMenu, false, false, bSelect, false);
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void GestionSave()
	{
		Vector2 posSave = new Vector2(20,20);
		if(m_Game.GetSaveManager().CantSave() && m_fTempsErrorMessage > 0.0f)
		{
			GUI.skin.label.font = m_Game.m_ErrorFont;
			GUI.Label(new Rect(posSave.x, posSave.y, 800, 300), "Dude! je peux pas sauvegarder, le dossier du jeu est protégé en écriture!");
			m_fTempsErrorMessage -= m_fDeltatime;
		}	
		else
			if(!m_Game.GetSaveManager().CantSave() && m_fTempsErrorMessage > 0.0f)
			{
				float fAngle = m_fTempsErrorMessage * 360.0f / m_Game.m_fTimerMenuError;
				float fSizeText = 50.0f;
			
				GUI.skin.label.font = m_Game.m_ErrorFont;
				GUI.Label(new Rect(posSave.x + fSizeText, posSave.y, 1080, 300), "Sauvegarde en cours!");
			
				
				GUIUtility.RotateAroundPivot (fAngle, new Vector2(posSave.x + fSizeText/2.0f, posSave.y + fSizeText/2.0f)); 
				GUI.DrawTexture(new Rect(posSave.x, posSave.y, fSizeText, fSizeText), m_Texture_LoadSave, ScaleMode.StretchToFill, true, 0);
			
				m_fTempsErrorMessage -= m_fDeltatime;
			}
	}
	
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void NagigationMenuMain(bool bPlay, bool bCredit, bool bBonus, bool bMenu, bool bQuit, bool bPause, bool bSelect, bool bResume)
	{
		if (bPlay)
		{	
			m_EState = EmenuState.e_menuState_selectLevel;
		}
		if(bResume)
		{
			ResumeGame();
			m_EState = EmenuState.e_menuState_inGame;
		}
		
		if (bSelect)
		{	
			m_Game.GoToLevel(m_nLevelToLoad);
			m_Game.StartGame();
			ResumeGame();
			m_EState = EmenuState.e_menuState_inGame;
		}
		
		if (bCredit)
		{
			m_EState = EmenuState.e_menuState_credits;
		}
		
		if (bBonus)
		{
			m_fTempsVideoBonus = 0.0f;
			m_EState = EmenuState.e_menuState_Bonus;
		}
	
		if (bMenu)
		{
			m_EState = EmenuState.e_menuState_main;
			m_EMainState = EMenuMain.e_Menu;
		}
	
		if (bQuit)
		{
			m_Game.QuitGame();
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
	
	public void SaveGame()
	{
		m_fTempsErrorMessage = m_Game.m_fTimerMenuError;	
	}
	
	
}
