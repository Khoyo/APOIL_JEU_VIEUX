using UnityEngine;
using System.Collections;

public class CMachineDistributeur : MonoBehaviour, IMachineAction {
	
	public GameObject prefabDistribue;
	
	public void Activate(CPlayer player){
		
		GameObject cannette = GameObject.Instantiate(prefabDistribue) as GameObject;
		CScriptTakeElement elem = cannette.GetComponent<CScriptTakeElement>();
		elem.GetTakeElement().Init();
		player.PickUpObject(elem.GetTakeElement());
		
	}
}
