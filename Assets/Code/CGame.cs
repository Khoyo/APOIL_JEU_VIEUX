using UnityEngine;
using System.Collections;

public class CGame : MonoBehaviour 
{
	public GameObject m_prefabPlayer;

	public static float ms_fVelocityPlayer;
	public static int ms_nNbPlayer;
	public static GameObject ms_LevelIn;

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
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () 
	{
		CApoilInput.Process (Time.deltaTime);
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
}
