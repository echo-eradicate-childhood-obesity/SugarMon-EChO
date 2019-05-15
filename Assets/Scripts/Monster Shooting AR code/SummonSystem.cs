using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using System.Linq;

public class SummonSystem : MonoBehaviour {

    public GameObject sugarDex, infoCard, summonSystem, familyContent, warningText, canvas;
    private string currentSugar, currentSugarFamily, currentSpot;
    public NumbersOfEachSugar sugarCardData;
    public GameObject[] summonSpots;
    
    private string counter;
    private int numberOfCard;


    //For Open Summon System Button
    private List<string> listOfSummonMonsters = new List<string>();

    public void OpenSummonSystem()
    {
        summonSystem.gameObject.SetActive(true);
        listOfSummonMonsters.Clear();
        var sugarDisk = sugarDex.GetComponent<SugarDisk>();
        var test = summonSpots.Select(item => item.GetComponent<Image>().sprite == null);  //Check new summon event
        if (test != null)
        {
            sugarDisk.UpdateDexData();
            sugarDisk.UpdateCounterOfEachSugar();
            GameObject.Find("Canvas").transform.Find("FamilyBackground").gameObject.SetActive(false);
        }
    }

    public void CloseSummonSystem()
    {
        summonSystem.gameObject.SetActive(false);
    }
    public void ReplaceSpot()
    {
        var spotImage = GameObject.Find(currentSpot).GetComponent<Image>().sprite;
        sugarDex.GetComponent<SugarDisk>().OpenSugarDisk();

        //Remove old sugar from list and add new one if the spot is already occupied 
        if (spotImage != null)
        {
            numberOfCard = GetNumberOfCard(spotImage.name);
            listOfSummonMonsters.Remove(spotImage.name);
            ChangeTheNumberOfCard(spotImage.name, "+");
        }
    }

    public void OpenSugarDexForChoosingMonster(string clickedSpot)
    {
        currentSpot = clickedSpot;
        ReplaceSpot();
        
    }

    public void PopupSugarInfoCardInSugarDex(string sugarName, string sugarFamily)
    {
        currentSugarFamily = sugarFamily;
        currentSugar = sugarName;
        infoCard.transform.Find("SugarName").GetComponent<Text>().text = char.ToUpper(sugarName[0]) + sugarName.Substring(1); ;
        infoCard.gameObject.SetActive(true);
        warningText.gameObject.SetActive(false);
    }

    public void CloseInfoCardInSugarDex()
    {
        infoCard.gameObject.SetActive(false);
    }

    public void AddSugarToSummonSystem()
    {
        numberOfCard = GetNumberOfCard(currentSugar);
        
        if(numberOfCard > 0)
        {
            ChangeTheNumberOfCard(currentSugar, "-");
            listOfSummonMonsters.Add(currentSugar);
            CloseInfoCardInSugarDex();
            sugarDex.GetComponent<SugarDisk>().CloseSugarDisk();
            GameObject.Find(currentSpot).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Monsters/" + currentSugarFamily + "/" + currentSugar);
        }
        else
        {
            warningText.gameObject.SetActive(true);
        }
    }

    public void SpawnMonsters()
    {
        if(listOfSummonMonsters.Count > 0)
        {
            ClearSpot();
            foreach (string s in listOfSummonMonsters)
            {
                sugarCardData.MinusOneCardInDatabase(s);
            }
            summonSystem.gameObject.SetActive(false);
            ARMon.GameManager.Instance.ChangeMode();
            foreach (string s in listOfSummonMonsters) ARMon.GameManager.Instance.Summon(s);
        }        
    }
    public void ClearSpot()
    {
        foreach (GameObject spot in summonSpots)
        {
            spot.GetComponent<Image>().sprite = null;
        }
    }
    public int GetNumberOfCard(string sugar)
    {
        counter = GameObject.Find(sugar).transform.Find("Counter").GetComponent<Text>().text;
        numberOfCard = Int32.Parse(Regex.Match(counter, @"\d+").Value);
        return numberOfCard;
    }
    public void ChangeTheNumberOfCard(string sugar, string op)
    {
        if (op == "+") numberOfCard++;
        else numberOfCard--;
        GameObject.Find(sugar).transform.Find("Counter").GetComponent<Text>().text = "X" + numberOfCard;
    }
}
