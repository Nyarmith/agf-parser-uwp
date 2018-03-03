using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace agf_parser_uwp.Parser
{
    //essentially the AdventureGame class from the python version
    class ActiveGame
    {
        private AdventureGame data; //adventure game data structure
        private string position;    //ptr to current state
        private Tuple<int, string>[] choices;  //[transition_id, transition_text], options for player after pruning
        private string text;        //current text after processing
        private Dictionary<string, Dictionary<string, int>> states;  //stuff like inventory::sword=true is here

        //==== Interface ====

        public void start()
        {
            //set initial position and process text based on state
        }

        public string[] getChoices()
        {
            string[] ret = null;   //is this valid?
            foreach (Tuple<int,string> e in choices)
            {
                ret.Append(e.Item2);
            }
            return ret;
        }

        public string getText()
        {
            return text;
        }

        public bool isEnd()
        {
            return (data.states[position].transitions.Length == 0);
        }

        public bool isWin()
        {
            return (Array.IndexOf(data.win_states, position) != -1);
        }

        public void choose(int c)
        {
            //translate choice c to og transition
            c = choices[c].Item1;
            string[] next_t = data.states[position].transitions[c];
            //TODO: Implement random transition check&choice here

            string pos = next_t[next_t.Length - 2];
            State nextNode = data.states[pos];

            //==== main transition steps ====
            execStmt(next_t[1]);
            pruneChoices(nextNode);
            processText(nextNode);
            position = pos;
        }


        //==== Helpers ====

        //execute setter in transition statement
        private void execStmt(string stmt)
        {
            
        }
        private void pruneChoices(State newState)
        {
            //prune the choices the user can pick
        }

        private void processText(State newState)
        {
            //process text based on env states
            XmlDocument xml = new XmlDocument();
            xml.LoadXml("<base>xml string</base>");
            //consider also: XmlReader
            //fuck I have no idea how to use the c# xml parser
        }
    }
}
