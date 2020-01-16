using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public interface IModuleClientService
    {
        Task<ModuleViewModel> GetModuleAsync(int id);
        Task<List<ModuleViewModel>> GetModuleListAsync();

        Task<List<StaffAndStudentModel>> GetUsersOnModuleListAsync(int id);
    }
}
