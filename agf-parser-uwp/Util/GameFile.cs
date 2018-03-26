using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agf_parser_uwp
{
    /* Ok, so we're not going to use a fileinfo class, we're going to use a custom class. GameInfo for a list of games (from the play menu),
     * ActivegameInfo for a list of info on activegames, and GameFile for the "browse local" and "import from local fileystem" options. */

    public class GameInfo
    {
        public string name { get; set; }
        public GameInfo() { name = "GameInfo";  }
    }

    public class ActiveGameInfo
    {
        public string name { get; set; }
        public ActiveGameInfo() { name = "ActiveGameInfo"; }
    }

    public class GameFile
    {
        public string name { get; set; }
        public GameFile() { name = "GameFile"; }
    }
}
