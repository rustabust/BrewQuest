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
 List<CompBasicInfo> compInfos = null;
const string BASIC_INFO_FILE = "C:\\Users\\rusty\\OneDrive\\Documents\\GitHub\\BrewQuest\\Data\\aha_comps_basic_info_list_for_test.json";
const string OG_STRINGS_FILE = "C:\\Users\\rusty\\OneDrive\\Documents\\GitHub\\BrewQuest\\Data\\aha_comps_og_strings_for_testing_parse.json";
if (HIT_LIVE_SUMMARY)
{
    compInfos = ScrapeFunctions.PullBasicEventInfoFromLive(BASIC_INFO_FILE);
}else
{
    compInfos = ScrapeFunctions.DeserializeFromJsonFile< List<CompBasicInfo>>(BASIC_INFO_FILE);
}

List<string> ogstrings = new List<string>();
if (HIT_LIVE_DETAILS)
{
    // testing parseeventdetails
    //for(int i=0;i<20;i++)
    //{
    //    string og = await ScrapeFunctions.GetOgDescription(compInfos[i].CompDetailsUrl);
    //    ogstrings.Add(og);
    //}
    //ScrapeFunctions.SerializeToJsonFile(ogstrings, OG_STRINGS_FILE);
}
else
{
    ogstrings = ScrapeFunctions.DeserializeFromJsonFile<List<string>>(OG_STRINGS_FILE);
}

 need to preserve the competition name from above or else scrape it from the details page
// parse details page info into eventDetails
// convert into common Competition objects
var competitions = new List<Competition>();
for (int i = 0; i < 5; i++)
{
    var eventDetails = ScrapeFunctions.ParseCompetitionInfo(ogstrings[i]);
    Console.WriteLine("parsed event details : ");
    Console.WriteLine("  entry fee : " + eventDetails.EntryFee);
    Console.WriteLine("  competition date : " + eventDetails.CompetitionDate);

    var competition = new Competition
    {
        CompetitionName = eventDetails.Name,
        EntryFee = eventDetails.EntryFee,
        EntryWindowClose = eventDetails.EntryDeadline,
        FinalJudgingDate = eventDetails.CompetitionDate,
        //phone number
        LocationCity = eventDetails.City,
        LocationState = eventDetails.State,
        LocationCountry = eventDetails.Country
    };
}


