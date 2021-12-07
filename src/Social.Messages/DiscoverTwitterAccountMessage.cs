using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Library.Dataflow;

namespace Social.Messages
{
    public class DiscoverTwitterAccountMessage : IMessage
    {
        [JsonPropertyName("correlationId")]
        public Guid CorrelationId { get; set; }
        [JsonPropertyName("providerId")]
        public string ProviderId { get; set; }
        [JsonPropertyName("twitterUsername")]
        public string TwitterUsername { get; set; }
    }
}