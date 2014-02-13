using UnityEngine;
using System.Collections;

public class CGame : MonoBehaviour 
{

	public static float m_fVelocityPlayer;

	// Use this for initialization
	void Start () 
	{
		CApoilInput.Init ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		CApoilInput.Process (Time.deltaTime);
	}
}
