using UnityEngine;
using System.Collections;

public class PlaySoundOnStep : MonoBehaviour {

	public AudioSource sound;

	// Use this for initialization
	void Start () {
		StepDetector.instance.OnStepDetected += OnStep;
	}

	void OnStep()
	{
		sound.Play();
	}
}
