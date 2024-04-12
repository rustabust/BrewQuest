using BrewQuest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class CompetitionsController : Controller
{


    public IActionResult Index()
    {
        return View(this.Competitions);
    }

    [HttpPost]
    public IActionResult FilterCompetitions(List<string> states)
    {
        var filteredCompetitions = this.Competitions.Where(c => states.Contains(c.LocationState)).ToList();
        return View("Index", filteredCompetitions);
    }

    private List<Competition> _competitions;
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
