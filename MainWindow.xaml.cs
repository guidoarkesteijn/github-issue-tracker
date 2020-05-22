using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace github_issue_tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class IssueListItem
        {
            public int Id { get; protected set; }
            public string Title { get; set; }
            public string Body { get; set; }

            public IssueListItem(Issue issue)
            {
                UpdateItem(issue);
            }

            public void UpdateItem(Issue issue)
            {
                Id = issue.Id;
                Title = issue.Title;
                Body = issue.Body;
            }
        }

        private App app;
        private ObservableCollection<IssueListItem> issueListItems = new ObservableCollection<IssueListItem>();

        public MainWindow()
        {
            InitializeComponent();

            app = App.Current as App;
            app.IssuesUpdatedEvent += OnIssuesUpdatedEvent;

            issuesDataGrid.ItemsSource = issueListItems;
            appTokenTextBox.Text = app.SettingsService.AppToken;
        }

        private void OnIssuesUpdatedEvent(IReadOnlyList<Issue> issues)
        {
            refreshLabel.Visibility = Visibility.Hidden;
            foreach (var item in issues)
            {
                IssueListItem listItem = issueListItems.FirstOrDefault(x => x.Id == item.Id);
                if (listItem != null)
                {
                    listItem.UpdateItem(item);
                }
                else
                {
                    IssueListItem newItem = new IssueListItem(item);
                    issueListItems.Add(newItem);
                }
            }
            this.issuesDataGrid.Items.Refresh();
        }

        private void OnIssuesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Open selected item in popup editing window.
            foreach (var item in e.AddedItems)
            {
                Debug.WriteLine("I: " + item);
            }
        }

        private void OnRefreshButtonClicked(object sender, RoutedEventArgs e)
        {
            refreshLabel.Visibility = Visibility.Visible;

            app.RefreshIssues();
        }

        private void OnSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            app.UpdateAppToken(appTokenTextBox.Text);
        }
    }
}
