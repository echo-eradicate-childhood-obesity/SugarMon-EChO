using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Category
{
    uncate,
    food,
    drink,
    snack,
    sauce
}

public struct NotifyInfo
{
    public float Offset { get; set; }
    public float RollingDis { get; set; }
    public Category Ct { get; set; }
}
public class ProductInfo
{
    string name;
    public string Name { get => name; set => name = value; }
    //string upc;
    Category type;
    public Category Type { get => type; set => type = value; }
    public bool IsSelected { get; set; }
    public ProductInfo(string name, Category type = Category.uncate)
    {
        this.name = name;
        this.type = type;
        this.IsSelected = false;
    }

    internal string PrintInfo()
    {
        return $"{Name}  cate: {Type}";
    }

    //dring-food-snack-default-sauce, selectedimg is CateImg[5]
    internal Sprite GetSprite()
    {
        if (IsSelected)
        {
            return GreenCartController.Instance.CateImg[5];
        }
        switch (type)
        {
            case Category.drink:
                return GreenCartController.Instance.CateImg[0];
            case Category.food:
                return GreenCartController.Instance.CateImg[1];
            case Category.snack:
                return GreenCartController.Instance.CateImg[2];
            case Category.sauce:
                return GreenCartController.Instance.CateImg[4];
            default:
                return GreenCartController.Instance.CateImg[3];
        }
    }

}
public enum ActionType
{
    trigger,
    boolean,
    other
}