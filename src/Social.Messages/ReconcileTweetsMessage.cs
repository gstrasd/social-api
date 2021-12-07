using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Library.Dataflow;
using Library.Platform.Queuing;

namespace Social.Messages
{
    public class ReconcileTweetsMessage : IMessage
    {
        [JsonPropertyName("correlationId")]
        public Guid CorrelationId { get; set; }
        [JsonPropertyName("providerId")]
        public string ProviderId { get; set; }
        [JsonPropertyName("twitterAccount")]
        public string TwitterUsername { get; set; }
    }
}