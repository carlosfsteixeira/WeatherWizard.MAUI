using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWizard.Helpers;

namespace WeatherWizard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        RestService _restService;

        public DashboardPage()
        {
            InitializeComponent();

            _restService = new RestService();

            GetProfileInfo();
        }

        public async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_cityEntry.Text))
            {
                WeatherData weatherData = await
                    _restService.
                    GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint));

                BindingContext = weatherData;
            }
        }

        public string GenerateRequestURL(string endPoint)
        {
            string requestUri = endPoint;
            requestUri += $"?q={_cityEntry.Text}";
            requestUri += "&units=imperial";
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }

        private void GetProfileInfo()
        {
            var userInfo = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FreshFirebaseToken", ""));
            UserEmail.Text = userInfo.User.Email;
        }



    }
}