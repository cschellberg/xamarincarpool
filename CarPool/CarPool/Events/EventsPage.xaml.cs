using Model;
using Service;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Util;
using Rides;
using OfferRides;

namespace Events
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventsPage : ContentPage
    {
        private List<OrgEvent> orgEvents;
        private MonthTracker monthTracker = new MonthTracker();

        public EventsPage()
        {
            initGrid();
        }

        private void initGrid()
        {
            orgEvents = EventService.getInstance().GetOrgEvents(monthTracker.GetMonthNumber(), 
                monthTracker.GetYearNumber());
            InitializeComponent();
            Grid grid = EventsGrid;
            grid.Children.Clear();
            Int16 counter = 0;
            Style eventGridStyle = (Style)Application.Current.Resources["eventWindowStyle"];
            Style labelStyle = (Style)Application.Current.Resources["labelStyle"];
            Style labelGridStyle = (Style)Application.Current.Resources["eventLabelStyle"];
            Style buttonGridStyle = (Style)Application.Current.Resources["eventButtonStyle"];
            Grid monthGrid = new Grid();
            monthGrid.Style = eventGridStyle;
            monthGrid.Padding = 2;
            Button previousMonth = new Button();
            previousMonth.Style = buttonGridStyle;
            previousMonth.Text = "Previous Month";
            previousMonth.Clicked += new EventHandler(OnPreviousMonthClicked);
            monthGrid.Children.Add(previousMonth, 0, 1, 0, 1);
            Label monthLabel = new Label();
            monthLabel.Text = monthTracker.GetMonthYearStr();
            monthLabel.HorizontalTextAlignment = TextAlignment.Center;
            monthLabel.Style = labelGridStyle;
            monthGrid.Children.Add(monthLabel, 1, 2, 0, 1);
            Button nextMonth = new Button();
            nextMonth.Style = buttonGridStyle;
            nextMonth.Text = "Next Month";
            nextMonth.Clicked += new EventHandler(OnNextMonthClicked);
            monthGrid.Children.Add(nextMonth, 2, 3, 0, 1);
            grid.Children.Add(monthGrid, 0, counter++);
            foreach (OrgEvent orgEvent in orgEvents)
            {
                Grid subGrid = new Grid();
                subGrid.Style = eventGridStyle;
                subGrid.Padding = 2;
                Label label = new Label();
                label.Text = orgEvent.GetDescription();
                label.Style = labelGridStyle;
                User user = UserService.getInstance().GetUser();
                bool isEventDriver = false;
                bool isEventPassenger = orgEvent.IsPassenger(user.Email);
                bool hasUserReqeuestedRide = EventService.getInstance().HasUserRequestedRide(orgEvent, user);
                if (user.Driver && !isEventPassenger)
                {
                    if (!orgEvent.IsDriver(user.Email))
                    {
                        if (orgEvent.RideRequests.Count > 0 && hasUserReqeuestedRide)
                        {
                            Button offerRideButton = new Button();
                            offerRideButton.Text = "Offer Ride";
                            offerRideButton.Style = buttonGridStyle;
                            offerRideButton.CommandParameter = orgEvent;
                            offerRideButton.Clicked += new EventHandler(OnOfferRideButtonClicked);
                            subGrid.Children.Add(offerRideButton, 0, 1, 1, 2);
                        }
                    }
                    else
                    {
                        isEventDriver = true;
                        Button offerRideButton = new Button();
                        offerRideButton.Text = "Cancel Ride";
                        offerRideButton.Style = buttonGridStyle;
                        subGrid.Children.Add(offerRideButton, 0, 1, 1, 2);
                        Button ridesButton = new Button();
                        ridesButton.Text = "Passenger List";
                        ridesButton.CommandParameter = orgEvent.Id;
                        ridesButton.Style = buttonGridStyle;
                        ridesButton.Clicked += new EventHandler(OnRidesButtonClicked);
                        subGrid.Children.Add(ridesButton, 1, 2, 1, 2);
                    }
                }
                if (!isEventDriver)
                {
                    if (!isEventPassenger && !hasUserReqeuestedRide)
                    {
                        Button requestRideButton = new Button();
                        requestRideButton.Text = "Request Ride";
                        requestRideButton.Style = buttonGridStyle;
                        RideRequest rideRequest = new RideRequest
                        {
                            Event = orgEvent,
                            Passenger = user
                        };
                        requestRideButton.CommandParameter = rideRequest;
                        requestRideButton.Clicked += new EventHandler(OnRequestRideButtonClicked);
                        subGrid.Children.Add(requestRideButton, 1, 2, 1, 2);
                    }
                    else
                    {
                        Label driverLabel = new Label();
                        if (orgEvent.GetDriver(user.Email) != null)
                        {
                            driverLabel.Text = "Driver: " + orgEvent.GetDriver(user.Email);
                        }
                        else if (hasUserReqeuestedRide)
                        {
                            driverLabel.Text = "Driver Requested";
                        }
                        driverLabel.Style = labelGridStyle;
                        subGrid.Children.Add(driverLabel, 0, 1, 1, 2);
                        Button cancelRideButton = new Button();
                        cancelRideButton.Text = "Cancel Request";
                        RideRequest rideRequest = new RideRequest
                        {
                            Event = orgEvent,
                            Passenger = user
                        };
                        cancelRideButton.CommandParameter = rideRequest;
                        cancelRideButton.Clicked += new EventHandler(OnCancelRideButtonClicked);
                        cancelRideButton.Style = buttonGridStyle;
                        subGrid.Children.Add(cancelRideButton, 1, 2, 1, 2);
                    }
                }
                subGrid.Children.Add(label, 0, 2, 0, 1);

                grid.Children.Add(subGrid, 0, counter++);
            }
        }

        void OnCancelRideButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            RideRequest rideRequest = (RideRequest)button.CommandParameter;
            if (EventService.getInstance().CancelRideRequest(rideRequest))
            {
                ((Grid)button.Parent).Children.Remove(button);
            }
            initGrid();
        }

        void OnPreviousMonthClicked(object sender, EventArgs e)
        {
            monthTracker.DecrementMonth();
            initGrid();
        }

        void OnNextMonthClicked(object sender, EventArgs e)
        {
            monthTracker.IncrementMonth();
            initGrid();
        }

        void OnRequestRideButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            RideRequest rideRequest = (RideRequest)button.CommandParameter;
            if (EventService.getInstance().AddRideRequest(rideRequest))
            {
                ((Grid)button.Parent).Children.Remove(button);
            }
            initGrid();
        }

        void OnOfferRideButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            OrgEvent orgEvent = (OrgEvent)button.CommandParameter;
            OfferRidesPage page = new OfferRidesPage(orgEvent);
            NavigationUtil.SwitchDetailPage(page);
        }

        void OnRidesButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Int32 eventId = (Int32)button.CommandParameter;
            RidesPage page = new RidesPage(eventId);//(RidesPage)Activator.CreateInstance(typeof(RidesPage));
            NavigationUtil.SwitchDetailPage(page);
        }
    }
}