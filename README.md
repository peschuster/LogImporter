# LogImporter

`LogImporter` reads webserver log files, applies transformations on data records and imports them into a database (currently only SQL Server is supported).

## Features

 * Parse log files
 * Determines which log files are new/modified since last import
 * Import log data into SQL Server
 * Remove guid ids in urls to generate statistics for actual pages
 * Resolve client ip address to country name and code (using GeoIP.dat by Maxmind)


### Supported formats

 * W3C extended (IIS)

## Build

How to build LogImporter:
 
1. Go to `\build\` directory.
2. Execute `go dist` on the command line.

*Note: Maybe you have to adjust the path to `MSBuild.exe` in `build\go.bat` to the right .NET version number.*