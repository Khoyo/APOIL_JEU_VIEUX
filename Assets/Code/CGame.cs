using UnityEngine;
using System.Collections;

public class CGame : MonoBehaviour 
{
	public GameObject m_prefabPlayer;
	public GameObject m_prefabCamera;

	public static float ms_fVelocityPlayer;
	public static float ms_fCoeffReverseWalk;
	public static float ms_fCoeffRun;
	public static float ms_fVelocityCreep;
	public static float ms_fGravityMonsterForce;
	public static float ms_fCoeffPararsitized;
	public static int ms_nNbPlayer;
	public static int ms_nEnergieTorchLightMax;
	public static GameObject ms_LevelIn;
	public static GameObject ms_Camera;
	public static Font ms_FontDebug;
	public static LayerMask ms_LayerMaskLight;

	public static float ms_fCreepTimerYeute;
	public static float ms_fCreepTimerSleep;
	public static float ms_fCreepTimerWakeUp;
	public static float ms_fCreepTimerTake;
	public static float ms_fMonsterTimerStopAlerte;
	public static float ms_fMonsterTimerStopActif;
	public static float ms_fMonsterTimerEat;
	public static float ms_fMonsterTimerStopEat;
	public static float ms_fDoorTimerClose;

	CPlayer[] m_Players;

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () 
	{
		CApoilInput.Init ();

		CGame.ms_LevelIn = GameObject.Find ("_LevelIn");

		m_Players = new CPlayer[CGame.ms_nNbPlayer];

		CreatePlayers();
		CreateCamera ();

		CGame.ms_fCreepTimerYeute = 2.0f;
		CGame.ms_fCreepTimerSleep = 2.0f;
		CGame.ms_fCreepTimerWakeUp = 2.0f;
		CGame.ms_fCreepTimerTake = 1.0f;
		CGame.ms_fMonsterTimerStopAlerte = 2.0f;
		CGame.ms_fMonsterTimerStopActif = 2.0f;
		CGame.ms_fMonsterTimerEat = 3.0f;
		CGame.ms_fMonsterTimerStopEat = 2.0f;
		CGame.ms_fDoorTimerClose = 3.0f;
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () 
	{
		CApoilInput.Process (Time.deltaTime);
		SetPositionCameraFromPlayers ();
	}

	void OnGUI()
	{
		GameObject LevelIn = GameObject.Find ("_LevelIn");
		GameObject LevelOut = GameObject.Find ("_LevelOut");
		if(LevelIn == null || LevelOut == null)
		{
			GUI.skin.label.font = CGame.ms_FontDebug; 
			GUI.Label(new Rect( 50, 50, 800, 600), "Putain Pierre!!");
			Debug.Log ("il faut mettre un LevelIn et un LevelOut pour que ca marche!");
		}
	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void GoToNextLevel()
	{

	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void WinLevel(CPlayer.EIdPlayer eId)
	{
		Debug.Log ("win dude!");
	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	void CreatePlayers()
	{
		for(int i = 0 ; i < CGame.ms_nNbPlayer ; ++i)
		{
			CPlayer player = ((GameObject) GameObject.Instantiate(m_prefabPlayer)).GetComponent<CPlayer>();
			player.SetIdPlayer(i);
			m_Players[i] = player;
		}
		SetPlayerInitPosition ();
	}

	void CreateCamera()
	{
		ms_Camera = ((GameObject) GameObject.Instantiate(m_prefabCamera));
	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	void SetPlayerInitPosition()
	{
		Vector3 pos3D = CGame.ms_LevelIn.transform.position;
		float fSizePlayer = 1.0f;
		Vector2[] posPlayer = new Vector2[4];
		posPlayer[0] = new Vector2(pos3D.x - fSizePlayer / 2.0f, pos3D.y - fSizePlayer / 2.0f);
		posPlayer[1] = new Vector2(pos3D.x + fSizePlayer / 2.0f, pos3D.y - fSizePlayer / 2.0f);
		posPlayer[2] = new Vector2(pos3D.x + fSizePlayer / 2.0f, pos3D.y + fSizePlayer / 2.0f);
		posPlayer[3] = new Vector2(pos3D.x - fSizePlayer / 2.0f, pos3D.y + fSizePlayer / 2.0f);

		for(int i = 0 ; i < CGame.ms_nNbPlayer ; ++i)
		{
			m_Players[i].SetPosition2D(posPlayer[i]);
			m_Players[i].SetPositionInit(posPlayer[i]);
		}
	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	void SetPositionCameraFromPlayers()
	{
		Vector3 pos = m_Players [0].transform.position;
		pos.z = CGame.ms_Camera.transform.position.z;
		CGame.ms_Camera.transform.position = pos;
	}

}
