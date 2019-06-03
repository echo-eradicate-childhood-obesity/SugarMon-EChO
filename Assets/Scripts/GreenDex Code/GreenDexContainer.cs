using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// * This Script Controls each Container Attached to CartDashCanvas
/// </summary>
public class GreenDexContainer : MonoBehaviour {

    ProductInfo pi;
    [SerializeField]
    GameObject pName;
    [SerializeField]
    GameObject pIcon;
    [SerializeField]
    GameObject pTimeDate;

    TextMeshProUGUI pText;
    TextMeshProUGUI pTimeDateDisplay;
    Image pImage;

    public ProductInfo PI { get { return pi; } }
	void Start () {
        pImage = pIcon.GetComponent<Image>();
        pText = pName.GetComponent<TextMeshProUGUI>();
        pTimeDateDisplay = pTimeDate.GetComponent<TextMeshProUGUI>();
	}
	
	void Update () {
        pText.text = pi.GetDisplayName();
        pImage.sprite = pi.GetSprite();
        pTimeDateDisplay.text = pi.displayDateTime();
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

    public ProductInfo GetPI()
    {
        return pi;
    }
}
