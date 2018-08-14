
using Xamarin.Forms;


namespace CarPool
{
    public partial class App : Application
	{
        public static bool IsUserLoggedIn = false;
        public static App self;

		public App ()
		{
			InitializeComponent();
            MainPage = new CPMasterDetailPage();
            self = this;
		}

        public static App getInstance()
        {
            return self;
        }

        public void  setMainPage(ContentPage newPage)
        {
            MainPage = newPage;
        }

		protected override void OnStart ()
		{
          

        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
