﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IHttpClientFactory _factory;
        public SettingsController(IHttpClientFactory factory)
        {
            _factory = factory;

        }

        //Get the settings for a user, if there is no settings currently, create those settings and return that.
        [HttpGet]
        public async Task<IActionResult> EditSettings(String id)
        {
            var model = new NotificationModelViewModel
            {
                Uid = id,
                Daily = false,
                Mentions = false,
                Replies = false,
                NotificationInterval = 1,
                LastUpdated = DateTime.Now
            };
            var client = _factory.CreateClient("SettingsClient");
            HttpResponseMessage response = await client.GetAsync($"/api/usernotifications/{id}");
            if(response.StatusCode == HttpStatusCode.NotFound)
            {  
                response = await client.PostAsJsonAsync("/api/usernotifications", model);
                if (response.IsSuccessStatusCode)
                {
                    return View(model);
                }
                else
                {
                    return View(response.Content);
                }
            }else if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<NotificationModelViewModel>(responseString);
            }

            return View(model);
        }

        //Edit the user settings
        [HttpPost]
        public async Task<IActionResult> EditSettings(NotificationModelViewModel model)
        {
            model.Uid = User.Identity.Name;
            if (ModelState.IsValid)
            {
                var client = _factory.CreateClient("SettingsClient");
                model.LastUpdated = DateTime.Now;
                HttpResponseMessage response = await client.PutAsJsonAsync($"/api/usernotifications/{model.Uid}", model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    return View(response.Content);
                }
            }
            return View(model);
        }
    }
}
