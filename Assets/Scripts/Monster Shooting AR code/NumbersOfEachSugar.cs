using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;


[CreateAssetMenu(fileName = "NumbersOfEachSugar", menuName = "ScriptableObjects/NumbersOfEachSugar")]
public class NumbersOfEachSugar : ScriptableObject
{
    
    [System.Serializable]
    public class Sugars
    {
        public string _name;
        public int number = 1;
        public Sugars(string name)
        {
            _name = name;
        }       
    }
    public List<Sugars> sugars;

    public int GetNumberOfSugar(string name)
    {
        Sugars sugar;
        if (sugars.Any(p => p._name == name))
        {
            sugar = sugars.First(item => item._name == name);
            return sugar.number;
        }
        else return 0;
        
        
    }
    public void GetSugar(string name)
    {
        if(sugars.Any(p => p._name == name))
        {
            sugars.Find(item => item._name == name).number++;
        }
        else
        {
            sugars.Add(new Sugars(name));
        }
    }
    public void MinusOneCardInDatabase(string name)
    {
        var sugar = sugars.First(item => item._name == name);
        sugar.number--;
    }
}




