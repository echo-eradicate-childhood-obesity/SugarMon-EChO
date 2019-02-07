using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class SearchController  {
    /*for sorted data, use this to do better search
     *this can be used for barcode id search
    */
    public static int BinarySearch(List<string> sList, long inputcode, int up, int low)
    {
        //int localhigh, locallow;
        //localhigh = up;
        //locallow = low;
        if (sList == null || (low > up))
        {
            return -1;
        }
        #region this cause stack overflow
        else
        {
            int i = (up + low) / 2;//half the index
            long currentEleInt;//current int in the element
            //here, if the barcode value(decimal wise), it will reach the scientific notation part in database
            //in that part, regex could not find the right code, need one more step to convert the number to decimal.
            long.TryParse(Regex.Match(sList[i], @"\d+").Value, out currentEleInt);

            if (inputcode > currentEleInt)
            {
                return BinarySearch(sList, inputcode, up, i + 1);
            }
            else if (inputcode < currentEleInt)
            {
                return BinarySearch(sList, inputcode, i - 1, low);
            }
            else
            {
                return i;
            }
        }

    }
    #endregion
}




