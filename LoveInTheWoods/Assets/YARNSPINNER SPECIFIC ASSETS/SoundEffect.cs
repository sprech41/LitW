using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yarn.Unity.Example{
	public class SoundEffect : MonoBehaviour {
		public AudioClip[] clips;
		private AudioSource source;

		// Use this for initialization
		void Start () {
			source = GetComponent<AudioSource> ();
		}
	
		[YarnCommand("playsound")]
		public void playSound (string n)
		{
			AudioClip clip = null;
			foreach (AudioClip c in clips) {
				if (c.name == n){
					clip = c;
					break;
				}
			}

			if (clip == null)
			{
				Debug.LogFormat ("Clip of name {0} not found", n);
				return;
			}

			source.PlayOneShot (clip,1F);
		}
	}
}
