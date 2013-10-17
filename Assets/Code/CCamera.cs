using UnityEngine;
using System.Collections;

public class CCamera 
{
	GameObject m_GameObject;
	GameObject m_CurrentRoom;
	CGame game;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public CCamera()
	{
		game = GameObject.Find("_Game").GetComponent<CGame>();
		m_GameObject = GameObject.Find("Cameras");
		m_CurrentRoom =  GameObject.Find("Salle1");
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
	public void Init()
	{	
		
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
	public void SetFactorScale(float fCoeff)
	{
		
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
