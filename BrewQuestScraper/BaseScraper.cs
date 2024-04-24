﻿using BrewQuest.Models;
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
                    Console.WriteLine("competition registration info not found on competition site " + competitionUrl);
                    return null;
                }
                string entryInfo = entryInfoNode.InnerText;

                entryInfo = entryInfo.ToLower();
                //entryInfo = entryInfo.Replace("entry registrations accepted", "").Trim();
               // entryInfo = entryInfo.Split(KEY_PHRASE.ToLower(), StringSplitOptions.RemoveEmptyEntries)[1];
               removeEverythingBeforeTag(ref entryInfo, KEY_PHRASE.ToLower(), false);

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
            catch (Exception ex)
            {
                Console.WriteLine("Error scraping competition site (url=" + competitionUrl + ") Exception Message : " + ex.Message);
                return null;
            }
        }

        private static void cleanupDateString(ref string dateString)
        {
            removeEverythingAfterTag(ref dateString, DateTime.Now.Year.ToString(), true);
            removeEverythingAfterTag(ref dateString, (DateTime.Now.Year + 1).ToString(), true);
            removeEverythingAfterTag(ref dateString, (DateTime.Now.Year -1).ToString(), true);

            //removeEverythingAfterTag(ref dateString, "AM");
            //removeEverythingAfterTag(ref dateString, "PM");

            //// remove everything after am
            //int amIndex = dateString.IndexOf("AM");
            //if (amIndex > 0)
            //{
            //    dateString = dateString.Substring(0, amIndex + 2);
            //}

            //// remove everything after pm       
            //int pmIndex = dateString.IndexOf("PM");
            //if (pmIndex > 0)
            //{
            //    dateString = dateString.Substring(0, pmIndex + 2);
            //}
        }   

        private static void removeEverythingBeforeTag(ref string html, string tag, bool includeTag)
        {
            int tagIndex = html.IndexOf(tag);
            if (tagIndex >= 0)
            {
                if (includeTag)
                {
                    html = html.Substring(tagIndex, html.Length - tagIndex);
                }
                else
                {
                    html = html.Substring(tagIndex + tag.Length, html.Length - tagIndex - tag.Length);
                }
                //html = html.Substring(tagIndex + tag.Length, html.Length - tagIndex - tag.Length);
            }
        }   
        private static void removeEverythingAfterTag(ref string html, string tag, bool includeTag)
        {
            int tagIndex = html.IndexOf(tag);
            if (tagIndex > 0)
            {
                var length = tagIndex;
                if (includeTag)
                {
                    length += tag.Length;
                }
                html = html.Substring(0, length);
            }
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
