using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class USDARequester : IRequester
{
    public USDARequester(List<string[]> list,int targetpos,string key)
    {
        this.List = list;
        this.TargetPos = targetpos;
        this.Key = key;
    }
    public List<string[]> List { get; set; }
    public int TargetPos { get; set; }
    public string Key { get; set; }


    private async Task<int> LookNDBAsync(string upc)
    {
        Task<int> t = Task.Run(() => {
            long val = long.Parse(upc);
            int result;
            Debug.Log($"Count: {List.Count} TargetPos: {TargetPos}");
            Debug.Log(List[0][0]);
            result = SearchController.BinarySearch(List, val, List.Count - 1, 0, TargetPos);
            return result;
        });
        return await t;
    }

    public async Task<string> SendRequest(string upc)
    {
        var ndbno = await LookNDBAsync(upc);
        if (ndbno != -1)
        {
            string url = $@"https://api.nal.usda.gov/ndb/V2/reports?ndbno={ndbno}&format=json&api_key={Key}&location=Denver+CO";
            #region foldthis
            //using (HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false }))
            //{
            //    try
            //    {
            //        string str = await client.GetStringAsync(url);
            //        //Console.WriteLine("reqeust got");
            //        JObject jjson = await DeserializerObjectAsync<JObject>(str);
            //        //JObject jjson = JsonConvert.DeserializeObject<JObject>(str);
            //        //structure of usda resopnd json
            //        /*
            //         * SelectToken will get the object we want. If it show as list then use First or other keyword accordingly
            //         * foods->list of food requested. as we only send one udbno here, so the first element in the list is what we want
            //         * food is the object we are looking for. SelectToken("food")
            //         * food obj will have {sr/type/desc/ing/nutrients/footnotes} and desc is the one we want
            //         * desc obj has {ndbno/name/ds/manu/ru} and name is the one we want
            //         * 
            //         */
            //        string newStr = jjson.SelectToken("foods").First.SelectToken("food").SelectToken("desc").SelectToken("name").ToString();
            //        return newStr;
            //    }
            //    catch (HttpRequestException e)
            //    {

            //        Console.WriteLine(e.Message);
            //        return "no ndb";
            //    }

            //} 
            #endregion
            return await Task.Run(async () =>
            {
                using (HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false }))
                {
                    try
                    {
                        string str = await client.GetStringAsync(url);
                        JObject jjson = await DeserializerObjectAsync<JObject>(str);
                        //structure of usda resopnd json
                        /*
                         * SelectToken will get the object we want. If it show as list then use First or other keyword accordingly
                         * foods->list of food requested. as we only send one udbno here, so the first element in the list is what we want
                         * food is the object we are looking for. SelectToken("food")
                         * food obj will have {sr/type/desc/ing/nutrients/footnotes} and desc is the one we want
                         * desc obj has {ndbno/name/ds/manu/ru} and name is the one we want
                         */
                        string newStr = jjson.SelectToken("foods").First.SelectToken("food").SelectToken("desc").SelectToken("name").ToString();
                        string output = "";
                        var strs = newStr.Split(' ');
                        foreach (string s in strs)
                        {
                            var val = char.ToUpper(s[0]) + s.Substring(1).ToLower();
                            output += string.Format("{0} ", val);
                        }
                        return output;
                    }
                    catch (HttpRequestException e)
                    {
                        Debug.Log(e.Message);
                        return upc;
                    }

                }
            });
            //return await Client(url);
        }
        else
        {
            Debug.Log($"UPC: {upc} does not match anything in USDA database");
            return upc;
        }
    }
    private async Task<JObject> DeserializerObjectAsync<JObject>(string str)
    {
        Task<JObject> t = Task.Run(() =>
        {
            JObject output;
            output = JsonConvert.DeserializeObject<JObject>(str);
            return output;
        });
        return await t;

    }
}
