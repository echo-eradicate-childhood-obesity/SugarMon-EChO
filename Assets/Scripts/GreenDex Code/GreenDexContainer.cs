using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GreenDexContainer : MonoBehaviour {

    ProductInfo pi;
    [SerializeField]
    GameObject pName;
    [SerializeField]
    GameObject pIcon;
    [SerializeField]
    GameObject pLocation;
    TextMeshProUGUI pText;
    TextMeshProUGUI pLocationText;
    Image pImage;
    public ProductInfo PI { get { return pi; } }
	void Start () {
        //this two go is in GreenDexContainer's hierarchy tree. drag&drop in editor instead
        //pName = transform.Find("ProductName").gameObject;
        //pIcon = transform.Find("ProductIcon").gameObject;

        pImage = pIcon.GetComponent<Image>();
        pText = pName.GetComponent<TextMeshProUGUI>();
        pLocationText = pLocation.GetComponent<TextMeshProUGUI>();
	}
	
	
	void Update () {
        pText.text = pi.PrintInfo();
        pImage.sprite = pi.GetSprite();
        pLocationText.text = pi.GetLocation();
	}

    private void SetPi(Category cate)
    {
        //change the value in pi
        pi.Type=cate;
    }

    public void PIUpdate(ProductInfo pi)
    {
        this.pi = pi;
    }
}
