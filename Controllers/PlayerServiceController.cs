using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LeanPoker;

namespace LeanPoker.Controllers
{
    [Route("/")]
    public class PlayerServiceController : Controller
    {
        // GET: /
        [HttpGet]
        public FileContentResult Get()
        {
            return File(Encoding.UTF8.GetBytes("OK"), "text/plain");
        }

        // POST /
        [HttpPost]
        public FileContentResult Post(string game_state)
        {
            string action = Request.Form["action"];
            if (string.IsNullOrEmpty(action))
                action = Request.Query["action"];
            switch (action)
            {
                case "bet_request":
                {
                    var json = JObject.Parse(game_state);
                    var bet = PokerPlayer.BetRequest(json).ToString();
                    var betBytes = Encoding.UTF8.GetBytes(bet);
                    return File(betBytes, "text/plain");
                }
                case "showdown":
                {
                    var json = JObject.Parse(game_state);
                    PokerPlayer.ShowDown(json);
                    return File(Encoding.UTF8.GetBytes("OK"), "text/plain");
                }
                case "version":
                    return File(Encoding.UTF8.GetBytes(PokerPlayer.VERSION), "text/plain");
                case "check":
                    return File(Encoding.UTF8.GetBytes("OK"), "text/plain");
                default:
                    return File(Encoding.UTF8.GetBytes("Not an allowed action or request:" + action), "text/plain");
            }
        }
    }
}
