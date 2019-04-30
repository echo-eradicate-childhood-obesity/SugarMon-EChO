using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimButtonAction : MonoBehaviour,IButtonAction {

    public List<string> Gos;
    public List<string> Orders;
    public ActionType type;

    private void Start()
    {
        this.Action(this.gameObject);
    }

    public virtual void ClickEventTrigger()
    {
        List<GameObject> gos = new List<GameObject>();
        foreach (string str in Gos)
        {
            var go = transform.Find(str)!=null?transform.Find(str).gameObject:GameObject.Find(str);
            gos.Add(go); 
        }
        for (int i = 0; i < gos.Count; i++)
        {
            try
            {
                switch (type)
                {
                    case ActionType.boolean:
                        var result = gos[i].GetComponent<Animator>().GetBool(Orders[i]);
                        gos[i].GetComponent<Animator>().SetBool(Orders[i], !result);
                        break;
                    case ActionType.trigger:
                        gos[i].GetComponent<Animator>().SetTrigger(Orders[i]);
                        break;
                }
            }
            catch (System.Exception ex) { Debug.Log(ex.StackTrace); }
        }
    }

    public void DeselectAction()
    {
        List<GameObject> gos = new List<GameObject>();
        gos = GetGOs();
        for (int i = 0; i < gos.Count; i++)
        {
            try
            {
                gos[i].GetComponent<Animator>().SetBool("Open", false);
            }
            catch (System.Exception ex) { Debug.Log(ex.StackTrace); }
        }
    }

    protected virtual List<GameObject> GetGOs()
    {
        List<GameObject> output = new List<GameObject>();
        foreach (string str in Gos)
        {
            var go = transform.Find(str).gameObject;
            output.Add(go);
        }
        return output;
    }

    public virtual void Action(GameObject go)
    {
        this.GetComponent<Button>().onClick.AddListener(ClickEventTrigger);
    }
}