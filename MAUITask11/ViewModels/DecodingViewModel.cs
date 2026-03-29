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

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DecodePeselCommand))]
        private string _pesel = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Gender))]
        [NotifyPropertyChangedFor(nameof(BirthDate))]
        [NotifyPropertyChangedFor(nameof(Age))]
        [NotifyPropertyChangedFor(nameof(NextBirthdayDays))]
        [NotifyPropertyChangedFor(nameof(ValidityText))]
        [NotifyPropertyChangedFor(nameof(ValiditySymbol))]
        [NotifyPropertyChangedFor(nameof(ValidityColor))]
        [NotifyPropertyChangedFor(nameof(GenderSymbol))]
        [NotifyPropertyChangedFor(nameof(GenderColor))]
        private Human? _human;

        [ObservableProperty]
        private bool _decoded;

        // --- Text properties ---

        public string Gender => Human?.Gender switch
        {
            Models.Gender.Man   => "Male",
            Models.Gender.Woman => "Female",
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
                return $"{age} years";
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
                    return days == 0 ? "Birthday is today! \U0001F389" : $"{days} days";
                }
                catch { return "\u2014"; }
            }
        }

        public string ValidityText => Human is null ? string.Empty
                                    : Human.IsValid ? "Valid PESEL" : "Invalid PESEL";

        // --- Icon symbol + color pairs (Unicode chars rendered as Label text) ---

        /// <summary>Unicode checkmark ✓ or cross ✗, coloured via ValidityColor.</summary>
        public string ValiditySymbol => Human?.IsValid == true ? "\u2713" : "\u2717";

        public Color ValidityColor => Human?.IsValid == true ? Colors.Green : Colors.Red;

        /// <summary>Unicode Mars ♂ or Venus ♀, coloured via GenderColor.</summary>
        public string GenderSymbol => Human?.Gender == Models.Gender.Man ? "\u2642" : "\u2640";

        public Color GenderColor => Human?.Gender == Models.Gender.Man
            ? Color.FromArgb("#2196F3")   // blue
            : Color.FromArgb("#E91E63");  // pink

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
                await Shell.Current.DisplayAlertAsync("Error",
                    $"Invalid PESEL length: {ex.Length} characters (11 required).", "OK");
            }
            catch (InvalidPeselMonthException ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
            catch (ArgumentException ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Unexpected error", ex.Message, "OK");
            }
        }
    }
}
