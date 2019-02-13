using Newtonsoft.Json;
using PoC.EatMyJson.Models;
using System.IO;

namespace PoC.EatMyJson.Repos
{
    public class MyObjectRepository
    {
        public MyObject GetMyObject()
        {
            var list = JsonConvert.DeserializeObject<MyObject>(File.ReadAllText("Datas/sample.json"));

            return list;
        }
    }
}