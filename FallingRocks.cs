using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace FallingRocks
{
    class FallingRocks
    {
        static int seed = 1;

        static void createRocks(Rock[] rocks)
        {
            char[] symbols = { '!', '@', '#', '$', '%', '^', '&', '*', '.', ',' };
            Random rand = new Random(seed);
            seed += (seed + 1) % 1000000;
            for (int i = 0; i < rocks.Length; i++)
            {
                string shape = new string(symbols[i % 10], rand.Next(3) + 1);
                int left = rand.Next(1,Console.WindowWidth - 3);
                int top = -rand.Next(100);
                ConsoleColor color = (ConsoleColor) rand.Next((int) ConsoleColor.Blue, (int) ConsoleColor.Yellow);
                rocks[i] = new Rock(shape, color, left, top);
            }
        }

        static void incrementRocks(Rock[] rocks)
        {
            foreach (Rock rock in rocks)
            {
                rock.increment();
            }
        }

        static int checkForCollision(Rock[] rocks, int left, int top, int score, int lives,Rock life)
        {
            foreach (Rock rock in rocks)
            {
                if ((rock.top == top) && (left - rock.left < rock.shape.Length) && (left - rock.left > -3))
                {
                    if (lives > 0)
                    {
                        lives--;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.SetCursorPosition(1, 1);
                        Console.Write("Lives: {0}", lives);
                        Console.BackgroundColor = ConsoleColor.Black;
                        //Thread.Sleep(200);
                        return lives;
                    }
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Thread.Sleep(1000);
                    Console.Clear();
                    Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 1);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Thread.Sleep(1000);
                    Console.WriteLine("Game Over");
                    Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 + 1);
                    Console.WriteLine("SCORE: {0}", score);
                    Console.Read();
                    Environment.Exit(0);
                    return 0;
                }
            }
            if ((life.top == top) && (left - life.left < life.shape.Length) && (left - life.left > -3))
            {
                lives++;
                Console.BackgroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(1, 1);
                Console.Write("Lives: {0}", lives);
                Console.BackgroundColor = ConsoleColor.Black;
                Thread.Sleep(200);
                return lives;
            }
            return lives;
        }


        static void Main(string[] args)
        {
            Console.SetWindowSize(80, 30);
            Console.SetBufferSize(80, 30);
            Rock[] rocks = new Rock[100];
            Random r = new Random();
            Rock life = new Rock("LIFE", ConsoleColor.Green, r.Next(1, Console.WindowWidth - 1), -r.Next(200));
            createRocks(rocks);
            int left = Console.WindowWidth/2 - 2;
            int top = Console.WindowHeight - 2;
            int sleepTime = 0;
            int score = 0,scoreCounter = 0;
            int lives = 3;
   
            while (true)
            {

                if (sleepTime == 0)
                {
                    //Console.Clear();
                   
                    if (scoreCounter == 0)
                    {
                       // Console.Clear();
                        score++;
                    }
                    scoreCounter = (scoreCounter + 1) % 5;
                    incrementRocks(rocks);
                    life.increment();
                    lives = checkForCollision(rocks, left, top, score, lives,life);
                }
                sleepTime = (sleepTime + 1) % 300;

                Console.CursorVisible = false;
                Console.SetCursorPosition(left, top);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("(0)");
                ConsoleKeyInfo key;

                if (Console.KeyAvailable)
                {

                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.LeftArrow)
                    {
                        Console.SetCursorPosition(left, top);
                        Console.Write("   ");
                        left = (left <= 2) ? 1 : left - 2;
                        Console.SetCursorPosition(left, top); 
                        Console.Write("(0)");
                    }
                    if (key.Key == ConsoleKey.RightArrow)
                    {
                        Console.SetCursorPosition(left, top);
                        Console.Write("   ");
                        left = (left >= Console.WindowWidth - 5) ? Console.WindowWidth - 3 : left + 2; 
                        Console.SetCursorPosition(left, top); 
                        Console.Write("(0)");
                    }
                }
                Console.SetCursorPosition(Console.WindowWidth - 15, 1);
                Console.Write("Score: {0:d8}", score);
                Console.SetCursorPosition(1, 1);
                Console.Write("Lives: {0}", lives);
            }
        }
    }

    class Rock
    {
        public string shape;
        public string emptyString;
        public ConsoleColor color;
        public int left;
        public int top;
        static int seed = 1;

        public Rock(string shape, ConsoleColor color, int left, int top)
        {
            this.shape = shape;
            emptyString = new string(' ', shape.Length);
            this.color = color;
            this.left = left;
            this.top = top;
        }

        private void print()
        {
            Console.SetCursorPosition(left, top);
            Console.ForegroundColor = color;
            Console.Write(shape);
        }

        private void printEmpty()
        {
            Console.SetCursorPosition(left, top);
            Console.Write(emptyString);
        }
        public void increment()
        {
            if (top < 1)
            {
                top++;
                return;
            }

            printEmpty();

            if (top >= Console.WindowHeight - 2)
            {
                Random rand = new Random(seed);
                int r = rand.Next(1,Console.WindowWidth - 1);
                left = r;
                top = -3 * (r % 100);
                seed += (seed + 1) % 1000000;
                return;
            }
            else
            {
                top++;
                print();
            }
        }
    }
}
