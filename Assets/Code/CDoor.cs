using UnityEngine;
using System.Collections;

public class CDoor : MonoBehaviour {

	public GameObject m_render;
	public GameObject m_collider;
	Animator m_animator;
	float m_fTimerCloseDoor;
	float m_fTimerBlockClose;

	bool m_bIsOnLight;
	bool m_bBlockCloseDoor;
	bool m_bOpen;

	// Use this for initialization
	void Start () 
	{
		m_animator = m_render.GetComponent<Animator> ();
		m_bIsOnLight = false;
		m_bBlockCloseDoor = false;
		m_bOpen = true;
		m_fTimerCloseDoor = 0.0f;
		m_fTimerBlockClose = 0.0f;
		Close ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bOpen)
		{
			if(m_fTimerCloseDoor < CGame.ms_fDoorTimerClose)
			{
				m_fTimerCloseDoor += Time.deltaTime;
			}
			else if(!m_bBlockCloseDoor)
			{
				m_fTimerBlockClose += Time.deltaTime;
				if(m_fTimerBlockClose > 0.3f)
				{
					m_fTimerBlockClose = 0.0f;
					Close();
				}
			}
		}
	}

	public void Open()
	{
		if(!m_bOpen)
		{
			m_animator.SetInteger("door_status", 0);
			m_bOpen = true;
			m_fTimerCloseDoor = 0.0f;
			m_collider.GetComponent<BoxCollider2D>().isTrigger = true;
		}
	}

	public void Close()
	{
		if(m_bOpen && !m_bBlockCloseDoor)
		{
			m_animator.SetInteger("door_status", 1);
			m_bOpen = false;
			m_collider.GetComponent<BoxCollider2D>().isTrigger = false;
		}
	}

	public void SetIsOnLight(bool bIsOnLight)
	{
		m_bIsOnLight = bIsOnLight;
	}

	public void SetBlockClose(bool bBlockClose)
	{
		m_bBlockCloseDoor = bBlockClose;
	}
}
