using UnityEngine;
using System.Collections;

public class Move3 : MonoBehaviour {

	public float veloc;
	public float rspeed = 20f;
	public float tspeed = 500f;
	public WheelCollider WheelFL, WheelFR, WheelBL, WheelBR;

	private Rigidbody rb;
	private Vector3 Resetposition;
	private Vector3 Resetvelocity;
	private Quaternion Resetrotation;

	//grabs the car's rigidbody, velocity, position, and rotation.
	void Start () {

		rb = GetComponent<Rigidbody> ();

		Resetvelocity = rb.velocity;
		Resetposition = transform.position;
		Resetrotation = transform.rotation;


	}
	

	void Update () {

		//Grabs the arrow keys being pressed.
		float translation = Input.GetAxis ("Vertical");
		float rotation = Input.GetAxis ("Horizontal");

		//Sets rate of rotation to per second rather than step.
		rotation *= rspeed * Time.deltaTime;

		//Keeps track of magnitude of velocity with a negative.
		if (translation >= 0) {
			veloc = rb.velocity.magnitude;
		}

		if (translation < 0) {
			veloc = -rb.velocity.magnitude;
		}

		//Applies force behind vehicle.
			rb.AddForce (transform.forward * translation * tspeed);

		//Checks which direction the car is moving, and moves in the respective direction.
		if (veloc != 0) {
			if (veloc > 0) {
				transform.Rotate (0f, rotation, 0f);
			}
			if (veloc < 0) {
				transform.Rotate (0f, -rotation, 0f);
			}
		}

		//Resets the car's position, rotation, and velocity
		if (Input.GetKeyDown (KeyCode.R)){
			transform.rotation = Resetrotation;
			transform.position = Resetposition;
			rb.velocity = Resetvelocity;
		}
	}
}