using SaveDataClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn.Unity.Example;

public class EventHandler : MonoBehaviour {
    //all events
    public LitwEvent[] events;

    public LitwEvent currentEvent;

    //holds a reference to the timekeeper script
    public TimeKeeper time;

    //array of all events that are currently available. this will probably be used to know which options to display when choosing between events.
    public List<LitwEvent> openEvents = new List<LitwEvent>();

    // Use this for initialization
    void Start() {
        //gonna have to have a starting event here at some point
    }

    // Update is called once per frame
    void Update() {
        //if (currentEvent != null)
        //Debug.LogFormat ("finished = {0}", currentEvent.finished);
        //This will likely check whether it recieved a "finished" signal from yarn;
    }

    public void startEvent(string n) {
        if (currentEvent != null)
            currentEvent.active = false;
        bool found = false;
        foreach (LitwEvent e in events) {
            if (e.name == n) {
                e.startMe();
                currentEvent = e;
                found = true;
                break;
            }
        }
        if (!found)
            Debug.LogErrorFormat("Event {0} not found!", n);
    }

    //check if an event is available based on time alone
    public bool checkTime(LitwEvent e) {
        foreach (TimeKeeper.dayTime t in e.available) {
            if (time.currentTime == t) {
                return true;
            }
        }
        return false;
    }

    //check if an event is available based on event requirements alone
    public bool checkReq(LitwEvent e) {
        if (e.reqEvents.Length == 0)
            return true;

        //all required events must be finished for this event to be available
        foreach (string n in e.reqEvents) {
            foreach (LitwEvent x in events) {
                if (x.name == n) {
                    //if any are not finished, immediately fail
                    if (x.finished == false)
                        return false;
                    //no duplicates, so break out of this loop
                    break;
                }
            }
        }
        return true;
    }

    //fill the list with all available events
    public void compileEvents() {
        foreach (LitwEvent e in events) {
            //must pass both tests to be available.
            if (checkTime(e) == true && checkReq(e) == true) {
                if (openEvents.Contains(e) == false)
                    openEvents.Add(e);
            }
        }
    }

    [YarnCommand("finished")]
    public void setFinished() {
        if (currentEvent != null)
            currentEvent.finished = true;
    }

    // tj's stuff tm
    public void SaveData(string filename) {
        currentEvent.d.SaveData(filename, SceneManager.GetActiveScene().name, Array.IndexOf(events, currentEvent));
    }

    public void LoadData(string fileName) {
        string filePath = String.Format("{0}/saves/{1}", Application.dataPath, fileName);
        if (File.Exists(filePath)) // Check to make sure the file exists first
        {
            BinaryFormatter bf = new BinaryFormatter(); // Make a serializer and open the file
            FileStream file = File.Open(filePath, FileMode.Open);

            SerSaveData saveData = (SerSaveData)bf.Deserialize(file); // Deserialize the stored data

            currentEvent = events[saveData.currentEvent];

            currentEvent.startFromSaveData(saveData);

            file.Close();
        }
    }
}