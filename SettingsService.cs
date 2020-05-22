using System;
using System.IO;
using Newtonsoft.Json;

namespace github_issue_tracker
{
    /// <summary>
    /// Contains settings used by the app.
    /// </summary>
    public class SettingsService
    {
        private const string SETTINGS_FILENAME = "settings.json";

        public string AppToken => currentSettings.AppToken;

        private SettingsData currentSettings;
        private string settingsPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + SETTINGS_FILENAME;

        public SettingsService()
        {
            Load();
        }

        public void Load()
        {
            if (File.Exists(settingsPath))
            {
                string json = File.ReadAllText(settingsPath);
                currentSettings = JsonConvert.DeserializeObject<SettingsData>(json);
            }
            else
            {
                currentSettings = SettingsData.CreateDefault();
                Save();
            }
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(settingsPath, json);
        }

        internal void UpdateAppToken(string appToken)
        {
            currentSettings.UpdateAppToken(appToken);
            Save();
        }
    }
}
