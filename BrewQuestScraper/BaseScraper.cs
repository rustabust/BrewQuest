using BrewQuest.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BrewQuestScraper.Models;

namespace BrewQuestScraper
{
    public abstract class BaseScraper
    {
        public static async Task<CompetitionSiteInfo?> ScrapeCompetitionSite(string competitionUrl)
        {
            try
            {
                CompetitionSiteInfo competitionSiteInfo = new CompetitionSiteInfo();

                if (string.IsNullOrEmpty(competitionUrl))
                {
                    Console.WriteLine("competitionUrl is null or empty");
                    return null;
                }

                HtmlDocument? doc = await getHtmlDocument(competitionUrl);
                if (doc == null)
                {
                    Console.WriteLine("Error getting html document for competition site " + competitionUrl);
                    return null;
                }
                const string KEY_PHRASE = "Entry registrations accepted";
                HtmlNode entryInfoNode = doc.DocumentNode.SelectSingleNode("//div[contains(text(), '" + KEY_PHRASE + "')]");

                // there might be some way to check the site content to see if its a BCOE&M site
                // for now, just check if the entry info node is null
                if (entryInfoNode == null)
                {
                    // check to see if its closed. sometimes the pull the dates if the limit has been reached
                    const string CLOSED_PHRASE = "The competition entry limit has been reached.";
                    HtmlNode closedNode = doc.DocumentNode.SelectSingleNode("//span[contains(text(), '" + CLOSED_PHRASE + "')]");
                    if (closedNode != null)
                    {
                        competitionSiteInfo.RegistrationClosed = true;
                        return null;
                    }
                    else
                    {
                        Console.WriteLine("competition registration info not found on competition site " + competitionUrl);
                        return null;
                    }
                }
                string entryInfo = entryInfoNode.InnerText;

                entryInfo = entryInfo.ToLower();
               entryInfo = entryInfo.RemoveEverythingBeforeTag(KEY_PHRASE.ToLower(), false);

                entryInfo = entryInfo.Replace(",", "").ToUpper();
                string[] dates = entryInfo.Split("THROUGH", StringSplitOptions.RemoveEmptyEntries);

                if (dates.Length == 2)
                {
                    string startDate = dates[0].Trim();
                    string endDate = dates[1].Trim();

                    cleanupDateString(ref startDate);
                    cleanupDateString(ref endDate);

                    bool isInternationalFormat = DateParsing.IsInternationalDateFormat(new string[] { startDate, endDate });
                    competitionSiteInfo.RegistrationOpenDate = DateParsing.GetDateTimeFromString(startDate, isInternationalFormat);
                    competitionSiteInfo.RegistrationCloseDate = DateParsing.GetDateTimeFromString(endDate, isInternationalFormat);

                    Console.WriteLine("Entry registration start date: " + startDate);
                    Console.WriteLine("Entry registration end date: " + endDate);
                }

                return competitionSiteInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error scraping competition site (url=" + competitionUrl + ") Exception Message : " + ex.Message);
                return null;
            }
        }

       

        private static void cleanupDateString(ref string dateString)
        {
            dateString = dateString.RemoveEverythingAfterTag(DateTime.Now.Year.ToString(), true);
            dateString = dateString.RemoveEverythingAfterTag((DateTime.Now.Year + 1).ToString(), true);
            dateString = dateString.RemoveEverythingAfterTag((DateTime.Now.Year -1).ToString(), true);
        }   

        protected static async Task<HtmlDocument?> getHtmlDocument(string url)
        {
            try
            {
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                return htmlDocument;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting html document : " + ex.Message);
                return null;
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
    }
}
