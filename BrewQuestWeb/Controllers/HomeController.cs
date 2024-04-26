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

        public IActionResult Index(int page = 1, string sortColumn = "CompetitionName", string sortDirection = "asc", List<string> states = null)
        {
            const int pageSize = 30;
            IEnumerable<Competition> filteredCompetitions = this.Competitions;

            // Filtering
            if (states != null && states.Count > 0)
            {
                filteredCompetitions = filteredCompetitions.Where(c => states.Contains(c.LocationState));
            }

            // Sorting
            filteredCompetitions = sortDirection == "asc" ?
                filteredCompetitions.OrderBy(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)) :
                filteredCompetitions.OrderByDescending(x => x.GetType().GetProperty(sortColumn).GetValue(x, null));

            // Paging
            var paginatedCompetitions = filteredCompetitions
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)filteredCompetitions.Count() / pageSize);
            ViewBag.SortColumn = sortColumn;
            ViewBag.SortDirection = sortDirection;
            ViewBag.StatesFilter = states;

            return View(paginatedCompetitions);
        }

        [HttpPost]
        public IActionResult FilterCompetitions(List<string> states)
        {
            var filteredCompetitions = this.Competitions.Where(c => states.Contains(c.LocationState)).ToList();
            return View("Index", filteredCompetitions);
        }

        private List<Competition>? _competitions;
        protected List<Competition> Competitions
        {
            get
            {
                if (_competitions == null)
                {
                    _competitions = CommonFunctions.LoadCompetitionsFromJson();

                    // for now, dont show international. to turn this on, just remove the where clause
                    // but also need to hook up the frontend checkboxes
                    _competitions = _competitions.Where(a => a.LocationCountry == null || a.LocationCountry == "US").ToList();
                    
                    // some analysis...
                    //var allCount = _competitions.Count();
                    //var noCountryForOldMen = _competitions.Count(a => string.IsNullOrEmpty(a.LocationCountry));
                    //Console.WriteLine("there are " + noCountryForOldMen + " out of " + allCount + " competitions with no country specified.");

                    //var uniqueCountries = _competitions.Select(a => a.LocationCountry).Distinct();
                    //Console.WriteLine("these are the unique countries :");
                    //foreach (var country in uniqueCountries)
                    //{
                    //    Console.WriteLine("  " + country);
                    //}
                }
                return _competitions;
            }
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
