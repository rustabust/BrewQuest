using BrewQuest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BrewQuest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Competition> competitions = new List<Competition>();

            // Path to your CSV file
            string csvFilePath = "C:\\Users\\rusty\\OneDrive\\Documents\\GitHub\\BrewQuest\\Data\\CompMockData.csv";

            // Read the CSV file
            using (var reader = new StreamReader(csvFilePath))
            {
                // Skip the header
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    competitions.Add(new Competition
                    {
                        CompetitionName = values[0],
                        Host = values[1],
                        EntryWindowOpen = values[2],
                        EntryWindowClose = values[3],
                        FinalJudgingDate = values[4],
                        EntryLimit = int.Parse(values[5]),
                        EntryFee = decimal.Parse(values[6]),
                        Status = values[7],
                        LocationCity = values[8],
                        LocationState = values[9],
                        ShippingAddress = values[10],
                        ShippingWindowOpen = values[11],
                        ShippingWindowClose = values[12],
                        CompetitionUrl = values[13],
                        HostUrl = values[14]
                    });
                }
            }

            return View(competitions);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
