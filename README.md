# LogImporter

`LogImporter` reads webserver log files, applies transformations on data records and imports them into a database (currently only SQL Server is supported).

## Features

 * **[done]** Parse log files
 * *[in progress]* Determines which log files are new/modified since last import
 * *[in progress]* Import log data into SQL Server
 * **[done]** Remove guid ids in urls to generate statistics for actual pages
 * **[done]** Resolve client ip address to country name and code (using GeoIP.dat by Maxmind)


## Supported formats

 * **[done]** W3C extended (IIS)