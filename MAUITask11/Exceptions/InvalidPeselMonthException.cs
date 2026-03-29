namespace MAUITask11.Exceptions
{
    public class InvalidPeselMonthException : Exception
    {
        public int RawMonth { get; private set; }

        public InvalidPeselMonthException(int rawMonth)
            : base($"Nieprawidłowy miesiąc w PESEL: {rawMonth}.")
        {
            RawMonth = rawMonth;
        }
    }
}
