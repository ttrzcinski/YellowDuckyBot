﻿using System;
using System.IO;
using System.Linq;
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
            var ping = false;

            //Prepared response to send back to user
            string response = null;//context.Activity.Text;

            // This bot is only handling Messages
            if (context.Activity.Type == ActivityTypes.Message)
            {
                //Read it as lower case - will be better
                var contextQuestion = context.Activity.Text.ToLower();
                
                //Old simple game of Lycopersicon
                var playingLycopersiconResult = mind.Facts.Read("playingLycopersicon");
                if (playingLycopersiconResult!= null 
                    && playingLycopersiconResult.Equals("true")                                  
                    && !contextQuestion.StartsWith("lycopersicon"))
                {
                    response = "Lycopersicon";
                    await context.SendActivity(response);
                    return;
                }
                else
                {
                    //Load retorts from JSON file
                    response = mind.Respond(contextQuestion);
                    if (response != null)
                    {
                        await context.SendActivity(response);
                        return;
                    }

                    // Get the conversation state from the turn context
                    var state = context.GetConversationState<EchoState>();

                    // Bump the turn count. 
                    state.TurnCount++;
                    
                    // Check for add retort - from original, not lower case
                    // TODO ADD ADMIN MODE
                    if (contextQuestion.StartsWith("simonsays"))
                    {
                        if (contextQuestion.StartsWith("simonsays addretort;"))
                        {
                            var split = context.Activity.Text.Split(";");
                            if (split.Length == 3)
                            {
                                if (split[1].Trim().Length > 0 && split[2].Trim().Length > 0)
                                {
                                    //TODO CHECK, IF RETORT ALREADY EXISTS
                                    var result = mind.AddRetort(split[1].Trim(), split[2].Trim());
                                    response = result
                                        ? $"Added new retort {split[1]}."
                                        : $"Couldn't add retort {split[1]}.";
                                }
                                else
                                {
                                    response = "One of parameters was empty.";
                                }
                            }
                            else
                            {
                                response = "It should follow pattern: simonsays addretort;question;answer";
                            }

                            await context.SendActivity(response);
                            return;
                        }
                        else if (contextQuestion.StartsWith("simonsays SPLIT;"))
                        {
                            var split = context.Activity.Text.Split(";");
                            foreach (var word in split)
                            {
                                //var result = AddRetort(context.Activity.Text);
                                response = word;
                                //response = result.StartsWith("Couldn't") ? "[ERROR] " + result : result;
                                await context.SendActivity(response);
                            }
                        }
                    }
                    
                    //Facts
                    // TODO Move this to mind
                    if (contextQuestion.Contains("fact"))
                    {
                        if (contextQuestion.StartsWith("addfact"))
                        {
                            var daFact = context.Activity.Text.Split(" ");
                            if (daFact.Length > 2)
                            {
                                // Omit first one as it is a command
                                var factName = daFact[1].ToLower();
                                var factValue = daFact[2];
                                //TODO Add processing and concatenation, if fact is longer, than just one word.
                                var result = mind.Facts.Add(factName, factValue);
                                response = result
                                    ? $"Fact {factName} was added."
                                    : $"Fact {factName} couldn't be added.";
                                await context.SendActivity(response);
                                return;
                            }
                        }
                        else if (contextQuestion.StartsWith("readfact"))
                        {
                            var daFact = contextQuestion.Split(" ");
                            if (daFact.Length == 2)
                            {
                                // Omit first one as it is a command
                                var factName = daFact[1];
                                var result = mind.Facts.Read(factName);
                                response = result != null
                                    ? $"Fact {factName} is {result}."
                                    : $"Fact {factName} doesn't exist.";
                                await context.SendActivity(response);
                                return;
                            }
                        }
                        else if (contextQuestion.StartsWith("forgetfact"))
                        {
                            var daFact = contextQuestion.Split(" ");
                            if (daFact.Length == 2)
                            {
                                // Omit first one as it is a command
                                var factName = daFact[1];
                                var result = mind.Facts.Remove(factName);
                                response = result
                                    ? $"Fact {factName} was forgotten."
                                    : $"Fact {factName} doesn't exist.";
                                await context.SendActivity(response);
                                return;
                            }
                        } 
                        else if (contextQuestion.StartsWith("countfact"))
                        {
                            var daFact = contextQuestion.Split(" ");
                            if (daFact.Length == 1)
                            {
                                // Omit first one as it is a command
                                var count = mind.Facts.Count();
                                response = $"Facts base contains {count} facts.";
                                await context.SendActivity(response);
                                return;
                            }
                        }
                    }

                    //
                    switch (contextQuestion)
                    {
                        case "hello":
                            response = "Hello to You!";
                            break;

                        // TODO Add some admin-mode with prior authorization
                        // TODO Add console entry level of extending retorts
                        case "how many retorts?":
                            response = $"I've {mind.CountRetorts()} retorts in my mind.";
                            break;

                        case "ping":
                            ping = true;
                            break;
                        
                        // TODO change it to game of Lycopersicon
                        case "let's play lycopersicon":
                            var playLycopersiconResult = mind.Facts.Add("playingLycopersicon","true");
                            response = playLycopersiconResult ? "Ok.. Lycopersicon" : "Hmm.. something is wrong wit that game.";
                            break;

                        case "lycopersicon":
                            playingLycopersiconResult = mind.Facts.Read("playingLycopersicon");
                            if (playingLycopersiconResult != null && playingLycopersiconResult.Equals("true"))
                            {
                                var stopPlayLycopersiconResult = mind.Facts.Remove("playingLycopersicon");
                                if (stopPlayLycopersiconResult)
                                {
                                    // TODO CANNOT STOP PLAYING..
                                    var playedLycopersiconResult = mind.Facts.Add("playedLycopersicon", "true");
                                    response = playedLycopersiconResult ? 
                                        "Ha ha, you lost. I'll remember that." 
                                        : "Ha ha, you lost... Wait, what just happened?";
                                }
                                else
                                {
                                    response = "I cannot stop.. Lycopersicon.";
                                }
                            }
                            else
                            {
                                playLycopersiconResult = mind.Facts.Add("playingLycopersicon","true");
                                response = playLycopersiconResult ? "Ok.. Lycopersicon" : "Hmm.. something is wrong wit that game.";
                            }
                            break;
                        
                        case "roll d20":
                            var lastRoll = new Random().Next(1, 20);
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
                            response = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                            break;

                        case "what do you see?":
                            //Environment.CurrentDirectory
                            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"wwwroot\");
                            var files = Directory.GetFiles(path);
                            response = $"Well, I see {files.Length} files around me.";
                            response = files.Aggregate(response, (current, filename) => current + $"\n {filename}");
                            //string[] files = File.ReadAllLines(path);
                            break;

                        default:
                            // TODO log down all given not recognized phrases in order to analyze them in the future and add new phrases
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
            }

            //Send response to user
            if (response != null)
            {
                await context.SendActivity(response); 
            }
        }
    }
}
