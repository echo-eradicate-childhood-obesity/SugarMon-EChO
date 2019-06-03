using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// * Attached to GreenDashCanvas RightIcon
/// </summary>
public class ToDetailBtn : AnimButtonAction {
    //reference to the DetailPage
    ////As the DetailPage is not an prefabe need drap/drop manually for each Container
    ////Potential fix: add reference to GreenCartContorller, then this script could get reference form there
    //public GameObject DetailPage;
	// Use this for initialization
	void Start () {
        //detailpage is in inspector, but this GO is an prefabe, inefficient to sign in incpector
       // DetailPage = GameObject.Find("CartDetailCanvas");
        this.Action(this.gameObject);
	}

    // Update is called once per frame
    void Update() {
        if (GreenCartController.Instance.editMode) {
            this.gameObject.GetComponent<Image>().sprite = GreenCartController.Instance.RightButtons[1];
            this.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(194, 100); // proportions of remove button
        }
        else {
            this.gameObject.GetComponent<Image>().sprite = GreenCartController.Instance.RightButtons[0];
            this.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100); // proportions of to detail button
        }
    }
    public override void ClickEventTrigger()
    {
        // when this is the to detail right button
        if (GreenCartController.Instance.editMode == false) {
            base.ClickEventTrigger();
            ApplyDetail();
        }
        // otherwise is the remove button
        else {
            ProductInfo pi = transform.GetComponentInParent<GreenDexContainer>().GetPI();
            GreenCartController.Instance.PCRemove(pi);
            if (GreenCartController.Instance.PC.products.Count == 0)
                GreenCartController.Instance.editMode = false;
        } 
    }
    /// <summary>
    /// * Change the content of detail page
    /// </summary>
    private void ApplyDetail()
    {
        GreenCartController.Instance.rollable = false;
        var pi = transform.GetComponentInParent<GreenDexContainer>().GetPI();
        GreenCartController.Instance.DetailPage.GetComponentInChildren<Image>().sprite = pi.GetSprite();
        GreenCartController.Instance.ProductName.GetComponentInChildren<TextMeshProUGUI>().text = $"{pi.GetDetailPageName()}";
        GreenCartController.Instance.ProductDate.GetComponentInChildren<TextMeshProUGUI>().text = $"{pi.displayFullDateTime()}";
        GreenCartController.Instance.ProductLocation.GetComponentInChildren<TextMeshProUGUI>().text = $"{pi.GetDetailPageLocation()}";
    }
}
