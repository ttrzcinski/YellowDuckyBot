using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using YellowDuckyBot.Backend.Model;

namespace YellowDuckyBot.Backend
{
    // Represents central hub of bot's capabilities and responses.
    /// <summary>
    /// Represents central hub of bot.
    /// </summary>
    public sealed class Mind
    {
        // TODO Should be externalized and references with relative path.
        
        /// <summary>
        /// Hardcoded path to fast retorts.
        /// </summary>
        private const string FullPath = "C:\\vsproj\\YellowDuckyBot\\YellowDuckyBot\\YellowDuckyBot\\Backend\\Repository\\fast_retorts.json";
        
        /// <summary>
        /// Retorts as quick responses to questions.
        /// </summary>
        private List<Retort> _retorts;

        /// <summary>
        /// Holds the only instance of this class.
        /// </summary>
        private static Mind _instance;

        /// <summary>
        /// Thread-safe lock of initialization
        /// </summary>
        private static readonly object Padlock = new object();

        /// <summary>
        /// Creates new instance of Mind.
        /// </summary>
        private Mind()
        {
            //Obtain retorts, if are not loaded
            if (_retorts == null)
            {
                LoadRetorts();
            }
        }

        /// <summary>
        /// Returns handle to existing instance with prior constructing it, if lacks one.
        /// </summary>
        public static Mind Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new Mind());
                }
            }
        }

        /// <summary>
        /// Reads JSON file with retorts.
        /// </summary>
        public void LoadRetorts()
        {
            // TODO: Fix the path top search from within the project
            // TODO: Change to relative path
            
            using (var reader = new StreamReader(FullPath))
            {
                var json = reader.ReadToEnd();
                var items = JsonConvert.DeserializeObject<List<Retort>>(json);
                _retorts = items;
            }
        }

        /// <summary>
        /// Goes through list of retorts in order to find the top id.
        /// </summary>
        /// <returns>the highest id of retorts</returns>
        private int GetMaxId()
        {
            //TODO move this class to load retorts
            var id = -1;
            if (_retorts == null || _retorts.Count == 0) return id;
            foreach (var retort in _retorts)
            {
                if (retort.Id > id)
                {
                    id = retort.Id;
                }
            }
            /*else
            {
                return id;
            }*/
            return id;
        }

        /// <summary>
        /// Adds new retort to retorts file.
        /// </summary>
        /// <param name="question">given question</param>
        /// <param name="answer">its answer</param>
        /// <returns>true means added, false means error</returns>
        public bool AddRetort(string question, string answer)
        {
            //Check, if params have content
            if (string.IsNullOrEmpty(question) || string.IsNullOrEmpty(answer))
            {
                return false;
            }

            var added = new Retort {Id = GetMaxId() + 1, Question = question, Answer = answer};

            // TODO Make a local copy of file faat retorts 

            bool endFlag;
            // TODO Open file of retorts for edit and add it at the end
            using (var writer = new StreamWriter(FullPath))
            {
                _retorts.Add(added);
                writer.Write(_retorts);
                writer.Flush();
                endFlag = true;
            }

            // TODO Add retort to retorts file

            return endFlag;
        }

        /// <summary>
        /// Searches for given questions in list of retorts and returns answer, if it exists.
        /// </summary>
        /// <param name="question">given question to bot</param>
        /// <returns>answer, if exists, null otherwise</returns>
        public string Respond(string question)
        {
            //Obtain retorts, if are not loaded
            if (_retorts == null)
            {
                LoadRetorts();
            }
            //
            string response = null;
            foreach (var retort in _retorts)
            {
                if (retort.Question.ToLower().Equals(question))
                {
                    response = retort.Answer;
                    break;
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
            return _retorts?.Count ?? 0;
        }
    }
}
