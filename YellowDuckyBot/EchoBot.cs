using System;
using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;

namespace YellowDuckyBot
{
    public class EchoBot : IBot
    {
        /// <summary>
        /// Every Conversation turn for our EchoBot will call this method. In here
        /// the bot checks the Activty type to verify it's a message, bumps the 
        /// turn conversation 'Turn' count, and then echoes the users typing
        /// back to them. 
        /// </summary>
        /// <param name="context">Turn scoped context containing all the data needed
        /// for processing this conversation turn. </param>        
        public async Task OnTurn(ITurnContext context)
        {
            // This bot is only handling Messages
            if (context.Activity.Type == ActivityTypes.Message)
            {
                // Get the conversation state from the turn context
                var state = context.GetConversationState<EchoState>();

                // Bump the turn count. 
                state.TurnCount++;

                //User has sent/asked
                Console.WriteLine($"User sent: {context.Activity.Text}");

                String response = "";
                switch (context.Activity.Text.ToLower())
                {
                    case "hello":
                        response = "Hello to You!";
                        break;

                    case "f**k you":
                        response = "..and F**k you too!";
                        break;

                    case "exit":
                        response = "Do I look like a Shell console?";
                        break;

                    case "quit":
                        response = "Do I look like a Unix console?";
                        break;

                    case "what is sense of life?":
                        response = "42. Read an Adam's book..";
                        break;

                    default:
                        response = "I didn't get this one. Can You repeat in simpler words.";
                        break;
                }

                // Echo back to the user whatever they typed.
                await context.SendActivity(response);//Turn {state.TurnCount}: 
            }
        }
    }    
}
