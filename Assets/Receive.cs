using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Receive : MonoBehaviour {


	// Use this for initialization
	void Start () {

		Debug.Log (GameObject.Find ("passMsg").GetComponent<Data> ().passTotalPrice.ToString ());
		gameObject.transform.GetComponent<Text>().text = GameObject.Find("passMsg").GetComponent<Data>().passTotalPrice.ToString(); 
	}
	

}
