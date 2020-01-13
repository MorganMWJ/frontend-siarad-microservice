using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModuleController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        public IActionResult ViewModule(String id)
        {

            var url = $"http://m56-docker1.dcs.aber.ac.uk:8100/api/modules/{id}";
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

            var stuff = Newtonsoft.Json.JsonConvert.DeserializeObject<ModuleViewModel>(sb);
            Debug.WriteLine(stuff);
            return View(stuff);
        }
        public IActionResult ListModules()
        {
            List<ModuleViewModel> moduleList = new List<ModuleViewModel>();
            var url = $"http://m56-docker1.dcs.aber.ac.uk:8100/api/modules";
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException e)
            {
                return View(moduleList);
            }
            StreamReader sr = new StreamReader(response.GetResponseStream());
            var sb = sr.ReadToEnd();
            if (sb.Equals(""))
            {
                return View(moduleList);
            }
            dynamic stuff = Newtonsoft.Json.JsonConvert.DeserializeObject(sb);
            foreach (JObject root in stuff)
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
                    if (app.Key.Equals("classCode"))
                    {
                        module.ClassCode = app.Value.ToString();
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

        [HttpGet]
        public IActionResult CreateModule()
        {
            return View();
        }

        //Source https://stackoverflow.com/questions/44676611/how-to-send-json-data-in-post-request-using-c-sharp
        [HttpPost]
        public IActionResult CreateModule(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var url = "http://m56-docker1.dcs.aber.ac.uk:8100/api/modules";
                var request = WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    //THIS FORMAT IS NEEDED
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        Code = model.Code,
                        Year = model.Year,
                        ClassCode = model.ClassCode,
                        CoordinatorUid = model.CoordinatorUid,
                        Title = model.Title
                    });
                    streamWriter.Write(json);
                }
                try
                {
                    var response = request.GetResponse();
                }
                catch (WebException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            return RedirectToAction("ListModules", "Module");
        }

        [HttpGet]
        public IActionResult DeleteModule()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteModule(string id)
        {
            var url = $"http://m56-docker1.dcs.aber.ac.uk:8100/api/modules/{id}";
            var request = WebRequest.Create(url);
            request.Method = "DELETE";
            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException e)
            {
                Debug.WriteLine(e.Message);
            }
            return RedirectToAction("ListModules", "Module");
        }
    }
}
