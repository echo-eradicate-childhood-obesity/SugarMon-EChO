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
            case Category.drink:
                category = "Drink";
                break;
            case Category.sauce:
                category = "Sauce";
                break;
            case Category.food:
                category = "Food";
                break;
            case Category.snack:
                category = "Snack";
                break;
            default:
                category = "Uncategorized";
                break;
        }
        GreenCartController.Instance.DetailPage.GetComponentInChildren<Image>().sprite = pi.GetSprite();
        GreenCartController.Instance.DetailPage.GetComponentInChildren<TextMeshProUGUI>().text = $"Product Name: {pi.Name}\nProduct Category: {category}\nScan Location: {pi.Location}";
    }
}
