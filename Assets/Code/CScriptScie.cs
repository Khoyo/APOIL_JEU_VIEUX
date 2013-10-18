using UnityEngine;
using System.Collections;

public class CScriptScie : MonoBehaviour {
	
	CGame m_Game;
	
	CScie m_parent;	


	void Start() {

	}
	
	public CScie GetScie()
	{
		return m_parent;
	}
	public void SetScie(CScie scie)
	{
		m_parent=scie;
	}
	
	void Update () {
	
	}
}
	