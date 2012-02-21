# LogImporter

*LogImporter* reads webserver log files (iis, w3c), applies transformations on data records and imports them into a database (currently only SQL Server is supported).

## Features

 * Parses log files
 * Determines which log entries are new since last import
 * Imports log data into SQL Server
 * Removes guid ids in urls to generate statistics for actual pages (e.g. ASP.NET MVC url routing with guid ids in the path)
 * Resolves client ip address to country name and code (using GeoIP.dat by MaxMind)


### Supported formats

 * W3C extended (IIS)

## Performance

*LogImporter* imports about 10 000 log entries per second (measured with activated multi-threading on a quad-core cpu).

## Usage

*LogImporter* is used from the command line.

The following options are required:

    -d=VALUE             Directory with log files
    -c=VALUE             Connection string for target database
    -t=VALUE             Target table name

Furthermore the following optional parameters are available:

    -p=VALUE             Pattern for log files
    -n                   Create the table if it does not exist already
    -f, --force          Force full import of all files
    -s, --sequential     Import data in one single thread

Example usage:

    LogImporter.exe -d "D:\logs\W3SVC8" -t w3c_testlog -n -c "Data Source=.\SQLEXPRESS;Initial Catalog=logimporter_test;Integrated Security=True" -p "*.log"

## Build

How to build *LogImporter*:
 
1. Go to `\build\` directory.
2. Execute `go dist` on the command line.

*Note: Maybe you have to adjust the path to `MSBuild.exe` in `build\go.bat` to your .NET version number.*