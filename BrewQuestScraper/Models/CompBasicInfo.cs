using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewQuestScraper.Models
{
    /// <summary>
    /// class gives a brief summary of a competition. basically a name and url for more info.
    /// </summary>
    public class CompetitionSummary
    {
        public string Name { get; set; }
        public string DetailsUrl { get; set; }
    }
}
