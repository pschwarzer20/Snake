using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Player
    {
        public List<Piece> pieces = new List<Piece> { };
        public List<Turn> turns = new List<Turn> { };

        public Player()
        {
            this.pieces.Add(new Piece(10, 10, 1));
        }
        public void AddPiece()
        {
            var lastPiece = this.pieces[this.pieces.Count - 1];

            var (positionX, positionY) = (0, 0);
            var position = lastPiece.position;

            switch (lastPiece.direction)
            {
                case 1:
                    (positionX, positionY) = (position.x,  position.y + 1);
                    break;
                case 2:
                    (positionX, positionY) = (position.x, position.y - 1);
                    break;
                case 3:
                    (positionX, positionY) = (position.x + 1, position.y);
                    break;
                case 4:
                    (positionX, positionY) = (position.x - 1, position.y);
                    break;

            }

            Piece newPiece = new Piece(positionX, positionY, lastPiece.direction);
            this.pieces.Add(newPiece);
        }
        public void AddTurn(Position position, int direction)
        {
            turns.Add(new Turn(position.x, position.y, direction));
        }
    }

    class Fruit
    {
        public Position position;

        public Fruit(int x, int y)
        {
            this.position = new Position(x, y);
        }
    }

    class Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Piece
    {
        public Position position;
        public int direction;

        public Piece(int x, int y, int direction)
        {
            this.position = new Position(x, y);
            this.direction = direction;
        }
    }

    class Turn
    {
        public Position position;
        public int newDirection;
        public List<Piece> completedPieces = new List<Piece> { };

        public Turn(int x, int y, int direction)
        {
            this.position = new Position(x, y);
            this.newDirection = direction;
        }
    }

    class Program
    {
        static int screenWidth = 50, screenHeight = 20;
        static Random rand = new Random();

        static void Main(string[] args)
        {
            bool isRunning = true;
            double gameTick = 0;
            ConsoleKey pressedKey = ConsoleKey.Zoom;

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

                var curTime = DateTime.Now.TimeOfDay.TotalSeconds;
                if (gameTick < curTime)
                {
                    var firstPiece = player.pieces[0];
                    var (playerPositionX, playerPositionY) = (firstPiece.position.x, firstPiece.position.y);
                    gameTick = curTime + 0.2;

                    // Input
                    switch (pressedKey)
                    {
                        case ConsoleKey.UpArrow:
                            firstPiece.direction = 1;
                            player.AddTurn(firstPiece.position, firstPiece.direction);
                            break;
                        case ConsoleKey.DownArrow:
                            firstPiece.direction = 2;
                            player.AddTurn(firstPiece.position, firstPiece.direction);
                            break;
                        case ConsoleKey.LeftArrow:
                            firstPiece.direction = 3;
                            player.AddTurn(firstPiece.position, firstPiece.direction);
                            break;
                        case ConsoleKey.RightArrow:
                            firstPiece.direction = 4;
                            player.AddTurn(firstPiece.position, firstPiece.direction);
                            break;
                    }


                    // Movement
                    foreach (var piece in player.pieces)
                    {
                        var (piecePositionX, piecePositionY) = (piece.position.x, piece.position.y);

                        // Check if the piece needs to make a turn
                        foreach (var turn in player.turns)
                        {
                            if (!turn.completedPieces.Contains(piece) && turn.position.x == piecePositionX && turn.position.y == piecePositionY)
                            {
                                piece.direction = turn.newDirection;
                                turn.completedPieces.Add(piece);
                                break;
                            }
                        }

                        Console.SetCursorPosition(piecePositionX, piecePositionY);
                        Console.Write("#");
                        switch (piece.direction)
                        {
                            case 1:
                                piece.position.Set(piecePositionX, piecePositionY - 1);
                                break;
                            case 2:
                                piece.position.Set(piecePositionX, piecePositionY + 1);
                                break;
                            case 3:
                                piece.position.Set(piecePositionX - 1, piecePositionY);
                                break;
                            case 4:
                                piece.position.Set(piecePositionX + 1, piecePositionY);
                                break;
                        }
                        (piecePositionX, piecePositionY) = (piece.position.x, piece.position.y);

                        if (piecePositionX > 0 && piecePositionY > 0)
                        {
                            Console.SetCursorPosition(piecePositionX, piecePositionY);
                            Console.Write("*");
                        }
                    }


                    // Collision
                    if ((playerPositionX < 0 || playerPositionX > screenWidth) || (playerPositionY < 0 || playerPositionY > screenHeight))
                    {
                        Console.SetCursorPosition(0, screenHeight + 1);
                        isRunning = false;
                    }
                    foreach (Fruit fruit in fruits.ToList())
                    {
                        var (fruitPositionX, fruitPositionY) = (fruit.position.x, fruit.position.y);
                        if (playerPositionX == fruitPositionX && playerPositionY == fruitPositionY)
                        {
                            Console.SetCursorPosition(fruitPositionX, fruitPositionY);
                            Console.Write("#");

                            player.AddPiece();
                            fruits.Remove(fruit);
                        }
                        else
                        {
                            Console.SetCursorPosition(fruitPositionX, fruitPositionY);
                            Console.Write("Ä");
                        }
                    }


                    // Fruit Spawning
                    if (fruits.Count < 3)
                    {
                        var x = rand.Next(0, screenWidth);
                        var y = rand.Next(0, screenHeight);
                        fruits.Add(new Fruit(x, y));
                    }

                    Console.SetCursorPosition(screenWidth+1, screenHeight);
                }
            }
            Console.WriteLine("Game Over - Score: " + player.pieces.Count());
        }
    }
}
