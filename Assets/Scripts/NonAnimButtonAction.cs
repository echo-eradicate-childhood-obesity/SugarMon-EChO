using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonAnimButtonAction : MonoBehaviour,IButtonAction {
    public List<string> Gos;
    public List<string> Orders;

    public void Action(GameObject go)
    {
        foreach (ProductInfo pi in GreenCartController.Instance.CurSelectedPI)
        {
            pi.Type=(Converter.StringEnumConverter<Category, string>(go.GetComponent<Image>().sprite.name));
        }
        GreenCartController.Instance.PC.PCSave();
        DeselectAction();
    }

    public void ClickEventTrigger()
    {
        throw new System.NotImplementedException();
    }

    public void DeselectAction()
    {
        for (int i = 0; i < Orders.Count; i++)
        { 
            this.GetComponent<Animator>().SetBool(Orders[i], false);
            GreenCartController.Instance.ClearCurSelectedPI();
        }
    }

    void Start () {
		foreach(string str in Gos)
        {
            GameObject go = GameObject.Find(str);
            go.GetComponent<Button>().onClick.AddListener(()=>Action(go));
        }
	}	
}