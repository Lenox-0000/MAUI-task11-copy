namespace MAUITask11.Exceptions
{
    public class InvalidPeselMonthException : Exception
    {
        public int RawMonth { get; private set; }

        public InvalidPeselMonthException(int rawMonth)
            : base($"Invalid month in PESEL: {rawMonth}.")
        {
            RawMonth = rawMonth;
        }
    }
}
