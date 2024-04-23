using BrewQuest.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BrewQuestScraper
{
    public abstract class BaseScraper
    {
        protected static async Task<CompetitionSiteInfo?> ScrapeCompetitionSite(string competitionUrl)
        {
            CompetitionSiteInfo competitionSiteInfo = new CompetitionSiteInfo();

            if (string.IsNullOrEmpty(competitionUrl))
            {
                Console.WriteLine("competitionUrl is null or empty");
                return null;
            }

            HtmlDocument doc = await getHtmlDocument(competitionUrl);

            //string html = "<html><body><div>Entry registrations accepted 03/02/2024 12:00 PM, EST through 04/19/2024 6:45 PM, EDT.</div></body></html>";

            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(html);

            HtmlNode entryInfoNode = doc.DocumentNode.SelectSingleNode("//div[contains(text(), 'Entry registrations accepted')]");

            // there might be some way to check the site content to see if its a BCOE&M site
            // for now, just check if the entry info node is null
            if (entryInfoNode == null)
            {
                Console.WriteLine("competition registration info not found on competition site " + competitionUrl);
                return null;
            }
            string entryInfo = entryInfoNode.InnerText;

            entryInfo = entryInfo.ToLower();
            entryInfo = entryInfo.Replace("entry registrations accepted","").Trim();


            //int startIndex = entryInfo.IndexOf("accepted") + "accepted".Length;
            //int endIndex = entryInfo.IndexOf("through");

            //string registrationPeriod = entryInfo.Substring(startIndex, endIndex - startIndex).Trim();

            //string[] dates = registrationPeriod.Split("through", StringSplitOptions.RemoveEmptyEntries);

            entryInfo = entryInfo.Replace(",", "").ToUpper();
            string[] dates = entryInfo.Split("THROUGH", StringSplitOptions.RemoveEmptyEntries);


            if (dates.Length == 2)
            {
                string startDate = dates[0].Trim();
                string endDate = dates[1].Trim();

              cleanupDateString(ref startDate);
                cleanupDateString(ref endDate);


                competitionSiteInfo.RegistrationOpenDate = DateTime.Parse(startDate);
                competitionSiteInfo.RegistrationCloseDate = DateTime.Parse(endDate);

                Console.WriteLine("Entry registration start date: " + startDate);
                Console.WriteLine("Entry registration end date: " + endDate);
            }

            return competitionSiteInfo;
        }

        private static void cleanupDateString(ref string dateString)
        {
            // remove everything after am
            int amIndex = dateString.IndexOf("AM");
            if (amIndex > 0)
            {
                dateString = dateString.Substring(0, amIndex + 2);
            }

            // remove everything after pm       
            int pmIndex = dateString.IndexOf("PM");
            if (pmIndex > 0)
            {
                dateString = dateString.Substring(0, pmIndex + 2);
            }
        }   
        protected static async Task<HtmlDocument> getHtmlDocument(string url)
        {
            try
            {
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                return htmlDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected static string getMetaTagFromHtmlDocument(HtmlDocument htmlDocument, string propertyName)
        {
            var metaTags = htmlDocument.DocumentNode.SelectNodes("//meta[@property='" + propertyName + "']");
            if (metaTags != null && metaTags.Count > 0)
            {
                var ogDescription = metaTags[0].Attributes["content"].Value;
                return ogDescription;
            }
            throw new Exception("no Meta Tag found in document with name " + propertyName);
        }

        //protected static void SyncCompetitionsJson(List<BrewQuest.Models.Competition> competitions, string fileName)
        //{
        //    CommonFunctions.AddObjectsToFile(competitions, fileName);
        //}


    }

    public class CompetitionSiteInfo
    {
        public DateTime? RegistrationOpenDate { get; set; }
        public DateTime? RegistrationCloseDate { get; set; }
    }
}
