using MAUITask11.ViewModels;

namespace MAUITask11
{
    public partial class DecodingView : ContentPage
    {
        public DecodingView(DecodingViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
