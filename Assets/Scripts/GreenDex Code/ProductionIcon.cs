using System.Collections.Generic;
using UnityEngine;

public class ProductionIcon :AnimButtonAction {

    GameObject parentGo;
    Sprite curIcon;
	void Start () {
        parentGo = transform.parent.gameObject;
        Action(this.gameObject);
	}
	

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
