using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Register;
using Login;
using Service;
using Events;

namespace CarPool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPMasterDetailPageDetail : ContentPage
    {
        public CPMasterDetailPageDetail()
        {
            InitializeComponent();
        }

       

        void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var page = (Page)Activator.CreateInstance(typeof(RegisterPage));
            page.Title = "Register";
            if (page != null)
            {
                CPMasterDetailPage.GetMasterDetail().Detail = new NavigationPage(page);
            }
           
        }

        void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var page = (Page)Activator.CreateInstance(typeof(LoginPage));
            page.Title = "Login";
            if (page != null)
            {
                CPMasterDetailPage.GetMasterDetail().Detail = new NavigationPage(page);
            }
        }
    }
}