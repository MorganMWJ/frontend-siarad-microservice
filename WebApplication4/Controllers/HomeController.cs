using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHttpClientFactory _factory;
        public HomeController(IHttpClientFactory factory)
        {
            _factory = factory;

        }
        //Returns a view containing a list of modules to be displayed, a student or staff member will only receive their own modules whilst admin will see a list of all modules.
        public async Task<IActionResult> Index()
        {
                List<ModuleViewModel> moduleList = new List<ModuleViewModel>();
                var client = _factory.CreateClient("ModuleClient");
               

            if (User.IsInRole("Admin")) //Admin view
            {
                HttpResponseMessage response = await client.GetAsync("/api/modules");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    moduleList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModuleViewModel>>(responseString);
                }
                return View(moduleList);
            }
            else if (User.IsInRole("Student") || User.IsInRole("Staff"))
            {
                var name = User.Identity.Name;
                var year = DateTime.Now.Year.ToString();
                HttpResponseMessage response = await client.GetAsync($"/api/modules/year/{year}/{name}");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    moduleList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModuleViewModel>>(responseString);
                }
                return View(moduleList);
            }
            else
            {
                return View(moduleList);
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
