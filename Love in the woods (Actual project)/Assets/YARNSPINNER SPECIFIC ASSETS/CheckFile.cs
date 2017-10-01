using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class CheckFile : MonoBehaviour {

	public Text t;
	public int num;
	public void Check()
	{
		string n = gameObject.name;
		if (File.Exists (Application.dataPath + "/saves/" + name)) {
			t.text = "Save File " + num;
		}

	}
	// Use this for initialization
	void Update () {
		Check ();
	}

}
