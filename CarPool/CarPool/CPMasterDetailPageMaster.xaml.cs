using Events;
using Login;
using Org;
using Register;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Settings;

namespace CarPool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public CPMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new CPMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class CPMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<CPMasterDetailPageMenuItem> MenuItems { get; set; }
            
            public CPMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<CPMasterDetailPageMenuItem>(new[]
                {
                    new CPMasterDetailPageMenuItem { Id = 0, Title = "Login", TargetType= typeof(LoginPage) },
                    new CPMasterDetailPageMenuItem { Id = 1, Title = "Register",TargetType =  typeof(RegisterPage) },
                    new CPMasterDetailPageMenuItem { Id = 2, Title = "Events",TargetType =  typeof(EventsPage) },
                    new CPMasterDetailPageMenuItem { Id = 3, Title = "Join Organization",TargetType =  typeof(OrganizationPicker) },
                    new CPMasterDetailPageMenuItem { Id = 4, Title = "Settings",TargetType =  typeof(SettingsPage) },
                });
               
                
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}