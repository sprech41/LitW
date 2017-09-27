using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yarn.Unity.Example {
	public class MoveCommand : MonoBehaviour {

		public Transform[] positions;

		//This function activates the movement function and sets the target and speed
		[YarnCommand("move")]
		public void MoveChar(string target, string speed)
		{
			Transform g = null;

			//search for the desired target
			foreach (var pos in positions) {
				if (pos.name == target) {
					g = pos;
					break;
				}
			}

			if (g == null)
				Debug.LogErrorFormat("Can't find position named {0}!", target);

			//enable the move function, causing the sprite to move every frame.
			gameObject.GetComponent<move> ().enabled = true;
			gameObject.GetComponent<move> ().target = g;
			gameObject.GetComponent<move> ().speed = float.Parse (speed);
		}
	}
}