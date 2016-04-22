using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICarScript : MonoBehaviour
{
	public Vector3 centerOfMass;
	public List<Transform> path;
	public Transform pathGroup;
	public float maxSteer = 30;
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelBL;
	public WheelCollider wheelBR;
	public int currentPathObj;
    public float distFromPath = 5;
	public float maxTorque = 50;
	public float currentSpeed;
	public float topSpeed = 150;
	public float decelerationSpeed = 10;
	public Renderer brakingMesh;
	public Material idleBrakeLight;
	public Material activeBrakeLight;
	public bool isBraking;
	public bool inSector;
	public float sensorLength = 5;
	public float frontSensorStartPoint = 5;
	public float frontSensorSideDistance = 5;
	public float frontSensorsAngle = 30;
	public float sidewaySensorLength = 5;
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
        if (flag == 0) {
            getSteer();
            Move();
            Sensors();
            //	BrakingEffect ();
        }
       else {
            getSteer();
            Move();
            Sensors();
            //	BrakingEffect ();
         //   AvoidSteer(0);
        }


            //}
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
		currentSpeed = 2 * (22 / 7) * wheelBL.radius * wheelBL.rpm * 60 / 1000;
		currentSpeed = Mathf.Round (currentSpeed);
		if (currentSpeed <= topSpeed && !inSector) {
			wheelBR.motorTorque = maxTorque;
			wheelBL.motorTorque = maxTorque;
			wheelBR.brakeTorque = 0;
			wheelBL.brakeTorque = 0;    
		} else if (!inSector) {
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
				Debug.DrawLine (pos, hit.point, Color.red);
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
				Debug.DrawLine (pos, hit.point, Color.red);  
			}  
		} else if (Physics.Raycast (pos, rightAngle, out hit, sensorLength)) {  
			if (hit.transform.tag != "Terrain") {  
				avoidSensitivity -= 0.5f;   
				flag++;  
				Debug.DrawLine (pos, hit.point, Color.red);  
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
				Debug.DrawLine (pos, hit.point, Color.red);  
			}  
		} else if (Physics.Raycast (pos, leftAngle, out hit, sensorLength)) {  
			if (hit.transform.tag != "Terrain") {  
				flag++;  
				avoidSensitivity += 0.5f;  
				Debug.DrawLine (pos, hit.point, Color.red);  
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
}