using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace agf_parser_uwp
{
    public class State
    {
        public string text;
        public List<List<string>> options = new List<List<string>>();

        public State() { }
        public State(string text, List<List<string>> transitions) { }
    }

    public class AdventureGame
    {
        public string title;
        public string author;
        public Dictionary<string, Dictionary<string, object>> gamevars = new Dictionary<string, Dictionary<string, object>>();
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

        public static AdventureGame loadFromString(string json_str)
        {
            AdventureGame ag = JsonConvert.DeserializeObject<AdventureGame>(json_str);
            return ag;
        }

        public static string saveToString(AdventureGame adv_obj)
        {
            string ser = JsonConvert.SerializeObject(adv_obj);
            return ser;  //let's assume this works for now then fix it later
        }
    }
}

/* ==== Stuff Like This Works ====
 * 
            dynamic ser = JsonConvert.DeserializeObject<dynamic>(json_str);
            Dictionary<string, Dictionary<string, dynamic>> res = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, dynamic>>>("{ \"inventory\":{ \"baseball\":true, \"bat\":true}, \"user\":{ } }");
            foreach (string key1 in res.Keys)
            {
                foreach(string key2 in res[key1].Keys)
                {
                    if (!ag.gamevars.ContainsKey(key1))
                        ag.gamevars[key1] = new Dictionary<string, int>();
                    dynamic obstate = res[key1][key2];
                    if (obstate is bool)
                        ag.gamevars[key1][key2] = (bool)obstate == true ? 1 : 0;
                    else if (obstate is int)
                        ag.gamevars[key1][key2] = (int)obstate;
                    else
                        throw new Exception("Invalid gamevar type for variable " + key1 + "::" + key2);
                }
            }
 *
 */

    /*
     * 
     * 
            string xstns = @"{ ""text"":""do you know what 2+2 is?"", ""options"":[
       ["","",""right_result"",""Yes, 4""],
     ["""","""",""wrong_result"",""Yes, 3""],
      ["""",""user::error=True"",""answer"",""No Just Tell Me""] ] }";

            State res = JsonConvert.DeserializeObject<State>(xstns);
            *
            * another test
            */