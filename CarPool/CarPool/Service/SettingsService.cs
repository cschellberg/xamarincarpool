using System;
using Model;
using Newtonsoft.Json;
using System.IO;

namespace Service
{
    public class SettingsService
    {
        private static SettingsService settingsService;

        private static String documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        private static String FILE_NAME = "/carpool_settings.json";

        private static String filePath = documentsPath + FILE_NAME;

        public static SettingsService getInstance()
        {
            if ( settingsService == null)
            {
                settingsService = new SettingsService();
            }
            return settingsService;
        }

        public void SaveSettings(CarpoolSettings settings)
        {
            String settingsStr = JsonConvert.SerializeObject(settings);
            File.WriteAllText(filePath, settingsStr);
        }

        public CarpoolSettings GetSettings()
        {

            if (File.Exists(filePath))
            {
                try
                {
                    String settingsStr = File.ReadAllText(filePath);
                    CarpoolSettings carpoolSettings = JsonConvert.DeserializeObject<CarpoolSettings>(settingsStr);
                    return carpoolSettings;
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine("Cannot deserialize user because " + ex.Message);
                    return new CarpoolSettings();
                }
            }
            else
            {
                return new CarpoolSettings();
            }

        }

    }
}

