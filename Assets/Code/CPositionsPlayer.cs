using UnityEngine;
using System.Collections;

class CPositionsPlayer 
{
	int m_PositionHoldNumber, m_currentIndex = 0, m_baseIndex = 0;
	bool m_bIsEmpty = true;
	
	Vector2[] m_Positions;
	
	public void Init(int size)
	{
		m_PositionHoldNumber = size;
		m_Positions = new Vector2[m_PositionHoldNumber];
		m_bIsEmpty = true;
	}
	
	public void Add(Vector2 pos)
	{
		int newIndex = (m_currentIndex + 1)%m_PositionHoldNumber;
		if(m_bIsEmpty){
			m_baseIndex = newIndex;
			m_currentIndex = newIndex;
			m_bIsEmpty = false;
		}
		else if(newIndex == m_baseIndex)
		{
			m_baseIndex = (newIndex+1)%m_PositionHoldNumber;
			m_currentIndex = newIndex;
		}
		else
		{
			m_currentIndex = newIndex;
		}
		m_Positions[m_currentIndex] = pos;
	}
	
	public Vector2 RollBackNFrameOrBest(int n)
	{
		if(m_baseIndex < m_currentIndex)
		{
			m_currentIndex = Mathf.Max(m_baseIndex, m_currentIndex-n);;
		}
		else if(m_baseIndex > m_currentIndex)
		{
			if(m_currentIndex-n >= 0)
				m_currentIndex = m_currentIndex -n;
			else
				m_currentIndex = Mathf.Max(m_baseIndex, m_PositionHoldNumber + m_currentIndex-n);			
				
		}
		else
		{
			m_currentIndex = m_baseIndex;
		}
		
		return m_Positions[m_currentIndex];
	}
	
	public Vector2 Get(int i)
	{
		return m_Positions[i];
	}
	
	public void DebugPrint()
	{
		string content="{";
		int index = m_baseIndex;
		for(int i = 0; i < m_PositionHoldNumber; i++)
		{
			content += m_Positions[index]+", ";
			if(index == m_currentIndex)
				break;
			index = (index + 1)%m_PositionHoldNumber;
		}
		content += '}';
		Debug.Log (content+"     current index: "+m_currentIndex+"   base: "+m_baseIndex);
		
		content = "{";
		for(int i = 0; i < m_PositionHoldNumber; i++)
		{
			content += m_Positions[i]+", ";
		}
		
		//Debug.Log (content);
		
	}
}
			
			
class CPositionsPlayerTest
{
	public static void DoTest()
	{
		CPositionsPlayer test = new CPositionsPlayer();
		test.Init(8);
		for(int i = 0; i< 10; i++)
		{
			test.Add(new Vector2(i,1337));
			
		}
		
		test.DebugPrint();
		Debug.Log(test.RollBackNFrameOrBest(2));
		test.DebugPrint();
		
		for(int i = 0; i< 8; i++)
			Debug.Log(i+": "+test.Get(i).ToString());
		
	}
}