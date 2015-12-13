using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersistentData : MonoBehaviour {

	//public List<ObjectInfo> itemsInformation;

	public List<itemInfo> itemList;

	void Awake(){

		itemList = new List<itemInfo> ();

		//itemsInformation = new List<ObjectInfo>();

		DontDestroyOnLoad(transform.gameObject);

	}



	public class itemInfo{

		public string Name = "";
		
		public string Size = "";
		
		public float Price;
		
		public string malefemale = "";
		
		public string material = "";
		
		public string shipping = "";
		
		public string brand= "";

		public itemInfo(ObjectInfo obj){
		
			Name = obj.Name;
			Size = obj.Size;
			Price = obj.Price;
			malefemale = obj.malefemale;
			material = obj.material;
			shipping = obj.shipping;
			brand= obj.brand;
		}

	}

}
