using System;
using agf_parser_uwp.Parser;

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
            while (choice < 1 && choice > files.Length)
            {
                res = prompt("Invalid File, Enter Valid Adventure # (starting from 1):\n" + String.Join("\n",files));
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
                ActiveGame ag = new ActiveGame(adv);
                while (!ag.isEnd())
                {
                    Console.WriteLine(ag.getText());
                    Console.WriteLine("--------");
                    string[] options = ag.getChoices();
                    foreach (string opt in options)
                    {
                        Console.WriteLine(opt);
                    }
                    int res = -1;
                    while (res<1 && res>options.Length)
                    {
                        string resp = prompt(String.Format("Choose an option [1-{0}]", options.Length));
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
