using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUITask11.Exceptions;
using MAUITask11.Models;
using MAUITask11.Services;
using System.Globalization;

namespace MAUITask11.ViewModels
{
    public partial class DecodingViewModel : ObservableObject
    {
        private readonly PeselService _svc;

        public DecodingViewModel(PeselService svc) => _svc = svc;

        // The raw PESEL string typed by the user.
        // NotifyCanExecuteChangedFor re-evaluates the button's IsEnabled each time this changes.
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DecodePeselCommand))]
        private string _pesel = string.Empty;

        // The decoded person — when set, all computed properties (Gender, BirthDate, …) notify the UI.
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Gender))]
        [NotifyPropertyChangedFor(nameof(BirthDate))]
        [NotifyPropertyChangedFor(nameof(Age))]
        [NotifyPropertyChangedFor(nameof(NextBirthdayDays))]
        [NotifyPropertyChangedFor(nameof(ValidityText))]
        [NotifyPropertyChangedFor(nameof(ValidityIcon))]
        [NotifyPropertyChangedFor(nameof(ValidityIconColor))]
        [NotifyPropertyChangedFor(nameof(GenderIcon))]
        [NotifyPropertyChangedFor(nameof(GenderIconColor))]
        private Human? _human;

        // Controls IsVisible on the results area.
        [ObservableProperty]
        private bool _decoded;

        // --- Computed display properties ---

        public string Gender => Human?.Gender switch
        {
            Models.Gender.Man   => "Mężczyzna",
            Models.Gender.Woman => "Kobieta",
            _                   => string.Empty
        };

        public string BirthDate => Human is null
            ? string.Empty
            : Human.BirthDate.ToString("dd. MMMM yyyy", new CultureInfo("pl-PL"));

        public string Age
        {
            get
            {
                if (Human is null) return string.Empty;
                var today = DateTime.Today;
                int age = today.Year - Human.BirthDate.Year;
                if (Human.BirthDate > today.AddYears(-age)) age--;
                return $"{age} lat";
            }
        }

        public string NextBirthdayDays
        {
            get
            {
                if (Human is null) return string.Empty;
                try
                {
                    var today = DateTime.Today;
                    var next = new DateTime(today.Year, Human.BirthDate.Month, Human.BirthDate.Day);
                    if (next < today) next = next.AddYears(1);
                    int days = (next - today).Days;
                    return days == 0 ? "Dziś urodziny! 🎉" : $"{days} dni";
                }
                catch { return "—"; }
            }
        }

        public string ValidityText      => Human is null ? string.Empty
                                        : Human.IsValid ? "PESEL prawidłowy" : "PESEL nieprawidłowy";
        public string ValidityIcon      => Human?.IsValid == true ? "check_circle" : "cancel";
        public Color  ValidityIconColor => Human?.IsValid == true ? Colors.Green : Colors.Red;

        public string GenderIcon        => Human?.Gender == Models.Gender.Man ? "male" : "female";
        public Color  GenderIconColor   => Human?.Gender == Models.Gender.Man
                                        ? Color.FromArgb("#2196F3")   // blue for male
                                        : Color.FromArgb("#E91E63");  // pink for female

        // --- Command ---

        private bool CanDecodePesel() => !string.IsNullOrEmpty(Pesel);

        [RelayCommand(CanExecute = nameof(CanDecodePesel))]
        private async Task DecodePeselAsync()
        {
            try
            {
                Human   = _svc.Decode(Pesel);
                Decoded = true;
            }
            catch (InvalidPeselLengthException ex)
            {
                await Shell.Current.DisplayAlert("Błąd",
                    $"Nieprawidłowa długość PESEL: {ex.Length} znaków (wymagane 11).", "OK");
            }
            catch (InvalidPeselMonthException ex)
            {
                await Shell.Current.DisplayAlert("Błąd", ex.Message, "OK");
            }
            catch (ArgumentException ex)
            {
                await Shell.Current.DisplayAlert("Błąd", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Nieoczekiwany błąd", ex.Message, "OK");
            }
        }
    }
}
