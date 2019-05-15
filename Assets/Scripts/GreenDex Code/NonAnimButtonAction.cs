using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// * This Class can used to manipulate the value, without using animation
/// </summary>
public class NonAnimButtonAction : MonoBehaviour,IButtonAction {
    //Gos are the name of GameObject in the hierarchy want to manipulate use this button
    public List<string> Gos;
    //Orders are the actions want to do
    public List<string> Orders;

    [SerializeField]
    GameObject targetGo;

    /// <summary>
    /// * Changing the Selected products to that same category as the Target category
    /// * go's name will be used to convert to the Category
    /// * Save user's change to the file
    /// * When Action is done, call DeselectAction();
    /// </summary>
    /// <param name="go">button which is being clikced</param>
    public void Action(GameObject go)
    {
        foreach (ProductInfo pi in GreenCartController.Instance.CurSelectedPI)
        {
            pi.Type=(Converter.StringEnumConverter<Category, string>(go.name));
        }
        GreenCartController.Instance.PC.PCSave();
        DeselectAction();
    }

    public void ClickEventTrigger()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// * When the Button is Deselcted/Clicked Clear the Selected products using GreenCartController.Instance.ClearCurSelectedPI()
    /// </summary>
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