using BrewQuestScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewQuest.Models.Tests
{
    [TestClass]
    public class BrewQuestScraperTests
    {
        [TestMethod]
        public async Task test_scrape_registration_dates()
        {
            try
            {
                //string url = "https://comp.michiganbeercup.com/";
                string url = "https://motownmash.brewingcompetitions.com";
                var siteInfo = await AHAScraper.ScrapeCompetitionSite(url);
                Assert.IsNotNull(siteInfo);
                Assert.IsNotNull(siteInfo.RegistrationOpenDate);
                Assert.IsNotNull(siteInfo.RegistrationCloseDate);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        //[TestCleanup]
        //public void TestCleanup()
        //{
           
        //}
    }
}
