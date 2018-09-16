using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CowsAndChicken.DTO;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Microsoft.AspNetCore.Http;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewGame()
        {
            return View("StartGame");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(Player player)
        {
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://localhost:44326/api/players", player);
            var playerResponse = await response.Content.ReadAsAsync<Player>();

            HttpContext.Session.SetString("PlayerId", playerResponse.Id.ToString());
            return RedirectToAction("NewGame");
            //return View("StartGame");
        }

        [HttpPost]
        public async Task<IActionResult> StartAsync(int nrOfDigits)
        {
            var playerId = Guid.Parse(HttpContext.Session.GetString("PlayerId"));
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync($"https://localhost:44326/api/players/{playerId}/game", nrOfDigits);
            var gameInfo = await response.Content.ReadAsAsync<GameInfo>();

            HttpContext.Session.SetString("GameInfo", $"Game ID: {gameInfo.GameId} - Player name: {gameInfo.PlayerName} - Player year of birth: {gameInfo.PlayerYearOfBirth}");

            ViewData["GameInfo"] = HttpContext.Session.GetString("GameInfo");
            return View("Game");
        }

        [HttpPost]
        public async Task<IActionResult> TakeTurnAsync(int guess)
        {
            var playerId = Guid.Parse(HttpContext.Session.GetString("PlayerId"));
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync($"https://localhost:44326/api/players/{playerId}/game/turn", guess);
            var result = await response.Content.ReadAsAsync<Result>();

            if (!result.IsGameCompleted)
            {
                ViewData["Hint"] = $"You had {result.NrOfCows} cows, {result.NrOfChickens} chickens.";
                ViewData["GameInfo"] = HttpContext.Session.GetString("GameInfo");
                return View("Game");
            }
            else
            {
                var statsModel = new StatsViewModel();
                var efficiencyResponse = await client.GetAsync($"https://localhost:44326/api/players/{playerId}/efficiency");
                statsModel.EfficiencyStats = await efficiencyResponse.Content.ReadAsAsync<Dictionary<int, Stats>>();
                var speedResponse = await client.GetAsync($"https://localhost:44326/api/players/{playerId}/speed");
                statsModel.SpeedStats = await speedResponse.Content.ReadAsAsync<Stats>();
                return View("GameStats", statsModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
