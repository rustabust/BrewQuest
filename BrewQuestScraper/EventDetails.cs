using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewQuestScraper
{
    public class AHACompetitionInfo
    {
        public string Name { get; set; }
        public string EntryFee { get; set; }
        public DateTime EntryDeadline { get; set; }
        public DateTime CompetitionDate { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
        //public class EventDetails
        // {
        //     public decimal EntryFee { get; set; }
        //     public string Currency { get; set; }
        //     public DateTime EntryDeadline { get; set; }
        //     public DateTime CompetitionDate { get; set; }
        //     public string PhoneNumber { get; set; }
        //     public string Location { get; set; }
        // }
    }
