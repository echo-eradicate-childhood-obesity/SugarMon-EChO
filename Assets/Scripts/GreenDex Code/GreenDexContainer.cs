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
    [SerializeField]
    GameObject pOutline;

    TextMeshProUGUI pText;
    TextMeshProUGUI pTimeDateDisplay;
    Image pOutlineImage;
    Image pImage;

    public ProductInfo PI { get { return pi; } }
	void Start () {
        pImage = pIcon.GetComponent<Image>();
        pText = pName.GetComponent<TextMeshProUGUI>();
        pTimeDateDisplay = pTimeDate.GetComponent<TextMeshProUGUI>();
        pOutlineImage = pOutline.GetComponent<Image>();
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
        if(cate == Category.all) {
            pOutlineImage.sprite = GreenCartController.Instance.Outlines[0];
        }
        else if(cate == Category.noaddedsugar) {
            pOutlineImage.sprite = GreenCartController.Instance.Outlines[1];
        }
        else if (cate == Category.containsaddedsugar) {
            pOutlineImage.sprite = GreenCartController.Instance.Outlines[2];
        }
        else {
            Debug.Log("False Category Error");
        }
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
