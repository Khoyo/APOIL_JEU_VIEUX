using UnityEngine;
using System.Collections;

public class CElement
{
	protected CGame m_Game;
	protected GameObject m_GameObject;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public CElement()
	{
		
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public virtual void Init()
	{	
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
	}
	
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public virtual void Reset()
	{
		
	}

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public virtual void Process(float fDeltatime)
	{
		
	}
	
	public virtual void Destroy()
	{
		m_Game.getLevel().UnregisterElement(this);
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public GameObject GetGameObject()
	{
		return m_GameObject;	
	}
	
	public void setGameObject(GameObject gameObject){
		m_GameObject = gameObject;
	}
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetPosition2D(Vector2 pos2D)
	{
		Vector3 pos = m_GameObject.transform.position;
		pos.x = pos2D.x;
		pos.y = pos2D.y;
		pos.z = -2;
		m_GameObject.transform.position = pos;
	}
	
	public Vector2 GetPosition2D()
	{
		return new Vector2(m_GameObject.transform.position.x, m_GameObject.transform.position.y);
	}

}
