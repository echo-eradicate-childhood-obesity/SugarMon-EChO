using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// * This Class Controls overall behavior of CartDetailCanvas
/// * This is a singleton
/// * Attached to GreenCartBack
/// </summary>
public class DetailPageController : MonoBehaviour {
    private static DetailPageController instance;
    public static DetailPageController Instance { get { return instance; } }
    ProductInfo pi;
    public ProductInfo PI { get { return pi; } set { pi = value; } }

    [SerializeField]
    public GameObject CategoryLabel;
    [SerializeField]
    public GameObject ProductIcon;
    [SerializeField]
    public GameObject LocationLabel;
    [SerializeField]
    public GameObject SugarsLabel;
    [SerializeField]
    public GameObject ProductSugars;
    [SerializeField]
    public GameObject ProductName;
    [SerializeField]
    public GameObject ProductLocation;
    [SerializeField]
    public GameObject ProductDate;

    public Color HeaderColor;
    public Color BodyColor;

    private Color GreenHeader = new Color(68,111,76,255);
    private Color RedHeader = new Color(111, 68, 76, 255);
    private Color GreenBody = new Color(2, 64, 14, 255);
    private Color RedBody = new Color(64, 2, 14, 255);

    // Use this for initialization
    void Start () {
        ProductIcon.GetComponent<Image>().sprite = pi.GetSprite();
        InitText();
        InitColors();
    }
    private void InitText() {
        ProductSugars.GetComponent<TextMeshProUGUI>().text = "";
        ProductName.GetComponent<TextMeshProUGUI>().text = $"{pi.GetDetailPageName()}";
        ProductLocation.GetComponent<TextMeshProUGUI>().text = $"{pi.GetDetailPageLocation()}";
        ProductDate.GetComponent<TextMeshProUGUI>().text = $"{pi.displayFullDateTime()}";
    }
    private void InitColors() {
        if (pi.Type == Category.containsaddedsugar) {
            UIManager.Instance.background.GetComponentInChildren<Image>().sprite = UIManager.Instance.Backgrounds[2];
            HeaderColor = RedHeader;
            BodyColor = RedBody;
        }
        else {
            UIManager.Instance.background.GetComponentInChildren<Image>().sprite = UIManager.Instance.Backgrounds[1];
            HeaderColor = GreenHeader;
            BodyColor = GreenBody;
        }
        CategoryLabel.GetComponent<TextMeshProUGUI>().color = HeaderColor;
        LocationLabel.GetComponent<TextMeshProUGUI>().color = HeaderColor;
        SugarsLabel.GetComponent<TextMeshProUGUI>().color = HeaderColor;

        ProductSugars.GetComponent<TextMeshProUGUI>().color = BodyColor;
        ProductName.GetComponent<TextMeshProUGUI>().color = BodyColor;
        ProductLocation.GetComponent<TextMeshProUGUI>().color = BodyColor;
        ProductDate.GetComponent<TextMeshProUGUI>().color = BodyColor;
    }
}
