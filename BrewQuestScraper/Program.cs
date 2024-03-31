using BrewQuest.Models;
using BrewQuestScraper;
using HtmlAgilityPack;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Text.RegularExpressions;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Bout to start the BrewQuestScraper 1.0!");

const bool HIT_LIVE_SUMMARY = false;
const bool HIT_LIVE_DETAILS = false;
 List<CompetitionSummary> compInfos = null;
const string BASIC_INFO_FILE = "C:\\Users\\rusty\\OneDrive\\Documents\\GitHub\\BrewQuest\\BrewQuestScraper\\Data\\aha_comps_basic_info_list_for_test.json";
const string COMP_INFOS_FILE = "C:\\Users\\rusty\\OneDrive\\Documents\\GitHub\\BrewQuest\\BrewQuestScraper\\Data\\aha_scrape_comp_infos.json";
if (HIT_LIVE_SUMMARY)
{
    compInfos = ScrapeFunctions.PullBasicEventInfoFromLive(BASIC_INFO_FILE);
}else
{
    compInfos = ScrapeFunctions.DeserializeFromJsonFile< List<CompetitionSummary>>(BASIC_INFO_FILE);
}


List<Competition> competitions = new List<Competition>();
if (HIT_LIVE_DETAILS)
{
    // crawl the details page, parse, and convert to common objects
    for (int i = 0; i < 5; i++)
    {
        Competition competition = await ScrapeFunctions.GetAHACompetitionInfoFromDetailsPage(compInfos[i].DetailsUrl);
        competitions.Add(competition);
    }

    // save objects listing to file for testing
    ScrapeFunctions.SerializeToJsonFile(competitions, COMP_INFOS_FILE);
}

// load from file for further testing/processing...
competitions = ScrapeFunctions.DeserializeFromJsonFile<List<Competition>>(COMP_INFOS_FILE);

// save somewhere to the cloud??
// what else next?

Console.WriteLine("scraper finished.");



