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
    /// * When click, add prouction informationt this container point at to the GreenCartController.Instance.CurSelectedPI When it is not in the list
    /// * Otherwise remove it from the list and change the icons to the based on current 
    /// </summary>
    public override void ClickEventTrigger()
    {
        base.ClickEventTrigger();
        var pi = parentGo.GetComponent<GreenDexContainer>().PI;
        if (!GreenCartController.Instance.CurSelectedPI.Contains(pi))
        {
            GreenCartController.Instance.CurSelectedPI.Add(pi);
            pi.IsSelected = true;
        }
        else
        {
            GreenCartController.Instance.CurSelectedPI.Remove(pi);
            if (GreenCartController.Instance.CurSelectedPI.Count == 0)
            {
                DeselectAction();
            }
            pi.IsSelected = false;
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
