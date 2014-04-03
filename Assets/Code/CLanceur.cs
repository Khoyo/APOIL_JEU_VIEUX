using UnityEngine;
using System.Collections;

public class CLanceur : MonoBehaviour {

	public GameObject PrefabLance;

	public float m_delay;
	public float m_intensity;

	float timer;

	// Use this for initialization
	void Start () {
		timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > m_delay)
		{
			timer = 0;
			Launch(); //Woop woop
		}
	}

	public void Launch(){
		GameObject projectile = (GameObject)GameObject.Instantiate(PrefabLance);
		projectile.transform.position = transform.TransformPoint(new Vector3(-0.85f, 0f, 0f));
		projectile.transform.rotation = transform.rotation;
		projectile.transform.rigidbody2D.AddForce(transform.TransformDirection(new Vector2(-1 * m_intensity, 0)));

		CSoundEngine.postEvent("TourelleTire", gameObject);
	}
}
