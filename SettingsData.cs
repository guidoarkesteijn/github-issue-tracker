using Newtonsoft.Json;
using System;

namespace github_issue_tracker
{
    public class SettingsData
    {
        [JsonProperty]
        public string AppToken { get; private set; } = "";

        internal static SettingsData CreateDefault()
        {
            SettingsData settingsData = new SettingsData();
            settingsData.AppToken = "";
            return settingsData;
        }

        internal void UpdateAppToken(string appToken)
        {
            AppToken = appToken;
        }
    }
}
