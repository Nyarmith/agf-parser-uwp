using Microsoft.VisualStudio.TestTools.UnitTesting;
using agf_parser_uwp;
using System.Collections.Generic;

namespace agf_parser_unit_tests
{
    [TestClass]
    public class BasicUnitTest
    {
        [TestMethod]
        public void SaveLoadTest()
        {
            string basic_game_text = @"{
  ""title"":""Your Aventure!"",
  ""author"":""defaultAuthor"",
  ""gamevars"" : { ""inventory"":{ ""baseball"":true, ""bat"":true}, ""user"":{ } },
  ""win_states"" : [""right_result""],
  ""start_state"" : ""question"",
  ""states"" : {

    ""question"" : {""text"":""do you know what 2+2 is?"", ""options"":[
      ["""","""",""right_result"",""Yes, 4""],
     ["""","""",""wrong_result"",""Yes, 3""],
      ["""",""user::error=True"",""answer"",""No Just Tell Me""]
    ]},

    ""right_result"" : {
      ""text"":""you are correct! <cond expr='user::error'>Good job fixing your error!</cond> "",
      ""options"":[]
},

    ""wrong_result"" : {""text"":""you are wrong!"", ""options"":[
        ["""",""user::error=true"",""question"",""Oops!""]]},

    ""answer"":{""text"":""it is 4!"", ""options"":[
        ["""","""",""question"",""Ok let’s try again""]
      ]}
    }
}";

            AdventureGame ag = new AdventureGame();
            ag = AdventureGame.loadFromString(basic_game_text);
            List<List<string>> transition_test_thing = new List<List<string>>();
            transition_test_thing.Add(new List<string>{ "pls", "work", "another", "time"});
            ag.states.Add("test_insert_state",new State("flavor texto", transition_test_thing));
            ag.gamevars["ayy"] = new System.Collections.Generic.Dictionary<string, object>();
            ag.gamevars["ayy"]["lmao"] = 23;
            string outStr = AdventureGame.saveToString(ag);
            System.Console.WriteLine(outStr);
            Assert.AreNotEqual(outStr, ""); //very good test
        }
    }
}
