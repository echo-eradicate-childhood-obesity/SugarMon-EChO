using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// * Attached to GreenDashCanvas RightIcon
/// * Allows the user to open up the product info for an item in their scan history
/// * and delete entries in their scan history
/// </summary>
public class ToDetailBtn : AnimButtonAction {
    
    void Start () {
        this.Action(this.gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (GreenCartController.Instance.editMode) {
            this.gameObject.GetComponent<Image>().sprite = GreenCartController.Instance.RightButtons[1];
            this.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(245, 129); // proportions of remove button
        }

        else {
            this.gameObject.GetComponent<Image>().sprite = GreenCartController.Instance.RightButtons[0];
            this.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100); // proportions of to detail button
        }
    }

    /// <summary>
    /// * IF the Edit button has NOT been pressed (the button with a pencil) 
    /// * AND the user selects a product, display the product's info
    /// * ELSE the right-arrow buttons will be replaced with "remove" buttons
    /// * now any selected product will be deleted
    /// </summary>
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
            Debug.Log(pi.Name);
            GreenCartController.Instance.PCRemove(pi);
            if (GreenCartController.Instance.PC.products.Count == 0)
                GreenCartController.Instance.editMode = false;
        } 
    }
    /// <summary>
    /// * Change the content of detail page
    /// </summary>
    private void ApplyDetail() {
        GreenCartController.Instance.rollable = false;
        ProductInfo pi = transform.GetComponentInParent<GreenDexContainer>().GetPI();
        DetailPageController.Instance.PIUpdate(pi);
    }
}
