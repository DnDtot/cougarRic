using UnityEngine;
using System.Collections;

public class Move2 : MonoBehaviour {

	public float speed;
	public Rigidbody rb;
	public float currentspeed = 0f;
	public float maxspeed = 50f;
	public float minspeed = 25f;
	public float accel = 20f;
	public float rspeed = 25f;
	public Terrain Terrain;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	

	void FixedUpdate () {
		float translation = Input.GetAxis ("Vertical");
		float rotation = Input.GetAxis ("Horizontal");
		rotation *= Time.deltaTime * rspeed;
		speed = rb.velocity.magnitude * translation;

		if (translation > 0) {
			currentspeed += accel * Time.deltaTime;
			if (currentspeed > maxspeed) {
				currentspeed = maxspeed;
			}
		}

		if (translation < 0) {
			currentspeed += accel * Time.deltaTime;
			if (currentspeed > minspeed) {
				currentspeed = minspeed;
			}
		}

		if (translation == 0) {
			currentspeed = 0f;
		}

		rb.AddForce (transform.forward * translation * currentspeed);
		if (speed != 0) {
				if (speed > 0) {
					transform.Rotate (0f, rotation, 0f);
				}
				if (speed < 0) {
					transform.Rotate (0f, -rotation, 0f);
				}
			}	
		}
	}