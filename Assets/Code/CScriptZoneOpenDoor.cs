using UnityEngine;
using System.Collections;

public class CScriptZoneOpenDoor : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		CGame game = GameObject.Find("_Game").GetComponent<CGame>();
		for(int i = 0 ; i < game.m_nNbPlayer ; ++i)
		{
			if(other.gameObject == game.getLevel().getPlayer(i).getGameObject())
			{
				gameObject.transform.parent.gameObject.GetComponent<CMachinePorte>().Open();	
				return;
			}
		}
	}
}
