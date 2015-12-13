using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GazeHitEventHandler : MonoBehaviour
{

    public float gazeTime = 5f;
    public bool hit;
    public float curGazeTime = 0f;

    public GameObject head;

    public GameObject GameObjectHit;

    public GameObject infoView;

    public GameObject mannequin;

	public GameObject rigidBody;

	private GameObject currentDress;

	public GameObject prefabObject;

	private GameObject persistentObject;

	private GameObject waitMSG;


	private bool goToNext;

	private List<string> itemsChecked;
    //private List<ObjectInfo> itemsInformation;


	public Transform overlayTransform;



    // Use this for initialization
    void Start()
    {
		goToNext = false;

		itemsChecked = new List<string> ();

		waitMSG = GameObject.Find ("WaitMSG");
		waitMSG.SetActive (false);


        infoView = GameObject.Find("InfoView");
        infoView.SetActive(false);

        head = GameObject.Find("CardboardHead");

        mannequin = GameObject.Find("Female_Mannequin");
        mannequin.SetActive(false);

        rigidBody = GameObject.Find("PlayerRigidBody");

		persistentObject = GameObject.Find ("PersistentObject");
	}

    void Update()
    {

        GameObjectHit = EventSystem.current.currentSelectedGameObject;

        if (GameObjectHit)
            if (GameObjectHit.layer == 8 || GameObjectHit.layer == 9) // layer 8 is the dress layer.
                curGazeTime += Time.deltaTime;
            else
            {
                if (curGazeTime > 0)
                    curGazeTime = 0;
                //curGazeTime -= 2*Time.deltaTime;
                infoView.SetActive(false);
            }

		if (curGazeTime >= gazeTime) {
			if (GameObjectHit.layer == 8) {
				vSelectHandle ();
			} else  if (GameObjectHit.layer == 9) {
				vOverlayHandle ();
			} else {
			}
		}
		if (Cardboard.SDK.Triggered) {

			if(GameObjectHit.layer==10)
				mannequin.SetActive(false);
			else
				vTriggerPulled(infoView.activeSelf);
		}
        
    }

    public void setGazedAt(bool gazedAt)
    {
//		Debug.Log(gazedAt);
		hit = gazedAt;
        GameObjectHit = (hit) ? (EventSystem.current.currentSelectedGameObject) : (null);
    }

    public void vSelectHandle()
    {
        infoView.transform.position = GameObjectHit.GetComponent<Renderer>().bounds.center;
        infoView.transform.rotation = head.transform.rotation;
        infoView.transform.Translate(new Vector3(0f, 0f, -1f), Space.Self);
		populateFields();
		infoView.SetActive(true);
		infoView.transform.Find ("Panel/Checked").gameObject.SetActive (itemsChecked.Contains(GameObjectHit.name));

    }
    void vTriggerPulled(bool checkOut)
    {
		if (checkOut)
        {

			infoView.transform.Find("Panel/Checked").gameObject.SetActive(true);
			PersistentData.itemInfo item  = new PersistentData.itemInfo(GameObjectHit.GetComponent<ObjectInfo>());
			persistentObject.GetComponent<PersistentData>().itemList.Add(item);
			itemsChecked.Add (GameObjectHit.name);
			//DontDestroyOnLoad(GameObjectHit.GetComponent<ObjectInfo>());
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

        infoView.transform.Find("Panel/NameVal").GetComponent<Text>().text = info.Name;
        infoView.transform.Find("Panel/SizeVal").GetComponent<Text>().text = info.Size;
        infoView.transform.Find("Panel/PriceVal").GetComponent<Text>().text = info.Price.ToString();
        infoView.transform.Find("Panel/BrandVal").GetComponent<Text>().text = info.brand;
        infoView.transform.Find("Panel/ShippingVal").GetComponent<Text>().text = info.shipping;
        infoView.transform.Find("Panel/MaterialVal").GetComponent<Text>().text = info.material;
        infoView.transform.Find("Panel/M_FVal").GetComponent<Text>().text = info.malefemale;

    }
	private void vOverlayHandle ()
	{ 
		mannequin.SetActive (true);

		if (currentDress != null)
		{
			Destroy(currentDress);
			currentDress = null;
		}
		currentDress = Instantiate(prefabObject);
		currentDress.transform.position = overlayTransform.position;
		currentDress.transform.parent = mannequin.transform;
		prefabObject.layer = 10;

		// re-position the camera;
		rigidBody.transform.position = overlayTransform.position;
		rigidBody.transform.Translate (new Vector3 (0f, 0f, -1.5f), Space.Self);

	}

	 IEnumerator takephoneout(float waittime){

		waitMSG.SetActive (true);

		infoView.SetActive (false);


		Debug.Log (rigidBody.transform.forward);

		waitMSG.transform.position = rigidBody.transform.position;

		waitMSG.transform.rotation = head.transform.rotation;

		waitMSG.transform.Translate (rigidBody.transform.forward * -3.5f);

		goToNext = true;

		yield return new WaitForSeconds(waittime);

		if(goToNext){
			Cardboard.SDK.VRModeEnabled=false;
			Application.LoadLevel(1);
		}
	
	}

}

