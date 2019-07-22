using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// * This Script Controls each Container Attached to CartDashCanvas
/// </summary>
public class FoodEntryContainer : MonoBehaviour {

    ProductInfo pi; // Object to be displayed
    public ProductInfo PI { get { return pi; } }

    public GameObject pName; // name of the food scanned
    public GameObject pIcon; // green check / red exclamation
    public GameObject pTimeDate; // time and date when scaned

    TextMeshProUGUI pText;
    TextMeshProUGUI pTimeDateDisplay;
    Image pImage;

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

    private void SetPi(Category cate) {
        pi.Type = cate;
    }

    public void PIUpdate(ProductInfo pi) {
        this.pi = pi;
    }

    public ProductInfo GetPI() {
        return pi;
    }
}
