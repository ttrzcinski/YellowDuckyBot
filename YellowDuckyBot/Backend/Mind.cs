using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// Hardcoded path to fast retorts file.
        /// </summary>
        private const string RetortsFullPath = "C:\\vsproj\\YellowDuckyBot\\YellowDuckyBot\\YellowDuckyBot\\Backend\\Repository\\fast_retorts.json";
        
        /// <summary>
        /// Retorts as quick responses to questions.
        /// </summary>
        private List<Retort> _retorts;

        /// <summary>
        /// Retort's the highest id.
        /// </summary>
        private int _retortsMaxId = -1;

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
            
            using (var reader = new StreamReader(RetortsFullPath))
            {
                var json = reader.ReadToEnd();
                var items = JsonConvert.DeserializeObject<List<Retort>>(json);
                _retorts = items;
                
                //Refresh _retortsMaxId
                this.FindMaxRetortsId();
            }
        }

        /// <summary>
        /// Finds the highest id from retorts.
        /// </summary>
        private void FindMaxRetortsId()
        {
            _retortsMaxId = _retorts.Select(t => t.Id).OrderByDescending(t => t).FirstOrDefault() + 1;
            
            /*if (_retorts != null && _retorts.Count > 0)
            {
                foreach (var retort in _retorts)
                {
                    if (_retortsMaxId < retort.Id)
                    {
                        _retortsMaxId = retort.Id;
                    }   
                }
            }*/
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

            var added = new Retort {Id = _retortsMaxId + 1, Question = question, Answer = answer};

            // TODO Make a local copy of file faat retorts 

            bool endFlag;
            // Opens file of retorts for edit and add it at the end
            using (var writer = new StreamWriter(RetortsFullPath))
            {
                _retorts.Add(added);
                writer.Write(_retorts);
                writer.Flush();
                endFlag = true;
                _retortsMaxId++;
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
        
        /// <summary>
        /// Returns top id of all retorts.
        /// </summary>
        /// <returns>top id, if there are some retorts, on null or empty returns 0</returns>
        public int CountRetortsMaxId()
        {
            return _retorts?.Count ?? 0;
        }
    }
}
