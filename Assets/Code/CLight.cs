using UnityEngine;
using System.Collections;

public class CLight : MonoBehaviour {
	
	int m_nPrecision = 30;
	float m_fDistance = 10.0f;
	public bool m_bDebug = false;

	// Use this for initialization
	void Start () 
	{
		m_fDistance = gameObject.transform.FindChild ("PointLight").GetComponent<Light> ().range * 2.0f/3.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 direction = new Vector2(1,0);
		//int nLayerMask = (1 << 9) | (1 << 10);
		for(int i = 0 ; i < m_nPrecision ; ++i)
		{
			float fAngle = CApoilMath.InterpolationLinear(i, 0, m_nPrecision, 0, 360);
			Matrix4x4 mat = Matrix4x4.TRS( Vector3.zero, Quaternion.Euler(0, 0, fAngle), Vector3.one);
			
			RaycastHit2D hit = Physics2D.Raycast(transform.position, mat*direction, m_fDistance, CGame.ms_LayerMaskLight);
			if(m_bDebug)
				Debug.DrawRay(transform.position, m_fDistance*(mat*direction));
			CTools.CollideLight(hit);
		}

	}
}
