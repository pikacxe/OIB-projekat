using System;


namespace Common
{
    public enum MessageType
    {
        Info,
        Error,
        Warning,
        Success
    }

    public static class CustomConsole
    {
        public static void WriteLine(string message, MessageType messageType)
        {
            ConsoleColor originalColor = Console.ForegroundColor;

            switch (messageType)
            {
                case MessageType.Info:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case MessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case MessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case MessageType.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }

            Console.WriteLine($"{DateTime.Now} - [{messageType}] {message}");

            // Reset the console color to the original color
            Console.ForegroundColor = originalColor;
        }
    }
}
