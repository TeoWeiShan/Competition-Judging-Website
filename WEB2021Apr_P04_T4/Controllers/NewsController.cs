using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.Models;

namespace WEB2021Apr_P04_T4.Controllers
{
    public class NewsController : Controller
    {
        // GET: NewsController
        public async Task<ActionResult> Index()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://weblcu-f967.restdb.io");
            HttpResponseMessage response = await client.GetAsync("/rest/news?apikey=60ffe4f149cd3a5cfbd22c73&sort=date&dir=-1");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<News> newsList = JsonConvert.DeserializeObject<List<News>>(data);
                return View(newsList);
            }
            else
            {
                return View(new List<News>());
            }
        }
    }
}
