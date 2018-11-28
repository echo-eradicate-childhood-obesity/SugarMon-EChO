using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class USDAapi : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TriggerAPI()
    {
        StartCoroutine("APITest");
    }
    private IEnumerator APITest()
    {
        string url = "https://developer.nrel.gov/api/alt-fuel-stations/v1/nearest.json?api_key=MdQT6uuqMt4epBN9IAGOO1qSPH9GhMmIdJLwVlNP&location=Denver+CO";
        Dictionary<string, string> headers;
        headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json; charset=UTF-8");
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes("07047000310");
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                string result = www.text;
                Debug.Log(result);
            }
            else
            {
                Debug.Log("Error: " + www.error);
            }
        }
        StopCoroutine("APITest");
    }
}
