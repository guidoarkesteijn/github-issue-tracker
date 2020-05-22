using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Octokit;

namespace github_issue_tracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public delegate void IssuesUpdatedHandler(IReadOnlyList<Issue> issues);
        public event IssuesUpdatedHandler IssuesUpdatedEvent;

        public IReadOnlyList<Issue> issues;

        public SettingsService SettingsService;
        public GitHubClient Client;

        public App()
        {
            SettingsService = new SettingsService();

            if (string.IsNullOrEmpty(SettingsService.AppToken))
            {
                Client = new GitHubClient(new ProductHeaderValue("OMHToken"));
                Client.Credentials = new Credentials(SettingsService.AppToken);
                
                RefreshIssues();
            }
        }

        public void RefreshIssues()
        {
            Task.Run(GetIssues);
        }

        private async void GetIssues()
        {
            issues = await Client.Issue.GetAllForRepository("guidoarkesteijn", "mine-sweeper");
            Dispatcher.Invoke(() =>
            {
                IssuesUpdatedEvent?.Invoke(issues);

                Debug.WriteLine(issues.Count);
            });
        }
    }
}
