using System.Runtime.InteropServices.JavaScript;

namespace TaskManager.Util;

public class InputHandler {
    public static String GetValidatedInput(String message, Func<String, String?> validator)
    {
        Console.WriteLine(message);
        String? input = Console.ReadLine();

        while (input == null || validator(input) != null)
        {
            Console.WriteLine(input == null ? "You must input something. Try again." : validator(input));
            input = Console.ReadLine();
        }

        return input;
    }
}

