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
        public string modifyDate { get; set; }
        public string downloadDate { get; set; }
        public string playDate { get; set; }
        public string author { get; set; }
        public GameInfo() { name = "GameInfo";  }
        public GameInfo(string name_, string modifyDate_, string downloadDate_, string playDate_, string author_)
        {
            name = name_;
            modifyDate = modifyDate_;
            downloadDate = downloadDate_;
            playDate = playDate_;
            author = author_;
        }
    }

    public class ActiveGameInfo
    {
        public string name { get; set; }
        public ActiveGameInfo() { name = "ActiveGameInfo"; }
    }

    public class GameFile
    {
        public string name { get; set; }
        public string modifyDate { get; set; }
        public string downloadDate { get; set; }
        public GameFile() { name = "GameFile"; }
        public GameFile(string name_, string modifyDate_, string downloadDate_)
        {
            name = name_;
            modifyDate = modifyDate_;
            downloadDate = downloadDate_;
        }
    }
}
