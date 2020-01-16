using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Models;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IModuleClientService _moduleClient;
        private readonly IHttpClientFactory _factory;
        private readonly IDataRepository _repo;        

        public GroupsController(IModuleClientService moduleClient, IHttpClientFactory factory, IDataRepository repo)
        {
            _moduleClient = moduleClient;
            _factory = factory;
            _repo = repo;
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _repo.GetGroupAsync((int)id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public async Task<IActionResult> Create(int module_id)
        {
            ModuleViewModel model = await _moduleClient.GetModuleAsync(module_id);
            ViewBag.CurrentModule = model;
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ModuleId,IsPrivate,Uid1,Uid2")] Group @group)
        {
            if (ModelState.IsValid)
            {
                await _repo.CreateGroupAsync(@group);
                return RedirectToAction("ViewModule", "Module", new { id = group.ModuleId });
            }
            return View(@group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _repo.GetGroupAsync((int)id);
            if (@group == null)
            {
                return NotFound();
            }
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ModuleId,IsPrivate,Uid1,Uid2")] Group @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.UpdateGroupAsync(@group);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repo.GroupExists(@group.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ViewModule", "Module", new { id = group.ModuleId });
            }
            return View(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _repo.GetGroupAsync((int)id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @group = await _repo.GetGroupAsync(id);
            if (@group != null)
            {
                await _repo.DeleteGroupAsync(@group);
            }
            return RedirectToAction("ViewModule", "Module", new { id = group.ModuleId });
        }

        
    }
}
