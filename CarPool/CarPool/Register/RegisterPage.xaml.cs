using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Service;
using Model;


namespace Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
       

        public RegisterPage()
        {
            InitializeComponent();
        
            BindingContext = new RegisterViewModel();
        }

        void OnSaveButtonClicked(object sender, EventArgs e)
        {
            string password = passwordEntry.Text;
            string confirmedPassword = confirmedPasswordEntry.Text;
            if (password.Equals(confirmedPassword))
            {
                RegisterViewModel registerViewModel = (RegisterViewModel)BindingContext;

                UserService.getInstance().SaveUser(registerViewModel.User);
                messageLabel.Text = "Profile saved";
            }
            else
            {
                messageLabel.Text = "Passwords do not match";
            }
        }

    }

    public class RegisterViewModel 
    {
        public User User { get; set; }
       
    
        public RegisterViewModel()
        {
            User = UserService.getInstance().GetUser();
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}