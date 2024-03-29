﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data;
using WebApplication4.Models;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    public class MessageController : Controller
    {
        private readonly IModuleClientService _moduleClient;
        private readonly IMessageClientService _messageClient;
        private readonly IDataRepository _repo;
        private readonly IHttpClientFactory _factory;

        public MessageController(IModuleClientService moduleClient, IMessageClientService messageClient, IDataRepository repo, IHttpClientFactory factory)
        {
            _moduleClient = moduleClient;
            _messageClient = messageClient;
            _repo = repo;
            _factory = factory;
        }

        public async Task<IActionResult> ListMessagesForGroup(int group_id)
        {
            /* if any of these objects are null we need to do something about the error */
            Group group = await _repo.GetGroupAsync(group_id);
            group.Module = await _moduleClient.GetModuleAsync(group.ModuleId);
            group.Messages = await _messageClient.GetMessagesAsync(group_id);
            group.Messages.Sort((x, y) => DateTime.Compare(x.TimeCreated, y.TimeCreated));

            if (group.IsPrivate && !User.IsInRole("Admin"))
            {
                var userOneValid = group.Uid1.Equals(User.Identity.Name);
                var userTwoValid = group.Uid2.Equals(User.Identity.Name);
                if(userOneValid == false && userTwoValid == false)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
            }
            if (!User.IsInRole("Admin"))//If the user is not an admin do
            {
                //Duplicate method from ModuleController serves as a suitable method for restricting URL insertion to navigate to unauthorised pages
                var client = _factory.CreateClient("ModuleClient");
                HttpResponseMessage response;
                List<StaffAndStudentModel> staffAndStudentList = new List<StaffAndStudentModel>();
                if (User.IsInRole("Student"))//If student
                {
                    response = await client.GetAsync($"/api/modules/{group.ModuleId}/students"); //Get a list of students
                }
                else if (User.IsInRole("Staff")) //If staff
                {
                    response = await client.GetAsync($"/api/modules/{group.ModuleId}/staff"); //Get a list of staff
                }
                else //If other
                {
                    response = null; //Create null to catch later
                }
                try
                {
                    if (response.IsSuccessStatusCode)//If successfully got data
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        staffAndStudentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StaffAndStudentModel>>(responseString);//Deserialise as model

                        var result = staffAndStudentList.Find(e => e.Uid.Equals(User.Identity.Name)); //Try to find the user within the list
                        if (result == null) //If the user is not there
                        {
                            return RedirectToAction("AccessDenied", "Account"); //Access denied
                        }
                    }
                }
                catch (NullReferenceException e) //Catch from earlier, if other user (No role assigned)
                {
                    //This will only ever be called if the user is unauthorised.
                    return RedirectToAction("AccessDenied", "Account"); //Access denied
                }
            }
            return View(group);
        }

        public async Task<IActionResult> Create(string body, int group_id)
        {
            Message message = new Message();
            message.Body = body;
            message.GroupId = group_id;
            message.HasReplies = false;
            message.IsDeleted = false;
            message.OwnerUid = User.Identity.Name;
            message.TimeCreated = DateTime.Now;
            message.TimeEdited = DateTime.Now;
            message.MessageCollection = new List<Message>();

            await _messageClient.PostCreateMessageAsync(message);

            /* Get group */
            Group group = await _repo.GetGroupAsync(group_id);

            /* Get Module */
            ModuleViewModel module = await _moduleClient.GetModuleAsync(group.ModuleId);

            /* Create associations for users in same group as message */
            if (group.IsPrivate)
            {
                //create single association for uid1 & uid2
                await _messageClient.PostUserAssociation(message.Id, group.Uid1);
                await _messageClient.PostUserAssociation(message.Id, group.Uid2);
            }
            else
            {
                /* Get users on module */
                List<StaffAndStudentModel> usrs = await _moduleClient.GetUsersOnModuleListAsync(module.Id);
                if(usrs.Count > 0)
                {
                    /* Create associations for users */
                    await _messageClient.PostUserAssociations(message.Id, getUids(usrs));
                }
            }

            return RedirectToAction("ListMessagesForGroup", new { group_id });
        }

        private string getUids(List<StaffAndStudentModel> usrs)
        {
            string res = "";
            foreach (StaffAndStudentModel user in usrs)
            {
                res += user.Uid;
                res += ",";
            }
            res = res.Remove(res.Length - 1);
            return res;
        }

        public async Task<IActionResult> Edit(string body, int id, int group_id)
        {
            Message message = await _messageClient.GetMessageAsync(id);
            message.Body = body;
            await _messageClient.PutUpdateMessageAsync(message);       
            return RedirectToAction("ListMessagesForGroup", new { group_id });
        }

        /**
         * Mark message as deleted in message store.
         */
        public async Task<IActionResult> Delete(int id, int group_id)
        {
            await _messageClient.PostMarkDeleteMessageAsync(id);
            return RedirectToAction("ListMessagesForGroup", new { group_id });
        }
    }
}