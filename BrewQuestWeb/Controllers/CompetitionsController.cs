using BrewQuest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class CompetitionsController : Controller
{
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
            }
            return _competitions;
        }
    }
}
