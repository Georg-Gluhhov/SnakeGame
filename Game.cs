using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using System.IO;
using System.Media;
using System.Security.Policy;
using System.Net.NetworkInformation;

namespace Snäke
{
    public class Game
    {
        static List<HighScore> highScores = new List<HighScore>();
        public static void RunGame(List<HighScore> hs, char UserInputSkin, string UserInputName){
            highScores = hs;
            Walls walls = new Walls(80, 25);
            walls.Draw();

            List<Point> TrapList = new List<Point>();
            List<Point> PowerUpList = new List<Point>();

            Console.SetCursorPosition(86, 8);
            Console.WriteLine("Press enter to start a game");
            Console.SetCursorPosition(86, 9);
            Console.WriteLine("Press esc to exit");
            ConsoleKeyInfo cki = Console.ReadKey(true);

            Console.SetCursorPosition(86, 9);
            switch (cki.Key)
            {
                case ConsoleKey.Enter:
                    Console.SetCursorPosition(86, 8);
                    Console.WriteLine("                               ");
                    Console.SetCursorPosition(86, 9);
                    Console.WriteLine("                               ");
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
            Point p = new Point(50, 10, UserInputSkin);
            Snäke snake = new Snäke(p, 4, Direction.LEFT);
            Console.SetCursorPosition(86, 12);
            snake.Draw();
            
            DynamicPointCreator dynamicPoint = new DynamicPointCreator(80, 25);
            Point food = dynamicPoint.CreatePoint('$');
            Console.ForegroundColor = ConsoleColor.Green; 
            food.Draw();

            Console.ResetColor(); 

            int Times = 100;

            int Count = 0;
            WritePoints(Count);
            while (true)

            {
                Random rnd1 = new Random();
                if (walls.IsHit(snake)  || snake.IsHitTail() || snake.Touch(TrapList))
                {
                    WriteGameOver(Count, UserInputName);


                    break;
                }
                if (snake.Touch(food))
                {

                    //Food Draw
                    food = dynamicPoint.CreatePoint('$');
                    Console.ForegroundColor = ConsoleColor.Green; 
                    food.Draw();
                    Times -= 5;
                    Count += 1;
                    WritePoints(Count);

                    //Trap Draw
                    Point trap = dynamicPoint.CreatePoint('O');
                    TrapList.Add(trap);
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach(Point trapp in TrapList){
                         trapp.Draw();
                    }

                    //PowerUp Draw
                    if (rnd1.Next(5) == 1)
                    {
                        Point PowerUp = dynamicPoint.CreatePoint('!');
                        PowerUpList.Add(PowerUp);
                    }
                    Console.ForegroundColor = ConsoleColor.Blue; 
                    foreach(Point power in PowerUpList){
                        power.Draw();
                    }
                    Console.ResetColor();



                }
                if (snake.Touch(PowerUpList))
                {
                    Count += 2;
                    WritePoints(Count);
                    Times += 10;
                }
                else
                {
                    snake.Move();
                }

                Thread.Sleep(Times);
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    snake.HandleKey(key.Key);
                }

            }
            Console.ReadLine();
        }

        static void WriteGameOver(int Count, string playerName)
        {
            int xOffset = 86;
            int yOffset = 8;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(xOffset, yOffset++);
            WriteText("============================", xOffset, yOffset++);
            WriteText("     G A M E  O V E R", xOffset + 1, yOffset++);
            WriteText("============================", xOffset, yOffset++);
            Console.ResetColor(); 
            try{
               highScores.Add(new HighScore { PlayerName = playerName, Score = Count });
            highScores.Sort((a, b) => b.Score.CompareTo(a.Score));
            Console.SetCursorPosition(xOffset, yOffset++);
            WriteText("======", xOffset, yOffset++);
            WriteText("High Scores", xOffset, yOffset++);
            WriteText("======", xOffset, yOffset++);
            for (int i = 0; i < highScores.Count; i++)
            {
               WriteText((i+1) + ". " + highScores[i].PlayerName + ": " + highScores[i].Score, xOffset, yOffset++);
            }

            // Save high scores
            using (StreamWriter writer = new StreamWriter("highscores.txt"))
            {
            foreach (var highScore in highScores)
            {
                writer.WriteLine(highScore.PlayerName + "," + highScore.Score);
            }
            }
            }
            catch (Exception ex)
            {
            Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        static void WritePoints(int Count)
        {
            int xOffset = 96;
            int yOffset = 4;
            Console.SetCursorPosition(xOffset, yOffset++);
            WriteText("======", xOffset, yOffset++);
            WriteText("Points", xOffset, yOffset++);
            WriteText("======", xOffset, yOffset++);
            WriteInt(Count, xOffset + 1, yOffset++);
        }
        static void WriteText(String text, int xOffset, int yOffset)
        {
            Console.SetCursorPosition(xOffset, yOffset);
            Console.WriteLine(text);

        }
        static void WriteInt(int points, int xOffset, int yOffset)
        {
            Console.SetCursorPosition(xOffset, yOffset);
            Console.WriteLine(" "+points);
        }
    }
}