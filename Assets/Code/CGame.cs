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
	//public GameObject m_ObjLevel1;
	
	//fonts
	public Font m_Font;
	public Font m_DebugFont;
	public Font m_ErrorFont;
	
	// materials
	public Material m_materialPlayer1Repos;
	
	public Material m_materialPlayer1UpUp;
	public Material m_materialPlayer1UpDown;
	public Material m_materialPlayer1UpLeft;
	public Material m_materialPlayer1UpRight;

	public Material m_materialPlayer1DownUp;
	public Material m_materialPlayer1DownDown;
	public Material m_materialPlayer1DownLeft;
	public Material m_materialPlayer1DownRight;
		
	public Material m_materialPlayer1LeftUp;
	public Material m_materialPlayer1LeftDown;
	public Material m_materialPlayer1LeftLeft;
	public Material m_materialPlayer1LeftRight;
	
	public Material m_materialPlayer1RightUp;
	public Material m_materialPlayer1RightDown;
	public Material m_materialPlayer1RightLeft;
	public Material m_materialPlayer1RightRight;
	
	public Material m_materialPlayer1DieHeadCut;
	public Material m_materialPlayer1DieFall;
	public Material m_materialPlayer1DieGravity;
	
	public Material m_materialPlayer2Repos;
	public Material m_materialPlayer3Repos;
	public Material m_materialPlayer4Repos;
	
	public Material m_materialMonterGravityActif;
	public Material m_materialMonterGravityAlerte;
	public Material m_materialMonterGravityEclaire;
	public Material m_materialMonterGravityMange;
	public Material m_materialMonterGravityVeille;
	
	public Material m_materialMonterCreepVeille;
	public Material m_materialMonterCreepBouge;
	public Material m_materialMonterCreepChoppe;
	public Material m_materialMonterCreepDort;
	public Material m_materialMonterCreepEclaire;
	
	// variables de LD
	public bool m_bPadXBox = false;
	public bool m_bDebug = false;
	public bool m_bDebugRendu = false;
	public bool m_bNotUseMasterGame = false;
	public float m_fSpeedPlayer = 1.0f;
	public float m_fSpeedMonster = 1.0f;
	
	public float m_fCoeffReverseWalk = 1.0f;
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
	
	public float m_fCameraDezoomMax = 800.0f;
	public float m_fCameraDezoomMin = 400.0f;
	
	public float m_fTimerDestructionPont = 2.0f;
	public float m_fTimerMenuError = 4.0f;
	public int m_nNbImpactMaxCaisse = 4;
	
	public float m_fCreepTimerParasiteMin = 4.0f;
	public float m_fCreepTimerParasiteMax = 8.0f;
	public float m_fCreepTimerSleep = 4.0f;
	public float m_fCreepCoeffRalentissement = 0.5f;
	public float m_fCreepVelocity = 100.0f;
	
	public float m_fGravityVeilleTimerMax = 3.0f;
	public float m_fGravityEatTimerMax = 3.0f;
	
	public bool m_BMute = false;
	public string soundbankName = "Jeu_apoil.bnk";
	public bool m_bLightIsOn = true;
	
	public int m_nNbPlayer = 1;
	
	public GameObject m_PrefabScie;
	
	public float m_fTimeToDischargeInSec = 15;
	
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
	
	CMenu m_Menu;
	//sauvegarde
	CApoilSaveManger m_SaveManager;
	
	static int m_instanceCount = 0;
	bool m_bStartCalled = false;
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void Init()
	{		
		//Make _Game (and CGame) persitent beetween scenes
		Object.DontDestroyOnLoad(this);
		
		m_nIdLevel = 0;
		m_pLevelIn = null;
		
		m_SoundEngine = new CSoundEngine();
		m_SoundEngine.Init();
		m_SoundEngine.LoadBank(soundbankName);
		
		m_Level = new CLevel();
		GameObject levelObj;
		if((levelObj = GameObject.Find("Level")) == null)
			Debug.LogError("No object named Level in scene");
		
		m_Level.SetObjetLevel(levelObj.gameObject);
		m_Level.Init();
		
		m_Menu = gameObject.GetComponent<CMenu>();
		
		m_nScreenWidth = 1280;
		m_nScreenHeight = 800;
		m_Camera = new CCamera();
		m_Camera.Init();	
		
		m_SaveManager = new CApoilSaveManger();
		m_SaveManager.Load();
		
		Object.DontDestroyOnLoad(transform.gameObject);
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
		CApoilInput.Process(fDeltatime);

		if(m_bInGame)
		{
			m_Level.Process(fDeltatime);
			m_Camera.Process(fDeltatime);
			//ProcessRoomState();
			CheckLooseGame();		
			
			//Quit on Escape
			if(CApoilInput.Quit)
				QuitGame();
			
			//Debug
			if(CApoilInput.DebugF9)
				GoToNextLevelOrRestart(false);
			if(CApoilInput.DebugF11)
				FinishLevel(true, CPlayer.EIdPlayer.e_IdPlayer_Player1);
			if(CApoilInput.DebugF12)
				FinishLevel(false, CPlayer.EIdPlayer.e_IdPlayer_Player1);
		}
		
	}

	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void DisplayDebug()
	{
		GUI.skin.label.font = m_DebugFont;
		GUI.Label(new Rect(10, 10, 100, 20), System.Convert.ToString(Time.deltaTime));
		GUI.Label(new Rect(10, 30, 100, 20), System.Convert.ToString(1f/Time.deltaTime));
	//	GUI.Label(new Rect(10, 50, 300, 20), System.Convert.ToString(getLevel().getPlayer(0).GetAnimation()));
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
			Reset();
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
			
			m_Menu.SetTexturePlayerWin(TexturePlayerWin);
		}

		m_Menu.SetMenuState(CMenu.EmenuState.e_menuState_menuWinLoose);
		
		//m_Level.DeleteCElements();
		
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void StartLevel()
	{
		GameObject levelObj; 
		if((levelObj = GameObject.Find("Level")) == null)
			Debug.LogError("No object named Level in scene");
		m_Level.SetObjetLevel(levelObj.gameObject);
		m_bInGame = true;
		m_Level.StartLevel();
		Reset();	
	}

	public void GoToNextLevelOrRestart (bool bWin)
	{
		if(bWin)
		{
			GoToNextLevel();
			m_SaveManager.SetLastLevelUnlock(Application.loadedLevel+2);
		}
		else
			RestartLevel();
	}
	
	public void GoToNextLevel()
	{
		Debug.Log ("Exiting level " + Application.loadedLevel);
		
		if(m_bNotUseMasterGame)
			GoToLevel(Application.loadedLevel);
		else
			GoToLevel(Application.loadedLevel+1);
		
	}
	
	public void GoToLevel(int nIdLevel)
	{		
		m_Level.DeleteCElements();
		if(nIdLevel < Application.levelCount)
		{
			Application.LoadLevel(nIdLevel);
		}	
	}
	
	public void RestartLevel()
	{
		m_Level.DeleteCElements();
		Debug.Log("Restarting level "+Application.loadedLevel);
		Application.LoadLevel(Application.loadedLevel);
		
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
	/// 	
	//-------------------------------------------------------------------------------
	public void QuitGame()
	{
		Save();
		Application.Quit();	
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
		m_bStartCalled =true;
		if(m_instanceCount++ != 0){
			//We are not the first CGame object :'( we need to abort !!
			Debug.Log("Deleting redundant _Game");
			Object.Destroy(gameObject);
			gameObject.name = "_Game____todestroydonotuseseriouslyimeanit";
			return;
		}
		
		m_bInGame = false;
		m_bGameStarted = false;
		m_bWin = false;
		Init();
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
	///		Unity
	//-------------------------------------------------------------------------------
	void Update()
	{
		Process(Time.deltaTime);
	}
	

	//-------------------------------------------------------------------------------
	/// 	Unity
	//-------------------------------------------------------------------------------
	void OnLevelWasLoaded(int level) 
	{	
		if(!m_bStartCalled)
			return;
		StartLevel();
    }
	
	void Save()
	{
		m_Menu.SaveGame();
		m_SaveManager.Save();
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
	
	public bool LightIsOn()
	{
		return m_bLightIsOn;	
	}
	
	public CApoilSaveManger GetSaveManager()
	{
		return m_SaveManager;	
	}
}
