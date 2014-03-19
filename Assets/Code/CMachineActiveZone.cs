using UnityEngine;
using System.Collections;

public class CMachineActiveZone : MonoBehaviour {
	
	public GameObject m_Machine;
	

	public void Start()
	{		
		//Debug.Log("Machine "+m_Machine.getGameObject().name);
		
		if(gameObject.GetComponent<Collider2D>() == null){
			Debug.LogWarning("Machine "+m_Machine.gameObject.name+" have no active zone collider");
		}
	}

 	void OnTriggerStay2D(Collider2D other)
	{	
			//ActivateMachine
		CPlayer player = other.gameObject.GetComponent<CPlayer>();

		if(player != null && CApoilInput.GetInput(player.GetIdPlayer()).ActivateMachine)
		{
			m_Machine.GetComponent<CMachine>().Activate(player); 
		}
		
		if(player != null && CApoilInput.InputPlayer[(int)player.GetIdPlayer()].ActivateMachineContinuous)
		{
			m_Machine.GetComponent<CMachine>().ActivateContinuous(player);
		}

	}

	
}
