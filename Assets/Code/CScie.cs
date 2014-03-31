using UnityEngine;
using System.Collections;

public class CScie : MonoBehaviour {

	public bool useTimer = false;
	public float m_fTimer = -1f;

	public float m_fFiredTime = 0f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(useTimer)
		{
			if(m_fFiredTime+m_fTimer < Time.time)
				gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("Ok");
		if(other.gameObject != null)
		{
			Debug.Log("Nice");
			if(other.gameObject.CompareTag("Solid"))
			{
				gameObject.SetActive(false);	
			}

			CPlayer player = other.gameObject.GetComponent<CPlayer>();
			if(player != null)
			{
				gameObject.SetActive(false);
				//player.DieHeadCut();
				Debug.Log("Executed player "+player.GetIdPlayer());
			}
		}
	}
}
