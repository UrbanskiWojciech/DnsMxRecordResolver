namespace DnsMxRecordResolver
{
    public static class WellKnownDnsResponsePositions
    {
        // ref: https://tools.ietf.org/html/rfc1035
        public const int StatusPosition = 3;
        public const int AnswersCountPosition = 7;
        public const int AnswerPreferencePosition = 13;
        public const int AnswerHeadersCount = 18;
    }
}