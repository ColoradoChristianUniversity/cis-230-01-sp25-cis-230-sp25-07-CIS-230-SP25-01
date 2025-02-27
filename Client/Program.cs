Console.CursorVisible = false;
Console.Title = "Hello, World!";

while (true)
{
    Console.Clear();

    string firstName = string.Empty;
    while (string.IsNullOrEmpty(firstName))
    {
        Console.WriteLine("Enter your first name: ");
        firstName = Console.ReadLine()!;
    }

    string lastName = string.Empty;
    while (string.IsNullOrEmpty(lastName))
    {
        Console.WriteLine("Enter your last name: ");
        lastName = Console.ReadLine()!;
    }

    int birthYear = 0;
    while (birthYear == 0)
    {
        Console.WriteLine("Enter your birth year: ");
        int.TryParse(Console.ReadLine(), out birthYear);
    }

    var message = Greeter.SayHello(firstName, lastName, birthYear);
    Console.Clear();
    Greeter.Print(message);

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

class Greeter
{
    public static string SayHello(string firstName, string lastName, int yearBirth)
    {
        int age = DateTime.Now.Year - yearBirth;
        var years = age == 1 ? "year" : "years";
        return $"Hello, {firstName} {lastName}! You are {age} {years} old.";
    }

    public static void Print(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}