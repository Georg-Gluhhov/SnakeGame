using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;


namespace Snäke
{
    class Program
    {
        
        static void Main()
        {
            List<HighScore> highScores = new List<HighScore>();
            Console.SetWindowSize(120, 25);
            Console.Clear();
            try
            {
                // Load high scores
                using (StreamReader reader = new StreamReader("highscores.txt"))
                {
                string line;
                while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(',');
                        highScores.Add(new HighScore { PlayerName = parts[0], Score = int.Parse(parts[1]) });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            var UserInputContinue = 'y';
            while(UserInputContinue != 'n')
            {
                try{
                Console.Clear();
                Console.WriteLine("Please Choose symbol of your snake");
                char UserInputSkin = Console.ReadLine().ToString()[0];
                Console.WriteLine("Please Enter your name");
                string UserInputName = Console.ReadLine();
                Console.Clear();
                Game.RunGame(highScores, UserInputSkin, UserInputName);
                Console.Clear();
                Console.WriteLine("Want to continue? Y/n");
                UserInputContinue = Console.ReadLine().ToString()[0];
                }
                catch(Exception ex){
                    Console.WriteLine("s", ex);
                }
            }
        }
    } 
}
