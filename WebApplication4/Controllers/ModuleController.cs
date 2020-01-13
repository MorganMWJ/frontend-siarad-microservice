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
        private readonly IHttpClientFactory _factory;
        public ModuleController(IHttpClientFactory factory)
        {
            _factory = factory;

        }

        [HttpGet]
        public IActionResult ViewModule()
        { 
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ViewModule(String id)
        {
            var client = _factory.CreateClient("ModuleClient");
            HttpResponseMessage response = await client.GetAsync($"/api/modules/{id}");

            ModuleViewModel model;
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<ModuleViewModel>(responseString);
            }
            else
            {
                model = null;
            }
            return View(model);
        }
        public async Task<IActionResult> ListModules()
        {
            List<ModuleViewModel> moduleList = new List<ModuleViewModel>();
            var client = _factory.CreateClient("ModuleClient");
            HttpResponseMessage response = await client.GetAsync("/api/modules");
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                moduleList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModuleViewModel>>(responseString);
            }
            else
            {
                return View(moduleList);
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
        public async Task<IActionResult> CreateModule(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = _factory.CreateClient("ModuleClient");
                //HttpResponseMessage response = await client.GetAsync("/api/modules");
                /*var url = "http://m56-docker1.dcs.aber.ac.uk:8100/api/modules";
                var request = WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;*/

                //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                /*{
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
                    return RedirectToAction("AccessDenied", "Account");
                }*/

                HttpResponseMessage response = await client.PostAsJsonAsync("/api/modules", model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ListModules", "Module");
                }
                else
                {

                    return View(response.Content);
                }
               
            }

            return View(model);
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
                return RedirectToAction("AccessDenied", "Account");
            }
            return RedirectToAction("ListModules", "Module");
        }

        [HttpGet]
        public IActionResult EditModule(string id)
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
                Debug.WriteLine(e.Message);
                return RedirectToAction("AccessDenied", "Account");
            }
            StreamReader sr = new StreamReader(response.GetResponseStream());
            var sb = sr.ReadToEnd();

            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ModuleViewModel>(sb);

            return View(model);
        }

        [HttpPost]
        public IActionResult EditModule(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var url = $"http://m56-docker1.dcs.aber.ac.uk:8100/api/modules/{model.Id}";
                var request = WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "PUT";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    //THIS FORMAT IS NEEDED
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        Id = model.Id,
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
                    return RedirectToAction("AccessDenied", "Account");
                }
                return RedirectToAction("ListModules", "Module");
            }

            return View(model);
        }
    }
}
