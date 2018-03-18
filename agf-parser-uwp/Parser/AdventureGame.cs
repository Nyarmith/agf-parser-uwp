using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace agf_parser_uwp.Parser
{
    public class State
    {
        public string text;
        public string[][] transitions;
    }

	public class AdventureGame
	{
        public string title;
        public string author;
        public Dictionary<string, Dictionary<string,int>> gamevars;
        public string[] win_states;
        public string start_state;
        public Dictionary<string, State> states;

        public static AdventureGame loadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("Error, no file found at path: " + path);
            }
            string contents = "";
            contents = File.ReadAllText(path);

            return loadObject(contents);
        }

        public static AdventureGame saveToFile(AdventureGame game, string path)
        {
            if (!File.Exists(path))
            {
                string contents = saveObject(game);
                File.WriteAllText(path, contents);
            }
            throw new Exception("File already exists: " + path);
        }

        //these should be able to just load the guy from json
        public static AdventureGame loadObject(string json_str)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(json_str);
            writer.Flush();
            stream.Position = 0;

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(AdventureGame));
            return (AdventureGame)ser.ReadObject(stream);
        }

        public static string saveObject(AdventureGame adv_obj)
        {
            var stream = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(AdventureGame));
            ser.WriteObject(stream,adv_obj);

            return stream.ToString();
        }
    }
}