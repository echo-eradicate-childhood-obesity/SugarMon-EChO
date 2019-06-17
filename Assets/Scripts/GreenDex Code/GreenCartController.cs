using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
/// <summary>
/// * This Class Controls overall behavior of GreenDex
/// * This is a singleton
/// * Attached to GreenCartBack
/// </summary>
public class GreenCartController : MonoBehaviour
{
    public bool rollable { get; set; }
    private static GreenCartController instance;
    public static GreenCartController Instance { get { return instance; } }
    public GameObject DetailPage;
    public GameObject ProductName;
    public GameObject ProductDate;
    public GameObject ProductLocation;
    public Button All;
    public Button NoAddedSugar;
    public Button ContainsAddedSugar;
    public GameObject EditBtn; // Button that allows you to edit the FoodDex
    public GameObject LeftBtn; // Button that exits the FoodDex

    public List<Sprite> RightButtons;

    [SerializeField]
    ProductCollection pc = new ProductCollection();
    public ProductCollection PC { get { return pc; } }
    [SerializeField]
    GameObject dashHolder;
    [SerializeField]
    List<GameObject> Containers;
    public List<GameObject> CONTAINERS { get { return Containers; } }
    [SerializeField]
    List<Sprite> cateImg;//0:uncate,1:redButton,2:greenButton
    public List<Sprite> CateImg { get { return cateImg; } }
    [SerializeField]
    float containerHeight;
    int position;
    int incre;

    public bool editMode = false;
    bool down;
    //variable be used to fast scrolling. function not implemented yet, so variable not in use
    //float downTimer;
    //Vector3 downPos = new Vector3();
    //Vector3 upPos = new Vector3();
    //bool fastRool;
    Vector3 lastPos;

    Vector3 lastTouchPos;

    public GameObject testtextbox;
#if UNITY_EDITOR
    int[] ints = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
#endif
    float totalDisRollingDis;

    [SerializeField]
    public List<Sprite> EditButtonSprites;

    [SerializeField]
    public List<Sprite> Backgrounds; //0 blue, 1 green, 2 red
    public GameObject background;

    [SerializeField]
    public List<Sprite> Buttons;

    private Category currentCate = new Category();
    public Category CurrentCate { get { return currentCate; } set { currentCate = value; } }


    private List<ProductInfo> curSelectedPI = new List<ProductInfo>();
    public List<ProductInfo> CurSelectedPI { get { return curSelectedPI; } }

    //this val is used to adjust the rooling
    //use this val to avoid rooling overflow
    float microAdjustVal;
    private void Awake()
    {
        InitCategoryBtns();
        EditBtn.GetComponent<Button>().onClick.AddListener(() => OnEditClick());
        LeftBtn.GetComponent<Button>().onClick.AddListener(() => OnLeftBtnClick());

        if (instance != null) Destroy(this);
        else instance = this;
        position = 0;
        down = false;
        totalDisRollingDis = 0;
        rollable = true;
        //there is an chance incre value and containerHeight is alway the same
        //so there should be only one value.
        incre = 150;
        containerHeight = 150f;
        try
        {
            pc.products = pc.Load();
            ///*pc.products = */pc.BinaryLoader();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.Log(ex.StackTrace);
        }
        microAdjustVal = 0.5f;
    }


    public void Update()
    {
        //when roolable, rolling
        //disable ro
        if (rollable)
        {
            RollingAction();
        }

    }
    /// <summary>
    /// *Simulate the Scrolling in Mobile
    /// *currentTouch record users Touch position at current frame
    /// *lastTouchPos is the recorded last frame Touch postition
    /// *! Unity Editor do not support Touch, use mouse input in editor
    /// </summary>
    private void RollingAction()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            down = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            down = false;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPos = touch.position;
            }
            else
            {
                var currentTouch = touch.position;
                NewRolling(currentTouch, lastTouchPos);
                lastTouchPos = touch.position;
            }
        }

#if UNITY_EDITOR
        #region mouseaction
        //action to drag test
        var currentPos = Input.mousePosition;
        //if (!fastRool)
        //{
        if (down)
        {
            NewRolling(currentPos, lastPos);
        }
        //}
        lastPos = Input.mousePosition;

        #endregion
#endif
    }

    /// <summary>
    /// * Check offSet of last/current Touch/mouse click position
    /// * Adjust the GreenDex Container's position accordingly
    /// </summary>
    /// <param name="currentPos">current user's Touch/mouse position</param>
    /// <param name="lastPos">Touch/mouse position of last frame</param>
    private void NewRolling(Vector3 currentPos, Vector3 lastPos)
    {
        var offSet = lastPos.y - currentPos.y;
        try
        {
            //when the rolling distance is more than the totaly data user have
            //then set offSet value to 0 to prevent furthe rolling
            //pc.GetCount(currentCate) is the total number of products in current selected category
            //Containers.Count is the container number in editor(number is 10 when writing this) 
            //containerHeight is height of each container
            //Debug.Log($"#of:{pc.GetCount(currentCate)}");
            if ((pc.GetCount(currentCate) - Containers.Count-microAdjustVal) * containerHeight < -totalDisRollingDis && offSet < 0)
            {
#if UNITY_EDITOR
                Debug.Log("there is no more data");
#endif
                offSet = 0;
            }
            else if (totalDisRollingDis > /*containerHeight*/0f && offSet > 0)
            {
#if UNITY_EDITOR
                Debug.Log("this is the top of data");
#endif
                offSet = 0;
            }
        }
        catch { }
        var info = new NotifyInfo();
        info.Offset = offSet;
        info.RollingDis = totalDisRollingDis;
        //rolling is the new method which have refatored
        foreach (GameObject go in Containers)
        {
            var rectTrans = go.GetComponent<RectTransform>();
            //var offSet = lastPos.y - currentPos.y;
            var curPos = new Vector3(rectTrans.localPosition.x, rectTrans.localPosition.y, rectTrans.localPosition.z);
            curPos.y -= offSet;
            if (curPos.y > containerHeight)
            {
                curPos.y -= containerHeight * Containers.Count;
                //Debug.Log("Move to bottom");
                //var text = go.transform.Find("ProductName").GetComponent<Text>();
                try
                {
                    int i = pc.GetCount(currentCate) + (int)(info.RollingDis / containerHeight) - Containers.Count - 1;

                    //go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i));
                    go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i, currentCate));
                }
                catch (System.Exception ex)
                {
                    Debug.Log(totalDisRollingDis);
                    Debug.Log(ex.StackTrace);
                }
            }
            if (curPos.y < (-Containers.Count+microAdjustVal) * containerHeight)
            {
                curPos.y += Containers.Count * containerHeight;
                try
                {
                    int i = pc.GetCount(currentCate) - (int)(-info.RollingDis / containerHeight) /*- Containers.Count*/ - 1;
                    //go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i);
                    go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i, currentCate));
                }
                catch (System.Exception ex)
                {
                    Debug.Log(totalDisRollingDis);
                    Debug.Log(ex.StackTrace);
                }
            }
            rectTrans.localPosition = curPos;
        }
        //Rolling(offSet, info);
        totalDisRollingDis += offSet;
    }
    private void OnLeftBtnClick() {
        PC.Load();
        editMode = false;
    }
    public void OnEditClick() {
        editMode = !editMode;
        if (editMode)
            EditBtn.GetComponentInChildren<Image>().sprite = EditButtonSprites[1]; // highlighted
        else
            EditBtn.GetComponentInChildren<Image>().sprite = EditButtonSprites[0]; // unhighlighted
    }
    public void ResetCategory() {
        CurrentCate = Category.all; //set the initial category to "all"
        SetHighlights(Category.all);
        PC.CurDic = PC.products;
        ResetContainer(Category.all);
    }
    private void InitCategoryBtns() {
        ResetCategory();
        System.Action<Category> act = (newCate) => {
            //set cate when the target cate is not the same as current cate
            //if current category is same as target category do nothing
            if (CurrentCate != newCate) {
                CurrentCate = newCate;
                PC.CurDic = new List<ProductInfo>();
                if (newCate == Category.all) {
                    PC.CurDic = PC.products;
                }
                else {
                    foreach (ProductInfo pi in PC.products) {
                        if (pi.Type == CurrentCate)
                            PC.CurDic.Add(pi);
                    }
                }
                SetHighlights(newCate);
            }
        };
        All.GetComponent<Button>().onClick.AddListener(() => act(Category.all));
        NoAddedSugar.GetComponent<Button>().onClick.AddListener(() => act(Category.noaddedsugar));
        ContainsAddedSugar.GetComponent<Button>().onClick.AddListener(() => act(Category.containsaddedsugar));
    }
    //have info passed here, active the target gameobject in with in the info parent  

    /// <summary>
    /// Highlights the selected category and resets other categories
    /// </summary>
    /// <param name="name">Button selected to be highlighted</param>
    private void SetHighlights(Category cate) {
        CurrentCate = cate;
        ResetContainer(cate);
        if (cate == Category.all) {
            background.GetComponentInChildren<Image>().sprite = Backgrounds[0];
            All.GetComponentInChildren<Image>().sprite = Buttons[3]; // set "all" button to selected
            NoAddedSugar.GetComponentInChildren<Image>().sprite = Buttons[1];
            ContainsAddedSugar.GetComponentInChildren<Image>().sprite = Buttons[2];
        }
        else if (cate == Category.noaddedsugar) {
            background.GetComponentInChildren<Image>().sprite = Backgrounds[1];
            All.GetComponentInChildren<Image>().sprite = Buttons[0];
            NoAddedSugar.GetComponentInChildren<Image>().sprite = Buttons[4]; // set "No Sugar Added" to selected
            ContainsAddedSugar.GetComponentInChildren<Image>().sprite = Buttons[2];
        }
        else {
            background.GetComponentInChildren<Image>().sprite = Backgrounds[2];
            All.GetComponentInChildren<Image>().sprite = Buttons[0];
            NoAddedSugar.GetComponentInChildren<Image>().sprite = Buttons[1];
            ContainsAddedSugar.GetComponentInChildren<Image>().sprite = Buttons[5]; // set "Contains Sugar Added" to selected
        }
    }
    /// <summary>
    /// * Add Scanned product to PC(Product Collection)
    /// </summary>
    /// <param name="name">prodcut name</param>
    /// <param name="pos">location where the product is scanned</param>
    public void PCAdd(string name, string upc, string pos, string sugars)
    {
        pc.AddProduct(name, upc, pos, sugars);
        ResetContainer(currentCate); // reloads current view of items from local storage removing the item from view
    }
    /// <summary>
    /// * Remove Scanned product from PC(Product Collection)
    /// </summary>
    /// <param name="pi">product to remove</param>
    public void PCRemove(ProductInfo pi) {
        pc.RemoveProduct(pi); // removes item from local storage
        ResetContainer(currentCate); // reloads current view of items from local storage removing the item from view
        PC.PCSave(); // sets txt file storage to local sorage removing the item from txt file storage
    }
    /// <summary>
    /// * Reset the Container's Position every time user Open GreenDex
    /// * Make it easier to control the container's UI position
    /// </summary>
    private void OnEnable()
    {
        ResetContainer(currentCate);
    }
    /// <summary>
    /// update container content
    /// * reset container pos
    /// * update content
    /// * When products in cate are less than container than disable the extra container
    /// </summary>
    /// <param name="cate">current user selected Categorys</param>
    public void ResetContainer(Category cate)
    {
        totalDisRollingDis = 0;
        //var pos = -200;
        var pos = 0;
        CurrentCate = cate;
        pc.ResetCurDic(cate);
        for (int i = 0; i < Containers.Count; i++)
        {
            // enable all containers 
            if (!Containers[i].activeSelf)
            {
                Containers[i].SetActive(!Containers[i].activeSelf);
            }
            var offset = dashHolder.GetComponent<RectTransform>().rect.width / 2;
            Containers[i].GetComponent<RectTransform>().localPosition = new Vector3(offset, pos, 0);
            pos -= incre;
            if (i > pc.CurDic.Count - 1)
                Containers[i].SetActive(false);
            else
                Containers[i].GetComponent<GreenDexContainer>().PIUpdate(pc.CurDic[pc.CurDic.Count - i - 1]);
        }
    }
    /// <summary>
    /// * IsSelected is recorded in pi(Prodcut Infomation)
    /// * Set the value to false make sure it can be selected in later action
    /// * Reset curSelelctedPI to en empty List.
    /// </summary>
    public void ClearCurSelectedPI()
    {
        foreach (ProductInfo pi in curSelectedPI)
        {
            pi.IsSelected = false;
        };
        curSelectedPI = new List<ProductInfo>();
    }

}
