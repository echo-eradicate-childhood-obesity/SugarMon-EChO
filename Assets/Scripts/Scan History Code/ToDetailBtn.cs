using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// * Attached to FoodEntryContainer RightIcon
/// </summary>
public class ToDetailBtn : AnimButtonAction {

    //Vector3 right;
    //Vector3 remove;
    void Start () {
        this.Action(this.gameObject);
        //right = this.gameObject.GetComponent<Image>().transform.position;
        //remove = this.gameObject.GetComponent<Image>().transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (ScanHistoryController.Instance.editMode) {
            this.gameObject.GetComponent<Image>().sprite = ScanHistoryController.Instance.RightButtons[1];
            //sthis.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(245, 129); // proportions of remove button
        }
        else {
            this.gameObject.GetComponent<Image>().sprite = ScanHistoryController.Instance.RightButtons[0];
            //this.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(75, 75); // proportions of to detail button
        }
    }
    public override void ClickEventTrigger()
    {
        // when this is the to detail right button
        if (ScanHistoryController.Instance.editMode == false) {
            base.ClickEventTrigger();
            ApplyDetail();
        }
        // otherwise is the remove button
        else {
            ProductInfo pi = transform.GetComponentInParent<FoodEntryContainer>().GetPI();
            Debug.Log(pi.Name);
            ScanHistoryController.Instance.PCRemove(pi);
            if (ScanHistoryController.Instance.PC.products.Count == 0)
                ScanHistoryController.Instance.editMode = false;
        } 
    }
    /// <summary>
    /// * Change the content of detail page
    /// </summary>
    private void ApplyDetail() {
        ScanHistoryController.Instance.rollable = false;
        ProductInfo pi = transform.GetComponentInParent<FoodEntryContainer>().GetPI();
        DetailPageController.Instance.PIUpdate(pi);
    }
}
