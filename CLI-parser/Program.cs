using System;
using System.Collections.Generic;
using agf_parser_uwp;

namespace CLI_parser
{
    class Program
    {
        static string prompt(string text = "")
        {
            Console.Write(text + "\n>");
            return Console.ReadLine();
        }

        static string fileDialogue()
        {
            //do a while loop, listing the .agf/.json files in the directory and returning its path when one is chosen
            string advDir = System.IO.Directory.GetCurrentDirectory() + "/Adventures/";
            string[] files = System.IO.Directory.GetFiles(advDir);
            string res = prompt("Enter Adventure # (starting from 1):\n" + String.Join("\n",files));
            int.TryParse(res, out int choice);
            while (choice < 1 || choice > files.Length)
            {
                res = prompt("Invalid File Number, Enter Valid Adventure # (starting from 1):\n" + String.Join("\n",files));
                int.TryParse(res, out choice);
            }
            return files[choice-1];
        }

        static void Main(string[] args)
        {
            while (true)
            {
                string filename = fileDialogue();
                AdventureGame adv = AdventureGame.loadFromFile(filename);
                string test = AdventureGame.saveToString(adv);
                ActiveGame ag = new ActiveGame(adv);
                while (!ag.isEnd())
                {
                    Console.WriteLine(ag.getText());
                    Console.WriteLine("--------");
                    List<string> options = ag.getChoices();
                    int optnum = 1;
                    foreach (string opt in options)
                    {
                        Console.WriteLine(Convert.ToString(optnum) + ") " + opt);
                        optnum++;
                    }
                    int res = -1;
                    while (res<1 || res>options.Count)
                    {
                        string resp = prompt(String.Format("Choose an option [1-{0}]", options.Count));
                        int.TryParse(resp, out res);
                    }
                    ag.choose(res - 1);
                }
                Console.WriteLine(ag.getText());
                Console.WriteLine("^^^^^^^^\n");
                if (ag.isWin())
                    Console.WriteLine("Congratulations, you've won!\n");
                else
                    Console.WriteLine("Unfortunately, it looks like you've hit a dead-end!\n");
            }
        }
    }
}
