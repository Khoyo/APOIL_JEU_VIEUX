using UnityEngine;
using System.Collections;

public class CMachineActiveZone : MonoBehaviour {
	
	CMachine m_Machine;
	CGame m_Game;
	

	public void Init(CMachine obj)
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		
		m_Machine = obj;
		
		//Debug.Log("Machine "+m_Machine.getGameObject().name);
		
		if(gameObject.GetComponent<Collider>() == null){
			Debug.LogError("Machine "+m_Machine.GetGameObject().name+" have no active zone collider");
		}
	}

	public void Process ()
	{
		
	}
	
 	void OnTriggerStay(Collider other)
	{	
		for(int i = 0 ; i < m_Game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject ==  m_Game.getLevel().getPlayer(i).GetGameObject() && m_Game.getLevel().getPlayer(i).GetPlayerInput().ActivateMachine)
			{
				m_Machine.Activate(m_Game.getLevel().getPlayer(i));
			}
		}
	}
	
	
}
