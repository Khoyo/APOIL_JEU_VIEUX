using UnityEngine;
using System.Collections;

public class CDoor : MonoBehaviour {

	public GameObject render;
	Animator m_animator;

	bool m_bOpen;

	// Use this for initialization
	void Start () 
	{
		m_animator = render.GetComponent<Animator> ();
		m_bOpen = true;
		Close ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(!m_bOpen)
				Open();
			else
				Close();
		}
	}

	void Open()
	{
		if(!m_bOpen)
		{
			m_animator.SetInteger("door_status", 0);
			m_bOpen = true;
		}
	}

	void Close()
	{
		if(m_bOpen)
		{
			m_animator.SetInteger("door_status", 1);
			m_bOpen = false;
		}
	}
}
