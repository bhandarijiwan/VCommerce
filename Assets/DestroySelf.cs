using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DestroySelf : MonoBehaviour {

	public GameObject textobject;
	public Text textComponent;
	public Image panelImage; 

	private float alphaInterp;

	private Color startPanelColor;
	private Color endPanelColor;

	private Color startTextColor;
	private Color endTextColor;
	// Use this for initialization
	void Start () {
		alphaInterp = 0.0f;
		startPanelColor = panelImage.color;
		endPanelColor = startPanelColor;
		endPanelColor.a = 0.0f;
		startTextColor = textComponent.color;
		endTextColor = startTextColor;
		endTextColor.a = 0.0f;
		panelImage.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {

		panelImage.color = Color.Lerp (startPanelColor, endPanelColor, alphaInterp);
		textComponent.color = Color.Lerp (startTextColor, endTextColor, alphaInterp);
		alphaInterp += Time.deltaTime *0.2f;
		if (alphaInterp > 1.0f) {
			GazeHitEventHandler.startGazeEvents = true;
			textobject.SetActive (false);
		}

	}
}
