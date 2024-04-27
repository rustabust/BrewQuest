using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewQuestScraper.Models
{
    public class CompetitionSiteInfo
    {
        public DateTime? RegistrationOpenDate { get; set; }
        public DateTime? RegistrationCloseDate { get; set; }
        public bool? RegistrationClosed { get; set; }

    }
}
