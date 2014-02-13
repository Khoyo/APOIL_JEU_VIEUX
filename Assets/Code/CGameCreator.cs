using UnityEngine;
using System.Collections;

public class CGameCreator : MonoBehaviour {

	static int m_instanceCount = 0;
	public GameObject m_prefabGame;

	public float LD_VelocityPlayer = 10.0f;

	void Awake() {
		if(m_instanceCount++ == 0)
		{
			((GameObject) GameObject.Instantiate(m_prefabGame)).GetComponent<CGame>();
			CGame.m_fVelocityPlayer = LD_VelocityPlayer;
		}
		
	}
	
	// Update is called once per frame
	void OnDestroy() {
		m_instanceCount--;
	}
}
