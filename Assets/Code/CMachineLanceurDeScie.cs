using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CMachineLanceurDeScie : MonoBehaviour, IMachineAction
{
	
	public GameObject m_PrefabScie;
	public bool shouldFire = true;
	public int m_MaxScie ;
	CScie[] m_Scies;
	int m_ScieNumber = 0;
	
	
	public void Init()
	{
		m_Scies = new CScie[m_MaxScie];
		for(int i = 0; i<m_MaxScie; i++)
		{
			m_Scies[i] = new CScie();
			m_Scies[i].m_number = m_ScieNumber++;
			m_Scies[i].Init();
			m_Scies[i].getGameObject().active = false;
			
		}
		
	}
	
	public void Process()
	{
		if(shouldFire){
			CScie saw = GetNewSaw();
			saw.getGameObject().active = true;
			saw.getGameObject().transform.position = transform.position + new Vector3(150, 0, 0);;
			saw.getGameObject().rigidbody.velocity = new Vector3(2500,0,0);
			
			shouldFire = false;
		}
	}
	
	CScie GetNewSaw()
	{
		int min = m_ScieNumber;
		int index_min = 0;
		for(int i = 0; i<m_MaxScie; i++){
			if(!m_Scies[i].getGameObject().active){
				return m_Scies[i];
			}
			if(m_Scies[i].m_number < min){
				min = m_Scies[i].m_number;
				index_min = i;
			}
		}
		Debug.Log ("returning Scie number "+index_min);
		m_Scies[index_min].m_number = m_ScieNumber++;
		
		return m_Scies[index_min];
	}
	
	public void Activate(CPlayer player)
	{
	}
}
