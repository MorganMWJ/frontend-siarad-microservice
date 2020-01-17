using System;

namespace WebApplication4.Models
{
    /*Sourced from the Webseries guide presented by Kudvenkat, referenced in the Group Report.*/
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}