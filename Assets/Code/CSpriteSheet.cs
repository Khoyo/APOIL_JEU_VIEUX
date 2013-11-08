using UnityEngine;
using System.Collections;

public class CSpriteSheet // : MonoBehaviour 
{
	bool m_bIsPlaying;
	bool m_bIsForward;
	bool m_bIsEnd;
	bool m_bVibration;
	int m_nColumns = 1;
	int m_nRows = 1;	
	float m_fFPS = 1.0f;
	float m_fCoeffVelocity;
	float m_fTemps;
	Vector2 m_Size;
	string[] m_sounds;
	CGame m_Game;
	const float m_fFixFPS = 30.0f;
	
	public enum EEndCondition
	{
		e_Loop,
		e_PingPong,
		e_Stop,
		e_FramPerFram
	};
	
	EEndCondition m_endCondition;
	
	private GameObject m_parent;
	private Renderer m_myRenderer;
	private int m_nIndex = 0;
	
	public CSpriteSheet(GameObject parent)
	{
		SetParent(parent);
	}
	
	//-------------------------------------------------------------------------------
	///	
	//-------------------------------------------------------------------------------
	public void Init () 
	{
		m_bIsPlaying = false;
		m_bIsForward = true;
		m_bIsEnd = false;
		m_bVibration = false;
		m_myRenderer = m_parent.renderer;
		m_fTemps = 0.0f;
		m_nIndex = 0;
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_endCondition = EEndCondition.e_Loop;
		m_fCoeffVelocity = 1.0f;
	}
	
	//-------------------------------------------------------------------------------
	///	
	//-------------------------------------------------------------------------------
	public void Reset()
	{
		m_fTemps = 0.0f;
		m_nIndex = 0;
		m_fCoeffVelocity = 1.0f;
		m_myRenderer = m_parent.renderer;
	}
	
	//-------------------------------------------------------------------------------
	///	
	//-------------------------------------------------------------------------------
	public void ResetAtEnd()
	{
		m_fTemps = 0.0f;
		m_nIndex = m_nRows * m_nColumns - 1;
		m_fCoeffVelocity = 1.0f;
	}
	
	public void GoToNextFram()
	{
		if (m_nIndex < m_nRows * m_nColumns)
			++m_nIndex;
	}
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public void Process () 
	{
		float fDeltatime = Time.deltaTime;
		m_fTemps += fDeltatime;
		if(m_endCondition != EEndCondition.e_FramPerFram)
		{
			if (m_fTemps > m_fFPS*m_fCoeffVelocity/m_fFixFPS && m_bIsPlaying)
			{
				// Calculate index
				if(m_bIsForward){
					m_nIndex++;
		            if (m_nIndex >= m_nRows * m_nColumns)
		                switch(m_endCondition)
						{
							case EEndCondition.e_Stop:
								m_nIndex--;
								AnimationStop();
						 		break;
							case EEndCondition.e_Loop:
								m_nIndex = 0;
								break;
							case EEndCondition.e_PingPong:
								m_nIndex--;
								Reverse();
								break;
						}
					
				}
				else {
					m_nIndex--;
		            if (m_nIndex < 0)
						switch(m_endCondition){
							case EEndCondition.e_Stop:
								AnimationStop();
								m_nIndex++;
						 		break;
							case EEndCondition.e_Loop:
								m_nIndex = m_nRows * m_nColumns;
								break;
							case EEndCondition.e_PingPong:
								m_nIndex++;
								Reverse();
								break;
					}
				}
				
				//Play sound if necessary
				if(m_sounds[m_nIndex] != "" && m_sounds[m_nIndex] != null)
					m_Game.getSoundEngine().postEvent(m_sounds[m_nIndex], m_parent);
				
				m_fTemps = 0.0f;
				
			}
		}
		

		Vector2 offset = new Vector2(	((float)m_nIndex / m_nColumns - (m_nIndex / m_nColumns)), //x index
                                      1-	((m_nIndex / m_nColumns) / (float)m_nRows));    //y index
		
		if(m_bVibration)
		{
			offset.x = Mathf.Cos(100.0f * m_fTemps) / 50.0f;
		}
		
		Vector2 textureSize = new Vector2(1f / m_nColumns, 1f / m_nRows);
        
		// Reset the y offset, if needed
       /* if (offset.y == 1)
       		offset.y = 0.0f; */
		
		// If we have scaled the texture, we need to reposition the texture to the center of the object
        offset.x += ((1f / m_nColumns) - textureSize.x) / 2.0f;
        offset.y += ((1f / m_nRows) - textureSize.y) / 2.0f;
		
		m_parent.renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
	}	
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void SetAnimation(CAnimation anim)
	{
		m_nColumns = anim.m_nColumns;
		m_nRows = anim.m_nRows;
		m_Size = new Vector2 (1.0f / m_nColumns , 1.0f / m_nRows);
		m_myRenderer.material = anim.m_Material;
		m_myRenderer.material.SetTextureScale("_MainTex", m_Size);
		m_fFPS = anim.m_fFPS;
		m_sounds = anim.m_sounds;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void AnimationStart()
	{
		m_bIsPlaying = true;
		m_bIsEnd = false;
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public void AnimationStop()
	{
		m_bIsPlaying = false;	
		m_bIsEnd = true;
	}
	
	public void SetCoeffVelocity(float fCoeff)
	{
		m_fCoeffVelocity = fCoeff;
	}
	
	public void SetDirection(bool forward)
	{
		m_bIsForward = forward;
	}
	
	public void Reverse(){
		m_bIsForward = !m_bIsForward;
	}
	
	
	public void setEndCondition(EEndCondition c){
		m_endCondition = c;
	}
	
	public bool IsEnd()
	{
		return ((m_endCondition == EEndCondition.e_Stop) && m_bIsEnd);
	}
	
	public void SetVibration(bool bOn)
	{
		m_bVibration = bOn;	
	}
	
	public void SetParent(GameObject parent)
	{
		m_parent = parent;
	}
}
