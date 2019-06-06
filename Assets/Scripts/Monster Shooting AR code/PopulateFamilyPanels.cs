using System.Collections;
using System.Globalization;
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
    public GameObject Cell, Panel, Title, NumberCircle;

    [SerializeField]
    private List<GameObject> familyBtn;
    // Use this for initialization

    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PopulateFamilies()
    {
        GameObject newCell, newPanel, newTitle, numberCircle;
        List<string> families = GameObject.Find("Canvas").GetComponent<FindAddedSugar>().fms;
        Dictionary<string, int> fd = GameObject.Find("Canvas").GetComponent<FindAddedSugar>().familyDictionary;
        List<string> sugarRepo = GameObject.Find("Canvas").GetComponent<FindAddedSugar>().repo;
        int cell = 0;
        Color col;
        if (colors[0] == null)
        {
            titleColor = new List<string>() { "#FCD7B0", "#D5A6BD", "#DD7E6B", "#D8C2EF", "#B6D7A8", "#A2C4C9", "#9FC5E8" };
        }
        else
        {
            titleColor = new List<string>();
            for (int i = 0; i < colors.Count(); i++)
            {
                titleColor.Add(colors[i].HexCode);
            }
        }


        for (int i = 0; i < families.Count; i++)
        {

            newTitle = (GameObject)Instantiate(Title, transform);
            newTitle.name = families[i] + " Title";

            //Convert hex code to RGB color and assign to titles
            ColorUtility.TryParseHtmlString(titleColor[i], out col);
            newTitle.GetComponent<Image>().color = col;
            familyBtn[i].GetComponent<Image>().color = col;

            newTitle.GetComponentInChildren<Text>().text = families[i];
            newPanel = (GameObject)Instantiate(Panel, transform);
            newPanel.name = families[i];

            for (int j = 0; j < fd[families[i]]; j++)
            {

                newCell = (GameObject)Instantiate(Cell, GameObject.Find(families[i]).transform);
                newCell.name = (cell + 1).ToString();
                Image monster = newCell.transform.GetChild(0).gameObject.GetComponent<Image>();
                string sugarName = sugarRepo[cell];
                monster.sprite = Resources.Load<Sprite>("Images/Monsters/" + sugarName);
                monster.color = Color.black;
                GameObject diskNumber = newCell.transform.GetChild(0).GetChild(0).gameObject;
                diskNumber.GetComponent<RectTransform>().anchoredPosition = new Vector2(74, 114);
                numberCircle = (GameObject)Instantiate(NumberCircle, newCell.transform.GetChild(0));
                numberCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(74, 136);
                numberCircle.GetComponent<RectTransform>().SetAsFirstSibling();

                diskNumber.GetComponent<Text>().text = newCell.name;
                if ( cell < 99)
                {
                    diskNumber.GetComponent<Text>().fontSize = 30;
                } else
                {
                    diskNumber.GetComponent<Text>().fontSize = 26;
                }
                
                diskNumber.GetComponent<Text>().color = Color.black;
                GameObject monsterName = newCell.transform.Find("Name").gameObject;
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                monsterName.GetComponent<Text>().text = textInfo.ToTitleCase(sugarRepo[cell]);

                cell++;
            }

        }
    }
}
