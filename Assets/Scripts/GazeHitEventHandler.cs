using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GazeHitEventHandler : MonoBehaviour
{
	public static bool startGazeEvents;
    public float gazeTime ;
    public bool hit;
    public float curGazeTime = 0f;

    public GameObject head;
	[HideInInspector]
	public GameObject GameObjectHit;
	public GameObject infoView;

	public GameObject player;

	private GameObject currentDress;

	public GameObject prefabObject;

	private GameObject persistentObject;

	private GameObject waitMSG;


	private bool goToNext;

	private List<string> itemsChecked;


	public Text NameVal;
	public Text SizeVal;
	public Text PriceVal;
	public Text BrandVal;
	public Text ShippingVal;
	public Text MaterialVal;
	public Text M_FVal;
	public Button CheckedButton;



	public Transform overlayTransform;



    // Use this for initialization
    void Start()
    {
		goToNext = false;

		startGazeEvents = false;
		itemsChecked = new List<string> ();

		waitMSG = GameObject.Find ("WaitMSG");
		waitMSG.SetActive (false);

		infoView.SetActive(false);

		persistentObject = GameObject.Find ("PersistentObject");
	}

    void Update()
    {
		GameObjectHit = EventSystem.current.currentSelectedGameObject;
		if (GameObjectHit && startGazeEvents)
            if (GameObjectHit.layer == 8 || GameObjectHit.layer == 9) // layer 8 is the dress layer.
                curGazeTime += Time.deltaTime;
            else
            {
                if (curGazeTime > 0)
                    curGazeTime = 0;
                infoView.SetActive(false);
            }

		if (curGazeTime >= gazeTime) {
			if (GameObjectHit.layer == 8 && !infoView.activeInHierarchy) {
				vSelectHandle ();
			}
		}
		if (Cardboard.SDK.Triggered) {
			vTriggerPulled(true);
		}
        
    }

    public void setGazedAt(bool gazedAt)
    {
		hit = gazedAt;
        GameObjectHit = (hit) ? (EventSystem.current.currentSelectedGameObject) : (null);
    }

    public void vSelectHandle()
    {


		Vector3 p = GameObjectHit.GetComponent<Renderer>().bounds.center;
		Vector3 v = (player.transform.position - p).normalized;
		infoView.transform.position = p + v * 1.30f;

		infoView.transform.rotation = head.transform.rotation;
        populateFields();
		CheckedButton.gameObject.SetActive (itemsChecked.Contains (GameObjectHit.name));
		infoView.SetActive(true);
	}
    void vTriggerPulled(bool checkOut)
    {
		if (checkOut)
        {
			CheckedButton.gameObject.SetActive (true);
			PersistentData.itemInfo item  = new PersistentData.itemInfo(GameObjectHit.GetComponent<ObjectInfo>());
			persistentObject.GetComponent<PersistentData>().itemList.Add(item);
			itemsChecked.Add (GameObjectHit.name);
		}
        else
        {
			// gonna have to go to the next scene
			StartCoroutine(takephoneout(7.0f));
        }
    }

    void populateFields()
    {

        ObjectInfo info = GameObjectHit.GetComponent<ObjectInfo>();
		NameVal.text = info.Name;
		SizeVal.text = info.Size;
		PriceVal.text = info.Price.ToString();
		BrandVal.text = info.brand;
		ShippingVal.text = info.shipping;
		MaterialVal.text = info.material;
		M_FVal.text = info.malefemale;

    }
//	private void vOverlayHandle ()
//	{ 
//		mannequin.SetActive (true);
//
//		if (currentDress != null)
//		{
//			Destroy(currentDress);
//			currentDress = null;
//		}
//		currentDress = Instantiate(prefabObject);
//		currentDress.transform.position = overlayTransform.position;
//		currentDress.transform.parent = mannequin.transform;
//		prefabObject.layer = 10;
//
//		// re-position the camera;
//		rigidBody.transform.position = overlayTransform.position;
//		rigidBody.transform.Translate (new Vector3 (0f, 0f, -1.5f), Space.Self);
//
//	}

	 IEnumerator takephoneout(float waittime){

		waitMSG.SetActive (true);

		infoView.SetActive (false);
		waitMSG.transform.position = player.transform.position;

		waitMSG.transform.rotation = head.transform.rotation;

		waitMSG.transform.Translate (player.transform.forward * -3.5f);

		goToNext = true;

		yield return new WaitForSeconds(waittime);

		if(goToNext){
			Cardboard.SDK.VRModeEnabled=false;
			Application.LoadLevel(1);
		}
	
	}

}

