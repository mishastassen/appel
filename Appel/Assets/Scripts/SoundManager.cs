using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
	public AudioClip testsound;
	AudioSource audio;

	void Start(){
		audio = GetComponent<AudioSource>();
	}
	// Use this for initialization
	void OnCollisionEnter (Collision koekje) {
		// Heel veel botsgeluiden
			// audio.PlayOneShot (testsound,1.0F);
		/* AudioSource audio = GetComponent<AudioSource>();
		audio.Play(); */
	}
		
}
