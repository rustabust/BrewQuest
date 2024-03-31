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
        //public abstract void Scrape();

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

        protected static void SyncCompetitionsJson(List<BrewQuest.Models.Competition> competitions, string fileName)
        {
            CommonFunctions.AddObjectsToFile(competitions, fileName);
        }

        
    }
}
