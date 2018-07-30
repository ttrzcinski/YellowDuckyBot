using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using YellowDuckyBot.Backend;
using YellowDuckyBot.Backend.Model;

namespace YellowDuckyBot
{
    public class YellowDuckyBot : IBot
    {
        // Variables
        Mind mind = Mind.Instance;
        
        /// <summary>
        /// Every Conversation turn for our YellowDuckyBot will call this method. In here
        /// the bot checks the Activty type to verify it's a message, bumps the 
        /// turn conversation 'Turn' count, and then echoes the users typing
        /// back to them. 
        /// </summary>
        /// <param name="context">Turn scoped context containing all the data needed
        /// for processing this conversation turn. </param>        
        public async Task OnTurn(ITurnContext context)
        {
            // TODO: MOVE OUTSIDE THE CLASS IN MEMORY MODULE
            bool playingLycopersicon = false;
            bool ping = false;

            //Prepared response to send back to user
            String response = null;//context.Activity.Text;

            // This bot is only handling Messages
            if (context.Activity.Type == ActivityTypes.Message)
            {
                //Old simple game of Lycopersicon
                if (playingLycopersicon == true)
                {
                    response = "Lycopersicon";
                }
                else
                {
                    //Load retorts from JSON file
                    response = mind.Respond(context.Activity.Text.ToLower());
                    if (response != null)
                    {
                        await context.SendActivity(response);
                        return;
                    }

                    // Get the conversation state from the turn context
                    var state = context.GetConversationState<EchoState>();

                    // Bump the turn count. 
                    state.TurnCount++;

                    //User has sent/asked
                    Console.WriteLine($"User sent: {context.Activity.Text}");

                    //
                    switch (context.Activity.Text.ToLower())
                    {
                        case "hello":
                            response = "Hello to You!";
                            break;

                        // TODO Add some admin-mode with prior authorization
                        // TODO Add console entry level of extending retords
                        case "how many retorts?":
                            response = $"{mind.CountRetorts()} Retorts in my mind.";
                            break;

                        case "let's play lycopersicon":
                            playingLycopersicon = true;
                            response = "Lycopersicon";
                            break;

                        case "lycopersicon":
                            playingLycopersicon = false;
                            response = "Ha ha, you lost.";
                            break;

                        case "ping":
                            ping = true;
                            break;

                        case "roll d20":
                            Random random = new Random();
                            var lastRoll = random.Next(1, 20);
                            //this.count++;
                            if (lastRoll == 1)
                            {
                                response = $"You rolled {lastRoll}. Critical Failure!";
                            }
                            else if (lastRoll == 20)
                            {
                                response = $"You rolled {lastRoll}. Critical Success!";
                            }
                            else
                            {
                                response = $"You rolled {lastRoll}.";
                            }
                            break;

                        case "where are you?":
                            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                            response = path;
                            break;

                        /*case "what do you see?":
                            //Environment.CurrentDirectory
                            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"wwwroot\");
                            string[] files = Directory.GetFiles(path);
                            response = $"Well, I see {files.Length} files around me.";
                            foreach (string filename in files)
                            {
                                response += $"\n {filename}";
                            }
                            //string[] files = File.ReadAllLines(path);
                            break;*/

                        default:
                            response = "I didn't get this one. Can You repeat in simpler words.";
                            break;
                    }
                }

                // Echo back to the user whatever they typed.
                //await context.SendActivity(response);//Turn {state.TurnCount}: 
            }
            else if (context.Activity.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (context.Activity.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (context.Activity.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (context.Activity.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (context.Activity.Type == ActivityTypes.Ping)
            {
                ping = true;
            }

            //if pinged
            if (ping == true)
            {
                //Add responses in parts with delay in between
                response = "Ping..";
                await context.SendActivity(response);
                await Task.Delay(2000);//wait 2 seconds
                response = "Yup, I'm alive..";
                await context.SendActivity(response);
                await Task.Delay(1500);//wait 2 seconds
                response = "I mean On.";
                //await context.SendActivity(response);
            }

            //Send response to user
            if (response != null)
            {
                await context.SendActivity(response); //Turn {state.TurnCount}: 
            }
        }
    }
}
