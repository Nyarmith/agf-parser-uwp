using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace agf_parser_uwp.Parser
{
    public class State
    {
        public string text;
        public List<List<string>> transitions = new List<List<string>>();

        public State() { }
        public State(string text, List<List<string>> transitions) { }
    }

	public class AdventureGame
	{
        public string title;
        public string author;
        public Dictionary<string, Dictionary<string,int>> gamevars = new Dictionary<string, Dictionary<string, int>>();
        public List<string> win_states = new List<string>();
        public string start_state;
        public Dictionary<string, State> states = new Dictionary<string, State>();

        public static AdventureGame loadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("Error, no file found at path: " + path);
            }
            string contents = "";
            contents = File.ReadAllText(path);

            return loadFromString(contents);
        }

        public static AdventureGame saveToFile(AdventureGame game, string path)
        {
            if (!File.Exists(path))
            {
                string contents = saveToString(game);
                File.WriteAllText(path, contents);
            }
            throw new Exception("File already exists: " + path);
        }

        //these should be able to just load the guy from json
        public static AdventureGame loadFromString(string json_str)
        {
            return new AdventureGame();
        }

        public static string saveToString(AdventureGame adv_obj)
        {
            JsonSerializer ser = new JsonSerializer();
            ser.Serialize(adv_obj);
            /*
             * 
             * output looks like this so far: {"author":"sergey","gamevars":[],"start_state":"question","states":[],"title":"basic addition","win_states":["right_result"]}
             * Ugh, next task is to define serialization correctly, which will require reading about it.
             */
            return "";
        }
    }
}