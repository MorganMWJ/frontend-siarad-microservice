using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public class MessageClientService : IMessageClientService
    {
        private readonly IHttpClientFactory _factory;

        public MessageClientService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<Message>> GetMessagesAsync(int group_id)
        {
            List<Message> messages = new List<Message>();
            var client = _factory.CreateClient("MessageClient");
            HttpResponseMessage response = await client.GetAsync("/MessageStore/api/messages/group/" + group_id);
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                messages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Message>>(responseString);
            }
            else
            {
                //error message?
            }
            return messages;
        }

        public async Task<Message> GetMessageAsync(int id)
        {
            Message message = new Message();
            var client = _factory.CreateClient("MessageClient");
            HttpResponseMessage response = await client.GetAsync("/MessageStore/api/messages/" + id);
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                message = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(responseString);
            }
            else
            {
                //error message?
            }
            return message;
        }

        public async Task DeleteMessageAsync(int id)
        {
            var client = _factory.CreateClient("MessageClient");
            HttpResponseMessage response = await client.DeleteAsync($"/MessageStore/api/messages/{id}");
            if (!response.IsSuccessStatusCode)
            {
               //error message??
            }
        }

        public async Task PostMarkDeleteMessageAsync(int id)
        {
            var client = _factory.CreateClient("MessageClient");
            HttpResponseMessage response = await client.PostAsync($"/MessageStore/api/messages/delete/{id}",null);
            if (!response.IsSuccessStatusCode)
            {
                //error message??
            }
        }

        public async Task PutUpdateMessageAsync(Message message)
        {
            var client = _factory.CreateClient("MessageClient");
            HttpResponseMessage response = await client.PutAsJsonAsync($"/MessageStore/api/messages/{message.Id}", message);
            if (!response.IsSuccessStatusCode)
            {
                //error message
            }
        }

        public async Task PostCreateMessageAsync(Message message)
        {
            var client = _factory.CreateClient("MessageClient");
            HttpResponseMessage response = await client.PostAsJsonAsync("/MessageStore/api/messages", message);
            if (!response.IsSuccessStatusCode)
            {
                //error message
            }            
        }
    }
}
