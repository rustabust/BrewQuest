using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace BrewQuest.Models
{
    public static class CommonFunctions
    {
        public const string COMPETITIONS_MASTER_JSON_FILEPATH = "C:\\Users\\rusty\\OneDrive\\Documents\\GitHub\\BrewQuest\\BrewQuestScraper\\Data\\BrewQuest_CompetitionsList_master.json";

        public static void AddObjectsToFile<T>(List<T> inputObjects, string fileName)
        {
            List<T> existingObjects = new List<T>();

            // Read existing objects from file
            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                existingObjects = JsonConvert.DeserializeObject<List<T>>(json);
            }

            // Add input objects if they don't already exist
            foreach (var obj in inputObjects)
            {
                if (!existingObjects.Contains(obj))
                {
                    existingObjects.Add(obj);
                }
            }

            // Write back to file
            string outputJson = JsonConvert.SerializeObject(existingObjects, Formatting.Indented);
            File.WriteAllText(fileName, outputJson);
        }

        public static string nonZeroDateTime(DateTime? date)
        {
            if (date == null || date == new DateTime())
                return "";
            else
                return date.ToString();
        }

        public static string nonZeroInt(int? theInt)
        {
            if (theInt == null || theInt == 0)
                return "";
            else
                return theInt.ToString();
        }

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
