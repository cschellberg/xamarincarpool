using Model;
using Register;
using System;
using Xamarin.Forms;
using Events;
using CarPool;
using Service;
using Util;

namespace Login
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            if (UserService.getInstance().GetUser() != null)
            {
                NavigationUtil.SwitchDetailPage(typeof(EventsPage), "Events");
            }
        }

        public void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            NavigationUtil.SwitchDetailPage(typeof(RegisterPage), "Register");
        }

        void OnLoginButtonClicked(object sender, EventArgs e)
        {
            User user = UserService.getInstance().GetUser(emailEntry.Text, passwordEntry.Text);
    
            if (user != null)
            {
                NavigationUtil.SwitchDetailPage(typeof(EventsPage), "Events");
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
            }
        }

    }
}
