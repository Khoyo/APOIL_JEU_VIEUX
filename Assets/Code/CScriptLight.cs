using UnityEngine;
using System.Collections;

public class CScriptLight : MonoBehaviour 
{
	
	CGame m_Game;
	float m_fAngleVise;
	
	public Material m_Material; 	// Mat appliqué au mesh de vue
	public bool m_bDebug = false; 		// Dessine les rayons dans la scene view
	public LayerMask m_Mask;		 	// Layers qui vont bloquer la vue
   
	Vector3[] m_pDirections;	// va contenir les rayons, déterminés par precision, distance et angle
	Mesh m_pSightMesh;			// Le mesh dont les vertex seront modifiés selons les obstacles
	Transform m_Transform;
	GameObject m_gameObject;
    
	float m_fAngleMax; 	// Angle d'ouverture, degré
	float m_fDistance;
	int m_nPrecision; 	// Nombre de rayons lancé dans l'angle ci dessus

	Vector3[] m_pPoints;
	int[] m_pIndices;
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	void setDirection()
	{
		// Préparation des rayons
		m_nPrecision = m_nPrecision > 1 ? m_nPrecision : 2;
		m_pDirections = new Vector3[m_nPrecision];
		float angle_start = 0.0f;
		//	float angle_start = -angle*0.5F;
		float angle_step = m_fAngleMax / (m_nPrecision-1);
		for( int i = 0; i < m_nPrecision; i++ )
		{
			Quaternion Rotation = Quaternion.Euler(0, angle_start + i*angle_step, Time.frameCount);
			Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Rotation , Vector3.one );
			m_pDirections[i] = mat * Vector3.forward;
		}	
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	private void UpdateSightMesh()
	{         
		//setDirection();	
		
		// Lance les rayons pour placer les vertices le plus loin possible
		for(int i = 0 ; i < m_nPrecision ; i++)
		{
			Vector3 dir = m_Transform.TransformDirection(m_pDirections[i]); // repere objet
			RaycastHit hit;
			float dist = m_fDistance;
			if(Physics.Raycast( m_Transform.position, dir, out hit, m_fDistance, m_Mask ) ) // Si on touche, on rétrécit le rayon
			{
				// CGameObject objet = hit.collider.gameObject.GetComponent<CGameObject>();	
				//if(!hit.collider.gameObject.tag.Equals("player") && hit.collider.gameObject.GetComponent<CGameObject>() != null)
				{
					dist = hit.distance;
					//objet.SetVisible();
				}
				
			}
			 
			if(m_bDebug)
				Debug.DrawRay( m_Transform.position, dir * dist );
			
			// Positionnement du vertex
			m_pPoints[i] = dir * dist;
			m_pPoints[i+m_nPrecision] = Vector3.zero;
		}
		
		// On réaffecte les vertices
		m_pSightMesh.vertices = m_pPoints;  
		m_pSightMesh.RecalculateNormals(); // normales doivent être calculé pour tout changement 
		                      			// du tableau vertices, même si on travaille sur un plan*/
		
		//translate le centre a la position du player!!! J'AI PASSE 3 PUTAINS DE JOUR POUR TROUVER QU'IL FALLAIT FAIRE LE CONE EN (0,0) ET LE TRANSLATER ENSUITE!!!
		m_gameObject.transform.position = m_Transform.position;
	}
	
	// Use this for initialization
	void Start () 
	{
		m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		m_fAngleMax = 360.0f;
		m_fDistance = m_Game.m_fDistanceConeDeVision;
		m_nPrecision = 100;
		// Initialisation du cone
		m_gameObject = new GameObject( "ConeSight" );
		m_pSightMesh = new Mesh();
		
		(m_gameObject.AddComponent( typeof( MeshFilter )) as MeshFilter).mesh = m_pSightMesh;
		(m_gameObject.AddComponent( typeof( MeshRenderer )) as MeshRenderer).material = m_Material;
		m_gameObject.layer = LayerMask.NameToLayer("Cone");
		m_gameObject.tag = "cone";
		m_Transform = gameObject.transform; //transform; // histoire de limiter les getcomponent dans la boucle
		
		// Préparation des rayons
		setDirection();
		
		// préparations des outils de manipulation du mesh
		int nbPoints =  m_nPrecision*2;
		int nbTriangle = nbPoints - 2;
		int nbFace = nbTriangle / 2;
		int nbIndice =  nbTriangle * 3;
		int row = nbFace+1;
		
		m_pPoints = new Vector3[nbPoints];
		m_pIndices = new int[ nbIndice ];
		
		for( int i = 0; i < nbFace; i++ )
		{
			m_pIndices[i*6+0] = i+0;
			m_pIndices[i*6+1] = i+1;
			m_pIndices[i*6+2] = i+row;
			m_pIndices[i*6+3] = i+1;
			m_pIndices[i*6+4] = i+row+1;
			m_pIndices[i*6+5] = i+row;
		}
		
		m_pSightMesh.vertices = new Vector3[nbPoints];
		m_pSightMesh.uv = new Vector2[nbPoints];
		m_pSightMesh.triangles = m_pIndices;    
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateSightMesh();
	}
}
