using UnityEngine;
using System.Collections;

public struct SSaveData
{
	public int m_nLastLevelUnlock; //1-N
}

public class CApoilSaveManger
{
	SSaveData m_SaveData;
	
	public CApoilSaveManger()
	{
		m_SaveData = new SSaveData();	
		SetLastLevelUnlock(1);
	}
	
	public void SetLastLevelUnlock(int nLastLevelUnlock)
	{
		m_SaveData.m_nLastLevelUnlock = nLastLevelUnlock;	
	}
	
	public SSaveData GetSaveData()
	{
		return m_SaveData;	
	}
	
}