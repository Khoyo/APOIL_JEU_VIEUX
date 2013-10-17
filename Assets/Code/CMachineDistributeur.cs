using UnityEngine;
using System.Collections;

public class CMachineDistributeur : MonoBehaviour, IMachineAction {
	
	public GameObject prefabDistribue;
	public int stock = -1;
	int m_framesSinceActivated = 0;
	GameObject m_cannette;
	bool m_activated = false;
	CPlayer m_user;
	
	public void Activate(CPlayer player){
		if(!m_activated && stock != 0){
			m_cannette = GameObject.Instantiate(prefabDistribue) as GameObject;
			m_activated = true;
			m_framesSinceActivated = 0;
			m_user = player;
		}		
	}
	
	public void Init(){}
	
	public void Process(){
		if(m_activated){
			if(m_framesSinceActivated == 1){
				m_user.PickUpObject(m_cannette.GetComponent<CScriptTakeElement>().GetTakeElement());
				stock--;
			}
			
			if(m_framesSinceActivated >= 100)
				m_activated = false;
		}		
		m_framesSinceActivated++;
	}
	
}
