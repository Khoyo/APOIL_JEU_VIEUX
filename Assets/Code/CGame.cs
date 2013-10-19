using UnityEngine;
using System.Collections;

public class CGame : MonoBehaviour
{
	// Objets
	public GameObject prefabPlayer;
	public GameObject prefabMonster;
	public Camera m_CameraCone;
	public GameObject m_debugDraw;
	public GameObject m_renderScreen;	
	public int m_nNbLevel;
	public GameObject m_ObjLevel1;
	
	// materials
	public Material m_materialPlayer1Repos;
	public Material m_materialPlayer1Horizontal;
	public Material m_materialPlayer1Vertical;
	public Material m_materialPlayer2Repos;
	public Material m_materialPlayer2Horizontal;
	public Material m_materialPlayer2Vertical;
	public Material m_materialPlayer3Repos;
	public Material m_materialPlayer3Horizontal;
	public Material m_materialPlayer3Vertical;
	public Material m_materialPlayer4Repos;
	public Material m_materialPlayer4Horizontal;
	public Material m_materialPlayer4Vertical;
	
	// variables de LD
	public bool m_bPadXBox = false;
	public bool m_bDebug = false;
	public bool m_bDebugRendu = false;
	public bool m_bNotUseMasterGame = false;
	public float m_fSpeedPlayer = 1.0f;
	public float m_fSpeedMonster = 1.0f;
	
	public float m_fCoeffReverseWalk = 1.0f;
	public float m_fCoeffSlowWalk = 1.0f;
	public float m_fCoeffNormalWalk = 1.0f;
	public float m_fCoeffRunWalk = 1.0f;
	
	public float m_fDiscretionBaseRadius = 30;
	public float m_fCoeffDiscretionAttente = 0;
	public float m_fCoeffDiscretionDiscret = 1;
	public float m_fCoeffDiscretionMarche = 4;
	public float m_fCoeffDiscretionCours = 10;
	
	public float m_fAngleConeDeVision = 1.0f;
	public float m_fDistanceConeDeVision = 1f;
	public int m_fPrecisionConeDeVision = 1; 
	public float m_fMonsterTimeErrance = 2.0f;
	public float m_fMonsterRadiusAlerte = 1.0f;
	
	public bool m_BMute = false;
	public string soundbankName = "Jeu_apoil.bnk";
	public bool m_bLightIsOn = true;
	
	public int m_nNbPlayer = 1;
	
	public GameObject m_PrefabScie;
	
	// variables
	bool m_bInGame;
	bool m_bGameStarted;
	bool m_bWin;
	int m_nScreenWidth;
	int m_nScreenHeight;
	int m_nIdLevel;
	CLevel m_Level;
	CCamera m_Camera;
	CSoundEngine m_SoundEngine;
	GameObject[] m_pLevelIn;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void Init()
	{		
		m_SoundEngine = new CSoundEngine();
		m_SoundEngine.Init();
		m_SoundEngine.LoadBank(soundbankName);
		
		/*m_nIdLevel = 0;
		m_pLevelIn = new GameObject[m_nNbLevel];
		m_pLevelIn[0] = m_LevelIn1;*/
		
		m_Level = new CLevel();
		m_Level.SetObjetLevel(m_ObjLevel1);
		m_Level.Init();
		
		m_nScreenWidth = 1280;
		m_nScreenHeight = 800;
		m_Camera = new CCamera();
		m_Camera.Init();		
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void Reset()
	{
		m_Level.Reset();
		m_Camera.Reset();
		m_bInGame = true;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void Process(float fDeltatime)
	{	
		if(m_bInGame)
		{
			CApoilInput.Process(fDeltatime);
			m_Level.Process(fDeltatime);
			m_Camera.Process(fDeltatime);
			//ProcessRoomState();
			CheckLooseGame();		
			
			//Quit on Escape
			if(Input.GetKey(KeyCode.Escape))
				Application.Quit();
			
			//Debug
			if(Input.GetKey(KeyCode.F8))
				StartLevel(m_nIdLevel);
			if(Input.GetKey(KeyCode.F9))
				FinishLevel(true, CPlayer.EIdPlayer.e_IdPlayer_Player1);
			if(Input.GetKey(KeyCode.F10))
				FinishLevel(false, CPlayer.EIdPlayer.e_IdPlayer_Player1);
		}
		
	}

	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void DisplayDebug()
	{
		GUI.Label(new Rect(10, 10, 100, 20), System.Convert.ToString(Time.deltaTime));
		GUI.Label(new Rect(10, 30, 100, 20), System.Convert.ToString(1f/Time.deltaTime));
		GUI.Label(new Rect(10, 50, 100, 20), System.Convert.ToString(m_nIdLevel));
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public CLevel getLevel()
	{
		return m_Level;
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public Vector2 GetSizeScreen()
	{
		return new Vector2(m_nScreenWidth, m_nScreenHeight);
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void StartGame()
	{
		if(!m_bGameStarted)
		{	
			Init();
			StartLevel(0);
			m_bGameStarted = true;
		}
		
		m_bInGame = true;	
	}

	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public bool IsPadXBoxMod()
	{
		return m_bPadXBox;	
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public bool IsDebug()
	{
		return m_bDebug;	
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public bool IsDebugRendu()
	{
		return m_bDebugRendu;	
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public bool IsNotUseMasterGame()
	{
		return m_bNotUseMasterGame;	
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void PauseGame()
	{
		m_bInGame = false;	
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void FinishLevel(bool bWin, CPlayer.EIdPlayer idPlayerWin)
	{
		m_bInGame = false;
		m_bWin = bWin;
		CMenu menu = gameObject.GetComponent<CMenu>();
		Texture TexturePlayerWin = m_materialPlayer1Repos.mainTexture;;
		if(m_bWin)
		{	
			switch(idPlayerWin)
			{
				case CPlayer.EIdPlayer.e_IdPlayer_Player1 :
				{
					TexturePlayerWin = m_materialPlayer1Repos.mainTexture;
					break;	
				}
				case CPlayer.EIdPlayer.e_IdPlayer_Player2 :
				{
					TexturePlayerWin = m_materialPlayer2Repos.mainTexture;
					break;	
				}
				case CPlayer.EIdPlayer.e_IdPlayer_Player3 :
				{
					TexturePlayerWin = m_materialPlayer3Repos.mainTexture;
					break;	
				}
				case CPlayer.EIdPlayer.e_IdPlayer_Player4 :
				{
					TexturePlayerWin = m_materialPlayer4Repos.mainTexture;
					break;	
				}
			}
			
			menu.SetTexturePlayerWin(TexturePlayerWin);
		}
		else
		{
			
		}
		menu.SetMenuState(CMenu.EmenuState.e_menuState_menuWinLoose);
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void StartLevel(int i)
	{
		m_nIdLevel = i;
		m_Level.SetObjetLevel(m_ObjLevel1);
		Reset();	
	}
	
	public void GoToNextLevel()
	{
		if(m_nIdLevel < m_nNbLevel)
			++m_nIdLevel;
	}
	
	public int GetIdLevel()
	{
		return m_nIdLevel;
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	void CheckLooseGame()
	{
		bool bLoose = true;
		for (int i = 0 ; i < m_nNbPlayer ; ++i)
		{
			if(getLevel().getPlayer(i).IsAlive())
				bLoose = false;
		}	
		if(bLoose)
			FinishLevel(false, CPlayer.EIdPlayer.e_IdPlayer_Player1);
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnGUI() 
	{
        if (m_bDebug)
		{
			DisplayDebug();
		}

    }

	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start()
	{
		m_bInGame = false;
		m_bGameStarted = false;
		m_bWin = false;
		if (m_bNotUseMasterGame)
		{
			StartGame();
		}
		if (m_bDebugRendu)
		{
			m_debugDraw.SetActiveRecursively(true);
		}
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update()
	{
		if(m_bInGame)
		{
			Process(Time.deltaTime);
		}
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public CCamera getCamera()
	{
		return m_Camera;	
	}
	
	public CSoundEngine getSoundEngine(){
		return m_SoundEngine;
	}
	
	public bool IsWin()
	{
		return m_bWin;	
	}
	
	public GameObject GetLevelIn(int i)
	{
		return m_pLevelIn[i];
	}
}
