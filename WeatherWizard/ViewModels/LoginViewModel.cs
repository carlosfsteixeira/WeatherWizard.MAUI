using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWizard.ViewModels
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyBD4SlmzABlwlMu_a6_WnvabF34DsljqJQ";
        private INavigation _navigation;
        private string userName;
        private string userPassword;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command RegisterBtn { get; }
        public Command LoginBtn { get; }
        public Command AboutBtn { get; }

        public string UserName
        {
            get => userName; set
            {
                userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public string UserPassword
        {
            get => userPassword; set
            {
                userPassword = value;
                RaisePropertyChanged("UserPassword");
            }
        }

        public LoginViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            RegisterBtn = new Command(RegisterBtnTappedAsync);
            LoginBtn = new Command(LoginBtnTappedAsync);
            AboutBtn = new Command(AboutBtnTappedAsync);
        }

        private async void AboutBtnTappedAsync(object obj)
        {
            await App.Current.MainPage.DisplayAlert("Version 1.0.0", "Author: Carlos Teixeira \nDate: 17/12/2022 \nPowered by OpenWeatherMap.org", "Close");
        }

        private async void LoginBtnTappedAsync(object obj)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));

            if (string.IsNullOrEmpty(UserName))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Email field is empty", "Ok");
                UserName = string.Empty;
                return;
            }

            if (string.IsNullOrEmpty(UserPassword))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Password field is empty", "Ok");
                UserPassword = string.Empty;
                return;
            }

            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(UserName, UserPassword);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("FreshFirebaseToken", serializedContent);
                await this._navigation.PushAsync(new DashboardPage());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                throw;
            }


        }

        private async void RegisterBtnTappedAsync(object obj)
        {
            await this._navigation.PushAsync(new RegisterPage());
        }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
