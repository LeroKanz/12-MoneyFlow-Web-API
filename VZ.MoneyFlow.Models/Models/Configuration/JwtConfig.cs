namespace VZ.MoneyFlow.Models.Models.Configuration
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public int DurationInMinutesForT { get; set; }
        public int DurationInMinutesForRT { get; set; }
    }
}
