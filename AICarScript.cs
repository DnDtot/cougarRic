using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICarScript : MonoBehaviour
{
	public int time = 1;
	public float disfail;
	public Vector3 centerOfMass;
	public List<Transform> path;
	public Transform pathGroup;
	public float maxSteer = 30;
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelBL;
	public WheelCollider wheelBR;
	public int currentPathObj;
    public float distFromPath = 55;
	public float maxTorque = 75;
	public float currentSpeed;
	public float topSpeed = 300;
	public float decelerationSpeed = 7.5f;
	public Renderer brakingMesh;
	public Material idleBrakeLight;
	public Material activeBrakeLight;
	public bool isBraking = false;
	public bool inSector = true;
	public float sensorLength =7;
	public float frontSensorStartPoint = 0.25f;
	public float frontSensorSideDistance = 0.25f;
	public float frontSensorsAngle = 30;
	public float sidewaySensorLength = 8;
	public float avoidSpeed = 10;
	private int flag = 0;

	void Start ()
	{
		GetComponent<Rigidbody>().centerOfMass = centerOfMass;
		getPath ();

   
        }

	void getPath ()
	{
		Transform[] path_objs = pathGroup.GetComponentsInChildren<Transform> ();
		path = new List<Transform> ();
		foreach (Transform path_obj in path_objs) {
			if (path_obj != pathGroup)
				path.Add (path_obj);
		}
    
	}

    void Update()
    {
     //  if (flag == 0) {
            getSteer();
            Move();
            Sensors();
            //	BrakingEffect ();
  //     }
  //  else {
    //     getSteer();
      //      Move();
       //    Sensors();
            //	BrakingEffect ();
   //        AvoidSteer(0);
     //  }


            //}
		if ((GetComponent<Rigidbody> ().velocity.magnitude <= 1f) && (inSector == true)) {
			StartCoroutine (Respawn());
		}

   }

        void getSteer ()
	{
		Vector3 steerVector = transform.InverseTransformPoint (new Vector3 (path [currentPathObj].position.x, transform.position.y, path [currentPathObj].position.z));
		float newSteer = maxSteer * (steerVector.x / steerVector.magnitude);
		wheelFL.steerAngle = newSteer;
		wheelFR.steerAngle = newSteer;

		if (steerVector.magnitude <= distFromPath) {
			currentPathObj++;    
			if (currentPathObj >= path.Count) {
				currentPathObj = 0;
			}
		}
	}

	void Move ()
	{
		currentSpeed = GetComponent<Rigidbody> ().velocity.magnitude;// 2 * (22 / 7) * wheelBL.radius * wheelBL.rpm * 60 / 1000;
		currentSpeed = Mathf.Round (currentSpeed);
		if (currentSpeed <= topSpeed && isBraking== false) {
			wheelBR.motorTorque = maxTorque;
			wheelBL.motorTorque = maxTorque;
			wheelBR.brakeTorque = 0;
			wheelBL.brakeTorque = 0;    
		} else  {
			wheelBR.motorTorque = 0;
			wheelBL.motorTorque = 0;
			wheelBR.brakeTorque = decelerationSpeed;    
			wheelBL.brakeTorque = decelerationSpeed;    
		}

	}

	//void BrakingEffect ()
	//{
		//if (isBraking) {
			//brakingMesh.material = activeBrakeLight;
		//} else {
			//brakingMesh.material = idleBrakeLight;
		//}    
	//}

	void Sensors ()
	{
		flag = 0;
		float avoidSensitivity = 0;
		Vector3 pos;
		RaycastHit hit;
		Vector3 rightAngle = Quaternion.AngleAxis (frontSensorsAngle, transform.up) * transform.forward;
		Vector3 leftAngle = Quaternion.AngleAxis (-frontSensorsAngle, transform.up) * transform.forward;

		pos = transform.position;
		pos += transform.forward * frontSensorStartPoint;

		// Braking sensor

		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {
			if (hit.transform.tag != "Terrain") {
				flag++;
				wheelBL.brakeTorque = decelerationSpeed;    
				wheelBR.brakeTorque = decelerationSpeed;
				Debug.DrawLine (pos, hit.point, Color.red, 1f);
			}
		} else {
			wheelBL.brakeTorque = 0;
			wheelBR.brakeTorque = 0;
		}

		//Front Straight Right Sensor  
		pos += transform.right * frontSensorSideDistance;  

		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {  
			if (hit.transform.tag != "Terrain") {  
				flag++;  
				avoidSensitivity -= 1f;   
				Debug.Log ("Avoiding");  
				Debug.DrawLine (pos, hit.point, Color.red, 1f);  
			}  
		} else if (Physics.Raycast (pos, rightAngle, out hit, sensorLength)) {  
			if (hit.transform.tag != "Terrain") {  
				avoidSensitivity -= 0.5f;   
				flag++;  
				Debug.DrawLine (pos, hit.point, Color.red, 1f);  
			}  
		}

		//Front Straight left Sensor  
		pos = transform.position;  
		pos += transform.forward * frontSensorStartPoint;  
		pos -= transform.right * frontSensorSideDistance;  

		if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {  
			if (hit.transform.tag != "Terrain") {  
				flag++;  
				avoidSensitivity += 1f;   
				Debug.Log ("Avoiding");  
				Debug.DrawLine (pos, hit.point, Color.red, 1f);  
			}  
		} else if (Physics.Raycast (pos, leftAngle, out hit, sensorLength)) {  
			if (hit.transform.tag != "Terrain") {  
				flag++;  
				avoidSensitivity += 0.5f;  
				Debug.DrawLine (pos, hit.point, Color.red, 1f);  
			}  
		}

		//Right SideWay Sensor  
		if (Physics.Raycast (transform.position, transform.right, out hit, sidewaySensorLength)) {  
			if (hit.transform.tag != "Terrain") {  
				flag++;  
				avoidSensitivity -= 0.5f;  
				Debug.DrawLine (transform.position, hit.point, Color.red);  
			}  
		}

		//Left SideWay Sensor  
		if (Physics.Raycast (transform.position, -transform.right, out hit, sidewaySensorLength)) {  
			if (hit.transform.tag != "Terrain") {  
				flag++;  
				avoidSensitivity += 0.5f;  
				Debug.DrawLine (transform.position, hit.point, Color.red);  
			}  
		}  

		pos = transform.position;
		pos += transform.forward * frontSensorStartPoint;

		//Front Mid Sensor  
		if (avoidSensitivity == 0) {  

			if (Physics.Raycast (pos, transform.forward, out hit, sensorLength)) {  
				if (hit.transform.tag != "Terrain") {  
					if (hit.normal.x < 0) {
						avoidSensitivity = -1;
					} else {
						avoidSensitivity = 1;
					} 
					Debug.DrawLine (pos, hit.point, Color.red);

				}  
			}  
		}
		if (flag != 0) {
			AvoidSteer (avoidSensitivity);        
		}
	}

	void AvoidSteer (float avoidSensitivity)
	{
		wheelFL.steerAngle = avoidSpeed * avoidSensitivity;
		wheelFR.steerAngle = avoidSpeed * avoidSensitivity;
	}

	void BackItUp(){
		//if (flag != 0) {

		//}
	}

	public IEnumerator Respawn (){

		inSector = false;
		Vector3 temp = transform.position;
		for (int i = 1;i < 5; i++) {
			yield return new WaitForSeconds (1f);
			time++;
		}
		time = 1;
		disfail = Vector3.Distance (transform.position, temp);
		if (disfail < .1f) {
			Debug.Log ("blah");
			transform.position = path [currentPathObj].transform.position;

			transform.rotation = path [currentPathObj].transform.rotation;

		}
		inSector = true;
	}
}