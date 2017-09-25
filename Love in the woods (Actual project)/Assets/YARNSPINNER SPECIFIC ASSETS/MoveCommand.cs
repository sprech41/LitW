using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yarn.Unity.Example {
	public class MoveCommand : MonoBehaviour {

		public Transform[] positions;

		[YarnCommand("move")]
		public void MoveChar(string target, string speed)
		{
			Transform g = null;

			foreach (var pos in positions) {
				if (pos.name == target) {
					g = pos;
					break;
				}
			}

			if (g == null)
				Debug.LogErrorFormat("Can't find position named {0}!", target);

			gameObject.GetComponent<move> ().enabled = true;
			gameObject.GetComponent<move> ().target = g;
			gameObject.GetComponent<move> ().speed = float.Parse (speed);
		}
	}
}