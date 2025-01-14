using static System.Net.Mime.MediaTypeNames;

namespace MauiTheme1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            buThemeLight.Clicked += (s, e) +> Application.Current.UserAppTheme = AppTheme.Light;
            buThemeDark.Clicked += (s, e) +> Application.Current.UserAppTheme = AppTheme.Dark;
            buThemeSystem.Clicked += (s, e) +> Application.Current.UserAppTheme = AppTheme.System;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
