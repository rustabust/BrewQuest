using BrewQuestScraper;
using BrewQuestScraper.Models;
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

        //public void test_parse_dates()
        //{
        //    string dateString = "12/12/2022 12:00 PM EST";
        //    string originalDateString = dateString;

        //    DateParsing.CleanupDateString(dateString);

        //    Assert.IsTrue(dateString != originalDateString);

        //    try
        //    {
        //        bool isInternationalFormat = DateParsing.IsInternationalDateFormat(dateString);
        //        var takeMeOnADate = DateParsing.GetDateTimeFromString(dateString, isInternationalFormat);
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}

        //[TestCleanup]
        //public void TestCleanup()
        //{

        //}
    }

    [TestClass]
    public class DateParsingTests
    {
        private void test_parse_date(string dateString)
        {
            {
                DateTime dateResult = DateTime.MinValue;
                if (!DateTime.TryParse(dateString, out dateResult))
                    throw new Exception("DateTime.TryParse does not work for : " + dateString);
            }

            string originalDateString = dateString;

            dateString = DateParsing.CleanupDateString(dateString);

            Assert.IsTrue(dateString != originalDateString);

            try
            {
                bool isInternationalFormat = DateParsing.IsInternationalDateFormat(dateString);
                var takeMeOnADate = DateParsing.GetDateTimeFromString(dateString, isInternationalFormat);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void test_parse_dates()
        {
            string[] dateStrings = new string[] { "12/12/2022 12:00 PM EST", "2023/08/20 12:00 AM EDT" };
            foreach (var dateString in dateStrings)
            {
                test_parse_date(dateString);
            }
        }
    }
}
