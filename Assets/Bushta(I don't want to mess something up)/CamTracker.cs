using UnityEngine;
using System.Collections;

public class CamTracker : MonoBehaviour {

	public GameObject I_Spy;
	public float Rspeed = 20f;

	private Vector3 Offset;


	// Use this for initialization
	void Start () {

		Offset = transform.position - I_Spy.transform.position;
	}

	// Update is called once per frame
	void LateUpdate () {

		transform.position = I_Spy.transform.position + Offset;

	}
}
