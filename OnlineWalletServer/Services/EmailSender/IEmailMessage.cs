using System.Collections;
using System.Collections.Generic;

namespace Services.EmailSender
{
    interface IEmailMessage
    {
        IEnumerable<string> From { get; }

        IEnumerable<string> To { get; }

        string Subject { get; }

        string Body { get; }
    }
}
