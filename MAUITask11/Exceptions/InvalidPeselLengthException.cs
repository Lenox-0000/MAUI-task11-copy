namespace MAUITask11.Exceptions
{
    public class InvalidPeselLengthException : Exception
    {
        public int Length { get; private set; }

        public InvalidPeselLengthException(int length)
            : base($"Invalid PESEL length: {length}. PESEL should consist of 11 digits.")
        {
            Length = length;
        }
    }
}
