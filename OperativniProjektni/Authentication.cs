using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OperativniProjektni
{
    class Authentication
    {
        private static bool client = false;
        private Dictionary<string, string>[] users;
        private string username;

        public string getUsername()
        {
            return username;
        }

        private Authentication()
        {
            try
            {
                users = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(File.ReadAllText("../../../users.json"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }
        }

        public static Authentication MakeAuthClient()
        {
            if (!client)
            {
                client = true;
                return new Authentication();
            }
            else
                Console.WriteLine("Only one instance of Authenticate is allowed.");
            return null;
        }

        public bool Login(string authUsername, string authPassword)
        {
            // Moze i brze, mach po index
            foreach (Dictionary<string, string> u in users)
                if (u["username"] == authUsername && u["password"] == authPassword)
                {
                    username = u["username"];
                    return true;
                }
            Console.WriteLine("Error. Username and password don't match!");
            return false;
        }

        public void Logout()
        { 
            username = null;
        }

        public bool IsAuth()
        {
            if (!String.IsNullOrEmpty(username))
                return true;
            return false;
        }
    }
}
