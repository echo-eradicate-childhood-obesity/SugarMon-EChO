using System.Collections.Generic;
using System.Threading.Tasks;
public interface IRequester
{
    List<string[]> List { get; set; }
    int TargetPos { get; set; }
    Task<int> LookNDBAsync(string upc);
}