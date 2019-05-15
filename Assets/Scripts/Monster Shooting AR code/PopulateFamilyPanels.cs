using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PopulateFamilyPanels : MonoBehaviour {

    [System.Serializable]
    public class TitleColor
    {
        public string name;
        public string HexCode;
    }

    public TitleColor[] colors;

    public List<string> titleColor;
    public GameObject Cell, Panel, Title;
    // Use this for initialization

    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PopulateFamilies()
    {
        GameObject newCell, newPanel, newTitle;
        List<string> families = GameObject.Find("Canvas").GetComponent<FindAddedSugar>().fms;
        Dictionary<string, int> fd = GameObject.Find("Canvas").GetComponent<FindAddedSugar>().familyDictionary;
        int cell = 0;
        Color col;
        if (colors[0] == null)
        {
            titleColor = new List<string>() { "#DD7E6B", "#FCD7B0", "#D5A6BD", "#B6D7A8", "#D8C2EF", "#A2C4C9", "#9FC5E8" };
        }
        else
        {
            titleColor = new List<string>();
            for (int i = 0; i < colors.Count(); i++)
            {
                titleColor.Add(colors[i].HexCode);
            }
        }


        GameObject.Find("SugarDisk").GetComponent<SugarDisk>().monsterColor = titleColor;

        for (int i = 0; i < families.Count; i++)
        {
            newTitle = (GameObject)Instantiate(Title, transform);
            newTitle.name = families[i] + " Title";

            //Convert hex code to RGB color and assign to titles
            ColorUtility.TryParseHtmlString(titleColor[i], out col);
            newTitle.GetComponent<Image>().color = col;

            newTitle.GetComponentInChildren<Text>().text = families[i];
            newPanel = (GameObject)Instantiate(Panel, transform);
            newPanel.name = families[i];

            for (int j = 0; j < fd[families[i]]; j++)
            {

                newCell = (GameObject)Instantiate(Cell, GameObject.Find(families[i]).transform);
                newCell.name = (cell + 1).ToString();
                GameObject diskNumber = newCell.transform.GetChild(0).GetChild(0).gameObject;
                diskNumber.GetComponent<Text>().text = newCell.name;
                GameObject monsterName = newCell.transform.Find("Name").gameObject;
                monsterName.GetComponent<Text>().text = "Monster";

                cell++;
            }

        }
    }
}
