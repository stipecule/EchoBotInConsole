using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoBotInConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hi, I will repeat your text,please dont write funny things...!");
            Console.WriteLine("Oh yeah, say \"quit\" to end...");
            ///Create console adapter,and add Conversation state to the bot.The conv.state is saved in memory
            var adapter = new ConsoleAdapter();
            var echoBot = new EchoBot();
            //connect console adapter to bot
            adapter.ProcessActivtyAsync(
                async (turnContext, cancellationToken) => await echoBot.OnTurnAsync(turnContext)).Wait();

        }
    }
}
