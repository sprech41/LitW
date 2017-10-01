using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
	
	// Update is called once per frame
	public void Unpause()
	{
		Time.timeScale = 1;
	}
	public void StopTime () {
		//ZA WARUDO
		Time.timeScale = 0;
	}
}
