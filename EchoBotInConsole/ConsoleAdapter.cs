﻿using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBotInConsole
{
  
    public class ConsoleAdapter : BotAdapter
    {
        public ConsoleAdapter() : base() ///initialize base class when drived is initialized
        {

        }
        //performs the actual translation of input coming from the console
        //into the 'activity' format that the Bot consumes...
        public async Task ProcessActivtyAsync(BotCallbackHandler callback = null)
        {
            while (true)
            {
                var msg = Console.ReadLine();
                if (msg == null)
                {
                    break;
                }
                //all processing is performed by the broader bot pipeline on the Activity object
                var activity = new Activity()
                {
                    Text = msg,
                    //bot framework channel is identified by uniwe id.
                    //skype is a common channel to represent skype service.
                    //we are inventing a new channel here.
                    ChannelId = "console",
                    From = new ChannelAccount(id: "user", name: "User1"),
                    Recipient = new ChannelAccount(id: "bot", name: "Bot"),
                    Conversation = new ConversationAccount(id: "Convo1"),
                    Timestamp = DateTime.UtcNow,
                    Id = Guid.NewGuid().ToString(),
                    Type = ActivityTypes.Message
                };
                using(var context = new TurnContext(this, activity))
                {
                    await this.RunPipelineAsync(context, callback, default(CancellationToken)).ConfigureAwait(false);
                }
            }
        }
        //sends activities to the conversation...
        public override async Task<ResourceResponse[]> SendActivitiesAsync(ITurnContext turnContext, Activity[] activities, CancellationToken cancellationToken)
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }
            if (activities == null)
            {
                throw new ArgumentNullException(nameof(activities));
            }
            if (activities.Length == 0)
            {
                throw new ArgumentException("Expecting one or more activities, but the array was empty",nameof(activities));
            }
            var responses = new ResourceResponse[activities.Length];
            for(var index = 0; index < activities.Length; index++)
            {
                var activity = activities[index];
                switch (activity.Type)
                {
                    case ActivityTypes.Message:
                        {
                            IMessageActivity message = activity.AsMessageActivity();
                            if(message.Attachments!=null && message.Attachments.Any())
                            {
                                var attachment = message.Attachments.Count == 1 ? "1 attachment" : $"{message.Attachments.Count()} attachments";
                                Console.WriteLine($"{message.Text} with {attachment}");
                            }
                            else
                            {
                                Console.WriteLine($"{message.Text}");
                            }
                        }
                        break;

                    case ActivityTypesEx.Delay:
                        {
                            int delayMs =(int)((Activity)activity).Value;
                            await Task.Delay(delayMs).ConfigureAwait(false);
                        }
                        break;

                    case ActivityTypes.Trace:
                        break;

                    default:
                        Console.WriteLine("Bot : activity type: {0}", activity.Type);
                        break;
                }
                responses[index] = new ResourceResponse(activity.Id);
            }
        return responses;
        }

        public override Task<ResourceResponse> UpdateActivityAsync(ITurnContext turnContext, Activity activity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteActivityAsync(ITurnContext turnContext, ConversationReference reference, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

      
      
    }
}
