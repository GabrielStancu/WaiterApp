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
            _model.SaveParameters();
            await Navigation.PopToRootAsync();
        }
    }
}