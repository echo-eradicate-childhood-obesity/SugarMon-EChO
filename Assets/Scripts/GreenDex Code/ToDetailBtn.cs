using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ToDetailBtn : AnimButtonAction {

    public GameObject DetailPage;
	// Use this for initialization
	void Start () {
        //detailpage is in inspector, but this GO is an prefabe, inefficient to sign in incpector
        DetailPage = GameObject.Find("CartDetailCanvas");
        this.Action(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void ClickEventTrigger()
    {
        base.ClickEventTrigger();
        ApplyDetail();
        ApplyIcon();
    }

    private void ApplyIcon()
    {
    }

    private void ApplyDetail()
    {
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
        DetailPage.GetComponentInChildren<Image>().sprite = pi.GetSprite();
        DetailPage.GetComponentInChildren<TextMeshProUGUI>().text = $"Product Name: {pi.Name}\nProduct Category: {category}\nScan Location: {pi.Location}";
    }
}
