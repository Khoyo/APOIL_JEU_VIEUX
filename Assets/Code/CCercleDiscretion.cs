using UnityEngine;
using System.Collections;

public class CCercleDiscretion : MonoBehaviour {

	CPlayer parent;
	CGame m_Game;
	
	float[] m_fCoeffState;
	float m_fspeedCoeff;
	float m_fBaseRadius;
	
	float m_fRadius;
	SphereCollider collider;
	
	// Use this for initialization
	public void Init(CPlayer pparent) {
		parent = pparent;
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		collider = GetComponent<SphereCollider>();
		
		//Get necessary coefficients for size calculation
		m_fCoeffState = new float[4];
		m_fCoeffState[0] = m_Game.m_fCoeffDiscretionAttente;
		m_fCoeffState[1] = m_Game.m_fCoeffDiscretionDiscret ;
		m_fCoeffState[2] = m_Game.m_fCoeffDiscretionMarche;
		m_fCoeffState[3] = m_Game.m_fCoeffDiscretionCours;
		m_fBaseRadius = m_Game.m_fDiscretionBaseRadius;
		//TODO: Add avatar coefficients when and if there is multiple avatar types
	}
	
	public void Process() {
		m_fRadius = m_fBaseRadius*m_fCoeffState[(int)parent.GetMoveModState()];
		collider.radius = m_fRadius;
	}
	
	public void OnTriggerEnter(Collider other){
		/*CMonster monster = game.getLevel().getMonster();
		if(other.gameObject == monster.getGameObject()){
			monster.detectedPlayerAudio();
		}*/
	}
}
