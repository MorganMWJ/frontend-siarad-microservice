using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Data
{
    public interface IDataRepository
    {
        Task<Group> GetGroupAsync(int id);
        Task<List<Group>> GroupListAsync();
        Task<List<Group>> GroupListAsync(int moduleId);
        Task CreateGroupAsync(Group group);
        Task UpdateGroupAsync(Group group);
        Task DeleteGroupAsync(Group group);
        bool GroupExists(int id);
    }
}
