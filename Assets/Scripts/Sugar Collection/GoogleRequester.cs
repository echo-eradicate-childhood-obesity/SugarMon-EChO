using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class GoogleRequester : IRequester
{
    public List<string[]> List { get; set; }

    public int TargetPos { get; set; }
    public string Key { get; set; }

    LocationService ls = new LocationService();
    public async Task<string> SendRequest(string info)
    {
        return await Task.Run(async () =>
        {
            
            string url = $@"https://maps.googleapis.com/maps/api/geocode/json?{info}&result_type=street_address&key=AIzaSyCuOulVvtJ6ftHzdX8JIO_kaVE6CvejXKY";
            using (HttpClient client=new HttpClient())
            {
                try
                {
                    string str =await client.GetStringAsync(url);
                    JObject json = JsonConvert.DeserializeObject<JObject>(str);
                    var output = json.SelectToken("results").First.SelectToken("formatted_address");
                    return output.ToString();
                }
                catch { }
            }
                return "not here";
        });
    }


}
