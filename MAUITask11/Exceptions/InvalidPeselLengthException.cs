namespace MAUITask11.Exceptions
{
    public class InvalidPeselLengthException : Exception
    {
        public int Length { get; private set; }

        public InvalidPeselLengthException(int length)
            : base($"Nieprawidłowa długość PESEL: {length}. PESEL powinien składać się z 11 cyfr.")
        {
            Length = length;
        }
    }
}
