using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
public class GreenCartController : MonoBehaviour
{

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
    public List<Sprite> CateImg { get { return cateImg; } }
    [SerializeField]
    float containerHeight;
    int position;
    int incre;

    [SerializeField]
    string key;

    [SerializeField]
    string gkey;
    //where the request file/properity is here
    public IRequester requester;
    public IRequester grequester;
    [SerializeField]
    TextAsset text;
    [SerializeField]
    char delimiter;
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


    private List<Category> currentCates = new List<Category>();
    public List<Category> CurrentCates { get { return currentCates; } set { currentCates = value; } }


    private List<ProductInfo> curSelectedPI = new List<ProductInfo>();
    public List<ProductInfo> CurSelectedPI { get { return curSelectedPI; } }


    private void Awake()
    {

        if (instance != null)
        {
            Destroy(this);
        }
        else instance = this;
        position = 0;
        down = false;
        totalDisRollingDis = 0;

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
        //creat the requester. no need for await, use this for the build. it is faster 
        //when in editor use this. but use Async method in build will be faster
#if UNITY_EDITOR
        StartCoroutine("InitRequester");
#else
        StartAsync(); 
#endif
    }

    private async Task StartAsync()
    {
        await Task.Run(() =>
        {
            List<string[]> strList = new List<string[]>();
            var textAssetArr = text.text.Split('\n');
            foreach (var line in textAssetArr)
            {
                var contentArr = line.Split(delimiter);
                strList.Add(contentArr);
            }
            requester = new USDARequester(strList, 1,key);
            grequester = new GoogleRequester(gkey);
        });
        //await SendRequest("123");
    }

    System.Collections.IEnumerator InitRequester()
    {

        List<string[]> strList = new List<string[]>();
        var textAssetArr = text.text.Split('\n');
        foreach (var line in textAssetArr)
        {
            var contentArr = line.Split(delimiter);
            strList.Add(contentArr);
        }
        requester = new USDARequester(strList, 1,key);
        grequester = new GoogleRequester(gkey);
        yield return null;
    }

    //move this to requester
    //public async Task<string> SendRequest(string upc)
    //{
    //    var ndbno = await requester.LookNDBAsync(upc);
    //    if (ndbno != -1)
    //    {
    //        string url = $@"https://api.nal.usda.gov/ndb/V2/reports?ndbno={ndbno}&format=json&api_key={key}&location=Denver+CO";
    //        #region foldthis
    //        //using (HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false }))
    //        //{
    //        //    try
    //        //    {
    //        //        string str = await client.GetStringAsync(url);
    //        //        //Console.WriteLine("reqeust got");
    //        //        JObject jjson = await DeserializerObjectAsync<JObject>(str);
    //        //        //JObject jjson = JsonConvert.DeserializeObject<JObject>(str);
    //        //        //structure of usda resopnd json
    //        //        /*
    //        //         * SelectToken will get the object we want. If it show as list then use First or other keyword accordingly
    //        //         * foods->list of food requested. as we only send one udbno here, so the first element in the list is what we want
    //        //         * food is the object we are looking for. SelectToken("food")
    //        //         * food obj will have {sr/type/desc/ing/nutrients/footnotes} and desc is the one we want
    //        //         * desc obj has {ndbno/name/ds/manu/ru} and name is the one we want
    //        //         * 
    //        //         */
    //        //        string newStr = jjson.SelectToken("foods").First.SelectToken("food").SelectToken("desc").SelectToken("name").ToString();
    //        //        return newStr;
    //        //    }
    //        //    catch (HttpRequestException e)
    //        //    {

    //        //        Console.WriteLine(e.Message);
    //        //        return "no ndb";
    //        //    }

    //        //} 
    //        #endregion
    //        return await Task.Run(async () =>
    //        {
    //            using (HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false }))
    //            {
    //                try
    //                {
    //                    string str = await client.GetStringAsync(url);
    //                    JObject jjson = await DeserializerObjectAsync<JObject>(str);
    //                    //structure of usda resopnd json
    //                    /*
    //                     * SelectToken will get the object we want. If it show as list then use First or other keyword accordingly
    //                     * foods->list of food requested. as we only send one udbno here, so the first element in the list is what we want
    //                     * food is the object we are looking for. SelectToken("food")
    //                     * food obj will have {sr/type/desc/ing/nutrients/footnotes} and desc is the one we want
    //                     * desc obj has {ndbno/name/ds/manu/ru} and name is the one we want
    //                     */
    //                    string newStr = jjson.SelectToken("foods").First.SelectToken("food").SelectToken("desc").SelectToken("name").ToString();
    //                    string output="";
    //                    var strs = newStr.Split(' ');
    //                    foreach (string s in strs)
    //                    {
    //                        var val = char.ToUpper(s[0])+s.Substring(1).ToLower();
    //                        output += string.Format("{0} ", val);
    //                    }
    //                    return output;
    //                }
    //                catch (HttpRequestException e)
    //                {
    //                    Console.WriteLine(e.Message);
    //                    return "no ndb";
    //                }

    //            }
    //        });
    //        //return await Client(url);
    //    }
    //    else
    //    {
    //        Console.WriteLine("upc incorrect");
    //        return "no ndb";
    //    }
    //}

    //private async Task<string> Client(string url)
    //{
    //    using (HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false }))
    //    {
    //        try
    //        {
    //            string str = await client.GetStringAsync(url);
    //            JObject jjson = await DeserializerObjectAsync<JObject>(str);
    //            //structure of usda resopnd json
    //            /*
    //             * SelectToken will get the object we want. If it show as list then use First or other keyword accordingly
    //             * foods->list of food requested. as we only send one udbno here, so the first element in the list is what we want
    //             * food is the object we are looking for. SelectToken("food")
    //             * food obj will have {sr/type/desc/ing/nutrients/footnotes} and desc is the one we want
    //             * desc obj has {ndbno/name/ds/manu/ru} and name is the one we want
    //             */
    //            string newStr = jjson.SelectToken("foods").First.SelectToken("food").SelectToken("desc").SelectToken("name").ToString();
    //            return newStr;
    //        }
    //        catch (HttpRequestException e)
    //        {

    //            Console.WriteLine(e.Message);
    //            return "no ndb";
    //        }

    //    }
    //}

    //private async Task<JObject> DeserializerObjectAsync<JObject>(string str)
    //{
    //    Task<JObject> t = Task.Run(() =>
    //    {
    //        JObject output;
    //        output = JsonConvert.DeserializeObject<JObject>(str);
    //        return output;
    //    });
    //    return await t;

    //}

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
            NewRolling(currentPos, lastPos);
        }
        //}
        lastPos = Input.mousePosition;

        #endregion
#endif
    }

    private void NewRolling(Vector3 currentPos, Vector3 lastPos)
    {
        var offSet = lastPos.y - currentPos.y;
        try
        {
            if ((pc.GetCount(currentCates) - Containers.Count) * containerHeight < -totalDisRollingDis && offSet < 0)
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

    public void PCAdd(string name,string pos)
    {
        pc.AddProduct(name,pos);
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
            if (cates.Count != 0)
            {
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
