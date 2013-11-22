using UnityEngine;
using System.Collections;
using System.IO;


public struct SSaveData
{
	public int m_nLastLevelUnlock; //1-N
}

public class CApoilSaveManger
{
	SSaveData m_SaveData;
	string pathFile = "Test.txt";
	string pathFolder = "Save";
	string path;
		
	public CApoilSaveManger()
	{
		m_SaveData = new SSaveData();
		path = pathFolder+"/"+pathFile;
	}
	
	public void Save()
	{
		string num = System.Convert.ToString(m_SaveData.m_nLastLevelUnlock);
		//Stream str = System.IO.File.OpenWrite(path);
		
		FileInfo theSourceFile = new FileInfo (path);	
		
		StreamWriter writte = new StreamWriter(path);
		
		writte.WriteLine(m_SaveData.m_nLastLevelUnlock);
		writte.WriteLine("end");

		writte.Close();
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
		
		SetLastLevelUnlock(int.Parse(text));
		reader.Close();
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