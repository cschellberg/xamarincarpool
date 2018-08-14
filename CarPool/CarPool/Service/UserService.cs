using Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;





namespace Service
{
    public class UserService
    {
        private static UserService userService;

        private User loggedInUser;

        private static String documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        private static String FILE_NAME = "/user.json";

        private static String filePath = documentsPath + FILE_NAME;

        public static UserService getInstance()
        {
            if (userService == null)
            {
                userService = new UserService();
            }
            return userService;
        }

        public void SaveUser(User user)
        {
            String userStr = JsonConvert.SerializeObject(user);
            File.WriteAllText(filePath, userStr);
        }

        public User GetUser(String email, String password)
        {
            if (loggedInUser != null)
            {
                return loggedInUser;
            }
            else
            {
                return getUserFromWebsite(email, password);
            }


        }

        private User getUserFromWebsite(String email, String password)
        {
            String url = "http://192.168.1.153:8080/login";
            email = "dschellberg@gmail.com";
            password = "p2345";
            var formContent = new FormUrlEncodedContent(new[]
    {
                new KeyValuePair<string, string>("username", email),
                new KeyValuePair<string, string>("password", password),
            });
            try
            {
                var myHttpClient = new HttpClient();
                HttpResponseMessage response = myHttpClient.PostAsync(url, formContent).Result;
                url = "http://192.168.1.153:8080/person/"+email;
                response = myHttpClient.GetAsync(url).Result;
                String json = response.Content.ReadAsStringAsync().Result;
                loggedInUser = JsonConvert.DeserializeObject<User>(json);
            }catch(Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
            return loggedInUser;
        }

        public User GetUser()
        {
            return loggedInUser;
        }

        private User getUserFromFile()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    String userStr = File.ReadAllText(filePath);
                    User user = JsonConvert.DeserializeObject<User>(userStr);
                    return user;
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine("Cannot deserialize user because " + ex.Message);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
