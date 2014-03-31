using UnityEngine;
using System.Collections;

public class CGameCreator : MonoBehaviour {

	static int m_instanceCount = 0;
	public GameObject m_prefabGame;
	public Font m_FontDebug;

	public float LD_VelocityPlayer = 10.0f;
	public float LD_CoeffReverseWalk = 1.0f;
	public float LD_CoeffRun = 1.0f;
	public float LD_VelocityCreep = 1.0f;
	public float LD_GravityMonsterForce = 1.0f;
	public int LD_NbPlyer = 1;
	public int LD_EnergirMaxTorchLight = 100;

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
			CGame.ms_fCoeffReverseWalk = LD_CoeffReverseWalk;
			CGame.ms_fCoeffRun = LD_CoeffRun;
			CGame.ms_fVelocityCreep = LD_VelocityCreep;
			CGame.ms_fGravityMonsterForce = LD_GravityMonsterForce;
			CGame.ms_FontDebug = m_FontDebug;
			CGame.ms_nEnergieTorchLightMax = LD_EnergirMaxTorchLight;
		}
		
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void OnDestroy() {
		m_instanceCount--;
	}
}
