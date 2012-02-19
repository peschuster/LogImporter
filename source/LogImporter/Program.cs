using System;
using LogImporter.Exceptions;

namespace LogImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLine();

            try
            {
                // Read arguments from command line.
                options.Parse(args);

                // Validate all options.
                options.Validate();

                var kernel = new Kernel();

                kernel.Run(options);

                Environment.Exit(0);
            }
            catch (UsageException exception)
            {
                var color = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(Environment.NewLine + exception.Message);

                Console.ForegroundColor = color;
                options.PrintUsage(Console.Error);

                Environment.Exit(2);
            }
            catch (SystemException exception)
            {
                Console.Error.WriteLine(exception.ToString());

                Environment.Exit(1);
            }
        }
    }
}
