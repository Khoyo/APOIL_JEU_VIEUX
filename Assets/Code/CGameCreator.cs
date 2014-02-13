using UnityEngine;
using System.Collections;

public class CGameCreator : MonoBehaviour {

	static int m_instanceCount = 0;
	public GameObject m_prefabGame;

	void Awake() {
		if(m_instanceCount++ == 0)
		{
			CGame game = ((GameObject) GameObject.Instantiate(m_prefabGame)).GetComponent<CGame>();
		}
		
	}
	
	// Update is called once per frame
	void OnDestroy() {
		m_instanceCount--;
	}
}
