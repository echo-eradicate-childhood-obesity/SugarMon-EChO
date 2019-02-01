using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //singleton attached to main camera
    private static UIManager _instance;

    public static UIManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    [SerializeField]
    List<GameObject> familyUIList;
    // Use this for initialization
    void Awake () {
        if (_instance == null)
        {
            _instance = this;
        }
        else { Destroy(this); }
	}
	
    //have info passed here, active the target gameobject in with in the info parent  
    public void IndicateController(Info info,string targetName)
    {
        foreach (GameObject go in familyUIList)
        {
            //"Monster" is the magic number here, change if later
            if ((go.name + " Monsters") == info.FamilyName)
            {
                var targetGO = go.transform.Find(targetName).gameObject;
                if (!targetGO.activeInHierarchy)
                {
                    targetGO.SetActive(true);
                }
                else return;
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
        if (go.activeSelf)
        {
            go.SetActive(false);
        }
    }

    //public void TestFoo()
    //{
    //    var i = new Info("Dextrin");
    //    IndicateController(i);
    //}
}
