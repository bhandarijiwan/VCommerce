using UnityEngine;
using System.Collections;

public class Persist : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform.gameObject);
	}

}
