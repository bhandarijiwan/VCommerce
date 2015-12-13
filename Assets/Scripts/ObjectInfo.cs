using UnityEngine;
using System.Collections;

public class ObjectInfo : MonoBehaviour{

	public string Name = "";

	public string Size = "";

	public float Price;

	public string malefemale = "";

	public string material = "";

	public string shipping = "";

	public string brand= "";

	public ObjectInfo deepcopy(){

		ObjectInfo copy = (ObjectInfo)this.MemberwiseClone ();

		copy.Name = string.Copy (this.Name);
		copy.Size = string.Copy (this.Size);
		copy.Price = Price;
		copy.malefemale = string.Copy (this.malefemale);
		copy.material = string.Copy (this.material);
		copy.shipping = string.Copy (this.shipping);
		copy.brand = string.Copy (this.brand);
		return copy;
	}
}
