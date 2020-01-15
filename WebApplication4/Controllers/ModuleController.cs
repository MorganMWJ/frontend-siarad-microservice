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
using WebApplication4.Data;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModuleController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly IDataRepository _repo;
        public ModuleController(IHttpClientFactory factory, IDataRepository repo)
        {
            _factory = factory;
            _repo = repo;

        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ViewModule()
        { 
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ViewModule(int id)
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

            List<Group> groupsForModule = await _repo.GroupListAsync(model.Id);
            model.Groups = groupsForModule;

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

        [HttpPost]
        public async Task<IActionResult> CreateModule(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = _factory.CreateClient("ModuleClient");
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
        public async Task<IActionResult> DeleteModule(string id)
        {
            var client = _factory.CreateClient("ModuleClient");
            HttpResponseMessage response = await client.DeleteAsync($"/api/modules/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListModules", "Module");
            }
            else
            {

                return View(response.Content);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditModule(string id)
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

        [HttpPost]
        public async Task<IActionResult> EditModule(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = _factory.CreateClient("ModuleClient");
                HttpResponseMessage response = await client.PutAsJsonAsync($"/api/modules/{model.Id}", model);
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

        public IActionResult FileUpload()
        {
            return View(new FileUploadModel());
        }

        //source https://stackoverflow.com/questions/39397278/post-files-from-asp-net-core-web-api-to-another-asp-net-core-web-api
        [HttpPost]
        public async Task<IActionResult> FileUpload(FileUploadModel model)
        {

            if (ModelState.IsValid)
            {
                //Getting file meta data
                var client = _factory.CreateClient("ModuleClient");

                byte[] data;
                using (var br = new BinaryReader(model.UploadModule.OpenReadStream()))
                    data = br.ReadBytes((int)model.UploadModule.OpenReadStream().Length);

                ByteArrayContent bytes = new ByteArrayContent(data);


                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                multiContent.Add(bytes, "file", model.UploadModule.FileName);
                HttpResponseMessage response = await client.PostAsync($"/api/data/modules", multiContent);

                if (response.IsSuccessStatusCode)
                {

                }


                using (var br = new BinaryReader(model.UploadStudentModule.OpenReadStream()))
                    data = br.ReadBytes((int)model.UploadStudentModule.OpenReadStream().Length);

                bytes = new ByteArrayContent(data);


                multiContent = new MultipartFormDataContent();

                multiContent.Add(bytes, "file", model.UploadStudentModule.FileName);
                response = await client.PostAsync($"/api/data/students/{model.CampusCode}", multiContent);

                if (response.IsSuccessStatusCode)
                {

                }



                using (var br = new BinaryReader(model.UploadStaff.OpenReadStream()))
                    data = br.ReadBytes((int)model.UploadStaff.OpenReadStream().Length);

                bytes = new ByteArrayContent(data);


                multiContent = new MultipartFormDataContent();

                multiContent.Add(bytes, "file", model.UploadStaff.FileName);
                response = await client.PostAsync($"/api/data/staff/{model.CampusCode}/{model.Year}", multiContent);

                if (response.IsSuccessStatusCode)
                {

                }


                return RedirectToAction("Index", "Home");
            }
            return View(model);
            // do something with the above data
            // to do : return something
        }
    }
}
