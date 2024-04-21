using System.Net;

namespace BrewQuest.Models.Tests
{
    [TestClass]
    public class CommonFunctionsTests
    {
        private const string TEST_JSON_FILE_PATH = "C:\\Users\\rusty\\OneDrive\\Documents\\GitHub\\BrewQuest\\BrewQuestScraper\\Data\\unit_test_file.json";

        [TestMethod]
        public void test_SyncObjectsToFile_basic_saving()
        {
            addTwoCompetitionsToFile();

            // check to see that the file contains the objects and no duplicates
            List<Competition> outputObjects = CommonFunctions.DeserializeFromJsonFile<List<Competition>>(TEST_JSON_FILE_PATH);
            Assert.AreEqual(2, outputObjects.Count);
            Assert.AreEqual("Test Competition 1", outputObjects[0].CompetitionName);
            Assert.AreEqual("Test Competition 2", outputObjects[1].CompetitionName);
        }

        private static List<Competition> addTwoCompetitionsToFile()
        {
            List<Competition> inputObjects = new List<Competition>();

            // get a couple objects
            Competition comp1 = new Competition
            {
                CompetitionName = "Test Competition 1"
            };
            Competition comp2 = new Competition
            {
                CompetitionName = "Test Competition 2"
            };
            inputObjects.Add(comp1);
            inputObjects.Add(comp2);

            // write to file does not exist
            CommonFunctions.SyncObjectsToFile<Competition>(inputObjects, TEST_JSON_FILE_PATH);

            return inputObjects
        }

        [TestMethod]
        public void test_SyncObjectsToFile_adding_existing_objects()
        {
            List<Competition> inputObjects = addTwoCompetitionsToFile();

            // add a new object on top of the existing
            Competition comp3 = new Competition
            {
                CompetitionName = "Test Competition 3"
            };
            inputObjects.Add(comp3);

            // write to file 
            CommonFunctions.SyncObjectsToFile<Competition>(inputObjects, TEST_JSON_FILE_PATH);

            // check to see that the file contains the objects and no duplicates
            List<Competition> outputObjects = CommonFunctions.DeserializeFromJsonFile<List<Competition>>(TEST_JSON_FILE_PATH);
            Assert.AreEqual(3, outputObjects.Count);
            Assert.AreEqual("Test Competition 1", outputObjects[0].CompetitionName);
            Assert.AreEqual("Test Competition 2", outputObjects[1].CompetitionName);
            Assert.AreEqual("Test Competition 3", outputObjects[2].CompetitionName);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                // delete the file
                File.Delete(TEST_JSON_FILE_PATH);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}