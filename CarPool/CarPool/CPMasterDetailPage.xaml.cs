using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Util;
using Service;
using Events;
using Model;
using Org;

namespace CarPool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPMasterDetailPage : MasterDetailPage
    {
        private static CPMasterDetailPage masterDetailPage;
        public CPMasterDetailPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            masterDetailPage = this;
        }

        public static CPMasterDetailPage GetMasterDetail()
        {
            return masterDetailPage;
        }


        protected override void OnAppearing() { 
            base.OnAppearing();
            User user = UserService.getInstance().GetUser();
            if ( user != null  )
            {
                if (user.Organizations.Count > 0)
                {
                    NavigationUtil.SwitchDetailPage(typeof(EventsPage), "Events");
                }else
                {
                    NavigationUtil.SwitchDetailPage(typeof(OrganizationPicker), "Join Organization");
                }
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as CPMasterDetailPageMenuItem;
            if (item == null)
                return;

            if (item.TargetType != null)
            {
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;
                if (page != null)
                {
                    Detail = new NavigationPage(page);
                }
            }
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}