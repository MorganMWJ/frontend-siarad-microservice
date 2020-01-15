using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly FrontendContext _context;

        public DataRepository(FrontendContext context)
        {
            _context = context;
        }

        public async Task<Group> GetGroupAsync(int id)
        {
            return await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Group>> GroupListAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<List<Group>> GroupListAsync(int moduleId)
        {
            return await _context.Groups.Where(g => g.ModuleId.Equals(moduleId)).ToListAsync();
        }

        public async Task CreateGroupAsync(Group group)
        {
            _context.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGroupAsync(Group group)
        {
            _context.Update(@group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGroupAsync(Group group)
        {
            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();
        }

        public bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
