using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
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

    public GameObject CategoryLabel; // Contains Added Sugar / No Added Sugar images

    public GameObject LocationLabel; // "Location:" TextMeshProUGUI
    public GameObject SugarsLabel;   // "Added Sugars:" TextMeshProUGUI
    public GameObject UPCLabel;      // "UPC:" TextMeshProUGUI

    public GameObject ProductSugars; // sugars within the product
    public GameObject ProductName; // name of the product
    public GameObject ProductLocation; // address where it was scanned
    public GameObject ProductDate; // time and date when scanned
    public GameObject UPC; // universal product number (number of bar code)

    public List<Sprite> SugarInfoImage; // No sugar added/ sugar added icon on detail page

    public Color32 HeaderColor;
    public Color32 BodyColor = Color.white;

    [SerializeField]
    private GameObject canvas;
    private Color32 GreenHeader = new Color32(68,111,76,255);
    private Color32 RedHeader = new Color32(111, 38, 46, 255);

    public void Awake() {
        if (instance != null) Destroy(this);
        else instance = this;
    }

    /// <summary>
    /// Updates GameObjects to display the current product info
    /// </summary>
    public void UpdateDisplay() {
        if (pi != null) {
            InitText();
            InitColorsAndImages();
        }
        else {
            Debug.Log("No Product Given");
        }
    }

    /// <summary>
    /// Changes the text of every TextMeshProUGUI to display the current product info
    /// </summary>
    private void InitText() {
        if (pi.Type == Category.containsaddedsugar) {
            SugarsLabel.GetComponent<TextMeshProUGUI>().text = "Added Sugars:";
            ProductSugars.GetComponent<TextMeshProUGUI>().text = $"{pi.GetDisplaySugars()}";
        }
        else {
            ProductSugars.GetComponent<TextMeshProUGUI>().text = "";
            SugarsLabel.GetComponent<TextMeshProUGUI>().text = "";
        }
        ProductName.GetComponent<TextMeshProUGUI>().text = $"{pi.GetDetailPageName()}";
        ProductLocation.GetComponent<TextMeshProUGUI>().text = $"{pi.GetDetailPageLocation()}";
        ProductDate.GetComponent<TextMeshProUGUI>().text = $"{pi.displayFullDateTime()}";
        UPC.GetComponent<TextMeshProUGUI>().text = $"{pi.UPC}";
    }

    /// <summary>
    /// Changes the colors sceme of the display screen to match Green / Red for No Added Sugar / Contains Added Sugar products
    /// </summary>
    private void InitColorsAndImages() {
        if (pi.Type == Category.containsaddedsugar) {
            GreenCartController.Instance.background.GetComponentInChildren<Image>().sprite = GreenCartController.Instance.Backgrounds[2];
            CategoryLabel.GetComponent<Image>().sprite = SugarInfoImage[0];
            HeaderColor = RedHeader;
        }
        else {
            GreenCartController.Instance.background.GetComponentInChildren<Image>().sprite = GreenCartController.Instance.Backgrounds[1];
            CategoryLabel.GetComponent<Image>().sprite = SugarInfoImage[1];
            HeaderColor = GreenHeader;
        }
        ProductName.GetComponent<TextMeshProUGUI>().color = HeaderColor;

        SugarsLabel.GetComponent<TextMeshProUGUI>().color = HeaderColor;
        ProductSugars.GetComponent<TextMeshProUGUI>().color = BodyColor;

        UPCLabel.GetComponent<TextMeshProUGUI>().color = HeaderColor;
        UPC.GetComponent<TextMeshProUGUI>().color = BodyColor;

        LocationLabel.GetComponent<TextMeshProUGUI>().color = HeaderColor;
        ProductLocation.GetComponent<TextMeshProUGUI>().color = BodyColor;

        ProductDate.GetComponent<TextMeshProUGUI>().color = BodyColor;
    }

    /// <summary>
    /// Changed the product to be displayed
    /// </summary>
    /// <param name="pi">New product to be displayed</param>
    public void PIUpdate(ProductInfo pi) {
        this.pi = pi;
        UpdateDisplay();
    }
}
