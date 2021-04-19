using Infrastructure.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage(SettingsViewModel model)
        {
            InitializeComponent();
            this.BindingContext = model;
        }

        private async void OnSaveParameters(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}