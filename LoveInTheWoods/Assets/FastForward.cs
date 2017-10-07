using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastForward : MonoBehaviour {

	private bool active = false;
	public void ChangeSpeed()
	{
		if (!active) {
			Time.timeScale = 5;
			active = true;
		} else {
			Time.timeScale = 1;
			active = false;
		}
	}
}
