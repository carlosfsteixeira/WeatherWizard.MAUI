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
    internal class RegisterViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyBD4SlmzABlwlMu_a6_WnvabF34DsljqJQ";

        private INavigation _navigation;
        private string email;
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string Password
        {
            get => password; set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        public Command RegisterUser { get; }

        public Command AboutBtn { get; }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public RegisterViewModel(INavigation navigation)
        {
            this._navigation = navigation;

            RegisterUser = new Command(RegisterUserTappedAsync);

            AboutBtn = new Command(AboutBtnTappedAsync);
        }

        private async void AboutBtnTappedAsync(object obj)
        {
            await App.Current.MainPage.DisplayAlert("Version 1.0.0", "Author: Carlos Teixeira \nDate: 17/12/2022 \nPowered by OpenWeatherMap.org", "Close");
        }

        private async void RegisterUserTappedAsync(object obj)
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Email field is empty", "Ok");
                Email = string.Empty;
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Password field is empty", "Ok");
                Password = string.Empty;
                return;
            }


            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);
                string token = auth.FirebaseToken;
                if (token != null)
                    await App.Current.MainPage.DisplayAlert("New Account", "User Registered successfully", "OK");
                await this._navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                throw;
            }
        }
    }
}
