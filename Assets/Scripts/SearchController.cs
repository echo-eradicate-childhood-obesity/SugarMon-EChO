using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class SearchController  {
    /*for sorted data, use this to do better search
     *this can be used for barcode id search
    */
    public static int BinarySearch(List<string> sList, long inputcode, int up, int low = 0)
    {
        int outInt;
        int localhigh, locallow;
        localhigh = up;
        locallow = low;
        if (sList == null || (low > up))
        {
            outInt = -1;
            return outInt;
        }
        #region this cause stack overflow
        else
        {
            int i = (up - low) / 2;//half the index
            long currentEleInt;//current int in the element
            currentEleInt = IntConverter(sList[i]);//get current int

            if (inputcode > currentEleInt)
            {
                return BinarySearch(sList, inputcode, localhigh, i + 1);
            }
            else if (inputcode < currentEleInt)
            {
                return BinarySearch(sList, inputcode, i - 1, locallow);
            }
            else
            {
                return outInt = i;
            }
        }
        

        #endregion
        #region this one crash the app
        //while (low < up)
        //{
        //    int mid = (up - low) / 2;
        //    if (inputcode == IntConverter(sList[mid]))
        //    {
        //        return mid;
        //    }
        //    else if (inputcode > IntConverter(sList[mid]))
        //    {
        //        low = mid + 1;
        //    }
        //    else { up = mid - 1; }
        //}
        //return -1;
        #endregion
    }


    public static long IntConverter(string input)
    {
        long outInt;
        StringBuilder sb = new StringBuilder();
        /*this part from ms offical site
         * https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-string-to-a-number
        */
        foreach (char c in input)
        {

            if ((c >= '0' && c <= '9') || c == ' ' || c == '-')
            {
                sb.Append(c);
            }
            else
                break;
        }
        outInt = long.Parse(sb.ToString());
        return outInt;
    }
}
