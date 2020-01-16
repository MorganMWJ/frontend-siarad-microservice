using System;
using System.Collections.Generic;
using System.Linq;
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

        public MessageController(IModuleClientService moduleClient, IMessageClientService messageClient, IDataRepository repo)
        {
            _moduleClient = moduleClient;
            _messageClient = messageClient;
            _repo = repo;
        }

        public async Task<IActionResult> ListMessagesForGroup(int group_id)
        {
            /* if any of these objects are null we need to do something about the error */
            Group group = await _repo.GetGroupAsync(group_id);
            group.Module = await _moduleClient.GetModuleAsync(group.ModuleId);
            group.Messages = await _messageClient.GetMessagesAsync(group_id);
            group.Messages.Sort((x, y) => DateTime.Compare(x.TimeCreated, y.TimeCreated));
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
            return RedirectToAction("ListMessagesForGroup", new { group_id });
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