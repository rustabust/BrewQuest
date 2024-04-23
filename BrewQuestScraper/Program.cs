using BrewQuest.Models;
using BrewQuestScraper;
using HtmlAgilityPack;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Text.RegularExpressions;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Bout to start the BrewQuestScraper 1.0!");

bool ahaCompleted = await AHAScraper.Scrape();
bool completed = await BCScraper.Scrape();

Console.WriteLine("scraper finished.");



