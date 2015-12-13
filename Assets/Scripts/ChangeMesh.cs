using UnityEngine;
using System.Collections;

public class ChangeMesh : MonoBehaviour {

    private GameObject currentDress;
    public Transform dressPosition;
    public GameObject prefabObject;

    public void vOverlayHandle()
    {
        setDress(prefabObject);
    }

    public void setDress(GameObject dressPrefab)
    { 
        if (currentDress != null)
        {
            Destroy(currentDress);
            currentDress = null;
        }

        currentDress = Instantiate(dressPrefab);
        currentDress.transform.position = dressPosition.position;
        currentDress.transform.parent = this.transform;
        dressPrefab.layer = 10;
    }
}
