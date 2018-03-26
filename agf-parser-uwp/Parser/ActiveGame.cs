using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AgfLang;
using agf_parser_uwp;

namespace agf_parser_uwp
{
    //essentially the AdventureGame class from the python version

    public class ActiveGame
    {
        private AdventureGame data;
        private string position;
        private List<Tuple<int, string>> choices = new List<Tuple<int, string>>();   //[transition_id, transition_text], options after pruning
        private Dictionary<string, Dictionary<string, int>> states = new Dictionary<string, Dictionary<string, int>>();

        private AgfLang.AgfInterpreter interp;
        private string text;  //after processing

        public ActiveGame(AdventureGame adventure_)
        {
            data = adventure_;
            interp = new AgfInterpreter(ref states);
            start();
        }

        private void start()
        {
            //set initial position and process text based on state
            position = data.start_state;
            //load gamevars
            foreach (string key1 in data.gamevars.Keys)
            {
                foreach (string key2 in data.gamevars[key1].Keys)
                {
                    if (!states.ContainsKey(key1))
                        states[key1] = new Dictionary<string, int>();
                    object st8 = data.gamevars[key1][key2];
                    if (st8 is bool)
                        states[key1][key2] = (bool)st8 == true ? 1 : 0;
                    else if (st8 is int)
                        states[key1][key2] = (int)st8;
                }
            }
            //load relevant gamevars
            State cur = data.states[position];
            pruneChoices(cur);
            processText(cur);
        }

        //==== Interface ====
        public List<string> getChoices()
        {
            List<string> ret = new List<string>();   //is this valid?
            foreach (Tuple<int,string> e in choices)
            {
                ret.Add(e.Item2);
            }
            return ret;
        }

        public string getText()
        {
            return text;
        }

        public bool isEnd()
        {
            return (data.states[position].options.Count == 0);
        }

        public bool isWin()
        {
            return (data.win_states.IndexOf(position) != -1);
        }

        public void choose(int c)
        {
            //translate choice c to og transition
            c = choices[c].Item1;
            List<string> next_t = data.states[position].options[c];
            //TODO: Implement random transition check&choice here

            string pos = next_t[next_t.Count - 2];
            State nextNode = data.states[pos];

            //==== main transition steps ====
            execStmt(next_t[1]);
            pruneChoices(nextNode);
            processText(nextNode);
            position = pos;
        }


        //==== Helpers ====

        //TODO: Actually parse these correctly with tokenizing and explicit production rules
        //execute setter in transition statement

        private void execStmt(string stmt)
        {
            if (stmt != "")
                interp.exec(stmt);
        }

        private int evalStmt(string stmt)
        {
            if (stmt != "")
                return Convert.ToInt32(interp.eval(stmt));
            else
                return 1;
        }

        private void pruneChoices(State newState)
        {
            List<List<string>> ch = newState.options;
            choices = new List<Tuple<int, string>>();
            for (int i=0; i<ch.Count; ++i)
            {
                List<string> c = ch[i];
                if (c[0] == "" || evalStmt(c[0]) != 0)
                {
                    choices.Add( new Tuple<int, string>(i, c.Last()) );
                }
            }
        }

        private void processText(State newState)
        {
            //process text based on env states
            XmlDocument xml = new XmlDocument();
            xml.LoadXml("<base>"+newState.text+"</base>");
            //parseXML(xml.GetElementsByTagName("base")[0]);
            text = parseXML(xml.DocumentElement);
        }

        //if in this method, assume the condition is true
        private string parseXML(XmlNode node)
        {
            string ret = "";

            foreach (XmlNode e in node.ChildNodes)
            {
                switch (e.Name)  //which xml tag are we?
                {
                    case "#text":
                        ret += e.InnerText;
                        break;
                    case "cond":
                        XmlAttributeCollection col = e.Attributes;
                        XmlAttribute a =(XmlAttribute)col.GetNamedItem("expr");
                        if (evalStmt(a.Value) != 0)
                            ret += parseXML(e);
                        break;
                    default:
                        throw new Exception("Unrecognized xml tag encountered while parsing: " + e.Name);
                }
            }
            return ret;
        }
    }
}
