using FillMyJson.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace FillMyJson.Helpers
{
    public static class ActionsOnJson
    {
        public static bool SaveMyUser(User userToSave)
        {
            bool success = false;

            var list = ReadMyUsers();
            using (StreamWriter file = File.CreateText(@"Datas/users.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                list.Add(userToSave);
                serializer.Serialize(file, list);
                success = true;
            }

            return success;
        }

        public static List<User> ReadMyUsers()
        {
            var list = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("Datas/users.json"));
            if (list == null)
                return new List<User>();
            return list;
        }

        public static void ResetJson()
        {
            using (StreamWriter file = File.CreateText(@"Datas/users.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, null);
            }
        }
    }
}