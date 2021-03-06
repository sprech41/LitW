﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;
using UnityEngine.SceneManagement;
using SaveDataClasses;

public class LitwEvent : MonoBehaviour {

	//the name of this event
	public new string name;

	//where in the dialogue to begin the scene
	public string startNode;

	//which scene this event is located in
	public string sceneName;

	//what times this event may happen. Currently only holds time of day
	public TimeKeeper.dayTime[] available;

	//which events must be finished in order for this event to occur
	public string[] reqEvents;

	//determines if the event has ended.
	public bool finished = false;

	//is this event currently running?
	public bool active = false;

	public DialogueRunner d;

	public LitwEvent()
	{
	}

	void Update()
	{
		//Debug.LogFormat ("finished = {0}", finished);
	}
	public void startMe()
	{
		d.StartDialogue (startNode);
		active = true;
		if (sceneName != null)
			SceneManager.LoadScene (sceneName, LoadSceneMode.Single);
	}

    // more tj stuff yea
    public void startFromSaveData(SerSaveData saveData) {
        d.StartDialogueFromSaveData(saveData);
        active = true;
        if (saveData.currentScene != null)
            SceneManager.LoadScene(saveData.currentScene, LoadSceneMode.Single);
    }
}
