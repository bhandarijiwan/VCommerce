using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour {
    private string message = "Order Placed Successfully!!";
    private float displayTime = 3;
    private bool dispMsg = false;
    private int flag = 1;
    public GameObject userName, address, phone, cardNum, mm, dd, price, valName, valAddress, valPhone, valCard, valExpiry;
    public InputField Name, Address, Phone, Cardnum, MM, DD, Price;

    void Start()
    {
        userName = GameObject.Find("InName");
        address = GameObject.Find("InStreet");
        phone = GameObject.Find("InPhone");
        cardNum = GameObject.Find("InCardNum");
        mm = GameObject.Find("InMM");
        dd = GameObject.Find("InDD");
        
        valName = GameObject.Find("ValidateName");
        valAddress = GameObject.Find("ValidateAddress");
        valPhone = GameObject.Find("ValidatePhone");
        valCard = GameObject.Find("ValidateCardNumber");
        valExpiry = GameObject.Find("ValidateExpiry");
        valName.SetActive(false);
        valAddress.SetActive(false);
        valPhone.SetActive(false);
        valCard.SetActive(false);
        valExpiry.SetActive(false);
     
        Name = userName.GetComponent<InputField>();
        Address = address.GetComponent<InputField>();
        Phone = phone.GetComponent<InputField>();
        Cardnum = cardNum.GetComponent<InputField>();
        MM = mm.GetComponent<InputField>();
        DD = dd.GetComponent<InputField>();
    }
    void Update()
    {
        displayTime -= Time.deltaTime;
        if (displayTime <= 0.0)
        {
            dispMsg = false;
        }
    }


    public void OnClick()
    {
        if (string.IsNullOrEmpty(Name.text))
        {
            flag = 1;
            valName.SetActive(true);
        }
        else
        {
            flag = 0;
            valName.SetActive(false);
        }
        if (string.IsNullOrEmpty(Address.text))
        {
            flag = 1;
            valAddress.SetActive(true);
        }
        else
        {
            flag = 0;
            valAddress.SetActive(false);
        }
        if (string.IsNullOrEmpty(Phone.text))
        {
            flag = 1;
            valPhone.SetActive(true);
        }
        else
        {
            flag = 0;
            valPhone.SetActive(false);
        }
        if (string.IsNullOrEmpty(Cardnum.text))
        {
            flag = 1;
            valCard.SetActive(true);
        }
        else
        {
            flag = 0;
            valCard.SetActive(false);
        }

        if (string.IsNullOrEmpty(DD.text) || string.IsNullOrEmpty(MM.text))
        {
            flag = 1;
            valExpiry.SetActive(true);
        }   
        else
        {
            flag = 0;
            valExpiry.SetActive(false);
        }
        if (flag == 0)
        {
            dispMsg = true;
            displayTime = 3.0f;
        }
        
    }
    
    void OnGUI()
    {
        GUI.color = Color.black;
        GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("label"));
        myStyle.fontSize = 25;

        if (dispMsg)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), message,myStyle);
        }
    }
}
