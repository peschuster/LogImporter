using System;
using System.IO;

namespace LogImporter
{
    internal static class ConsoleWriter
    {
        public static void WriteLine(TextWriter target, ConsoleColor color, string message, params object[] parameter)
        {
            var previousColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            target.WriteLine(message, parameter);

            Console.ForegroundColor = previousColor;
        }

        public static void WriteError(string message, params object[] parameter)
        {
            WriteLine(Console.Error, ConsoleColor.Red, message, parameter);
        }

        public static void WriteSuccess(string message, params object[] parameter)
        {
            WriteLine(Console.Out, ConsoleColor.Green, message, parameter);
        }

        public static void WriteInfo(string message, params object[] parameter)
        {
            WriteLine(Console.Out, ConsoleColor.White, message, parameter);
        }
    }
}
