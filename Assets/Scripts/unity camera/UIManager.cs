﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

/*info is boxed and send when scanner found an item
* 
*/
public struct Info{
    
    private string familyName;

    public Info(string fname)
    {
        familyName = fname;
    }
    public string FamilyName { get { return familyName; }set { familyName = value; } }
}

public class UIManager : MonoBehaviour {
    public List<Sprite> Sprites;

    //singleton attached to main camera
    private static UIManager _instance;

    public static UIManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }
    public List<GameObject> CateBtn;

    private SimpleDemo simpleDemo;
    [SerializeField]
    List<GameObject> familyUIList;
    // Use this for initialization
    void Awake ()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else { Destroy(this); }
        //init four catebtn
    }

    private void Start()
    {
        InitCateBtn();
        simpleDemo = GameObject.Find("Main Camera").GetComponent<SimpleDemo>();
    }

    private void InitCateBtn()
    {
        var cav = GameObject.Find("GreenCartBack");
        var cavRect = cav.GetComponent<RectTransform>().rect;
        var catebtnWidth = cavRect.width / (CateBtn.Count);
        var pos = -(catebtnWidth * CateBtn.Count / 2);
        var colorA = new Color(0.292f, 0.340f, 0.310f, 1f);
        var colorB = new Color(1f, 1f, 1f, 1f);
        GreenCartController.Instance.CurrentCate = Category.all;
        CateBtn[0].GetComponentInChildren<TextMeshProUGUI>().color = colorB; // set "all" button to white
        foreach (GameObject go in CateBtn)
        {
            System.Action<string> act = (aa) =>
            {
                var newCate = Converter.StringEnumConverter<Category, string>(aa);
                //set cate when the target cate is not the same as current cate
                //reset to default(all/uncate) when current cate is same as target cate
                if (GreenCartController.Instance.CurrentCate != newCate) {
                    GreenCartController.Instance.CurrentCate = newCate;
                    GreenCartController.Instance.PC.CurDic = new List<ProductInfo>();
                    if (newCate == Category.all) {
                        GreenCartController.Instance.PC.CurDic = GreenCartController.Instance.PC.products;
                    }
                    else {
                        foreach (ProductInfo pi in GreenCartController.Instance.PC.products) {
                            if (pi.Type == GreenCartController.Instance.CurrentCate)
                                GreenCartController.Instance.PC.CurDic.Add(pi);
                            else
                                GreenCartController.Instance.PC.CurDic.Remove(pi);
                        }
                    }
                    // reset all buttons to black
                    GreenCartController.Instance.ResetContainer(GreenCartController.Instance.CurrentCate);
                    foreach (GameObject g in CateBtn) {
                        g.GetComponentInChildren<TextMeshProUGUI>().color = colorA;
                    }
                    // set selected buttom to white
                    go.GetComponentInChildren<TextMeshProUGUI>().color = colorB;
                }
            };
            string name = "";
            foreach (char c in go.name.ToLower()) {
                if (c != ' ')
                    name += c;
            }
            go.GetComponent<Button>().onClick.AddListener(() => act(name));
            var rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(catebtnWidth, rect.rect.height);
            rect.localPosition = new Vector3(pos, 0f);
            pos += catebtnWidth;
        }
    }

    //have info passed here, active the target gameobject in with in the info parent  
    
    public void IndicateController(Info info,string targetName)
    {
        foreach (GameObject go in familyUIList)
        {
            //"Monster" is the magic number here, change if later
            if ((go.name + " Monsters") == info.FamilyName)
            {
                // testing it out here    
                  
                //end              
                var targetGO = go.transform.Find(targetName).gameObject;
                if (!targetGO.activeInHierarchy)
                {
                    targetGO.SetActive(true);
                }
                else return;
            }
        }
    }
    public void IndicateController(Info info, string targetName, List<TMP_Dropdown.OptionData> list)
    {
        IndicateControllerHelper(info, targetName, list);
    }
    public Sprite dextrose;
    private void IndicateControllerHelper(Info info, string targetName, List<TMP_Dropdown.OptionData> list)
    {
        foreach (TMP_Dropdown.OptionData go in list)
        {
      
            if ((go.text.Substring(0, 2)) == info.FamilyName.Substring(0, 2))
            {
                if (go.text.Contains(" ("))
                {

                    int length = go.text.Length;
                    int x = 0;
                    var number = go.text.Substring(length - 7, 1);
                    var largernumber = go.text.Substring(length - 8, 2);
                    if(Int32.TryParse(largernumber, out x))
                    {
                        go.text = go.text.Substring(0, go.text.Length - 8) + (x + 1) + " new!)";
                    }
                    else if (Int32.TryParse(number, out x))
                    {
                        go.text = go.text.Substring(0, go.text.Length - 7)+(x+1) + " new!)";
                    }
                }
                else if(!go.text.Contains("("))
                {
                    if (go.text.Equals("Dextrin"))
                    {
                        go.image = Sprites[0];
                    }
                    if (go.text.Equals("OSE"))
                    {
                        go.image = Sprites[1];
                    }
                    if (go.text.Equals("Cane"))
                    {
                        go.image = Sprites[2];
                    }
                    if (go.text.Equals("Syrup"))
                    {
                        go.image = Sprites[3];
                    }
                    if (go.text.Equals("Concentrate"))
                    {
                        go.image = Sprites[4];
                    }
                    if (go.text.Equals("Obvious"))
                    {
                        go.image = Sprites[5];
                    }
                    if (go.text.Equals("Strange"))
                    {
                        go.image = Sprites[6];
                    }
                    go.text += " (1 new!)";
                }
             
            }
          
        }
    }

    //temp func
    public void DisAllUp(string targetName)
    {
        foreach (GameObject go in familyUIList)
        {
            var targetGO = go.transform.Find(targetName).gameObject;
            if (!targetGO.activeInHierarchy)
            {
                targetGO.SetActive(false);
            }
            else return;
        }
    }



    public void DisableUI(GameObject go)
    {
        simpleDemo.enabled = !simpleDemo.enabled;
        go.SetActive(!go.activeSelf);
    }
}
