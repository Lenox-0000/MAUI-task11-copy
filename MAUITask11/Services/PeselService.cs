using MAUITask11.Exceptions;
using MAUITask11.Models;

namespace MAUITask11.Services
{
    public class PeselService
    {
        private static readonly int[] Weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        private readonly Random _random = new();

        /// <summary>
        /// Decodes an 11-digit PESEL string into a Human object.
        /// Throws InvalidPeselLengthException, InvalidPeselMonthException, or ArgumentException on bad input.
        /// </summary>
        public Human Decode(string pesel)
        {
            if (pesel.Length != 11)
                throw new InvalidPeselLengthException(pesel.Length);

            if (!pesel.All(char.IsDigit))
                throw new ArgumentException("PESEL może zawierać tylko cyfry.");

            int[] d = pesel.Select(c => c - '0').ToArray();
            int rr = d[0] * 10 + d[1];
            int mm = d[2] * 10 + d[3];
            int dd = d[4] * 10 + d[5];

            int year, month;
            if      (mm >= 81 && mm <= 92) { year = 1800 + rr; month = mm - 80; }
            else if (mm >= 21 && mm <= 32) { year = 2000 + rr; month = mm - 20; }
            else if (mm >= 41 && mm <= 52) { year = 2100 + rr; month = mm - 40; }
            else if (mm >= 61 && mm <= 72) { year = 2200 + rr; month = mm - 60; }
            else if (mm >= 1  && mm <= 12) { year = 1900 + rr; month = mm; }
            else throw new InvalidPeselMonthException(mm);

            DateTime birthDate;
            try { birthDate = new DateTime(year, month, dd); }
            catch { throw new ArgumentException($"Nieprawidłowa data w PESEL: {dd:D2}.{month:D2}.{year}."); }

            return new Human
            {
                BirthDate = birthDate,
                Gender    = d[9] % 2 == 0 ? Gender.Woman : Gender.Man,
                IsValid   = ValidateChecksum(d)
            };
        }

        /// <summary>
        /// Encodes a birth date and gender into a valid 11-digit PESEL string.
        /// </summary>
        public string Encode(DateTime date, Gender gender)
        {
            string part = EncodeBirthDate(date) + EncodePpp(gender);
            int[] digits = part.Select(c => c - '0').ToArray();
            return part + CalcCheck(digits);
        }

        // --- private helpers ---

        private static string EncodeBirthDate(DateTime d)
        {
            int mm = d.Year switch
            {
                >= 2200 => d.Month + 60,
                >= 2100 => d.Month + 40,
                >= 2000 => d.Month + 20,
                >= 1900 => d.Month,
                _       => d.Month + 80
            };
            return $"{d.Year % 100:D2}{mm:D2}{d.Day:D2}";
        }

        private string EncodePpp(Gender gender)
        {
            int serial = _random.Next(0, 1000);
            int genderDigit = _random.Next(0, 5) * 2 + (int)gender;
            return $"{serial:D3}{genderDigit}";
        }

        private static bool ValidateChecksum(int[] d)
        {
            int sum = 0;
            for (int i = 0; i < 10; i++)
                sum += (d[i] * Weights[i]) % 10;
            int expected = (10 - (sum % 10)) % 10;
            return expected == d[10];
        }

        private static int CalcCheck(int[] d10)
        {
            int sum = 0;
            for (int i = 0; i < 10; i++)
                sum += (d10[i] * Weights[i]) % 10;
            return (10 - (sum % 10)) % 10;
        }
    }
}
