using UnityEngine;
using System.Collections;

public class CMachineDistributeur : MonoBehaviour, IMachineAction {
	
	public GameObject prefabDistribue;
	
	bool givingOnNextTurn;
	GameObject m_cannette;
	
	
	public void Activate(CPlayer player){
		m_cannette = GameObject.Instantiate(prefabDistribue) as GameObject;		
	}
	
	public void Process(){
	}
	
}
