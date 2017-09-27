using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

	public Transform target;
	public float speed;
	public float minSpeed;
	private Vector3 newXPos;

	//moves the character towards a target position. Speed decreases linearly via Lerp
	void FixedUpdate() {
		if (target != null) {
			
			speed = Mathf.Lerp (speed, minSpeed, Time.deltaTime);
			float step = speed * Time.deltaTime;

			//move the character towards the target, but ONLY on the x axis.
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (target.transform.position.x,
				transform.position.y, transform.position.z), step);
		}
	}
}
