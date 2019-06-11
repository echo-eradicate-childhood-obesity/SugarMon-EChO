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
    //Vector3 right;
    //Vector3 remove;
    void Start () {
        //detailpage is in inspector, but this GO is an prefabe, inefficient to sign in incpector
       // DetailPage = GameObject.Find("CartDetailCanvas");

        this.Action(this.gameObject);
        //right = this.gameObject.GetComponent<Image>().transform.position;
        //remove = this.gameObject.GetComponent<Image>().transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (GreenCartController.Instance.editMode) {
            this.gameObject.GetComponent<Image>().sprite = GreenCartController.Instance.RightButtons[1];
            this.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(245, 127); // proportions of remove button
            //this.gameObject.GetComponent<Image>().transform.position = remove;
        }
        else {
            this.gameObject.GetComponent<Image>().sprite = GreenCartController.Instance.RightButtons[0];
            this.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100); // proportions of to detail button
            //this.gameObject.GetComponent<Image>().transform.position = right;
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
