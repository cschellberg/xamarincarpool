using Events;
using Model;
using Service;
using System;
using Util;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Org
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrganizationPicker : ContentPage
    {
        public OrganizationPicker()
        {
            InitializeComponent();
            Grid grid = OrganizationsGrid;
            grid.Children.Clear();
            Int16 counter = 0;
            User user = UserService.getInstance().GetUser();
            foreach (Organization organization in OrganizationService.getInstance().GetOrganizations())
            {
                Grid subGrid = new Grid();
                Style organizationGridStyle = (Style)Application.Current.Resources["eventWindowStyle"];
                Style labelGridStyle = (Style)Application.Current.Resources["eventLabelStyle"];
                Style buttonGridStyle = (Style)Application.Current.Resources["eventButtonStyle"];
                subGrid.Style = organizationGridStyle;
                subGrid.Padding = 2;
                Label label = new Label();
                label.Text = organization.Name;
                label.Style = labelGridStyle;
                subGrid.Children.Add(label, 0, 1, 0, 1);
                if (!user.Organizations.Contains(organization.Id))
                {
                    Button joinButton = new Button();
                    joinButton.Text = "Join";
                    joinButton.Style = buttonGridStyle;
                    joinButton.Clicked += new EventHandler(OnJoinButtonClicked);
                    joinButton.CommandParameter = organization.Id;
                    subGrid.Children.Add(joinButton, 1, 2, 0, 1);
                }
                else
                {
                    Button leaveButton = new Button();
                    leaveButton.Text = "Leave";
                    leaveButton.Style = buttonGridStyle;
                    leaveButton.CommandParameter = organization.Id;
                    leaveButton.Clicked += new EventHandler(OnLeaveButtonClicked);
                    subGrid.Children.Add(leaveButton, 1, 2, 0, 1);
                }                
                grid.Children.Add(subGrid, 0, counter++);
            }
        }


        void OnJoinButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Int16 orgId = (Int16)button.CommandParameter;
            User user = UserService.getInstance().GetUser();
            if (!user.Organizations.Contains(orgId))
            {
                user.Organizations.Add(orgId);
                UserService.getInstance().SaveUser(user);
                Console.WriteLine("Org ID " + orgId + " is added");
                NavigationUtil.SwitchDetailPage(typeof(EventsPage), "events");
            }
        }

        void OnLeaveButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Int16 orgId = (Int16)button.CommandParameter;
            User user = UserService.getInstance().GetUser();
            if (user.Organizations.Contains(orgId))
            {
                user.Organizations.Remove(orgId);
                UserService.getInstance().SaveUser(user);
                Console.WriteLine("Org ID " + orgId + " was removed");
            }
        }
    }


}