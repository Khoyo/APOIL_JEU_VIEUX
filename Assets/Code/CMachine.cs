using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CMachine : MonoBehaviour
{

	public CMachine()
	{
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public void Start()
	{	
		Component[] components = gameObject.GetComponents<Component>();
		foreach(Component component in components){
			IMachineAction action = component as IMachineAction;
			if(action != null)
				action.Init();
		}
		
	}

	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public void Update()
	{

	}
	
	public void Activate(CPlayer player){
		Component[] components = gameObject.GetComponents<Component>();
		
		foreach(Component component in components){
			IMachineAction action = component as IMachineAction;
			if(action != null)
				action.Activate(player);
		}
	}

	public void ActivateContinuous(CPlayer player)
	{
	}
}