using Infrastructure.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParametersPage : ContentPage
    {
        private readonly ParametersViewModel _model;

        public ParametersPage(ParametersViewModel model)
        {
            InitializeComponent();
            _model = model;
            BindingContext = _model;
        }

        private async void OnSaveParameters(object sender, System.EventArgs e)
        {
            if(!_model.SaveParameters())
            {
                await DisplayAlert("Error", "Not all parameters completed.", "OK");
            }
            else
            {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                await Navigation.PopAsync();
            }
        }

        private async void OnEntryUnfocused(object sender, FocusEventArgs e)
        {
            string text = (sender as Entry).Text;

            if(!int.TryParse(text, out _))
            {
                await DisplayAlert("Error", "Invalid value, must be numeric.", "OK");
            }
        }
    }
}