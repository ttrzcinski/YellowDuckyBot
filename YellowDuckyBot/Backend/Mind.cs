using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using YellowDuckyBot.Backend.Model;

namespace YellowDuckyBot.Backend
{
    // Represents central hub of bot's capabilities and responses.
    /// <summary>
    /// Represents central hub of bot.
    /// </summary>
    public sealed class Mind
    {
        /// <summary>
        /// Retorts as quick responses to questions.
        /// </summary>
        private List<Retort> retorts;

        /// <summary>
        /// Holds the only instance of this class.
        /// </summary>
        private static Mind instance = null;

        /// <summary>
        /// Thread-safe lock of initialization
        /// </summary>
        private static readonly object padlock = new object();

        /// <summary>
        /// Creates new instance of Mind.
        /// </summary>
        private Mind()
        {
            //Obtain retorts, if are not loaded
            if (retorts == null)
            {
                LoadRetorts();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Mind Instance
        {
            get
            {
                lock (padlock)
                { 
                    if (instance == null)
                    {
                        instance = new Mind();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Reads JSON file with retorts.
        /// </summary>
        public void LoadRetorts()
        {
            //TODO: Fix the path top search from within the project
            //TODO: Change to relative path
            string fullPath = "C:\\vsproj\\YellowDuckyBot\\YellowDuckyBot\\YellowDuckyBot\\Backend\\Repository\\fast_retorts.json";
            using (StreamReader r = new StreamReader(fullPath))
            {
                string json = r.ReadToEnd();
                List<Retort> items = JsonConvert.DeserializeObject<List<Retort>>(json);
                retorts = items;
            }
        }

        /// <summary>
        /// Searches for given questions in list of retorts and returns answer, if it exists.
        /// </summary>
        /// <param name="question"></param>
        /// <returns>answer, if exists, null otherwise</returns>
        public String Respond(String question)
        {
            //Obtain retorts, if are not loaded
            if (retorts == null)
            {
                LoadRetorts();
            }
            //
            String response = null;
            for (var i = 0; i < retorts.Count; i++)
            {
                if (retorts[i].Question.ToLower().Equals(question))
                {
                    response = retorts[i].Answer;
                    return response;
                }
            }
            //Return found response
            return response;
        }

        /// <summary>
        /// Returns count of retorts.
        /// </summary>
        /// <returns>counts, if there are some retorts, on null returns 0</returns>
        public int CountRetorts()
        {
            return retorts != null ? retorts.Count : 0;
        }
    }
}
