using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agf_parser_uwp
{
    /* Ok, so we're not going to use a fileinfo class, we're going to use a custom class. GameInfo for a list of games (from the play menu),
     * ActivegameInfo for a list of info on activegames, and GameFile for the "browse local" and "import from local fileystem" options. */

    public class GameFile
    {
        public string fileName { get; set; }
        public string createDate { get; set; }
        public string modifyDate { get; set; }
        public GameFile(GameFile gf) : this(gf.fileName, gf.createDate, gf.modifyDate) { }
        public GameFile(string fn, string create, string modify)
        {
            fileName = fn;
            createDate = create;
            modifyDate = modify;
        }
    }

    public class GameInfo : GameFile
    {
        public string gameTitle  { get; set; }
        public string gameAuthor { get; set; }
        public int gameRating { get; set; } = 3;

        public GameInfo(string title, string author, GameFile gf) : base(gf)
        {
            gameTitle = title;
            gameAuthor = author;
        }

        public GameInfo(string title, string author, string filename,
            string createdate, string modifydate)  : 
            this(title, author,new GameFile(filename, createdate, modifydate))
        { }

    }

    /*
    public class ActiveGameInfo : GameInfo
    {
        public string name { get; set; }
        public ActiveGameInfo(string currentState)
    }
    */

}
