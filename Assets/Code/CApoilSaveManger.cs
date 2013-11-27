using UnityEngine;
using System.Collections;
using System.IO;
using System;


public struct SSaveData
{
	public int m_nLastLevelUnlock; //1-N
}

public class CApoilSaveManger
{
	SSaveData m_SaveData;
	bool m_bCantSave;
	string pathFile = "Test.txt";
	string pathFolder = "Save";
	string path;
		
	public CApoilSaveManger()
	{
		m_SaveData = new SSaveData();
		path = pathFolder+"/"+pathFile;
		m_bCantSave = false;
	}
	
	public void Save()
	{
		string num = System.Convert.ToString(m_SaveData.m_nLastLevelUnlock);
		//Stream str = System.IO.File.OpenWrite(path);
		
		FileInfo theSourceFile = new FileInfo (path);	
		
		try
		{
			StreamWriter writte = new StreamWriter(path);		
		
			writte.WriteLine(m_SaveData.m_nLastLevelUnlock);
			writte.WriteLine("end");

			writte.Close();
		}
		catch (Exception e)
		{
		   // throw new Exception(e.ToString());
			m_bCantSave = true;
		}
	}
	
	public void Load()
	{	
		FileInfo theSourceFile = new FileInfo (path);
		if(!theSourceFile.Directory.Exists)
		{
			Directory.CreateDirectory(pathFolder);	
			System.IO.File.WriteAllText(path, "1");
			theSourceFile = new FileInfo (path);
		}
		if(!theSourceFile.Exists)
		{
			System.IO.File.WriteAllText(path, "1");
			theSourceFile = new FileInfo (path);
		}
		StreamReader reader = theSourceFile.OpenText();
		string text;
		
		text = reader.ReadLine();
		
		int nNbLevel = 1;
		if(int.TryParse(text,out nNbLevel))
			nNbLevel = int.Parse(text);
		
		SetLastLevelUnlock(nNbLevel);
		reader.Close();
	}
	
	public SSaveData ReadSaveData()
	{
		SSaveData Data = new SSaveData();
		FileInfo theSourceFile = new FileInfo (path);
		StreamReader reader = theSourceFile.OpenText();
		string text;
		
		text = reader.ReadLine();
		
		int nNbLevel = 1;
		if(int.TryParse(text,out nNbLevel))
			nNbLevel = int.Parse(text);
		
		Data.m_nLastLevelUnlock = nNbLevel;
		
		reader.Close();
		return Data;
	}
		
	
	public void SetLastLevelUnlock(int nLastLevelUnlock)
	{
		m_SaveData.m_nLastLevelUnlock = nLastLevelUnlock;	
	}
	
	public SSaveData GetSaveData()
	{
		return m_SaveData;	
	}
	
	public bool CantSave()
	{
		return m_bCantSave;
	}
	
}