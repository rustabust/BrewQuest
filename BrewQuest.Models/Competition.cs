namespace BrewQuest.Models
{
    public class Competition
    {
        public string? CompetitionName { get; set; }
        public string? Host { get; set; }
        public DateTime? RegistrationWindowOpen { get; set; }
        public DateTime? RegistrationWindowClose { get; set; }
        public DateTime? EntryWindowOpen { get; set; }
        public DateTime? EntryWindowClose { get; set; }
        public DateTime? FinalJudgingDate { get; set; }
        public int? EntryLimit { get; set; }
        public string? EntryFee { get; set; }
        public string? Status { get; set; }
        public string? LocationCity { get; set; }
        public string? LocationState { get; set; }
        public string? LocationCountry { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingWindowOpen { get; set; }
        public string? ShippingWindowClose { get; set; }
        public string? CompetitionUrl { get; set; }
        public string? HostUrl { get; set; }
        public CompetitionDataSourceTypes CompetitionDataSourceType { get; set; }
        public string? CompetitionDataSourceUrl { get; set; }

        public override bool Equals(object? obj)
        {
            Competition otherComp = obj as Competition;
            if (otherComp != null)
            {
                if (this.CompetitionName.ToLower() == otherComp.CompetitionName)
                {
                    if (this.LocationCity.ToLower() == otherComp.LocationCity.ToLower())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.CompetitionName, this.LocationCity);
        }
    }

    public enum CompetitionDataSourceTypes
    {
        Unknown =0,
        AHA = 1,
        BrewCompetitions = 2
    }
}
