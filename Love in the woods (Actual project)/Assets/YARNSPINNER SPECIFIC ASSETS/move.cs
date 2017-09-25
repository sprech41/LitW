using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

	public Transform target;
	public float speed;
	public float minSpeed;
	private Vector3 newXPos;

	void FixedUpdate() {
		if (target != null) {
			
			speed = Mathf.Lerp (speed, minSpeed, Time.deltaTime);
			float step = speed * Time.deltaTime;
			//float dist = Vector3.Distance (target.position, transform.position);
			//newXPos = new Vector3 (target.position.x, 0, 0);	//only move on x axis
			//transform.position = Vector3.MoveTowards (transform.position, target.transform.position, step);
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (target.transform.position.x,
				transform.position.y, transform.position.z), step);
		}
	}
}
