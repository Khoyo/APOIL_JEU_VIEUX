using UnityEngine;
using System.Collections;

public class CMachineActiveZone : MonoBehaviour {
	
	public CMachine m_Machine;
	

	public void Start()
	{		
		//Debug.Log("Machine "+m_Machine.getGameObject().name);
		
		if(gameObject.GetComponent<Collider>() == null){
			Debug.LogError("Machine "+m_Machine.gameObject.name+" have no active zone collider");
		}
	}

 	void OnTriggerStay(Collider other)
	{	
			//ActivateMachine
		CPlayer player = other.gameObject.GetComponent<CPlayer>();

		if(player != null && CApoilInput.InputPlayer[player.GetIdPlayer()].ActivateMachine)
		{
			m_Machine.Activate(player);
		}
		
		if(player != null && CApoilInput.InputPlayer[player.GetIdPlayer()].ActivateMachineContinuous)
		{
			m_Machine.ActivateContinuous(player);
		}

	}

	
}
