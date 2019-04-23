using System;
using System.Collections.Generic;
using UnityEngine;

public class GreenCartController : MonoBehaviour {

    private static GreenCartController instance;
    public static GreenCartController Instance { get { return instance; } }

    public GameObject dashPrefab;
    [SerializeField]
    ProductCollection pc = new ProductCollection();
    public ProductCollection PC { get { return pc; } }
    [SerializeField]
    GameObject dashHolder;
    [SerializeField]
    List<GameObject> Containers;
    public List<GameObject> CONTAINERS { get { return Containers; } }
    [SerializeField]
    List<Sprite> cateImg;//0:food,1:drink,2:snack,3:uncate,4:sauce,5:not cate but a check mark
    public List<Sprite> CateImg { get => cateImg; }
    [SerializeField]
    float containerHeight;
    int position;
    int incre;

    bool down;
    //variable be used to fast scrolling. function not implemented yet, so variable not in use
    //float downTimer;
    //Vector3 downPos = new Vector3();
    //Vector3 upPos = new Vector3();
    //bool fastRool;
    Vector3 lastPos;

    Vector3 lastTouchPos;
#if UNITY_EDITOR
    int[] ints = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }; 
#endif
    float totalDisRollingDis;


    private List<Category> currentCates = new List<Category>();
    public List<Category> CurrentCates { get { return currentCates; } set {currentCates=value; } }


    private List<ProductInfo> curSelectedPI = new List<ProductInfo>();
    public List<ProductInfo> CurSelectedPI{ get { return curSelectedPI; }}


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else instance = this;
        position = 0;
        incre = 200;
        down = false;
        totalDisRollingDis = 0;
        containerHeight = 200f;

        try
        {
            pc.products = pc.Load();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.Log(ex.StackTrace);
        }
        
    }

    public void Update()
    {
        //drag test
        RollingAction();
    }

    private void RollingAction()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            down = true;
            //downPos = Input.mousePosition;
            //downTimer += Time.deltaTime;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            down = false;
            //upPos = Input.mousePosition;
            //downTimer = 0;
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
                #region replace with newrolling same as click
                //if (touch.phase == TouchPhase.Moved)
                //{
                //    var touchoffSet = lastTouchPos.y - currentTouch.y;
                //    //var curPos = this.GetComponent<RectTransform>().localPosition.y;
                //    //curPos -= offSet;
                //    //this.GetComponent<RectTransform>().localPosition = new Vector3(0,curPos);
                //    try
                //    {
                //        if ((pc.GetCount(currentCate) - Containers.Count+1) * containerHeight < -totalDisRollingDis && touchoffSet < 0)
                //        {
                //            Debug.Log("there is no more data");
                //            touchoffSet = 0;
                //        }
                //        else if (totalDisRollingDis > 0 && touchoffSet > 0)
                //        {
                //            Debug.Log("this is the top of data");
                //            touchoffSet = 0;
                //        }
                //    }
                //    catch { }
                //    var info = new NotifyInfo();
                //    info.Offset = touchoffSet;
                //    info.RollingDis = totalDisRollingDis;

                //    //Rolling(touchoffSet,info);
                //    #region replace with rolling method: same as click
                //    foreach (GameObject go in Containers)
                //    {
                //        var rectTrans = go.GetComponent<RectTransform>();
                //        //var offSet = lastPos.y - currentPos.y;
                //        var curPos = new Vector3(rectTrans.localPosition.x, rectTrans.localPosition.y, rectTrans.localPosition.z);
                //        curPos.y -= touchoffSet;
                //        if (curPos.y > 0)
                //        {
                //            curPos.y -= containerHeight * Containers.Count;
                //            //Debug.Log("Move to bottom");
                //            //var text = go.transform.Find("ProductName").GetComponent<Text>();
                //            try
                //            {
                //                int i = pc.GetCount(currentCate) + (int)(info.RollingDis / containerHeight) - Containers.Count - 1;

                //                //go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i));
                //                go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i, currentCate));
                //            }
                //            catch (System.Exception ex)
                //            {
                //                Debug.Log(totalDisRollingDis);
                //                Debug.Log(ex.StackTrace);
                //            }
                //        }
                //        if (curPos.y < -Containers.Count * containerHeight)
                //        {
                //            curPos.y += Containers.Count * containerHeight;
                //            try
                //            {
                //                int i = pc.GetCount(currentCate) - (int)(-info.RollingDis / containerHeight) /*- Containers.Count*/ - 1;
                //                //go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i);
                //                go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i, currentCate));
                //            }
                //            catch (System.Exception ex)
                //            {
                //                Debug.Log(totalDisRollingDis);
                //                Debug.Log(ex.StackTrace);
                //            }
                //        }
                //        rectTrans.localPosition = curPos;
                //    }

                //    totalDisRollingDis += touchoffSet;
                #endregion
                //}
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
            NewRolling(currentPos,lastPos);
        }
        //}
        lastPos = Input.mousePosition;

        #endregion
#endif
    }

    private void NewRolling(Vector3 currentPos,Vector3 lastPos)
    {
        var offSet = lastPos.y - currentPos.y;
        try
        {
            if ((pc.GetCount(currentCates) - Containers.Count + 2) * containerHeight < -totalDisRollingDis && offSet < 0)
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
        #region will repalce this in container update
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
                    int i = pc.GetCount(currentCates) + (int)(info.RollingDis / containerHeight) - Containers.Count - 1;

                    //go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i));
                    go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i, currentCates));
                }
                catch (System.Exception ex)
                {
                    Debug.Log(totalDisRollingDis);
                    Debug.Log(ex.StackTrace);
                }
            }
            if (curPos.y < -Containers.Count * containerHeight)
            {
                curPos.y += Containers.Count * containerHeight;
                try
                {
                    int i = pc.GetCount(currentCates) - (int)(-info.RollingDis / containerHeight) /*- Containers.Count*/ - 1;
                    //go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i);
                    go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i, currentCates));
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
        #endregion

        totalDisRollingDis += offSet;
    }

    //private void Rolling(float offSet, NotifyInfo info)
    //{
    //    foreach (GameObject go in Containers)
    //    {
    //        var rectTrans = go.GetComponent<RectTransform>();
    //        //var offSet = lastPos.y - currentPos.y;
    //        var curPos = new Vector3(rectTrans.localPosition.x, rectTrans.localPosition.y, rectTrans.localPosition.z);
    //        curPos.y -= offSet;
    //        if (curPos.y > 0)
    //        {
    //            curPos.y -= containerHeight * Containers.Count;
    //            //Debug.Log("Move to bottom");
    //            //var text = go.transform.Find("ProductName").GetComponent<Text>();
    //            try
    //            {
    //                int i = pc.GetCount(currentCate) + (int)(info.RollingDis / containerHeight) - Containers.Count - 1;

    //                //go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i));
    //                go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i, currentCate));
    //            }
    //            catch (System.Exception ex)
    //            {
    //                Debug.Log(totalDisRollingDis);
    //                Debug.Log(ex.StackTrace);
    //            }
    //        }
    //        if (curPos.y < -Containers.Count * containerHeight)
    //        {
    //            curPos.y += Containers.Count * containerHeight;
    //            try
    //            {
    //                int i = pc.GetCount(currentCate) - (int)(-info.RollingDis / containerHeight) /*- Containers.Count*/ - 1;
    //                //go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i);
    //                go.GetComponent<GreenDexContainer>().PIUpdate(pc.GetProduct(i, currentCate));
    //            }
    //            catch (System.Exception ex)
    //            {
    //                Debug.Log(totalDisRollingDis);
    //                Debug.Log(ex.StackTrace);
    //            }
    //        }
    //        rectTrans.localPosition = curPos;
    //    }
    //}

    public void PCAdd(string s)
    {
        pc.AddProduct(s);
    }

    private void OnEnable()
    {
        ResetContainer(currentCates);
    }

    //update container content
    //1. reset container pos
    //2. update content
    public void ResetContainer(List<Category> cates)
    {
        totalDisRollingDis = 0;
        //var pos = -200;
        var pos = 0;
        for (int i = 0; i < Containers.Count; i++)
        {
            //as some container may have be disabled when there is not enough product in category
            //so enable all container first.
            if (!Containers[i].activeSelf)
            {
                Containers[i].SetActive(!Containers[i].activeSelf);
            }
            var offset = dashHolder.GetComponent<RectTransform>().rect.width / 2;
            Containers[i].GetComponent<RectTransform>().localPosition = new Vector3(offset, pos, 0);
            pos -= incre;
            #region foldthis
            //Debug.Log(pc.products.Count);
            //Containers[i].GetComponentInChildren<Text>().text = pc.products[pc.products.Count - 1 - i].GetName() ;
            //try
            //{
            //if (cate != Category.uncate)
            //{
            //    var piList = pc.greenDic[cate];
            //    var count = piList.Count;
            //    if (i > piList.Count - 1)
            //    {
            //        Containers[i].SetActive(false);
            //        //return;
            //    }
            //    else Containers[i].GetComponent<GreenDexContainer>().PIUpdate(piList[count - i - 1]);
            //} 
            #endregion
            #region foldthis
            //if (cates.Count != 0)
            //{
            //    #region foldthis
            //    //foreach (Category cate in cates)
            //    //{
            //    //    var piList = pc.greenDic[cate];
            //    //    var count = piList.Count;
            //    //    if (i > piList.Count - 1)
            //    //    {
            //    //        Containers[i].SetActive(false);
            //    //        //return;
            //    //    }
            //    //    else Containers[i].GetComponent<GreenDexContainer>().PIUpdate(piList[count - i - 1]);
            //    //} 
            //    #endregion
            //    foreach (Category cate in cates)
            //    {
            //        var piList = pc.CurDic;
            //        var count = piList.Count;
            //        if (i > piList.Count - 1)
            //        {
            //            Containers[i].SetActive(false);
            //            //return;
            //        }
            //        else Containers[i].GetComponent<GreenDexContainer>().PIUpdate(piList[count - i - 1]);
            //    }
            //}
            //else
            //{
            //    if (i > pc.products.Count - 1)
            //    {

            //        Containers[i].SetActive(false);
            //    }
            //    else Containers[i].GetComponent<GreenDexContainer>().PIUpdate(pc.products[pc.products.Count - 1 - i]);
            //} 
            #endregion
            if (cates.Count != 0)
            {
                #region foldthis
                //foreach (Category cate in cates)
                //{
                //    var piList = pc.greenDic[cate];
                //    var count = piList.Count;
                //    if (i > piList.Count - 1)
                //    {
                //        Containers[i].SetActive(false);
                //        //return;
                //    }
                //    else Containers[i].GetComponent<GreenDexContainer>().PIUpdate(piList[count - i - 1]);
                //} 
                #endregion
                //dupe
                foreach (Category cate in cates)
                {
                    if (i > pc.CurDic.Count - 1)
                    {
                        Containers[i].SetActive(false);
                    }
                    else Containers[i].GetComponent<GreenDexContainer>().PIUpdate(pc.CurDic[pc.CurDic.Count - i - 1]);
                }
            }
            else
            {
                if (i > pc.products.Count - 1)
                {

                    Containers[i].SetActive(false);
                }
                else Containers[i].GetComponent<GreenDexContainer>().PIUpdate(pc.products[pc.products.Count - 1 - i]);
            }

        }
    }

    public void ClearCurSelectedPI()
    {
        foreach (ProductInfo pi in curSelectedPI)
        {
            pi.IsSelected = false;
        };
        curSelectedPI = new List<ProductInfo>();
    }
}
