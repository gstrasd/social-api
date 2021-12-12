using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Library.Dataflow;

namespace Social.Messages
{
    public class DiscoverTwitterAccountMessage : IMessage
    {
        [Required]
        [JsonPropertyName("correlationId")]
        public Guid CorrelationId { get; set; }
        
        [Required]
        [JsonPropertyName("providerId")]
        public string ProviderId { get; set; }
        
        [Required]
        [RegularExpression("^[a-zA-Z\\d_]{4, 15}$", ErrorMessage = "Twitter username must match expression ^[a-zA-Z\\d_]{4, 15}$")]
        [JsonPropertyName("twitterUsername")]
        public string TwitterUsername { get; set; }
    }
}