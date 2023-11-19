using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Media;

namespace Snäke
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(120, 25);
        
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




            Point p = new Point(50, 10, '*');
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
                    WriteGameOver();
                  //  InputName(Count);

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
                    Point trap = dynamicPoint.CreatePoint('¤');
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
                   // PlaySound(3);
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

       




        static void InputName(int Count)
        {
            Console.SetCursorPosition(86, 12);
            Console.WriteLine("Enter your name: ");
            Console.SetCursorPosition(86, 13);
            string userName = Console.ReadLine();
            Console.SetCursorPosition(86, 12);
            Console.WriteLine("Added to leaderboard!");
            Console.SetCursorPosition(86, 13);
            Console.WriteLine("Name: "+userName +"  Points: "+ Count);


            StreamWriter f = new StreamWriter(@"..\..\LeaderBoard.txt");
            f.WriteLine('\n' + "Name:" + userName + " Points:" + Count);
            f.Close();




            
        }
        static void PlayRandomSound()
        {
            Random rnd1 = new Random();
            int s = rnd1.Next(10);
            if (s == 5)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.IO.Stream resouceStream =
                    assembly.GetManifestResourceStream(@"Snäke.Powerup4.wav");
                SoundPlayer player = new SoundPlayer(resouceStream);
                player.Play();
                player.PlaySync();
            }
            else
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.IO.Stream resouceStream =
                    assembly.GetManifestResourceStream(@"Snäke.Powerup3.wav");
                SoundPlayer player = new SoundPlayer(resouceStream);
                player.Play();
                player.PlaySync();

            }


        }
        static void PlaySound(int Sound)
        {
            if (Sound == 0)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.IO.Stream resouceStream =
                    assembly.GetManifestResourceStream(@"Snäke.Hit_Hurt4.wav");
                SoundPlayer player = new SoundPlayer(resouceStream);
                player.Play();
                player.PlaySync();
            }
            if (Sound == 1)
            {
                PlayRandomSound();
            }

            if (Sound == 3)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.IO.Stream resouceStream =
                    assembly.GetManifestResourceStream(@"Snäke.Powerup27.wav");
                SoundPlayer player = new SoundPlayer(resouceStream);
                player.Play();
            }
        }

        

        static void WriteGameOver()
        {
            int xOffset = 86;
            int yOffset = 8;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(xOffset, yOffset++);
            WriteText("============================", xOffset, yOffset++);
            WriteText("G A M E  O V E R", xOffset + 1, yOffset++);
            WriteText("============================", xOffset, yOffset++);
          //  PlaySound(0);
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