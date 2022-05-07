using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBotInConsole
{
    public class EchoBot : IBot
    {
        //every conversation turn calls this methos. Here bot checks activity i smessage and echoes back
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            if(turnContext.Activity.Type == ActivityTypes.Message && !string.IsNullOrEmpty(turnContext.Activity.Text))
            {
                //check if its a quit message
                if (turnContext.Activity.Text.Equals("quit", StringComparison.InvariantCultureIgnoreCase))
                {
                    //send a reply
                    await turnContext.SendActivityAsync($"See you later...", cancellationToken: cancellationToken);
                    System.Environment.Exit(0);
                }
                else
                {
                    //echo back
                    await turnContext.SendActivityAsync("You said : " + turnContext.Activity.Text, cancellationToken: cancellationToken);
                }
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected..", cancellationToken: cancellationToken);
            }
        }
    }
}
