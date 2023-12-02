using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Player
    {
        public int x = 10;
        public int y = 10;
        public int size = 1;
        public int direction = 1;
    }

    class Fruit
    {
        public int x;
        public int y;
        public Fruit(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Turn
    {

    }

    class Program
    {
        static Random rand = new Random();

        static void Main(string[] args)
        {
            int screenWidth = 50, screenHeight = 20;

            bool isRunning = true;
            ConsoleKey pressedKey = ConsoleKey.Zoom;
            double gameTick = 0;
            Player player = new Player();
            List<Fruit> fruits = new List<Fruit> { };

            for (int y = 0; y <= screenHeight; y++)
            {
                for (int x = 0; x <= screenWidth; x++)
                {
                    Console.Write("#");
                }
                Console.WriteLine("");
            }

            while (isRunning)
            {
                if (Console.KeyAvailable)
                {
                    pressedKey = Console.ReadKey().Key;
                    if (pressedKey == ConsoleKey.Escape)
                    {
                        isRunning = false;
                        return;
                    }
                }

                if (gameTick < DateTime.Now.TimeOfDay.TotalSeconds)
                {
                    gameTick = DateTime.Now.TimeOfDay.TotalSeconds + 0.25;

                    Console.SetCursorPosition(player.x, player.y);
                    Console.Write("*");

                    // Input
                    switch (pressedKey)
                    {
                        case ConsoleKey.W:
                            player.direction = 1;
                            break;
                        case ConsoleKey.S:
                            player.direction = 2;
                            break;
                        case ConsoleKey.A:
                            player.direction = 3;
                            break;
                        case ConsoleKey.D:
                            player.direction = 4;
                            break;
                    }
                    Console.SetCursorPosition(player.x + 1, player.y);
                    Console.Write("#");

                    // Movement
                    Console.SetCursorPosition(player.x, player.y);
                    Console.Write("#");
                    switch (player.direction)
                    {
                        case 1:
                            --player.y;
                            break;
                        case 2:
                            ++player.y;
                            break;
                        case 3:
                            --player.x;
                            break;
                        case 4:
                            ++player.x;
                            break;
                    }
                    if (player.x > 0 && player.y > 0)
                    {
                        Console.SetCursorPosition(player.x, player.y);
                        Console.Write("*");
                    }

                    // Collision
                    if ((player.y < 0 || player.y > screenHeight) || (player.x < 0 || player.x > screenWidth))
                    {
                        Console.SetCursorPosition(0, screenHeight + 1);
                        isRunning = false;
                    }

                    foreach (Fruit fruit in fruits)
                    {
                        if (player.x == fruit.x && player.y == fruit.y)
                        {
                            fruits.Remove(fruit);
                            ++player.size;
                        }

                        Console.SetCursorPosition(fruit.x, fruit.y);
                        Console.Write("Ä");
                    }

                    // Fruit Spawning
                    if (fruits.Count < 3)
                    {
                        var x = rand.Next(0, screenWidth);
                        var y = rand.Next(0, screenHeight);
                        fruits.Add(new Fruit(x, y));
                    }
                }
            }
            Console.WriteLine("Game Over - Score: " + player.size);
        }
    }
}
