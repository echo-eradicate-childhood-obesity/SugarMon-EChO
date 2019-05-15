using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
/// <summary>
/// Used to send to google map api
/// Repalce the url in SendRequest() to make it work for other requester
/// </summary>
public class GoogleRequester : IRequester
{
    public GoogleRequester(string key)
    {
        this.Key = key;
    }
    public List<string[]> List { get; set; }

    public int TargetPos { get; set; }
    public string Key { get; set; }
    LocationService ls = new LocationService();
    /// <summary>
    /// * This is used only for geocode
    /// * change the url to make it work for other request
    /// * When got the stree name, return street name otherwise return fail
    /// </summary>
    /// <param name="info">latlng combo</param>
    /// <returns>when work properly, return stree name</returns>
    public async Task<string> SendRequest(string info)
    {
        return await Task.Run(async () =>
        {
            
            string url = $@"https://maps.googleapis.com/maps/api/geocode/json?{info}&result_type=street_address&key={Key}";
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
                return "Fail To Get Location Information";
        });
    }
}

//One Respond example is here
//{
//   "plus_code" : {
//      "compound_code" : "9VFQ+FG Cambridge, MA, USA",
//      "global_code" : "87JC9VFQ+FG"
//   },
//   "results" : [
//      {
//         "address_components" : [
//            {
//               "long_name" : "447",
//               "short_name" : "447",
//               "types" : [ "street_number" ]
//            },
//            {
//               "long_name" : "Broadway",
//               "short_name" : "Broadway",
//               "types" : [ "route" ]
//            },
//            {
//               "long_name" : "Mid-Cambridge",
//               "short_name" : "Mid-Cambridge",
//               "types" : [ "neighborhood", "political" ]
//            },
//            {          
//		"long_name" : "Cambridge",
//               "short_name" : "Cambridge",
//               "types" : [ "locality", "political" ]
//            },
//            {
//               "long_name" : "Middlesex County",
//               "short_name" : "Middlesex County",
//               "types" : [ "administrative_area_level_2", "political" ]
//            },
//            {
//               "long_name" : "Massachusetts",
//               "short_name" : "MA",
//               "types" : [ "administrative_area_level_1", "political" ]
//            },
//            {
//               "long_name" : "United States",
//               "short_name" : "US",
//               "types" : [ "country", "political" ]
//            },
//            {
//               "long_name" : "02138",
//               "short_name" : "02138",
//               "types" : [ "postal_code" ]
//            }
//         ],
//         "formatted_address" : "447 Broadway, Cambridge, MA 02138, USA",
//         "geometry" : {
//            "location" : {
//               "lat" : 42.3734657,
//               "lng" : -71.1115006
//            },
//            "location_type" : "RANGE_INTERPOLATED",
//            "viewport" : {
//               "northeast" : {
//                  "lat" : 42.37481468029151,
//                  "lng" : -71.1101516197085
//               },
//               "southwest" : {
//                  "lat" : 42.37211671970851,
//                  "lng" : -71.11284958029151
//               }
//            }
//         },
//         "place_id" : "EiY0NDcgQnJvYWR3YXksIENhbWJyaWRnZSwgTUEgMDIxMzgsIFVTQSIbEhkKFAoSCXGj96hFd-OJEQCxuvoUloG0EL8D",
//         "types" : [ "street_address" ]
//      }
//   ],
//   "status" : "OK"
//}
