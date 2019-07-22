using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
public enum Category
{
    all,
    noaddedsugar,
    containsaddedsugar
}

public struct NotifyInfo
{
    public float Offset { get; set; }
    public float RollingDis { get; set; }
    public Category Ct { get; set; }
}
[System.Serializable]
public class ProductInfo {
    string name;
    public string Name { get => name; set => name = value; }
    string upc;
    public string UPC { get => upc; set => upc = value; } // universal product number (bar code number)
    string location;
    public string Location { get => location; set => name = value; }
    DateTime scanDateTime;
    public DateTime ScanDateTime { get => scanDateTime; set => scanDateTime = value; }
    string sugars;
    public string Sugars { get => sugars; set => sugars = value; }
    //string upc;
    Category type;
    public Category Type { get => type; set => type = value; }
    public bool IsSelected { get; set; }
    public ProductInfo(string name, string upc, string location, DateTime dt, string sugars) { 
        this.name = name;
        this.UPC = upc;
        this.location = location;
        this.type = Converter.StringEnumConverter<Category, string>("containsaddedsugar");
        if (sugars == "")
            this.type = Converter.StringEnumConverter<Category, string>("noaddedsugar");
        this.scanDateTime = dt;
        this.sugars = sugars;
        this.IsSelected = false;
    }
    internal string GetDisplaySugars() {
        List<string> sugarList = sugars.Split(',').ToList();
        for(int i = 0; i < sugarList.Count; i++)
            if (sugarList[i][0] == ' ')
                sugarList[i] = sugarList[i].Substring(1);
        if (sugarList.Count > 10)
            return String.Join(", ", sugarList.ToArray());
        else
            return String.Join("\n", sugarList.ToArray());
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
    internal string GetDetailPageName() {
        string displayName = "";
        int i = 0;
        bool firstComma = true;
        while (i < Name.Length) {
            displayName += Name[i];
            if (Name[i] == ',' && firstComma == true) {
                firstComma = false;
                displayName += '\n';
                i++; // skip comma
            }
            i++;
        }
        return displayName;
    }
    /// <summary>
    /// Returns the formatted location for the detail page
    /// </summary>
    /// <returns>Location of the product</returns>
    internal string GetDetailPageLocation() {
        string displayLocation = "";
        int i = 0;
        int commaCount = 0;
        while (i < Location.Length) {
            displayLocation += Location[i];
            if (Location[i] == ',' && commaCount != 1) {
                commaCount++;
                displayLocation += '\n';
                i++;
            }
            i++;
        }
        return displayLocation;
    }
    /// <summary>
    /// Used for storage of date and time in database
    /// </summary>
    /// <returns>Formatted date and time</returns>
    internal string getScanDateTimeAsString() {
        return ScanDateTime.ToString("yyyyMMddHHmmss");
    }
    /// <summary>
    /// Displays recent dates with day of the week and less recent in date format
    /// </summary>
    /// <returns>Date of product</returns>
    internal string displayDateTime() {
        TimeSpan since = DateTime.Now.Subtract(ScanDateTime);
        if (since.Days > 6)
            return displayFullDateTime();
        else
            return ScanDateTime.ToString("dddd, h:mm tt");
    }
    /// <summary>
    /// Display date and time in readable format
    /// </summary>
    /// <returns></returns>
    internal string displayFullDateTime() {
        return ScanDateTime.ToString("M/d/yy - h:mm tt");
    }
    internal static DateTime getScanDateTimeFromString(string date) {
        return DateTime.ParseExact(date, "yyyyMMddHHmmss", /*CultureInfo.InvariantCulture*/null);
    }
    internal Sprite GetSprite()
    {
        switch (type)
        {
            case Category.containsaddedsugar:
                return ScanHistoryController.Instance.CateImg[1]; // exclamation
            case Category.noaddedsugar:
                return ScanHistoryController.Instance.CateImg[2]; // check
            default: // should never get here 
                return ScanHistoryController.Instance.CateImg[0];
        }
    }

}
public enum ActionType
{
    trigger,
    boolean,
    other
}