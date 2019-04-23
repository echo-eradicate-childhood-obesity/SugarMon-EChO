using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
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
    public List<GameObject> CateBtn;

    public GameObject greenCartBack;

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
        //InitCateBtn();
    }

    private void Start()
    {
       InitCateBtn();
    }

    private void InitCateBtn()
    {
        //var cav = GameObject.Find("GreenCartBack");
        var cavRect = greenCartBack.GetComponent<RectTransform>().rect;
        var catebtnWidth = cavRect.width / (CateBtn.Count);
        var pos = -(catebtnWidth * CateBtn.Count / 2);
        foreach (GameObject go in CateBtn)
        {
            var colorA = new Color(0.292f,0.340f,0.310f,1f);
            var colorB = new Color(1f,1f,1f,1f);
            System.Action<string> act = (aa) =>
            {
                var cate = Converter.StringEnumConverter<Category, string>(aa);
                //set cate when the target cate is not the same as current cate
                //reset to default(all/uncate) when current cate is same as target cate
                if (!GreenCartController.Instance.CurrentCates.Contains(cate))
                {
                    GreenCartController.Instance.CurrentCates.Add(cate);
                    GreenCartController.Instance.PC.CurDic = new List<ProductInfo>();
                    foreach (ProductInfo pi in GreenCartController.Instance.PC.products) {
                        foreach(Category ct in GreenCartController.Instance.CurrentCates)
                        {
                            if (pi.Type!=Category.uncate&&pi.Type == ct)
                            {
                                GreenCartController.Instance.PC.CurDic.Add(pi);
                            }
                        }
                    }
                }
                else if (GreenCartController.Instance.CurrentCates.Contains(cate))
                {
                    GreenCartController.Instance.CurrentCates.Remove(cate);
                    foreach(ProductInfo pi in GreenCartController.Instance.PC.products.Where(item=>item.Type==cate))
                    {
                        GreenCartController.Instance.PC.CurDic.Remove(pi);
                    }
                }
                GreenCartController.Instance.ResetContainer(GreenCartController.Instance.CurrentCates);
                var tmpro = go.GetComponentInChildren<TextMeshProUGUI>();
                tmpro.color = tmpro.color == colorB ? colorA : colorB;
            };
            go.GetComponent<Button>().onClick.AddListener(() => act(go.name.ToLower()));
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
        go.SetActive(!go.activeSelf);
    }
}
