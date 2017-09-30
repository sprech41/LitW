using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

	public Transform target;
	public float speed;
	public float minSpeed;
	private Vector3 newXPos;
	private float epsilon = 0.0001f;	//acceptable error

	//moves the character towards a target position. Speed decreases linearly via Lerp
	void FixedUpdate() {
		if (target != null) {
			
			speed = Mathf.Lerp (speed, minSpeed, Time.deltaTime);
			float step = speed * Time.deltaTime;

			//move the character towards the target, but ONLY on the x axis.
			newXPos = new Vector3 (target.transform.position.x, transform.position.y, transform.position.z);
			transform.position = Vector3.MoveTowards (transform.position, newXPos, step);
			//Debug.LogFormat ("Transform position is {0}", transform.position.x);
			//Debug.LogFormat ("Target position is {0}", newXPos.x);

			//if (Mathf.Approximately(transform.position.x, newXPos.x))
			if (Mathf.Abs (transform.position.x - newXPos.x) < epsilon) {
				enabled = false;
			}
		}
	}
}
