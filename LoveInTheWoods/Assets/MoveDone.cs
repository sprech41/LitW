using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDone : MonoBehaviour {

	public bool stillMoving = true;

	// Update is called once per frame
	void Update () {
		Component[] chars = this.GetComponentsInChildren<move>();
		bool done = true;
		foreach (move m in chars) {
			if (m.enabled == true) {
				done = false;
				break;
			}
		}

		stillMoving = !done;
	}
}
