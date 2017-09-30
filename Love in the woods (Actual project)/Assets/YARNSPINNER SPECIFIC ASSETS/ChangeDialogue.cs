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

			//Search through the list for the correct dialogue box
			foreach (var x in dialogueList) {
				if (x.name == arg) 
				{
					t = x;
					break;
				}
			}

			if (t == null)
				Debug.LogErrorFormat("Can't find dialogue named {0}!", arg);

			//StartCoroutine (CheckDone ());
			//deactivate the current dialogue box, activate the new one.
			current.enabled = false;
			current.GetComponentInParent<SpriteRenderer> ().enabled = false;
			t.enabled = true;
			t.GetComponentInParent<SpriteRenderer> ().enabled = true;
			gameObject.GetComponent<ExampleDialogueUI> ().lineText = t;
		}	
	
		/**********************
		 * This stuff is me trying to have a delay on the dialogue. Didn't work.
		 * 
		public bool MoveDone()
		{
			return GetComponentInChildren<MoveDone> ().stillMoving;
		}
			
		IEnumerator CheckDone()
		{
			//while (!MoveDone ()) 
			//{
				//Debug.Log ("Failed");
			if (!MoveDone())
				yield return new WaitForSeconds (1);
			//}
			//Debug.Log ("Passed");
			//deactivate the current dialogue box, activate the new one.
			//current.enabled = false;
			//current.GetComponentInParent<SpriteRenderer> ().enabled = false;
			//t.enabled = true;
			//t.GetComponentInParent<SpriteRenderer> ().enabled = true;
			//gameObject.GetComponent<ExampleDialogueUI> ().lineText = t;
		}
		*********************/
	}
}
