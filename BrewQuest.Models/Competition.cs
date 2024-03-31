namespace BrewQuest.Models
{
    public class Competition
    {
        public string CompetitionName { get; set; }
        public string Host { get; set; }
        public DateTime? EntryWindowOpen { get; set; }
        public DateTime? EntryWindowClose { get; set; }
        public DateTime? FinalJudgingDate { get; set; }
        public int? EntryLimit { get; set; }
        public string EntryFee { get; set; }
        public string Status { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationCountry { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingWindowOpen { get; set; }
        public string ShippingWindowClose { get; set; }
        public string CompetitionUrl { get; set; }
        public string HostUrl { get; set; }
    }
}
