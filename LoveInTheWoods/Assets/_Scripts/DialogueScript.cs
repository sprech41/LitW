using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class DialogueScript : MonoBehaviour {
    private List<string> messages;
    private int currentMessage;
    private float currentCharacter;
    private int lastCharacter;
    private Regex getBrackets;

    public Text label;
    public Image character;

    public Sprite maepose1;
    public Sprite maePose2;
    public Sprite maepose3;

    public int CharacterHeight;

    void Start() {
        messages = Load("Assets/Mae1.txt");
        if (messages != null) {
            currentMessage = 0;
            currentCharacter = 0;
            getBrackets = new Regex(@"\[(.*?)\]", RegexOptions.IgnoreCase);
        } 
    }


    void FixedUpdate() {
        if (messages == null) return;

        if (currentMessage >= messages.Count) return;
        string currentMessageString = messages[currentMessage];

        if (Input.GetKeyUp("space")) {
            if (currentMessageString.Length <= (int) currentCharacter) { // If message has finished displaying
                if (currentMessage < messages.Count) {
                    currentMessage++;
                    currentCharacter = 0.0f;
                    lastCharacter = 0;
                    label.text = "";

                    
                }
            } else {
                label.text = currentMessageString;
                currentCharacter = (float) currentMessageString.Length;
            }
        } else {
            if (currentMessageString.Length <= currentCharacter) { // If message has finished displaying
                // Do nothing 
            } else {
                if ((int) currentCharacter == 0) { // If start of the message
                    Match m = getBrackets.Match(currentMessageString);
                    if (m.Success) {
                        Group g = m.Groups[0];

                        currentMessageString = currentMessageString.Replace(g.ToString(), "");
                        messages[currentMessage] = currentMessageString;

                        switch (g.ToString().ToLower()) {
                            case "[maepose1]":
                                character.sprite = maepose1;
                                break;
                            case "[maepose2]":
                                character.sprite = maePose2;
                                break;
                        }

                        double newWidthToHeight = character.sprite.texture.width / (double) character.sprite.texture.height;
                        RectTransform characterTransform = character.transform as RectTransform;
                        characterTransform.sizeDelta = new Vector2((float) (newWidthToHeight * CharacterHeight), CharacterHeight);
                    }

                    label.text = currentMessageString[0].ToString();
                }

                if (lastCharacter != (int)currentCharacter) { // To prevent letter from being repeated
                    label.text += currentMessageString[(int) currentCharacter];
                    lastCharacter = (int)currentCharacter;
                }
                currentCharacter += 0.5f;
            }
        }
    }

    private List<string> Load(string fileName) {
        try {
            string currLine;
            List<string> entries = new List<string>();

            StreamReader inStream = new StreamReader(fileName, Encoding.Default);

            using (inStream) {
                while (true) {
                    currLine = inStream.ReadLine();

                    if (currLine != null) 
                        entries.Add(currLine);
                    else 
                        break;
  
                }
                inStream.Close();
            }
            return entries;
        }
        catch (Exception e) {
            Debug.Log("Error trying to load file " + fileName + ": " + e.Message);
            return null;
        }
    }
}

