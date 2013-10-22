using UnityEngine;
using System.Collections;

public class CScriptZoneOpenDoor : MonoBehaviour 
{
	bool m_bOpen;
	// Use this for initialization
	void Start () {
		m_bOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		CGame game = GameObject.Find("_Game").GetComponent<CGame>();
		Debug.DrawLine(other.gameObject.transform.position, gameObject.transform.position);
		
		bool bCanOpen = true;
		
		for(int i = 0 ; i < game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == game.getLevel().getPlayer(i).getGameObject() && bCanOpen)
			{
				gameObject.transform.parent.gameObject.GetComponent<CMachinePorte>().Open();	
			}
		}
	}
}
