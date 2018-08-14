using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Service;
using Model;
using System.Collections.Generic;
using Events;
using Util;

namespace Rides
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RidesPage : ContentPage
    {
        public Int32 EventId { get; set; }
        public RidesPage(Int32 eventId)
        {
            EventId = eventId;
            this.Title = "rides";
            InitializeComponent();
            List<Person> passengers = EventService.getInstance().GetPassengers(eventId);
            Style eventGridStyle = (Style)Application.Current.Resources["eventWindowStyle"];
            Style labelGridStyle = (Style)Application.Current.Resources["eventLabelStyle"];
            Style buttonGridStyle = (Style)Application.Current.Resources["eventButtonStyle"];
            Style labelStyle = (Style)Application.Current.Resources["labelStyle"];
            Grid grid = RidesGrid;
            grid.Children.Clear();
            Label ridesLabel = new Label();
            ridesLabel.Style = labelStyle;
            grid.Children.Add(ridesLabel, 0, 1, 0, 1);
            int counter = 1;
            if (passengers.Count == 0)
            {
                ridesLabel.Text = "There are not passengers for this event";
            }
            else
            {
                ridesLabel.Text = "There passengers for this event are:";

                foreach (Person passenger in passengers)
                {
                    Grid subGrid = new Grid();

                    subGrid.Style = eventGridStyle;
                    subGrid.Padding = 2;
                    Label passengerLabel = new Label();
                    passengerLabel.Style = labelGridStyle;
                    passengerLabel.Text = passenger.GetFullName();
                    subGrid.Children.Add(passengerLabel, 0, 1, 0, 1);
                    Button callButton = new Button();
                    callButton.Style = buttonGridStyle;
                    callButton.Text = "Call";
                    callButton.CommandParameter = passenger;
                    callButton.Clicked += new EventHandler(OnCallButtonClicked);
                    Button mapButton = new Button();
                    mapButton.Style = buttonGridStyle;
                    mapButton.Text = "Map";
                    mapButton.CommandParameter = passenger;
                    mapButton.Clicked += new EventHandler(OnMapButtonClicked);
                    subGrid.Children.Add(callButton, 1, 2, 0, 1);
                    subGrid.Children.Add(mapButton, 2, 3, 0, 1);
                    grid.Children.Add(subGrid, 0, counter++);
                }
            }
            Button backButton = new Button();
            backButton.Text = "Back to Events";
            backButton.Clicked += new EventHandler(OnBackButtonClicked);
            grid.Children.Add(backButton, 0, counter);

        }

        void OnBackButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Page page = (Page)Activator.CreateInstance(typeof(EventsPage));
            NavigationUtil.SwitchDetailPage(page);
        }

        void OnCallButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Person passenger=(Person)button.CommandParameter;
            if (passenger.PhoneNumber != null)
            {
                Device.OpenUri(new Uri(String.Format("tel:{0}", passenger.PhoneNumber)));
            }
        }

        void OnMapButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Person passenger = (Person)button.CommandParameter;
            if (passenger.Location != null )
            {
                String mapParameter = passenger.Location.GetLocationParameter();
                Device.OpenUri(new Uri(String.Format("http://maps.google.com/maps?daddr={0}", mapParameter)));
            }
        }
    }
}