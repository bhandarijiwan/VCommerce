using UnityEngine;
using System.Collections;

public class CheckoutHandler : MonoBehaviour {

	public void handlcheckout(){

		Cardboard.SDK.VRModeEnabled=false;

		Application.LoadLevel (2);

	}
}
