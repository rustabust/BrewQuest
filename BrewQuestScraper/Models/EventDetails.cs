using BrewQuest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewQuestScraper.Models
{
    public class AHACompetitionInfo
    {
        public string Name { get; set; }
        public string EntryFee { get; set; }
        public DateTime EntryDeadline { get; set; }
        public DateTime CompetitionDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

        /// <summary>
        /// this is the url we gathered this object details from
        /// </summary>
        public string? SourceUrl { get; set; }

        /// <summary>
        /// this is the url to the actual competition website
        /// </summary>
        public string? CompetitionUrl { get; set; }
        public CompetitionDataSourceTypes CompetitionDataSourceType
        {
            get
            {
                return CompetitionDataSourceTypes.AHA;
            }
        }
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
