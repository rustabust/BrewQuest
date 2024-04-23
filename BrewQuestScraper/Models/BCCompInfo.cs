using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewQuestScraper.Models
{
    public class BCCompInfo
    {
        public string? CompetitionName { get; set; }
        public string? HostClub { get; set; }
        public DateTime EntryStartDate { get; set; }
        public DateTime EntryEndDate { get; set; }
        public DateTime FinalJudgingDate { get; set; }
        public int Entries { get; set; }
        public decimal Fee { get; set; }
        public string? Status { get; set; }
        public string? CompetitionUrl { get; set; }
    }
}
