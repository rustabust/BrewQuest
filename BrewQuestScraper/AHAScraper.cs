using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Globalization;
using BrewQuest.Models;

namespace BrewQuestScraper
{
    public class AHAScraper : BaseScraper
    {
        public static async Task<bool> Scrape()
        {
            const bool HIT_LIVE_DETAILS = true;

            List<Competition> competitions = new List<Competition>();
            if (HIT_LIVE_DETAILS)
            {
                var compSummaries = pullCompetitionSummaries();

                // crawl the details page, parse, and convert to common objects
                const int LIMIT_FOR_TESTING = -1; //-1 for all
                int counter = 0;
                Console.WriteLine("pulling competition infos from AHA details pages...");
                int compSummaryTotal = compSummaries.Count;
                foreach (var compSummary in compSummaries)
                {
                    if (LIMIT_FOR_TESTING != -1 && counter >= LIMIT_FOR_TESTING)
                    {
                        break;
                    }   

                    counter++;
                    Console.WriteLine(counter + "/" + compSummaryTotal + " - pulling info for competition name=" + compSummary.Name + " url:" + compSummary.DetailsUrl);
                    Competition competition = await getAHACompetitionInfoFromDetailsPage(compSummary.DetailsUrl);
                    competitions.Add(competition);
                }

                // save/sync objects listing to file
                int compsAdded = CommonFunctions.SyncCompetitionsToFile(competitions);
                Console.WriteLine("Added " + compsAdded + " competitions to the master list.");
            }

            // load from file for further testing/processing...
           // competitions = CommonFunctions.DeserializeFromJsonFile<List<Competition>>(SCRAPE_OUTPUT_FILE_AHA);

            // save somewhere to the cloud??
            // what else next?

            return true;
        }

        /// <summary>
        /// this function currently has some issue that it doesnt properly pull everything unless debugged / stepped through...
        /// </summary>
        /// <returns></returns>
        private static List<CompetitionSummary> pullBasicEventInfoFromLive(string saveToFileName)
        {
            var compInfos = new List<CompetitionSummary>();

            // Set up ChromeOptions to run in headless mode (optional)
            var options = new ChromeOptions();
            options.AddArgument("--headless");

            // Initialize ChromeDriver with options
            using (var driver = new ChromeDriver(options))
            {
                // Navigate to the webpage
                driver.Navigate().GoToUrl("https://www.homebrewersassociation.org/aha-events/aha-bjcp-sanctioned-competition/");

                // Wait for the page to load completely (adjust the timeout as needed)
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);

                // Perform any necessary interactions (clicks, inputs, etc.) to trigger dynamic content loading
                // For example, you may need to click on a button or scroll down the page

                // Once the dynamic content is loaded, you can scrape the data
                // Find elements using Selenium's FindElement or FindElements methods
                var parentDiv = driver.FindElement(By.XPath("//div[@id='event-list']"));

                // Find all <article> elements within the parent <div>
                var articleElements = parentDiv.FindElements(By.XPath(".//article"));

                // Iterate through each <article> element
                foreach (var article in articleElements)
                {
                    // Extract data from the <article> element
                    var articleText = article.Text;

                    // Extract hyperlink text and URL
                    var hyperlinkText = article.FindElement(By.XPath(".//a")).Text;
                    var hyperlinkUrl = article.FindElement(By.XPath(".//a")).GetAttribute("href");

                    var compInfo = new CompetitionSummary { Name = hyperlinkText, DetailsUrl = hyperlinkUrl };
                    compInfos.Add(compInfo);

                    // Print or process the extracted data
                    //Console.WriteLine("compname=" + compInfo.CompName + "        compDetailsUrl=" + compInfo.CompDetailsUrl);
                }

                // save to json file in case we want to just use it later for testing purpose so as to not get blocked 
                CommonFunctions.SerializeToJsonFile(compInfos, saveToFileName);
            }
            return compInfos;
        }
       
        private static List<CompetitionSummary> pullCompetitionSummaries()
        {
            List<CompetitionSummary>? compSummaries = null;

            const bool HIT_LIVE_SUMMARY = true;
            const string BASIC_INFO_FILE = "C:\\Users\\rusty\\OneDrive\\Documents\\GitHub\\BrewQuest\\BrewQuestScraper\\Data\\aha_comps_basic_info_list_for_test.json";

            if (HIT_LIVE_SUMMARY)
            {
                compSummaries = pullBasicEventInfoFromLive(BASIC_INFO_FILE);
            }
            else
            {
                compSummaries = CommonFunctions.DeserializeFromJsonFile<List<CompetitionSummary>>(BASIC_INFO_FILE);
            }

            return compSummaries;
        }

        private static async Task<BrewQuest.Models.Competition> getAHACompetitionInfoFromDetailsPage(string url)
        {
            HtmlDocument htmlDocument = await getHtmlDocument(url);
           
            string ogDescriptionString = getMetaTagFromHtmlDocument(htmlDocument, "og:description");
            string title = getMetaTagFromHtmlDocument(htmlDocument, "og:title");

            var ahaCompetitionInfo = parseCompetitionInfo(ogDescriptionString);
            ahaCompetitionInfo.Name = title;
            ahaCompetitionInfo.SourceUrl = url;

            var result = getCompetitionFromAhaCompInfo(ahaCompetitionInfo);

            return result;
        }

        private static Competition getCompetitionFromAhaCompInfo(AHACompetitionInfo ahaCompInfo)
        {
            var competition = new Competition
            {
                CompetitionName = ahaCompInfo.Name,
                EntryFee = ahaCompInfo.EntryFee,
                EntryWindowClose = ahaCompInfo.EntryDeadline,
                FinalJudgingDate = ahaCompInfo.CompetitionDate,
                //phone number
                LocationCity = ahaCompInfo.City,
                LocationState = ahaCompInfo.State,
                LocationCountry = ahaCompInfo.Country,
                CompetitionDataSourceType = ahaCompInfo.CompetitionDataSourceType,
                CompetitionUrl = ahaCompInfo.SourceUrl
            };
            return competition;
        }

        private static AHACompetitionInfo parseCompetitionInfo(string ogDescriptionContentString)
        {
            var infoArray = ogDescriptionContentString.Split(new string[] { "Entry Fee: ", " Entry Deadline: ", " Competition Date: ", " Phone Number: ", " Location: " }, StringSplitOptions.RemoveEmptyEntries);

            if (infoArray.Length < 5)
            {
                throw new ArgumentException("Invalid information string.");
            }

            var entryFee = infoArray[0].Trim();
            var entryDeadline = DateTime.ParseExact(infoArray[1].Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var competitionDate = DateTime.ParseExact(infoArray[2].Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var phoneNumber = infoArray[3].Trim();
            var location = infoArray[4].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var city = location[0].Trim();
            var state = location.Length > 1 ? location[1].Trim() : string.Empty;
            var country = location.Length > 2 ? location[2].Trim() : string.Empty;

            return new AHACompetitionInfo
            {
                EntryFee = entryFee,
                EntryDeadline = entryDeadline,
                CompetitionDate = competitionDate,
                PhoneNumber = phoneNumber,
                City = city,
                State = state,
                Country = country,
            };
        }
    }
}
