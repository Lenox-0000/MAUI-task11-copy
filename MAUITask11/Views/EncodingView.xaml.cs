using MAUITask11.ViewModels;

namespace MAUITask11
{
    public partial class EncodingView : ContentPage
    {

        public EncodingView( EncodingViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
