using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication4.Data;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public class ModuleClientService : IModuleClientService
    {
        private readonly IHttpClientFactory _factory;

        public ModuleClientService(IHttpClientFactory factory, IDataRepository repo)
        {
            _factory = factory;
        }

        public async Task<ModuleViewModel> GetModuleAsync(int id)
        {
            var client = _factory.CreateClient("ModuleClient");
            HttpResponseMessage response = await client.GetAsync($"/api/modules/{id}");

            ModuleViewModel module;
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                module = Newtonsoft.Json.JsonConvert.DeserializeObject<ModuleViewModel>(responseString);
            }
            else
            {
                module = null;
            }

            return module;
        }

        public async Task<List<ModuleViewModel>> GetModuleListAsync()
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
                //error message?
            }
            return moduleList;
        }
    }
}
