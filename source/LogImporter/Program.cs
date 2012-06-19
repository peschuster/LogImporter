using System;
using LogImporter.Exceptions;
using System.Diagnostics;

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

                var s = Stopwatch.StartNew();

                // Run log import
                kernel.Run(options);
                
                s.Stop();

                ConsoleWriter.WriteInfo("Time taken: {0}", s.Elapsed);

                Environment.Exit(0);
            }
            catch (UsageException exception)
            {
                ConsoleWriter.WriteError(Environment.NewLine + exception.Message);

                options.PrintUsage(Console.Error);

                Environment.Exit(2);
            }
            catch (SystemException exception)
            {
                ConsoleWriter.WriteError(exception.ToString());

                Environment.Exit(1);
            }
        }
    }
}
