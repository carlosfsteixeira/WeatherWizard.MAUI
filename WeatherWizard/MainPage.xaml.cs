using WeatherWizard.ViewModels;

namespace WeatherWizard;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

        BindingContext = new LoginViewModel(Navigation);
    }
}

