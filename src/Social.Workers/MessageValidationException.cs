using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Dataflow;

namespace Social.Workers
{
    internal class MessageValidationException : ApplicationException
    {
        public MessageValidationException(string message, IMessage queueMessage, List<ValidationResult> validationResults) : base(message)
        {
            QueueMessage = queueMessage;
            ValidationResults = validationResults;
        }

        public IMessage QueueMessage { get; }
        public List<ValidationResult> ValidationResults { get; set; }
    }
}