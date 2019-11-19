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

    //Vector3 right;
    //Vector3 remove;
    void Start () {
        this.Action(this.gameObject);
        //right = this.gameObject.GetComponent<Image>().transform.position;
        //remove = this.gameObject.GetComponent<Image>().transform.position;
    }

    void Update()
    {
        // delete buttons
        if (GreenCartController.Instance.editMode)
        {
            this.gameObject.GetComponent<Image>().sprite = GreenCartController.Instance.RightButtons[1];
            this.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100); // proportions of remove button
        }
        else
        {
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
