using BrewQuest.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewQuestScraper
{
    public class BCScraper : BaseScraper
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

        public static async Task<bool> Scrape()
        {
            const string BREW_COMPETITIONS_DOTCOM_SCRAPE_URL = "https://www.brewingcompetitions.com/";
            var doc = await getHtmlDocument(BREW_COMPETITIONS_DOTCOM_SCRAPE_URL);
            var bcCompInfos = ParseHtmlTable(doc);

            var competitions = bcCompInfos.Select(a => new Competition
            {
                CompetitionName = a.CompetitionName,
                EntryWindowOpen = a.EntryStartDate,
                EntryWindowClose = a.EntryEndDate,
                FinalJudgingDate = a.FinalJudgingDate,
                EntryFee = a.Fee.ToString(),
                Status = a.Status,
                CompetitionUrl = a.CompetitionUrl,

            }).ToList();

            // go to the individual sites to get the rest of the info
            {

                var compsWithSites = competitions.Where(a => a.CompetitionUrl != null).ToList();
                foreach (var competition in compsWithSites)
                {
                    var siteScrapeInfo = await ScrapeCompetitionSite(competition.CompetitionUrl);
                    if (siteScrapeInfo != null)
                    {
                        competition.RegistrationWindowOpen = siteScrapeInfo.RegistrationOpenDate;
                        competition.RegistrationWindowClose = siteScrapeInfo.RegistrationCloseDate;
                    }
                }
            }

            int compsAdded = CommonFunctions.SyncCompetitionsToFile(competitions);
            Console.WriteLine("Added " + compsAdded + " competitions to the master list.");

            return true;
        }

        public static List<BCCompInfo> ParseHtmlTable(HtmlDocument document)
        {
            var comps = new List<BCCompInfo>();
            var table = document.GetElementbyId("hosted-comps");
            if (table != null)
            {
                var tbody = table.SelectSingleNode("tbody");
                if (tbody != null)
                {
                    foreach (HtmlNode row in tbody.SelectNodes("tr"))
                    {
                        var cells = row.SelectNodes("td");
                        if (cells != null && cells.Count >= 7)
                        {
                            var comp = new BCCompInfo();
                            comp.CompetitionName = cells[0].InnerText.Trim();

                            // try to grab the url
                            var aTag = cells[0].SelectSingleNode("a");
                            if (aTag != null)
                            {
                                comp.CompetitionUrl = aTag.Attributes["href"].Value;
                            }

                            comp.HostClub = cells[1].InnerText.Trim();

                            var sEntryRange = cells[2].InnerText.Trim();
                            var arrEntryRange = sEntryRange.Split(" - ");

                            if (DateTime.TryParse(arrEntryRange[0], out DateTime startDate))
                                comp.EntryStartDate = startDate;
                            if (DateTime.TryParse(arrEntryRange[1], out DateTime endDate))
                                comp.EntryEndDate = endDate;

                            //if (DateTime.TryParse(cells[2].InnerText.Trim(), out DateTime startDate))
                            //    comp.StartDate = startDate;
                            //if (DateTime.TryParse(cells[3].InnerText.Trim(), out DateTime endDate))
                            //    comp.EndDate = endDate;


                            if (DateTime.TryParse(cells[3].InnerText.Trim(), out DateTime finalJudgingDate))
                                comp.FinalJudgingDate = finalJudgingDate;

                            if (int.TryParse(cells[4].InnerText.Trim().Split('/')[0], out int entries))
                                comp.Entries = entries;
                            if (decimal.TryParse(cells[5].InnerText.Trim('$', ' ', 'U', 'S', 'D'), out decimal fee))
                                comp.Fee = fee;
                            comp.Status = cells[6].InnerText.Trim();
                            comps.Add(comp);
                        }
                    }
                }
            }
            return comps;
        }
    }
}
