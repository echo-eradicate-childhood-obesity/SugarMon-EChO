using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class TreeManager : MonoBehaviour {

    private static TreeManager _instance;
    public static TreeManager Instance;

    GameObject buck;
    TextMeshPro text;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    private void Start()
    {
        buck = GameObject.Find("EChOBUcKS");
        text = buck.GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        text.text = "$"+ARMon.CustomController.GetScore().ToString();
    }

    public void ReturnTest()
    {
        SceneManager.LoadScene("ARScene");
    }
}
