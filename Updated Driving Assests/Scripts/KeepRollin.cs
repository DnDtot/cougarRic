using UnityEngine;
using System.Collections;

//temporarily removed lines of code.
/* comments about code */

public class KeepRollin : MonoBehaviour {

	public GameObject body;
	public WheelCollider FR, FL, BR, BL;
	//public Transform FRT, FLT, BRT, BLT;

	//public float tforce = 100f;

	/* public variables */
	public float maxtorque = 1000;
	public float topspeed = 50;
	public float maxturn = 30;
	public float veloc;
	public float aveloc;
	public bool braketime = false;

	/* private variables */
	private float btorque;
	private float maxbtorque;
	private float minbtorque;
	private Rigidbody rb;
	private Vector3 pos;
	private Vector3 vel;
	private Quaternion rot;


	void Start () {

		/* variables initialized */
		btorque = maxtorque / 2;
		maxbtorque = maxtorque * 20;
		minbtorque = btorque;
		rb = GetComponent<Rigidbody> ();
		pos = transform.position;
		rot = transform.rotation;
		vel = rb.velocity;

	}
	
	/* fixed update for physics engine, do not use Time.deltaTime */
	void FixedUpdate () {

		/* turns directions into percents */
		float ctorque = maxtorque * Input.GetAxis ("Vertical");
		float cturn = maxturn * Input.GetAxis ("Horizontal");

		/* retrieves current velocity and rotations per second */
		veloc = rb.velocity.magnitude;
		aveloc = BR.rpm / 60;

		//rb.drag = veloc / tforce;

		/* checks to see if vehicle needs to slow down due to top speed or brakes */
		if ((topspeed >= veloc)&&(!braketime)) {
			
			BR.motorTorque = ctorque;
			BL.motorTorque = ctorque;
			FR.brakeTorque = 0;
			FL.brakeTorque = 0;
			BR.brakeTorque = 0;
			BL.brakeTorque = 0;

		} else {

			BR.motorTorque = 0;
			BL.motorTorque = 0;
			FR.brakeTorque = btorque;
			FL.brakeTorque = btorque;
			BR.brakeTorque = btorque;
			BL.brakeTorque = btorque;

		}


		//FRT.transform.localRotation = new Quaternion (0f, ctorque, 0f, 0f);
		//FLT.transform.localRotation = new Quaternion (0f, ctorque, 0f, 0f);

		FR.steerAngle = cturn;
		FL.steerAngle = cturn;

		/* adjusts btrque based on whether top speed or brakes initialize it */
		if (Input.GetKey (KeyCode.RightShift)) {

			if (btorque < maxbtorque) {

				btorque *= 20;

			}
			braketime = true;

		} else {

			if (btorque > minbtorque) {
				
				btorque /= 20;

			}
			braketime = false;

		}

		/* resets car to last reset point */
		if (Input.GetKeyDown ("r")) {

			transform.position = pos;
			transform.rotation = rot;
			rb.velocity = vel;

		}
	}

	void OnCollisionEnter(Collision otherobj){

		/* sets new reset point when collided */
		if (otherobj.gameObject.tag == "Checkpoint"){
			
			Debug.Log ("CHECKPOINT!");
			pos = new Vector3 (transform.position.x, 0, transform.position.z);
			Destroy (otherobj.gameObject);

		}
	}
}