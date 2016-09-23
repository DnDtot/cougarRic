using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Paths : MonoBehaviour {

	public Color rayColor = Color.white;
	public List <Transform> path_obj = new List<Transform> ();
	Transform[] theArray;

	void Start(){
		theArray = GetComponentsInChildren<Transform> ();
		path_obj.Clear ();
		foreach (Transform path_objs in theArray) {
			if (path_objs != this.transform) {

				path_obj.Add (path_objs);
			}
		}
	for (int i = 0; i < path_obj.Count; i++){
			if (i == transform.childCount - 1) {
				path_obj [i].LookAt (path_obj [0].position);
			} else {
				path_obj [i].LookAt (path_obj [i + 1].position);
			}
		}
	}
	void OnDrawGizmos()
	{
		Gizmos.color = rayColor;

		for (int i = 0; i < path_obj.Count; i++) {
			Vector3 position = path_obj [i].position;

			if (i > 0) {
				Vector3 previous = path_obj [ i - 1 ].position;
					Gizmos.DrawLine (previous, position);
				Gizmos.DrawWireSphere (position, 0.3f);

			}
		}
	}
}