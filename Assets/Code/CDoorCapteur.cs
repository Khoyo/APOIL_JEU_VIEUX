using UnityEngine;
using System.Collections;

public class CDoorCapteur : MonoBehaviour 
{
	public GameObject objetDoor;

	CDoor m_Door;
	bool m_bIsOnLight;
	float m_fTimerStopIsOnLight;

	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Start () 
	{
		m_bIsOnLight = false;
		m_Door = objetDoor.GetComponent<CDoor> ();
	}
	
	//-------------------------------------------------------------------------------
	/// Unity
	//-------------------------------------------------------------------------------
	void Update () 
	{
		m_Door.SetIsOnLight (m_bIsOnLight);
		SetSprite ();

		if(m_bIsOnLight)
		{
			m_fTimerStopIsOnLight += Time.deltaTime;
			if(m_fTimerStopIsOnLight > 0.1f)
			{
				m_fTimerStopIsOnLight = 0.0f;
				SetIsOnLight(false);
			}
		}

	}

	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void CollideWithLight()
	{
		SetIsOnLight(true);
	}

	void SetIsOnLight(bool bIsOnLight)
	{
		m_bIsOnLight = bIsOnLight;
	}

	void SetSprite()
	{
		if(m_bIsOnLight)
			gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		else
			gameObject.GetComponent<SpriteRenderer>().color = Color.red;
	}
}
