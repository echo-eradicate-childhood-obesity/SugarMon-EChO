using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// * Attached to CartDetailCanvas LeftButton
/// </summary>
public class ReturnFromDetailBtn : AnimButtonAction {
    void Start() {
        this.Action(this.gameObject);
    }
    public override void ClickEventTrigger() {
        base.ClickEventTrigger();
        GreenCartController.Instance.ResetCategory();
    }
}