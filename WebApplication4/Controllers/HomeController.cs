using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
        public IActionResult Index()
        {
            var url = "";
            WebRequest request;
            WebResponse response;
            List<ModuleViewModel> moduleList = new List<ModuleViewModel>();
            if (User.IsInRole("Admin")) //Admin view
            {
                url = "http://m56-docker1.dcs.aber.ac.uk:8100/api/modules";
                request = HttpWebRequest.Create(url);
            }
            else if (User.IsInRole("Student") || User.IsInRole("Staff"))
            {
                var name = User.Identity.Name;
                var year = DateTime.Now.Year.ToString();
                url = $"http://m56-docker1.dcs.aber.ac.uk:8100/api/modules/year/{year}/{name}";
                request = HttpWebRequest.Create(url);
            }
            else
            {
                request = HttpWebRequest.Create("http://m56-docker1.dcs.aber.ac.uk:8100/api/modules");
            }
            try
            {
                response = request.GetResponse();
            }catch(WebException e)
            {
                return View(moduleList);
            }
            StreamReader sr = new StreamReader(response.GetResponseStream());
            var sb = sr.ReadToEnd();
            if (sb.Equals("")) {
                return View(moduleList);
            }
            dynamic stuff = Newtonsoft.Json.JsonConvert.DeserializeObject(sb);
            foreach(JObject root in stuff)
            {
                var module = new ModuleViewModel();
                foreach (KeyValuePair<String, JToken> app in root)
                {
                    if (app.Key.Equals("id"))
                    {
                        module.Id = app.Value.ToString();
                    }
                    if (app.Key.Equals("code"))
                    {
                        module.Code = app.Value.ToString();
                    }
                    if (app.Key.Equals("year"))
                    {
                        module.Year = app.Value.ToString();
                    }
                    if (app.Key.Equals("coordinatorUid"))
                    {
                        module.CoordinatorUid = app.Value.ToString();
                    }
                    if (app.Key.Equals("title"))
                    {
                        module.Title = app.Value.ToString();
                    }
                }
                moduleList.Add(module);
            }
            return View(moduleList);
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
