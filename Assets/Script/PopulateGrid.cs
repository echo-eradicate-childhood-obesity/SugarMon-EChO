using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class SugarMonster {
    public string monsterName;
    public Sprite monsterImage;
    public GameObject monsterEntry;
    public bool isFound;
}

public class PopulateGrid : MonoBehaviour {


    //Ways to populate monster sprites to Grid
    //##############################################
    //private List<Sprite> Monsters = new List<Sprite>();
    //##############################################
    private bool Found = false;

    public GameObject Cell;
    //public Sprite Monster;
    public int numberToGenerate;

    //a dictionary to keep track of the monsters player has found
    List<SugarMonster> allMonsters = new List<SugarMonster>();
    Dictionary<string, SugarMonster> userMonsterDictionary = new Dictionary<string, SugarMonster>();


	// Use this for initialization
	void Start () {
        //Populate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Populate() {
        GameObject newCell;
        for (int i = 0; i < numberToGenerate; i++ ) {
            //if (!Found) {
            newCell = (GameObject)Instantiate(Cell, transform);
            newCell.name = (i + 1).ToString();
            GameObject diskNumber = newCell.transform.GetChild(0).GetChild(0).gameObject;
            diskNumber.GetComponent<Text>().text = newCell.name;
            GameObject monsterName = newCell.transform.GetChild(1).gameObject;
            monsterName.GetComponent<Text>().text = "Monster";
            //newCell.GetComponentsInChildren<Text>().text = (i + 1).ToString();
            //}

            //else {
            //    newCell = (GameObject)Instantiate(Cell, transform);
            //    newCell.GetComponent<Image>().sprite = Monster;
            //}

        }

        //for (int i = 0; i < numberToGenerate; i++) {
        //    userMonsterDictionary.Add()
        //}

    }
}
