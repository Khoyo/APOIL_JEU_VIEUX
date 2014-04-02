using UnityEngine;
using System.Collections;

public class CTools{

	public static void CollideLight(RaycastHit2D hit)
	{
		if(hit.collider != null)
		{
			//Debug.Log (hit.collider.name);
			if(hit.collider.CompareTag("GravityMonster"))
			{
				hit.collider.gameObject.GetComponent<CGravityMonster>().CollideWithLight();
			}
			
			if(hit.collider.CompareTag("Creep"))
			{
				hit.collider.gameObject.GetComponent<CCreep>().CollideWithLight();
			}

			if(hit.collider.CompareTag("Capteur"))
			{

			}
		}
	}

}
