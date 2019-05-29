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
	void Update () {
		
	}

    public override void ClickEventTrigger()
    {
        base.ClickEventTrigger();
        ApplyDetail();
    }
    /// <summary>
    /// * Change the content of detail page
    /// </summary>
    private void ApplyDetail()
    {
        GreenCartController.Instance.rollable = false;
        var pi = transform.GetComponentInParent<GreenDexContainer>().GetPI();
        string category="";
        switch (pi.Type)
        {
            case Category.all:
                category = "All";
                break;
            case Category.noaddedsugar:
                category = "No Added Sugar";
                break;
            case Category.addedsugar:
                category = "Added Sugar";
                break;
            default:
                category = "Uncategorized";
                break;
        }
        GreenCartController.Instance.DetailPage.GetComponentInChildren<Image>().sprite = pi.GetSprite();
        GreenCartController.Instance.ProductName.GetComponentInChildren<TextMeshProUGUI>().text = $"{pi.GetName()}";
        GreenCartController.Instance.ProductDate.GetComponentInChildren<TextMeshProUGUI>().text = $"{pi.displayFullDateTime()}";
        GreenCartController.Instance.ProductLocation.GetComponentInChildren<TextMeshProUGUI>().text = $"{pi.GetLocation()}";
    }
}
