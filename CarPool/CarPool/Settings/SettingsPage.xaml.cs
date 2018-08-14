using Model;
using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private Label messageLabel;

        public SettingsPage()
        {
            List<String> languageList = new List<String>();
            languageList.Add("English");
            Style entryStyle = (Style)Application.Current.Resources["entryStyle"];
            Style labelStyle = (Style)Application.Current.Resources["labelStyle"];
            Style buttonGridStyle = (Style)Application.Current.Resources["eventButtonStyle"];
            InitializeComponent();
            Grid grid = SettingsGrid;
            Label serverLabel = new Label();
            serverLabel.Style = labelStyle;
            serverLabel.Text = "Server URL";
            grid.Children.Add(serverLabel, 0, 0);
            Entry serverUrlEntry = new Entry(); 
            serverUrlEntry.Style = entryStyle;
            serverUrlEntry.SetBinding(Entry.TextProperty, "Settings.ServerURL");
            grid.Children.Add(serverUrlEntry, 0, 1);
            Label languageLabel = new Label();
            languageLabel.Style = labelStyle;
            languageLabel.Text = "Language";
            grid.Children.Add(languageLabel, 0, 2);
            Picker picker = new Picker();
            picker.Title = "Select a Language";
            picker.ItemsSource = languageList;
            picker.SetBinding(Picker.SelectedItemProperty, "Settings.Language");
            grid.Children.Add(picker, 0, 3);
            Button saveButton = new Button();
            saveButton.Text = "Save";
            saveButton.Style = buttonGridStyle;
            saveButton.Clicked+=new EventHandler(OnSaveButtonClicked);
            grid.Children.Add(saveButton, 0, 4);
            messageLabel = new Label();
            messageLabel.Style = labelStyle;
            messageLabel.Text = "";
            grid.Children.Add(messageLabel, 0, 5);
            BindingContext = new SettingsViewModel();

        }


        void OnSaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                SettingsViewModel settingsViewModel = (SettingsViewModel)BindingContext;

                SettingsService.getInstance().SaveSettings(settingsViewModel.Settings);
                messageLabel.Text = "Profile saved";
            }
            catch (Exception ex)
            {
                messageLabel.Text = "Could not save settings because " + ex.Message;
            }
        }

    }

    public class SettingsViewModel
    {
        public CarpoolSettings Settings { get; set; } = new CarpoolSettings();


        public SettingsViewModel()
        {
            Settings = SettingsService.getInstance().GetSettings();
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
