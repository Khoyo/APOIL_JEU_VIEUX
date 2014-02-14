using UnityEngine;
using System.Collections;

public class CGameCreator : MonoBehaviour {

	static int m_instanceCount = 0;
	public GameObject m_prefabGame;

	public float LD_VelocityPlayer = 10.0f;
	public int LD_NbPlyer = 1;

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Awake() {
		if(m_instanceCount++ == 0)
		{
			GameObject game = ((GameObject) GameObject.Instantiate(m_prefabGame));/*.GetComponent<CGame>();*/
			game.name = "Game";
			CGame.ms_fVelocityPlayer = LD_VelocityPlayer;
			CGame.ms_nNbPlayer = LD_NbPlyer;
		}
		
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnDestroy() {
		m_instanceCount--;
	}
}
