using UnityEngine;
using System.Collections;

public class CMachineBreakable : MonoBehaviour, IMachineAction
{
	public void Init()
	{
	}
	
	public void Process()
	{
	}
	
	public void Activate(CPlayer player)
	{
		CTakeElement obj = player.GetHeldElement();
		
		//Debug.Log(obj.GetTypeElement() +" "+CTakeElement.ETypeObject.e_TypeObject_Torche);
		if(obj != null && obj.GetTypeElement() == CTakeElement.ETypeObject.e_TypeObject_Torche)
		{
			gameObject.active = false;
		}
	}
	
}
