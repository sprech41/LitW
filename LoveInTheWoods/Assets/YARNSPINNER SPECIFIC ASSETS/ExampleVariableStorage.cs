/*
The MIT License (MIT)
Copyright (c) 2015-2017 Secret Lab Pty. Ltd. and Yarn Spinner contributors.
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Yarn.Unity;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using Yarn;

/// An extremely simple implementation of DialogueUnityVariableStorage, which
/// just stores everything in a Dictionary.
public class ExampleVariableStorage : VariableStorageBehaviour {

    /// Where we actually keeping our variables
    Dictionary<string, Yarn.Value> variables = new Dictionary<string, Yarn.Value>();

    [System.Serializable]
    public class SerSaveData { // Class that stores variables
        public DefaultVariable[] variables;
        public String currentNode;
        public SerCharacterPos[] characterPositions;
    }

    [System.Serializable]
    public class SerCharacterPos {
        public string name;
        public SerVector3 pos;
    }

    [System.Serializable]
    public class SerVector3 {
        public float x, y, z;
    }

    public SerVector3 Vector3ToSerVector3(Vector3 v) {
        SerVector3 sv = new SerVector3();
        sv.x = v.x;
        sv.y = v.y;
        sv.z = v.z;
        return sv;
    }

    public Vector3 SerVector3ToVector3(SerVector3 sv) {
        return new Vector3(sv.x, sv.y, sv.z);
    }

    public Transform characterHolder;
    
    public SerSaveData saveData;

    public void SaveData(string fileName, string currentNodeName) { // Saves data to a folder called "saves" with the given filename
        if (!Directory.Exists(Application.dataPath + "/saves")) // Create saves directory
            Directory.CreateDirectory(Application.dataPath + "/saves");

        BinaryFormatter bf = new BinaryFormatter(); // Create serializer and savedata file
        FileStream file = File.Create(String.Format("{0}/saves/{1}", Application.dataPath, fileName));

        saveData = new SerSaveData();
        saveData.variables = new DefaultVariable[variables.Count];

        int i = 0;
        foreach (KeyValuePair<string, Yarn.Value> entry in variables) // Iterate throught the variables dictionary and put them into saveData
        {
            saveData.variables[i] = new DefaultVariable();
            saveData.variables[i].name = entry.Key;
            saveData.variables[i].type = entry.Value.type;
            saveData.variables[i].value = entry.Value.AsString;
            i++;
        }

        saveData.currentNode = currentNodeName;

        if (characterHolder != null) {
            saveData.characterPositions = new SerCharacterPos[characterHolder.childCount];

            i = 0;
            foreach (Transform child in characterHolder) {
                SerCharacterPos spos = new SerCharacterPos();
                spos.name = child.gameObject.name;
                spos.pos = Vector3ToSerVector3(child.position);
                saveData.characterPositions[i] = spos;
                i++;
            }   
        }

        bf.Serialize(file, saveData); // Serialize the data and save it
        file.Close(); // Close the file
    }


    public bool LoadData(string fileName) // Loads saveData from the specified location; returns true if successful and false if not
    {
        string filePath = String.Format("{0}/saves/{1}", Application.dataPath, fileName);
        if (File.Exists(filePath)) // Check to make sure the file exists first
        {
            BinaryFormatter bf = new BinaryFormatter(); // Make a serializer and open the file
            FileStream file = File.Open(filePath, FileMode.Open);

            saveData = (SerSaveData)bf.Deserialize(file); // Deserialize the stored data

            Clear(); // Clear previous variables

            foreach (var variable in saveData.variables) { // Copied code because I'm lazy
                object value;

                switch (variable.type) {
                    case Yarn.Value.Type.Number:
                        float f = 0.0f;
                        float.TryParse(variable.value, out f);
                        value = f;
                        break;

                    case Yarn.Value.Type.String:
                        value = variable.value;
                        break;

                    case Yarn.Value.Type.Bool:
                        bool b = false;
                        bool.TryParse(variable.value, out b);
                        value = b;
                        break;

                    case Yarn.Value.Type.Variable:

                        continue;

                    case Yarn.Value.Type.Null:
                        value = null;
                        break;

                    default:
                        throw new System.ArgumentOutOfRangeException();

                }

                var v = new Yarn.Value(value);

                SetValue(variable.name, v);
            } // End of the code I copied because I'm lazy

            if (saveData.characterPositions != null) {
                for (int i = 0; i < saveData.characterPositions.Length; i++) {
                    Transform character = characterHolder.Find(saveData.characterPositions[i].name);
                    if (character) {
                        character.position = SerVector3ToVector3(saveData.characterPositions[i].pos);
                    }
                }
            }

            file.Close(); // Close the file and return a success
            return true;
        } else {
            return false;
        }
    }

    /// A default value to apply when the object wakes up, or
    /// when ResetToDefaults is called
    [System.Serializable]
    public class DefaultVariable {
        /// Name of the variable
        public string name;
        /// Value of the variable
        public string value;

        //public string getVal()
        //{
        //	return value;
        //}
        /// Type of the variable
        public Yarn.Value.Type type;
    }

    /// Our list of default variables, for debugging.
    public DefaultVariable[] defaultVariables;

    [Header("Optional debugging tools")]
    /// A UI.Text that can show the current list of all variables. Optional.
    public UnityEngine.UI.Text debugTextView;

    /// Reset to our default values when the game starts
    void Awake() {
        ResetToDefaults();
    }

    /// Erase all variables and reset to default values
    public override void ResetToDefaults() {
        Clear();

        // For each default variable that's been defined, parse the string
        // that the user typed in in Unity and store the variable
        foreach (var variable in defaultVariables) {

            object value;

            switch (variable.type) {
                case Yarn.Value.Type.Number:
                    float f = 0.0f;
                    float.TryParse(variable.value, out f);
                    value = f;
                    break;

                case Yarn.Value.Type.String:
                    value = variable.value;
                    break;

                case Yarn.Value.Type.Bool:
                    bool b = false;
                    bool.TryParse(variable.value, out b);
                    value = b;
                    break;

                case Yarn.Value.Type.Variable:
                    // We don't support assigning default variables from other variables
                    // yet
                    Debug.LogErrorFormat("Can't set variable {0} to {1}: You can't " +
                        "set a default variable to be another variable, because it " +
                        "may not have been initialised yet.", variable.name, variable.value);
                    continue;

                case Yarn.Value.Type.Null:
                    value = null;
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException();

            }

            var v = new Yarn.Value(value);

            SetValue(variable.name, v);
        }
    }

    /// Set a variable's value
    public override void SetValue(string variableName, Yarn.Value value) {
        // Copy this value into our list
        variables[variableName] = new Yarn.Value(value);
    }

    /// Get a variable's value
    public override Yarn.Value GetValue(string variableName) {
        // If we don't have a variable with this name, return the null value
        if (variables.ContainsKey(variableName) == false)
            return Yarn.Value.NULL;

        return variables[variableName];
    }

    /// Erase all variables
    public override void Clear() {
        variables.Clear();
    }

	public void setPlayerName(Text x) {
		object val = x.text;
		var v = new Yarn.Value(val);
		//defaultVariables[0].value = x.text;
		SetValue ("$playername", v);
		Debug.LogErrorFormat("player name is {0}", getPlayerName());
	}

	[YarnCommand("getname")]
	public string getPlayerName() {
		//return defaultVariables[0].value;
		return GetValue("$playername").AsString;
	}

    /// If we have a debug view, show the list of all variables in it
    void Update() {
        if (debugTextView != null) {
            var stringBuilder = new System.Text.StringBuilder();
            foreach (KeyValuePair<string, Yarn.Value> item in variables) {
                stringBuilder.AppendLine(string.Format("{0} = {1}",
                                                         item.Key,
                                                         item.Value));
            }
            debugTextView.text = stringBuilder.ToString();
        }
    }

}