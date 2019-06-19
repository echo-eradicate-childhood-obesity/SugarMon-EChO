using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

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
    public List<Sprite> NewSprites;
    //singleton attached to main camera
    private static UIManager _instance;

    public static UIManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }
    [HideInInspector]
    public SimpleDemo simpleDemo;
    //private Color colorA = new Color(0.292f, 0.340f, 0.310f, 1f);
    //private Color colorB = new Color(1f, 1f, 1f, 1f);
    [SerializeField]
    List<GameObject> familyUIList;
    // Use this for initialization
    void Awake() {
        if (_instance == null) {
            _instance = this;
        }
        else { Destroy(this); }
        //init four catebtn

    }

    private void Start()
    {
        simpleDemo = GameObject.Find("Main Camera").GetComponent<SimpleDemo>();
    }
    public void IndicateController(Info info, string targetName)
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
    public void IndicateController(Info info, string targetName, List<TMP_Dropdown.OptionData> list) {
        IndicateControllerHelper(info, targetName, list);
    }
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
                        go.image = NewSprites[0];
                    }
                    if (go.text.Equals("OSE"))
                    {
                        go.image = NewSprites[1];
                    }
                    if (go.text.Equals("Cane"))
                    {
                        go.image = NewSprites[2];
                    }
                    if (go.text.Equals("Syrup"))
                    {
                        go.image = NewSprites[3];
                    }
                    if (go.text.Equals("Concentrate"))
                    {
                        go.image = NewSprites[4];
                    }
                    if (go.text.Equals("Obvious"))
                    {
                        go.image = NewSprites[5];
                    }
                    if (go.text.Equals("Strange"))
                    {
                        go.image = NewSprites[6];
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
    public void OpenFoodDex() 
    {
        GreenCartController.Instance.ResetCategory();
        GreenCartController.Instance.rollable = true;
        ToggleEnable(GreenCartController.Instance.gameObject);
    }
    public void CloseFoodDex() {
        ToggleEnable(GreenCartController.Instance.gameObject);
        GreenCartController.Instance.rollable = false;
    }
    public void ToggleEnable(GameObject go)
    {
        simpleDemo.enabled = !simpleDemo.enabled;
        go.SetActive(!go.activeSelf);
    }
}
