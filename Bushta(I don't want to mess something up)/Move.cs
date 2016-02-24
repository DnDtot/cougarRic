using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public float currentspeed = 0f;
	public float maxspeed = 50f;
	public float minspeed = 25f;
	public float accel = 10f;
	public float rspeed = 25f;
	
	// Update is called once per frame
	void Update () {

		//gets what arrow keys are being pressed.
		float translation = Input.GetAxis ("Vertical"); 
		float rotation = Input.GetAxis ("Horizontal");

		//accelerates car forward, and caps current speed to max speed.
		if (translation > 0) {
			currentspeed += accel * Time.deltaTime;
			if (currentspeed > maxspeed){
				currentspeed = maxspeed;
			}
		}

		//accelerates car backward, and caps current speed to min speed.
		if (translation < 0){
			currentspeed += accel * Time.deltaTime;
			if (currentspeed > minspeed) {
				currentspeed = minspeed;
			}
		}
			
		//resets current speed
		if (translation == 0) {
			currentspeed = 0;
		}

		//sets the rate of change of turn and rotation to per second, rather than per step.
		translation *= Time.deltaTime * currentspeed;;
		rotation *= Time.deltaTime * rspeed;

		//moves vehicle forward and backward.
		transform.Translate (0f, 0f, translation);

		//locks the vehicle to only rotate when moving forward or backward,
		//and adjusts the vehicle to turn opposite when moving backwards.
		if (translation != 0){
			if (translation > 0) {
				transform.Rotate (0f, rotation * 3, 0f);
			} else {
				transform.Rotate (0f, -rotation * 3, 0f);
			}
		}
	}
}