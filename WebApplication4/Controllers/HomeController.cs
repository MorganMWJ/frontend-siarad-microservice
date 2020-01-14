using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
                return RedirectToAction("AccessDenied", "Administration");
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
