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
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Yarn.Unity.Example {
    /// Displays dialogue lines to the player, and sends
    /// user choices back to the dialogue system.

    /** Note that this is just one way of presenting the
     * dialogue to the user. The only hard requirement
     * is that you provide the RunLine, RunOptions, RunCommand
     * and DialogueComplete coroutines; what they do is up to you.
     */
    public class ExampleDialogueUI : Yarn.Unity.DialogueUIBehaviour
    {
		//used so this script knows when to not accept input
		public bool menuActive = false;
		public GameObject menuButton;

		public bool ffwdActive = false;

		//log of dialogue
		public Text log;

        /// The object that contains the dialogue and the options.
        /** This object will be enabled when conversation starts, and 
         * disabled when it ends.
         */
        public GameObject dialogueContainer;

        /// The UI element that displays lines
        public Text lineText;

        /// A UI element that appears after lines have finished appearing
        public GameObject continuePrompt;

        /// A delegate (ie a function-stored-in-a-variable) that
        /// we call to tell the dialogue system about what option
        /// the user selected
        private Yarn.OptionChooser SetSelectedOption;

        /// How quickly to show the text, in seconds per character
        [Tooltip("How quickly to show the text, in seconds per character")]
        public float textSpeed = 0.025f;

        /// Make it possible to temporarily disable the controls when
        /// dialogue is active and to restore them when dialogue ends
        public RectTransform gameControlsContainer;

        /// The arrows for the dialogue
        public Image LeftArrow;
        public Image RightArrow;

        /// Dots that show which option you're on
        public List<Component> Dots;

        // The text that shows the options
        public Text OptionsText;

        void Awake ()
        {
            // Start by hiding the container, line and option buttons
            if (dialogueContainer != null)
                dialogueContainer.SetActive(false);

            lineText.gameObject.SetActive (false);

            // Hide the continue prompt if it exists
            if (continuePrompt != null)
                continuePrompt.SetActive (false);

            LeftArrow.gameObject.SetActive(false);
            RightArrow.gameObject.SetActive(false);

            foreach (Component dot in Dots) {
                dot.gameObject.SetActive(false);
            }

            OptionsText.gameObject.SetActive(false);
        }

		//The following is a workaround solution that allows
		// yarn to print variables.
		/***************************************************/
		string CheckVars (string input)
		{
			string output = string.Empty;
			bool checkingVar = false;
			string currentVar = string.Empty;

			int index = 0;
			while (index < input.Length) {
				if (input [index] == '[') {
					checkingVar = true;
					currentVar = string.Empty;
				} else if (input [index] == ']') {
					checkingVar = false;
					output += ParseVariable(currentVar);
					currentVar = string.Empty;
				} else if (checkingVar) {
					currentVar += input [index];
				} else {
					output += input[index];
				}
				index += 1;
			}

			return output;
		}

		string ParseVariable (string varName)
		{
			//Check YarnSpinner's variable storage first
			if (GetComponent<ExampleVariableStorage>().GetValue (varName) != Yarn.Value.NULL) {
				Debug.LogErrorFormat ("varname is {0}", varName);
				return GetComponent<ExampleVariableStorage>().GetValue (varName).AsString;
			}

			//Handle other variables here
			if(varName == "$playerName") {
				return GetComponent<ExampleVariableStorage> ().getPlayerName ();
			}

			//If no variables are found, return the variable name
			return varName;
		}
			
		/**************************************/

        /// Show a line of dialogue, gradually
        public override IEnumerator RunLine (Yarn.Line line)
        {
            // Show the text
            lineText.gameObject.SetActive (true);

            if (textSpeed > 0.0f) {
                // Display the line one character at a time
                var stringBuilder = new StringBuilder ();

				foreach (char c in CheckVars(line.text)) {
                    stringBuilder.Append (c);
                    lineText.text = stringBuilder.ToString ();
                    yield return new WaitForSeconds (textSpeed);
                }
            } else {
                // Display the line immediately if textSpeed == 0
				lineText.text = CheckVars(line.text);
            }

			log.text += CheckVars (line.text) + "\n";	//update the log

            // Show the 'press any key' prompt when done, if we have one
            if (continuePrompt != null)
                continuePrompt.SetActive (true);

			if (!ffwdActive) {
				// Wait for any user input. DON'T ACCEPT INPUT IF USER CLICKED MENU OR IS IN THE MENU
				while (Input.anyKeyDown == false || EventSystem.current.currentSelectedGameObject == menuButton || menuActive) {
					yield return null;
				}
			}
            // Hide the text and prompt
            //lineText.gameObject.SetActive (false);

            if (continuePrompt != null)
                continuePrompt.SetActive (false);

        }

        // tj's stuff

        int currentOption;
        bool messageDisplayed;
        int totalOptions;

        public void incrementOption() {
            if (messageDisplayed)
            {
                Dots[currentOption].transform.Find("LightDot").gameObject.SetActive(false);

                currentOption = (currentOption + 1) % totalOptions;

                messageDisplayed = false;

                //RightArrow.gameObject.SetActive(currentOption < totalOptions - 1);
                //LeftArrow.gameObject.SetActive(currentOption > 0);

                Dots[currentOption].transform.Find("LightDot").gameObject.SetActive(true);
            }
        }

        public void decrementOption() {
            if (messageDisplayed)
            {
                Dots[currentOption].transform.Find("LightDot").gameObject.SetActive(false);
                
                currentOption--;
                if (currentOption < 0)
                    currentOption = totalOptions - 1;

                messageDisplayed = false;

                //RightArrow.gameObject.SetActive(currentOption < totalOptions - 1);
                //LeftArrow.gameObject.SetActive(currentOption > 0);

                Dots[currentOption].transform.Find("LightDot").gameObject.SetActive(true);
            }
        }

        public override IEnumerator RunOptions(Yarn.Options optionsCollection,
                                            Yarn.OptionChooser optionChooser)
        {
            yield return null; // this fixes a bug i don't know why just leave it here

            currentOption = 0;
            messageDisplayed = false;
            totalOptions = optionsCollection.options.Count;

            // Record that we're using it
            SetSelectedOption = optionChooser;

            RightArrow.gameObject.SetActive(true);
            LeftArrow.gameObject.SetActive(true);

            Dots[0].transform.Find("LightDot").gameObject.SetActive(true);

            for (int i = 0; i < optionsCollection.options.Count; i++) {
                Dots[i].gameObject.SetActive(true);
            }

            while (SetSelectedOption != null)
            {
                if (!(EventSystem.current.currentSelectedGameObject == menuButton || menuActive))
                { // Don't run if menu is active
                    if (Input.GetKeyDown("right"))
                    {
                        incrementOption();
                    }
                    else if (Input.GetKeyDown("left"))
                    {
                        decrementOption();
                    }
                    else if (Input.anyKeyDown && !Input.GetMouseButtonDown(0))
                    { // Key was pressed that wasnt left or right or left mouse button
                        SetOption(currentOption);
                        break;
                    }

                    if (!messageDisplayed)
                    {

                        // Show the text
                        OptionsText.gameObject.SetActive(true);

                        if (textSpeed > 0.0f)
                        {
                            // Display the line one character at a time
                            var stringBuilder = new StringBuilder();

                            foreach (char c in CheckVars(optionsCollection.options[currentOption]))
                            {
                                stringBuilder.Append(c);
                                OptionsText.text = stringBuilder.ToString();
                                yield return new WaitForSeconds(textSpeed);
                            }
                        }
                        else
                        {
                            // Display the line immediately if textSpeed == 0
                            OptionsText.text = CheckVars(optionsCollection.options[currentOption]);
                        }

                        messageDisplayed = true;
                    }
                }

                yield return null;
            }


            // Wait until the chooser has been used and then removed (see SetOption below)
            while (SetSelectedOption != null)
            {
                yield return null;
            }

            LeftArrow.gameObject.SetActive(false);
            RightArrow.gameObject.SetActive(false);

            Dots[currentOption].transform.Find("LightDot").gameObject.SetActive(false);
            for (int i = 0; i < optionsCollection.options.Count; i++)
            {
                Dots[i].gameObject.SetActive(false);
            }

            OptionsText.gameObject.SetActive(false);
        }

        /// Called by buttons to make a selection.
        public void SetOption (int selectedOption)
        {

            // Call the delegate to tell the dialogue system that we've
            // selected an option.
            SetSelectedOption (selectedOption);

            // Now remove the delegate so that the loop in RunOptions will exit
            SetSelectedOption = null; 
        }

        /// Run an internal command.
        public override IEnumerator RunCommand (Yarn.Command command)
        {
            // "Perform" the command
            Debug.Log ("Command: " + command.text);

            yield break;
        }

        /// Called when the dialogue system has started running.
        public override IEnumerator DialogueStarted ()
        {
            Debug.Log ("Dialogue starting!");

            // Enable the dialogue controls.
            if (dialogueContainer != null)
                dialogueContainer.SetActive(true);

            // Hide the game controls.
            if (gameControlsContainer != null) {
                gameControlsContainer.gameObject.SetActive(false);
            }

            yield break;
        }

        /// Called when the dialogue system has finished running.
        public override IEnumerator DialogueComplete ()
        {
            Debug.Log ("Complete!");

            // Hide the dialogue interface.
            if (dialogueContainer != null)
                dialogueContainer.SetActive(false);

            // Show the game controls.
            if (gameControlsContainer != null) {
                gameControlsContainer.gameObject.SetActive(true);
            }

            yield break;
        }

		void Update()
		{
			if (Input.GetButton ("Fire1") || Input.GetButton("Submit"))
				textSpeed = 0.007f;
			else
				textSpeed = 0.025f;
		}

		public void SetMenuActive(bool b)
		{
			menuActive = b;
		}

		public void SetFFWD()
		{
			if (ffwdActive)
				ffwdActive = false;
			else
				ffwdActive = true;
		}

    }

}
