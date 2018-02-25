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
        string text;
        string[][] options;
    }

	public class AdventureGame
	{
        string title;
        string author;
        Dictionary<string, State> gamevars;
        string[] win_states;
        string start_state;
        //make new obj type for satates
        Dictionary<string, Dictionary<string, string>> states;



		public AdventureGame ()
		{
            //idk do something?
        }

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