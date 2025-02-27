class SnakeGame
{
    const int Width = 40;
    const int Height = 20;
    static int Delay = 20;  // Faster game
    const char SnakeBodyChar = 'o';
    const char FoodChar = '*';
    const char BorderChar = '█';
    static readonly Random Random = new();
    
    static (int x, int y) food;
    static List<(int x, int y)> snake = new();
    static (int dx, int dy) direction = (1, 0); // Start moving right
    static bool running = true;
    static char SnakeHeadChar = '>';
    static bool flashBorder = false;
    static int flashTimer = 0;
    
    static void Main()
    {
        Console.CursorVisible = false;
        InitializeGame();
        Console.ReadKey();
        
        while (running)
        {
            Thread.Sleep(Delay);
            MoveSnake();
            Render();
        }

        Console.SetCursorPosition(0, Height + 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Game Over! Score: {snake.Count - 1}");
        Console.ResetColor();
        Console.ReadKey();
    }

    static void InitializeGame()
    {
        (int x, int y) startPosition = (Random.Next(1, Width - 1), Random.Next(1, Height - 1));
        snake.Add(startPosition);
        PlaceFood();
    }

    static void MoveSnake()
    {
        ChooseDirection();

        (int x, int y) newHead = (snake[0].x + direction.dx, snake[0].y + direction.dy);

        // Wrap around logic
        newHead.x = (newHead.x + Width) % Width;
        newHead.y = (newHead.y + Height) % Height;

        if (snake.Contains(newHead)) // Collision with itself
        {
            running = false;
            return;
        }

        snake.Insert(0, newHead);

        if (newHead == food)
        {
            PlaceFood();
            flashBorder = true;
            flashTimer = 5; // Flash effect for 5 frames
        }
        else
        {
            snake.RemoveAt(snake.Count - 1);
        }

        // Update head character based on direction
        SnakeHeadChar = direction switch
        {
            (1, 0) => '>',  // Right
            (-1, 0) => '<', // Left
            (0, -1) => '^', // Up
            (0, 1) => 'v',  // Down
            _ => SnakeHeadChar
        };
    }

    static void PlaceFood()
    {
        do
        {
            food = (Random.Next(0, Width), Random.Next(0, Height));
        } while (snake.Contains(food));
    }

    static void ChooseDirection()
    {
        var possibleMoves = new List<(int dx, int dy, int distance)>
        {
            (0, -1, DistanceToFood(snake[0].x, snake[0].y - 1)), // Up
            (0, 1, DistanceToFood(snake[0].x, snake[0].y + 1)),  // Down
            (-1, 0, DistanceToFood(snake[0].x - 1, snake[0].y)), // Left
            (1, 0, DistanceToFood(snake[0].x + 1, snake[0].y))   // Right
        };

        // Remove moves that hit the snake
        possibleMoves = possibleMoves.Where(move =>
        {
            (int nx, int ny) = ((snake[0].x + move.dx + Width) % Width, (snake[0].y + move.dy + Height) % Height);
            return !snake.Contains((nx, ny));
        }).ToList();

        if (possibleMoves.Count > 0)
        {
            direction = possibleMoves.OrderBy(m => m.distance).Select(m => (m.dx, m.dy)).First();
        }
    }

    static int DistanceToFood(int x, int y)
    {
        int dx = Math.Min(Math.Abs(x - food.x), Width - Math.Abs(x - food.x));
        int dy = Math.Min(Math.Abs(y - food.y), Height - Math.Abs(y - food.y));
        return dx + dy;
    }

    static void Render()
    {
        Console.SetCursorPosition(0, 0);

        // Toggle border color for flash effect
        if (flashBorder && flashTimer > 0)
        {
            Console.ForegroundColor = flashTimer % 2 == 0 ? ConsoleColor.Red : ConsoleColor.White;
            flashTimer--;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            flashBorder = false;
        }

        Console.WriteLine(new string(BorderChar, Width + 2));

        for (int y = 0; y < Height; y++)
        {
            Console.ForegroundColor = flashBorder ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write(BorderChar); // Left border

            for (int x = 0; x < Width; x++)
            {
                if ((x, y) == snake[0])
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(SnakeHeadChar);
                }
                else if (snake.Contains((x, y)))
                {
                    // Cycle colors through Red, Green, Blue, Cyan, Magenta
                    ConsoleColor[] bodyColors = { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Magenta };
                    int colorIndex = (snake.IndexOf((x, y)) % bodyColors.Length);
                    Console.ForegroundColor = bodyColors[colorIndex];
                    Console.Write(SnakeBodyChar);
                }
                else if ((x, y) == food)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(FoodChar);
                }
                else
                {
                    Console.Write(' ');
                }
            }

            Console.ForegroundColor = flashBorder ? ConsoleColor.Red : ConsoleColor.White;
            Console.WriteLine(BorderChar); // Right border
        }

        Console.WriteLine(new string(BorderChar, Width + 2));

        Console.ResetColor();
    }
}
