using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class SettingsController : Controller
    {

        [HttpGet]
        public IActionResult EditSettings()
        {
            var url = $"http://m56-docker1.dcs.aber.ac.uk:8200/api/notifications/{User.Identity.Name}";
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException e)
            {
                ViewBag.Error = "Error";
                return View();
            }
            StreamReader sr = new StreamReader(response.GetResponseStream());
            var sb = sr.ReadToEnd();

            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<NotificationModelViewModel>(sb);
            return View(model);
        }

        //[HttpPost]
       /* public IActionResult EditSettings(NotificationModelViewModel model)
        {

        }*/
    }
}
