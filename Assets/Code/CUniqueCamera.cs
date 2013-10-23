using UnityEngine;
using System.Collections;

public class CUniqueCamera : MonoBehaviour {
	
	static int m_instanceCount = 0;
	
	// Use this for initialization
	void Start () {
		if(m_instanceCount++ != 0){
			//We are not the first CUniqueCamera object :'( we need to abort !!
			Debug.Log("Deleting redundant Camera");
			Object.Destroy(gameObject);
			gameObject.name = "_Camera____todestroydonotuseseriouslyifreakingmeanit";
			return;
		}
		
	}
}
