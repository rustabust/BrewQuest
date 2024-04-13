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
        public static void MergeCompetitionFiles(string masterFile, List<string> filesToMerge)
        {
            // load master file
            List<Competition> masterList = CommonFunctions.DeserializeFromJsonFile<List<Competition>>(masterFile);

            // review the files
            foreach (var fileToMerge in filesToMerge)
            {
                var fileComps = CommonFunctions.DeserializeFromJsonFile<List<Competition>>(fileToMerge);

                // foreach the comps or query?
                foreach(var fileComp in fileComps)
                {
                    // how to compare?
                    masterList.FirstOrDefault(a => a.)
                }

                // load file
                // compare objects to those in the master file
                // skip exact matches
                // update any details
                // dont delete any
            }
             
            // save master list


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

        protected static void SyncCompetitionsJson(List<BrewQuest.Models.Competition> competitions, string fileName)
        {
            CommonFunctions.AddObjectsToFile(competitions, fileName);
        }

        
    }
}
