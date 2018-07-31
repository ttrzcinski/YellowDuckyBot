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
            var playingLycopersicon = false;
            var ping = false;

            //Prepared response to send back to user
            string response = null;//context.Activity.Text;

            // This bot is only handling Messages
            if (context.Activity.Type == ActivityTypes.Message)
            {
                //Read it as lower case - will be better
                var contextQuestion = context.Activity.Text.ToLower();
                
                //Old simple game of Lycopersicon
                if (playingLycopersicon)
                {
                    response = "Lycopersicon";
                    // TODO change it to game of Lycopersicon
                    /*
                     
                     case "let's play lycopersicon":
                            playingLycopersicon = true;
                            response = "Lycopersicon";
                            break;

                        case "lycopersicon":
                            playingLycopersicon = false;
                            response = "Ha ha, you lost.";
                            break;
 
                     */
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
                    //Console.WriteLine($"User sent: {context.Activity.Text}");
                    
                    //Check for add retort - from original, not lower case
                    if (context.Activity.Text.ToLower().StartsWith("simonsays"))
                    {
                        string result = AddRetort(context.Activity.Text);
                        if (result.StartsWith("Couldn't"))
                        {
                            response = "[ERROR] " + result;
                        } else
                        {
                            response = result;
                            await context.SendActivity(response);
                            return;
                        }
                    }

                    //
                    switch (contextQuestion)
                    {
                        case "hello":
                            response = "Hello to You!";
                            break;

                        // TODO Add some admin-mode with prior authorization
                        // TODO Add console entry level of extending retords
                        case "how many retorts?":
                            response = $"{mind.CountRetorts()} Retorts in my mind.";
                            break;

                        case "ping":
                            ping = true;
                            break;

                        case "roll d20":
                            Random random = new Random();
                            var lastRoll = random.Next(1, 20);
                            switch (lastRoll)
                            {
                                //this.count++;
                                case 1:
                                    response = $"You rolled {lastRoll}. Critical Failure!";
                                    break;
                                case 20:
                                    response = $"You rolled {lastRoll}. Critical Success!";
                                    break;
                                default:
                                    response = $"You rolled {lastRoll}.";
                                    break;
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
            if (ping)
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

        /// <summary>
        /// Calls the mind to add new retort to file.
        /// </summary>
        /// <param name="line">line to split and analyze</param>
        /// <returns>Added.. means that it worked, errors start with "Couldn't"</returns>
        private string AddRetort(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return "Couldn't add retort from empty line.";
            }

            var response = "";
            //Check for add retort - from original, not lower case
            if (line.ToLower().StartsWith("simonsays addretort "))
            {
                var split = line.Split(";");//
                //check size of 3 - simonsays addretort; question; answer
                // TODO uncomment it, if ti splits
                if (split.Length > 2)
                {
                    var added = mind.AddRetort(split[1], split[2]);
                    response = added
                        ? $"Added retort, ask for it with {split[1]}"
                        : $"Couldn't add retort with question {split[1]}";
                }
                else
                {
                    response =
                        "Couldn't add retort, because line is too short and doesn't follow pattern\n" +
                        "simonsays addretort; question; answer";
                }
            }
            //Returns a response, errors start with "Couldn't"
            return response;
        }
    }
}
