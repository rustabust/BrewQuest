using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Globalization;
using BrewQuest.Models;

namespace BrewQuestScraper
{
    public class ScrapeFunctions
    {
        public static List<CompetitionSummary> PullBasicEventInfoFromLive(string saveToFileName)
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
                SerializeToJsonFile(compInfos, saveToFileName);
            }
            return compInfos;
        }

        public static async Task<BrewQuest.Models.Competition> GetAHACompetitionInfoFromDetailsPage(string url)
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            string ogDescriptionString = getMetaTagFromHtmlDocument(htmlDocument, "og:description");
            string title = getMetaTagFromHtmlDocument(htmlDocument, "og:title");

            var ahaCompetitionInfo = parseCompetitionInfo(ogDescriptionString);
            ahaCompetitionInfo.Name = title;

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
                LocationCountry = ahaCompInfo.Country
            };
            return competition;
        }

        private static string getMetaTagFromHtmlDocument(HtmlDocument htmlDocument, string propertyName)
        {
            var metaTags = htmlDocument.DocumentNode.SelectNodes("//meta[@property='" + propertyName + "']");
            if (metaTags != null && metaTags.Count > 0)
            {
                var ogDescription = metaTags[0].Attributes["content"].Value;
                return ogDescription;
            }
            throw new Exception("no Meta Tag found in document with name " + propertyName);
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
                Country = country
            };
        }

        //public static EventDetails ParseEventDetails(string content)
        //{
        //    EventDetails eventDetails = new EventDetails();

        //    // Extracting entry fee and currency
        //    var entryFeeMatch = Regex.Match(content, @"Entry Fee: (\d+(\.\d+)?)(\s+([A-Za-z]+))?");
        //    if (entryFeeMatch.Success)
        //    {
        //        eventDetails.EntryFee = decimal.Parse(entryFeeMatch.Groups[1].Value);
        //        eventDetails.Currency = entryFeeMatch.Groups[4].Success ? entryFeeMatch.Groups[4].Value : "USD";
        //    }

        //    // Extracting entry deadline
        //    var entryDeadlineMatch = Regex.Match(content, @"Entry Deadline: (\d{2}/\d{2}/\d{4})");
        //    if (entryDeadlineMatch.Success)
        //    {
        //        eventDetails.EntryDeadline = DateTime.Parse(entryDeadlineMatch.Groups[1].Value);
        //    }

        //    // Extracting competition date
        //    var competitionDateMatch = Regex.Match(content, @"Competition Date: (\d{2}/\d{2}/\d{4})");
        //    if (competitionDateMatch.Success)
        //    {
        //        eventDetails.CompetitionDate = DateTime.Parse(competitionDateMatch.Groups[1].Value);
        //    }

        //    // Extracting phone number
        //    var phoneNumberMatch = Regex.Match(content, @"Phone Number: (\(\d{3}\) \d{3}-\d{4})");
        //    if (phoneNumberMatch.Success)
        //    {
        //        eventDetails.PhoneNumber = phoneNumberMatch.Groups[1].Value;
        //    }

        //    // Extracting location
        //    var locationMatch = Regex.Match(content, @"Location: (.+)$");
        //    if (locationMatch.Success)
        //    {
        //        eventDetails.Location = locationMatch.Groups[1].Value;
        //    }

        //    return eventDetails;
        //}

        /// <summary>
        /// Serialize a C# object to JSON and write to a file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        public static void SerializeToJsonFile<T>(T obj, string fileName)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }

        /// <summary>
        /// Deserialize JSON from a file back to a C# object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeserializeFromJsonFile<T>(string fileName)
        {
            string json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
