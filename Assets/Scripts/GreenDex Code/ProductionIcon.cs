using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// * Attached To each product icon in the GreenDashCanvas
/// * Used to selected and change the Category of each product
/// </summary>
public class ProductionIcon :AnimButtonAction {

    GameObject parentGo;
    Sprite curIcon;
	void Start () {
        parentGo = transform.parent.gameObject;
        Action(this.gameObject);
	}
    /// <summary>
    /// * Inaddition to the based Class
    /// * When clicked, check if in edit mode, and if so, delete the given item
    /// </summary>
    public override void ClickEventTrigger()
    {
        if(GreenCartController.Instance.editMode == true) {
            ProductInfo pi = parentGo.GetComponent<GreenDexContainer>().PI;
            GreenCartController.Instance.PCRemove(pi);
        }
    }

    protected override List<GameObject> GetGOs()
    {
        List<GameObject> output = new List<GameObject>();
        foreach (string str in Gos)
        {   
            var go = transform.Find(str)!= null ? transform.Find(str).gameObject : GameObject.Find(str);
            output.Add(go);
        }
        return output;
    }
}
