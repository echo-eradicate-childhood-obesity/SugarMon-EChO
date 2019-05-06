using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class USDARequester : IRequester
{
    public USDARequester(List<string[]> list,int targetpos)
    {
        this.List = list;
        this.TargetPos = targetpos;
    }
    public List<string[]> List { get; set; }
    public int TargetPos { get; set; }


    public async Task<int> LookNDBAsync(string upc)
    {
        Task<int> t = Task.Run(() =>
        {
            long val = long.Parse(upc);
            int result;
            result = SearchController.BinarySearch(List, val, List.Count - 1, 0, TargetPos);
            return result;
        });
        return await t;
    }

}
