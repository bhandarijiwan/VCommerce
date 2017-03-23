
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollableList : MonoBehaviour
{
    public GameObject itemPrefab;

	public GameObject checkoutPrefab;


    private int itemCount = 0, columnCount = 1;

	private List <PersistentData.itemInfo> ItemList;

	private GameObject passObject;

	void Awake(){
	
		//Screen.orientation = ScreenOrientation.Portrait;
		GameObject.Find ("CartList/Panel/ContainerPanel/NoItemMSG").SetActive (false);
		ItemList = GameObject.Find ("PersistentObject").GetComponent<PersistentData> ().itemList;




	}



    void Start()
    {

		passObject = GameObject.Find ("passMsg");

		float totalPrice = 0f;

		itemCount = ItemList.Count;

		if (itemCount <= 0) {
			GameObject.Find ("CartList/Panel/ContainerPanel/NoItemMSG").SetActive (true);
		} else {
		

			List<PersistentData.itemInfo>.Enumerator enumerator = ItemList.GetEnumerator();


			RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform>();
			RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
			
			//calculate the width and height of each child item.
			float width = containerRectTransform.rect.width / columnCount;
			float ratio = width / rowRectTransform.rect.width;
			float height = rowRectTransform.rect.height * ratio;
			int rowCount = itemCount / columnCount;
			
			if (itemCount % rowCount > 0)
				rowCount++;
			//adjust the height of the container so that it will just barely fit all its children
			float scrollHeight = height * rowCount;
			containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
			containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
			
			int j = 0;
			//itemCount++;
			for (int i = 0; i < itemCount; i++)
			{
				//this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
				if (i % columnCount == 0)
					j++;
				

				GameObject newItem;
				newItem =Instantiate(itemPrefab)as GameObject;

				//				if(i==itemCount-1){
//					newItem = Instantiate(checkoutPrefab) as GameObject;
//				}
//				else{
//					newItem = Instantiate(itemPrefab) as GameObject;
//				}


				//create a new item, name it, and set the parent

				newItem.name = gameObject.name + " item at (" + i + "," + j + ")";
				newItem.transform.parent = gameObject.transform;
				
				//move and size the new item
				RectTransform rectTransform = newItem.GetComponent<RectTransform>();
				
				float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
				float y = containerRectTransform.rect.height / 2 - height * j;
				rectTransform.offsetMin = new Vector2(x, y);
				
				x = rectTransform.offsetMin.x + width;
				y = rectTransform.offsetMin.y + height;
				rectTransform.offsetMax = new Vector2(x, y);

				enumerator.MoveNext();


				//totalPrice+=enumerator.Current.Price;



				passObject.GetComponent<Data>().passTotalPrice += enumerator.Current.Price;

				//if(i<itemCount)
					populateList(newItem,enumerator.Current);

			}




        }

		GameObject.Find ("CartList/Panel").GetComponent<ScrollRect>().verticalScrollbar.value=1f;
	}
	void populateList(GameObject newitem, PersistentData.itemInfo obj){
	
//		Debug.Log (newitem);
//
//		Debug.Log (newitem.transform.Find ("Name").GetComponent<Text>().text);
//
		newitem.transform.Find ("Name").GetComponent<Text> ().text = obj.Name;
		newitem.transform.Find ("Information").GetComponent<Text> ().text = obj.brand;
		newitem.transform.Find ("Price").GetComponent<Text> ().text = obj.Price.ToString();
		newitem.transform.Find ("Shipping").GetComponent<Text> ().text = obj.shipping;
//		newitem.transform.Find ("Image").GetComponent<Image> ().sprite = obj.;
	}




}


