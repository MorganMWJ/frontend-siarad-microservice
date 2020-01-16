using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Message
    {
        [JsonProperty("id", Order=1)]
        public virtual int Id { get; set; }

        [Required]
        [JsonProperty("body", Order = 2)]
        public virtual string Body { get; set; }

        [JsonProperty("ownerUid", Order = 3)]
        public virtual  string OwnerUid { get; set; }

        [Required]
        [JsonProperty("isDeleted", Order = 4)]
        public virtual bool IsDeleted { get; set; }

        [Required]
        [JsonProperty("timeCreated", Order = 5)]
        public virtual DateTime TimeCreated { get; set; }

        [Required]
        [JsonProperty("timeEdited", Order = 6)]
        public virtual DateTime TimeEdited { get; set; }

        [Required]
        [JsonProperty("groupId", Order = 7)]
        public virtual int GroupId { get; set; }

        [JsonProperty("messageCollection", Order = 8)]
        public virtual ICollection<Message> MessageCollection { get; set; }

        [JsonProperty("hasReplies", Order = 9)]
        public virtual bool HasReplies { get; set; }
    }
}
