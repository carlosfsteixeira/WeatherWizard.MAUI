using Newtonsoft.Json;
using Plugin.Connectivity;
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
            if (!CrossConnectivity.Current.IsConnected)
            {
                await App.Current.MainPage.DisplayAlert("Warning", "No Internet Connectivity", "Close");

                return;
            }

            if (!string.IsNullOrWhiteSpace(_cityEntry.Text))
            {
                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint));

                BindingContext = weatherData;

                _cityEntry.Text = "";
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Warning", "City Name field is empty", "Close");

                return;
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