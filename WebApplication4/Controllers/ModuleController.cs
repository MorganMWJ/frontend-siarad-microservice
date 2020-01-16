using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModuleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IModuleClientService _moduleClient;
        private readonly IHttpClientFactory _factory;
        private readonly IDataRepository _repo;

        public ModuleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, 
            IHttpClientFactory factory, IModuleClientService moduleClient, IDataRepository repo)

        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _factory = factory;
            _repo = repo;
            _moduleClient = moduleClient;
        }
        //[AllowAnonymous]
        //[HttpGet]
        //public IActionResult ViewModule()
        //{ 
        //    return View();
        //}

        [AllowAnonymous]
        public async Task<IActionResult> ViewModule(int id)
        {
            ModuleViewModel module = await _moduleClient.GetModuleAsync(id);
            List<Group> groupsForModule = await _repo.GroupListAsync(module.Id);
            module.Groups = groupsForModule;
            return View(module);
        }
        public async Task<IActionResult> ListModules()
        {
            List<ModuleViewModel> moduleList = await _moduleClient.GetModuleListAsync();
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
        public async Task<IActionResult> EditModule(int id)
        {
            var client = _factory.CreateClient("ModuleClient");
           
            ModuleViewModel model;

            //Get a list of students on a specific module
            var studentsOnModule = await userManager.GetUsersInRoleAsync("Student");

            HttpResponseMessage response = await client.GetAsync($"/api/modules/{id}/students");
            List<StaffAndStudentModel> studentList = new List<StaffAndStudentModel>();
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                studentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StaffAndStudentModel>>(responseString);
            }

            //Get a list of staff on a specific module
            var staffOnModule = await userManager.GetUsersInRoleAsync("Staff");

            response = await client.GetAsync($"/api/modules/{id}/staff");
            List<StaffAndStudentModel> staffList = new List<StaffAndStudentModel>();
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                staffList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StaffAndStudentModel>>(responseString);
            }

            response = await client.GetAsync($"/api/modules/{id}");
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<ModuleViewModel>(responseString);

                List<String> modelStudentsToAdd = new List<String>();
                List<String> studentListInSystem = studentsOnModule.Select(c => c.UserName).ToList();
                foreach (StaffAndStudentModel studentUser in studentList)
                {
                    if (studentListInSystem.Contains(studentUser.Uid))
                    {
                        modelStudentsToAdd.Add(studentUser.Uid);
                    }
                }
                model.Students = modelStudentsToAdd;
                List<String> modelStaffToAdd = new List<String>();
                List<String> staffListInSystem = staffOnModule.Select(c => c.UserName).ToList();
                foreach (StaffAndStudentModel staffUser in staffList)
                {
                    if (staffListInSystem.Contains(staffUser.Uid))
                    {
                        modelStaffToAdd.Add(staffUser.Uid);
                    }
                }
                model.Staff = modelStaffToAdd;
            }
            else
            {
                model = null;
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ManageStudentsRegistered(int id)
        {
            var model = new List<ManageRegisteredStudentsAndStaffModel>();
            var client = _factory.CreateClient("ModuleClient");
            ViewBag.id = id;

            var studentsInSystem = await userManager.GetUsersInRoleAsync("Student");

            HttpResponseMessage response = await client.GetAsync($"/api/modules/{id}/students");
            List<StaffAndStudentModel> studentList = new List<StaffAndStudentModel>();
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                studentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StaffAndStudentModel>>(responseString);
            }


            foreach (ApplicationUser student in studentsInSystem)
            {
                var registeredStudentModel = new ManageRegisteredStudentsAndStaffModel()
                {
                    UserName = student.UserName
                };
                if(studentList.Find(e=> e.Uid.Equals(student.UserName)) != null){
                    registeredStudentModel.IsSelected = true;
                }
                else
                {
                    registeredStudentModel.IsSelected = false;
                }

                model.Add(registeredStudentModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageStudentsRegistered(List<ManageRegisteredStudentsAndStaffModel> model, int id)
        {
            var client = _factory.CreateClient("ModuleClient");
            HttpResponseMessage response;
            foreach (var registeredStudent in model)
            {
                //api/students/{uid}/{mid]
                if (registeredStudent.IsSelected)
                {
                    response = await client.PostAsJsonAsync($"/api/students/{registeredStudent.UserName}/{id}", "");
                    if (!response.IsSuccessStatusCode)
                    {
                        return View(response.Content);
                    }
                }
                else
                {
                    response = await client.DeleteAsync($"/api/students/{registeredStudent.UserName}/{id}");
                }
            }



            return RedirectToAction("EditModule", new { Id = id });
        }

        [HttpGet]
        public async Task<IActionResult> ManageStaffRegistered(int id)
        {
            var model = new List<ManageRegisteredStudentsAndStaffModel>();
            var client = _factory.CreateClient("ModuleClient");
            ViewBag.id = id;

            var staffInSystem = await userManager.GetUsersInRoleAsync("Staff");

            HttpResponseMessage response = await client.GetAsync($"/api/modules/{id}/staff");
            List<StaffAndStudentModel> staffList = new List<StaffAndStudentModel>();
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                staffList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StaffAndStudentModel>>(responseString);
            }


            foreach (ApplicationUser staff in staffInSystem)
            {
                var registeredStaffModel = new ManageRegisteredStudentsAndStaffModel()
                {
                    UserName = staff.UserName
                };
                if (staffList.Find(e => e.Uid.Equals(staff.UserName)) != null)
                {
                    registeredStaffModel.IsSelected = true;
                }
                else
                {
                    registeredStaffModel.IsSelected = false;
                }

                model.Add(registeredStaffModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageStaffRegistered(List<ManageRegisteredStudentsAndStaffModel> model, int id)
        {
            var client = _factory.CreateClient("ModuleClient");
            HttpResponseMessage response;
            foreach (var registeredStaff in model)
            {
                //api/students/{uid}/{mid]
                if (registeredStaff.IsSelected)
                {
                    response = await client.PostAsJsonAsync($"/api/staff/{registeredStaff.UserName}/{id}", "");
                    if (!response.IsSuccessStatusCode)
                    {
                        return View(response.Content);
                    }
                }
                else
                {
                    response = await client.DeleteAsync($"/api/staff/{registeredStaff.UserName}/{id}");
                }
            }



            return RedirectToAction("EditModule", new { Id = id });
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
                ByteArrayContent bytes;
                MultipartFormDataContent multiContent;
                HttpResponseMessage response;
                if (model.UploadStudentModule == null && model.UploadStaff == null && model.UploadModule == null)
                {
                    ViewBag.ErrorMessage = "Please upload a file";
                    return View(model);
                }
                if (model.UploadModule != null)
                {
                    
                    using (var br = new BinaryReader(model.UploadModule.OpenReadStream()))
                        data = br.ReadBytes((int)model.UploadModule.OpenReadStream().Length);

                     bytes = new ByteArrayContent(data);


                    multiContent = new MultipartFormDataContent();

                    multiContent.Add(bytes, "file", model.UploadModule.FileName);
                    response = await client.PostAsync($"/api/data/modules", multiContent);
                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.ErrorMessage = "Invalid format on file uploaded - Module.";
                        return View(model);
                    }

                }
                    if (model.UploadStudentModule != null && (model.Year >= 1900 && model.Year <= 2099))
                {

                    using (var br = new BinaryReader(model.UploadStudentModule.OpenReadStream()))
                        data = br.ReadBytes((int)model.UploadStudentModule.OpenReadStream().Length);

                    bytes = new ByteArrayContent(data);


                    multiContent = new MultipartFormDataContent();

                    multiContent.Add(bytes, "file", model.UploadStudentModule.FileName);
                    response = await client.PostAsync($"/api/data/students/{model.CampusCode}", multiContent);
                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.ErrorMessage = "Invalid format on file uploaded - Student Module.";
                        return View(model);
                    }
                }
                else if (model.UploadStudentModule != null && (model.Year < 1900 || model.Year >2099))
                {
                    ViewBag.ErrorMessage = "Please enter a year between 1900-2099";
                    return View(model);
                }


                    if (model.UploadStaff != null)
                {
                    using (var br = new BinaryReader(model.UploadStaff.OpenReadStream()))
                        data = br.ReadBytes((int)model.UploadStaff.OpenReadStream().Length);

                    bytes = new ByteArrayContent(data);


                    multiContent = new MultipartFormDataContent();

                    multiContent.Add(bytes, "file", model.UploadStaff.FileName);
                    response = await client.PostAsync($"/api/data/staff/{model.CampusCode}/{model.Year}", multiContent);
                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.ErrorMessage = "Invalid format on file uploaded - Staff.";
                        return View(model);
                    }
                }
               

                return RedirectToAction("Index", "Home");
            }
            return View(model);
            // do something with the above data
            // to do : return something
        }


       /* sealed class FileExportCSVMap : ClassMap<FileExportModel>
        {
            public FileExportCSVMap()
            {
                Map(m => m.module_code).Name("module_code");
                Map(m => m.mod_full_name).Name("mod_full_name");
                Map(m => m.academic_year).Name("academic_year");
                Map(m => m.email).Name("email");
                Map(m => m.name).Name("name");
            }
        }
        public async Task<FileStreamResult> FileExport(int id)
        {
            var client = _factory.CreateClient("ModuleClient");

            HttpResponseMessage response = await client.GetAsync($"/api/modules/{id}");
            string responseString = response.Content.ReadAsStringAsync().Result;
            ModuleViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<ModuleViewModel>(responseString);

            response = await client.GetAsync($"/api/modules/{id}/students");
            List<StaffAndStudentModel> studentList = new List<StaffAndStudentModel>();
            responseString = response.Content.ReadAsStringAsync().Result;
            studentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StaffAndStudentModel>>(responseString);

            List<FileExportModel> fileExport = new List<FileExportModel>();
            foreach(StaffAndStudentModel student in studentList)
            {
                var studentName = student.Surname + ", " + student.Forename;
                var file = new FileExportModel()
                {
                    module_code = model.Code,
                    mod_full_name = model.Title,
                    academic_year = model.Year,
                    email = student.Uid,
                    name = studentName
                };
                fileExport.Add(file);
            }

            var stream = new MemoryStream();
            using (var writeFile = new StreamWriter(stream))
            {
                var csv = new CsvWriter(writeFile);
                csv.Configuration.RegisterClassMap<FileExportCSVMap>();
                csv.WriteRecords(fileExport);
            }
            stream.Position = 0;

            return File(stream, "application/octet-stream", $"Exported{model.Code}.csv");
        }*/
    }
}
