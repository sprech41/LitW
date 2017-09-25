using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Yarn.Unity.Example {
	
	public class ChangeDialogue : MonoBehaviour {

		public Text[] dialogueList;

		//sets which dialogue box will display the text
		[YarnCommand("setdialogue")]
		public void SetDialogue(string arg)
		{
			Text current = gameObject.GetComponent<ExampleDialogueUI> ().lineText;
			Text t = null;
			foreach (var x in dialogueList) {
				if (x.name == arg) 
				{
					t = x;
					break;
				}
			}

			if (t == null)
				Debug.LogErrorFormat("Can't find dialogue named {0}!", arg);

			current.enabled = false;
			t.enabled = true;
			gameObject.GetComponent<ExampleDialogueUI> ().lineText = t;
		}
		
	}
}
