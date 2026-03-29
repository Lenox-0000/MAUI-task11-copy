using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUITask11.Models;
using MAUITask11.Services;

namespace MAUITask11.ViewModels
{
    public partial class EncodingViewModel : ObservableObject
    {
        private readonly PeselService _svc;

        public EncodingViewModel(PeselService svc)
        {
            _svc = svc;
            Genders = new List<ItemPicker<Gender>>
            {
                new(Gender.Woman, "Kobieta"),
                new(Gender.Man,   "Mężczyzna")
            };
            _birthDay = DateTime.Today;
        }

        [ObservableProperty]
        private DateTime _birthDay;

        public List<ItemPicker<Gender>> Genders { get; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(GeneratePeselCommand))]
        private ItemPicker<Gender>? _selectedGender;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CopyToClipboardCommand))]
        [NotifyPropertyChangedFor(nameof(IsGenerated))]
        private string? _generatedPesel;

        // True once a PESEL has been generated — used for IsVisible on the result Border.
        public bool IsGenerated => !string.IsNullOrEmpty(GeneratedPesel);

        private bool CanGeneratePesel()   => SelectedGender is not null;
        private bool CanCopyToClipboard() => IsGenerated;

        [RelayCommand(CanExecute = nameof(CanGeneratePesel))]
        private async Task GeneratePeselAsync()
        {
            try
            {
                GeneratedPesel = _svc.Encode(BirthDay, SelectedGender!.Value);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Błąd", ex.Message, "OK");
            }
        }

        [RelayCommand(CanExecute = nameof(CanCopyToClipboard))]
        private async Task CopyToClipboardAsync()
        {
            await Clipboard.SetTextAsync(GeneratedPesel);
            await Toast.Make("PESEL skopiowany do schowka").Show();
        }
    }
}
