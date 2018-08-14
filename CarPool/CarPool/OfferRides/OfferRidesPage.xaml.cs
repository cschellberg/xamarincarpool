using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Service;
using Model;
using Util;
using Events;


namespace OfferRides
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfferRidesPage : ContentPage
    {
        private OrgEvent orgEvent;
        public OfferRidesPage(OrgEvent orgEvent)
        {
            this.orgEvent = orgEvent;
            InitializeComponent();
            List<Person> requestList = EventService.getInstance().GetRequests(orgEvent.Id);
            Style eventGridStyle = (Style)Application.Current.Resources["eventWindowStyle"];
            Style labelGridStyle = (Style)Application.Current.Resources["eventLabelStyle"];
            Style buttonGridStyle = (Style)Application.Current.Resources["eventButtonStyle"];
            Style labelStyle = (Style)Application.Current.Resources["labelStyle"];
            Grid grid = OfferRidesGrid;
            grid.Children.Clear();
            int counter = 0;
            Label eventDescriptionLabel = new Label();
            eventDescriptionLabel.Style = labelStyle;
            eventDescriptionLabel.Text = orgEvent.GetDescription();
            grid.Children.Add(eventDescriptionLabel, 0,counter);
            counter++;
            if (requestList.Count == 0)
            {
                Label offerRidesLabel = new Label();
                offerRidesLabel.Style = labelStyle;
                offerRidesLabel.Text = "There are no pending requests for this event";
                grid.Children.Add(offerRidesLabel, 0, counter);
                counter++;
            }
            foreach (Person rideRequester in requestList)
            {
                Grid subGrid = new Grid();
                subGrid.Style = eventGridStyle;
                subGrid.Padding = 2;
                Label nameLabel = new Label();
                nameLabel.Text = rideRequester.GetFullName();
                nameLabel.Style = labelGridStyle;
                subGrid.Children.Add(nameLabel, 0, 1, 0, 1);
                subGrid.Padding = 2;
                Label addressLabel = new Label();
                addressLabel.Text = rideRequester.GetAddress();
                addressLabel.Style = labelGridStyle;
                subGrid.Children.Add(addressLabel, 1, 2, 0, 1);
                Button offerRideButton = new Button();
                offerRideButton.Text = "Offer Ride to " + rideRequester.GetFullName();
                offerRideButton.Style = buttonGridStyle;
                offerRideButton.CommandParameter = rideRequester;
                offerRideButton.Clicked += new EventHandler(OnOfferRideButtonClicked);
                subGrid.Children.Add(offerRideButton, 0, 2, 1, 2);
                grid.Children.Add(subGrid, 0, counter);
                counter++;
            }
            Button backButton = new Button();
            backButton.Text = "Back to Events";
            backButton.Clicked += new EventHandler(OnBackButtonClicked);
            grid.Children.Add(backButton, 0, counter);
        }

        void OnOfferRideButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Person requester = (Person)button.CommandParameter;
            if (EventService.getInstance().AssignRequest(orgEvent.Id, requester))
            {
                removeRequester(requester);
            }
        }

        private void removeRequester(Person requester)
        {
            Grid grid = OfferRidesGrid;
            int counter = 0;
            foreach (Element element in grid.Children)
            {
                if (element.GetType() == typeof(Grid))
                {
                    Grid subGrid = (Grid)element;
                    bool nameMatches = false;
                    bool addressMatches = false;
                    foreach (Element subElement in subGrid.Children)
                    {
                        if ( subElement.GetType() == typeof(Label))
                        {
                            Label sublabel = (Label)subElement;
                            if (sublabel.Text.Equals(requester.GetFullName()))
                            {
                                nameMatches = true;
                            }
                            if (sublabel.Text.Equals(requester.GetAddress()))
                            {
                                addressMatches = true;
                            }
                        }
                    }
                    if (nameMatches && addressMatches)
                    {
                        grid.Children.RemoveAt(counter);
                        break;
                    }
                }
                counter++;
            }
        }


        void OnBackButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Page page = (Page)Activator.CreateInstance(typeof(EventsPage));
            NavigationUtil.SwitchDetailPage(page);
        }
    }
}