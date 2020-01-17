using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

//https://www.youtube.com/watch?v=TzhqymQm5kw
{
    [Authorize(Roles = "Admin")]
    //Authorize(Roles="Admin,User")] Either role can access

    /*
     *  [Authorize(Roles ="Admin")]
     *   [Authorize(Roles ="User")]
     *   Both roles are required to access
     */
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpClientFactory _factory;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IHttpClientFactory factory)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _factory = factory;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }
        [HttpGet]

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"{id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name

            };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Forename = user.Forename,
                Surname = user.Surname,
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var client = _factory.CreateClient("ModuleClient");
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                model.UserName = user.UserName; //Model requires the username incase of re-render
                user.Forename = model.Forename;
                user.Surname = model.Surname;
                var studentOrStaffModel = new StaffAndStudentModel()
                {
                    Uid = user.UserName,
                    Forename = user.Forename,
                    Surname = user.Surname
                };
                if (await userManager.IsInRoleAsync(user, "Student"))
                {
                    HttpResponseMessage response = await client.PutAsJsonAsync($"/api/students/{user.UserName}", studentOrStaffModel);
                }
                if (await userManager.IsInRoleAsync(user, "Staff"))
                {
                    HttpResponseMessage response = await client.PutAsJsonAsync($"/api/staff/{user.UserName}", studentOrStaffModel);
                }
                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var client = _factory.CreateClient("ModuleClient");
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                if (await userManager.IsInRoleAsync(user, "Student"))
                {
                    HttpResponseMessage response = await client.DeleteAsync($"/api/students/{user.UserName}");
                }
                if (await userManager.IsInRoleAsync(user, "Staff"))
                {
                    HttpResponseMessage response = await client.DeleteAsync($"/api/staff/{user.UserName}");
                }
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("ListUsers");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("ListRoles");
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };   
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            var client = _factory.CreateClient("ModuleClient");
            

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                var staffAndStudent = new StaffAndStudentModel
                {
                    Forename = user.Forename,
                    Surname = user.Surname,
                    Uid = user.UserName
                };
                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                    if (role.Name.Equals("Student"))
                    {
                        HttpResponseMessage response = await client.PostAsJsonAsync($"/api/students", staffAndStudent);
                    }
                    else if (role.Name.Equals("Staff"))
                    {
                        HttpResponseMessage response = await client.PostAsJsonAsync($"/api/staff", staffAndStudent);
                    }
                } else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                    if (role.Name.Equals("Student"))
                    {
                        HttpResponseMessage response = await client.DeleteAsync($"/api/students/{staffAndStudent.Uid}");
                    }
                    else if (role.Name.Equals("Staff"))
                    {
                        HttpResponseMessage response = await client.DeleteAsync($"/api/staff/{staffAndStudent.Uid}");
                    }
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach(var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var client = _factory.CreateClient("ModuleClient");
            var user = await userManager.FindByIdAsync(userId);
            var staffAndStudent = new StaffAndStudentModel
            {
                Forename = user.Forename,
                Surname = user.Surname,
                Uid = user.UserName
            };

            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            foreach (var role in roles)
            {
                Debug.WriteLine("\n" + role + "\n");
                if (role.Equals("Student"))
                {
                    HttpResponseMessage response = await client.DeleteAsync($"/api/students/{staffAndStudent.Uid}");
                    if (!response.IsSuccessStatusCode)
                    {
                        return View(response.Content);
                    }
                }
                else if (role.Equals("Staff"))
                {
                    HttpResponseMessage response = await client.DeleteAsync($"/api/staff/{staffAndStudent.Uid}");
                }
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            var roleModel = model.Where(x => x.IsSelected).Select(y => y.RoleName);
            foreach (var roleTwo in roleModel)
            {

                if (roleTwo.Equals("Student"))
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync($"/api/students", staffAndStudent);
                }
                if (roleTwo.Equals("Staff"))
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync($"/api/staff", staffAndStudent);
                }
            }
            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }
    }
}