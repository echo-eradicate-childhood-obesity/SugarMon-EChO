using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public enum Category
{
    all,
    noaddedsugar,
    addedsugar
}

public struct NotifyInfo
{
    public float Offset { get; set; }
    public float RollingDis { get; set; }
    public Category Ct { get; set; }
}
[System.Serializable]
public class ProductInfo
{
    string name;
    public string Name { get => name; set => name = value; }
    string location;
    public string Location { get => location; set => name = value; }
    DateTime scanDateTime;
    public DateTime ScanDateTime { get => scanDateTime; set => scanDateTime = value; }

    //string upc;
    Category type;
    public Category Type { get => type; set => type = value; }
    public bool IsSelected { get; set; }
    public ProductInfo(string name,string location, DateTime dt, Category type) { 
        this.name = name;
        this.location = location;
        this.type = type;
        this.scanDateTime = dt;
        this.IsSelected = false;
    }

    internal string GetName()
    {
        //return $"{Name}  cate: {Type}";
        return $"{Name}";
    }
    internal string GetDisplayName() {
        string displayName = "";
        int i = 0;
        while(i < Name.Length && Name[i] != ',') {
            displayName += Name[i];
            i++;
        }
        return displayName;
    }
    internal string GetLocation()
    {
        return Location;
    }
    internal Category GetType() {
        return Type;
    }
    internal DateTime GetScanDateTime() 
    {
        return ScanDateTime;
    }
    internal string getScanDateTimeAsString() {
        return ScanDateTime.ToString("yyyyMMddHHmmss");
    }
    internal string displayDateTime() {
        TimeSpan since = DateTime.Now.Subtract(ScanDateTime);
        if (since.Days > 6)
            return displayFullDateTime();
        else
            return ScanDateTime.ToString("dddd, h:mm tt");
    }
    internal string displayFullDateTime() {
        return ScanDateTime.ToString("M/dd/yy - h:mm tt");
    }
    internal static DateTime getScanDateTimeFromString(string date) {
        return DateTime.ParseExact(date, "yyyyMMddHHmmss", /*CultureInfo.InvariantCulture*/null);
    }
    //dring-food-snack-default-sauce, selectedimg is CateImg[5]
    internal Sprite GetSprite()
    {
        if (IsSelected)
        {
            return GreenCartController.Instance.CateImg[3];
        }
        switch (type)
        {
            case Category.all:
                return GreenCartController.Instance.CateImg[5];
            case Category.addedsugar:
                return GreenCartController.Instance.CateImg[1];
            case Category.noaddedsugar:
                return GreenCartController.Instance.CateImg[2];
            default:
                return GreenCartController.Instance.CateImg[4];
        }
    }

}
public enum ActionType
{
    trigger,
    boolean,
    other
}