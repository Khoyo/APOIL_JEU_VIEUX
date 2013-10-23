using UnityEngine;
using System.Collections;

public class CCamera 
{
	GameObject m_GameObject;
	GameObject m_CurrentRoom;
	CGame m_Game;
	Camera m_Camera;
	
	const float m_fSizeMax = 800.0f;
	const float m_fSizeMin = 400.0f;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CCamera()
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_GameObject = GameObject.Find("Cameras");
		m_CurrentRoom =  GameObject.Find("Salle1");
		m_Camera = m_GameObject.transform.FindChild("RenderCamera").GetComponent<Camera>();
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public void Init()
	{	
		Object.DontDestroyOnLoad(m_GameObject);
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void Reset()
	{
		
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public void Process(float fDeltatime)
	{
		//SetPositionFromRoom();
		//SetPositionFromObj(GameObject.Find("_Game").GetComponent<CGame>().getLevel().getPlayer().getGameObject());
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetPosition(Vector2 pos)
	{
		Vector3 NewPos = m_GameObject.transform.position;
		NewPos.x =  pos.x;
		NewPos.y =  pos.y;
		m_GameObject.transform.position = NewPos;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetFactorScale(float fSize)
	{
		if(fSize <= m_fSizeMax && fSize >= m_fSizeMin)
			m_Camera.orthographicSize = fSize;
		else if(fSize > m_fSizeMax)
			m_Camera.orthographicSize = m_fSizeMax;
		else if(fSize < m_fSizeMin)
			m_Camera.orthographicSize = m_fSizeMin;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetCurrentRoom(GameObject room)
	{
		m_CurrentRoom = room;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void SetPositionFromRoom()
	{
		SetPositionFromObj(m_CurrentRoom);
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void SetPositionFromObj(GameObject obj)
	{
		Vector3 pos = m_GameObject.transform.position;
		pos.x =  obj.transform.position.x;
		pos.y =  obj.transform.position.y;
		m_GameObject.transform.position = pos;
	}
}
