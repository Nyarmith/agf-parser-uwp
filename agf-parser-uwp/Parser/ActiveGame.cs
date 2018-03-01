using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agf_parser_uwp.Parser
{
    //essentially the AdventureGame class from the python version
    class ActiveGame
    {
        private AdventureGame game; //adventure game data structure
        private string state;       //ptr to current state
        

        public string[] getChoices() {
            string[] ret = new string[] { "temp" };
            return ret;
        }

        public string getText() {
            return "temp";
        }
    }
}
