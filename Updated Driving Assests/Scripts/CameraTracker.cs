using UnityEngine;
using System.Collections;

public class CameraTracker : MonoBehaviour {

	public GameObject target;
	public Vector3 locres;

	// Use this for initialization
	void Start () {

		locres = target.transform.position - transform.position;

	}
	
	// Update is called once per frame
	void Update () {
	
		transform.position = target.transform.position - locres;

	}
}
