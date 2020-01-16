using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public interface IMessageClientService
    {
        Task<List<Message>> GetMessagesAsync(int group_id);
        Task<Message> GetMessageAsync(int id);
        Task DeleteMessageAsync(int id);
        Task PostMarkDeleteMessageAsync(int id);
        Task PutUpdateMessageAsync(Message message);

        Task PostCreateMessageAsync(Message message);

        Task PostUserAssociation(int id, string uid);

        Task PostUserAssociations(int id, string uidsCSV);
    }
}
