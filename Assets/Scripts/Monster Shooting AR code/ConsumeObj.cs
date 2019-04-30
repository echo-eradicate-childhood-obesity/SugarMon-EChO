using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeObj : MonoBehaviour {
    [SerializeField]
    protected int cost;
    public int Cost { get { return cost; } }


    protected void Consume(){
        ARMon.CustomController.SetCoin(-cost);
    }
}
