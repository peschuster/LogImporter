using System;
using System.Collections.Generic;
using LogImporter.Converters;

namespace LogImporter.Configurations
{
    public class W3CExtended : ILogConfiguration
    {
        private readonly OptionalStringConverter stringConverter;

        private readonly OptionalDateTimeConverter dateTimeConverter;

        private readonly OptionalIntegerConverter intConverter;

        private readonly IDictionary<string, Action<LogEntry, string>> mapping;

        public W3CExtended()
        {
            this.stringConverter = new OptionalStringConverter();
            this.dateTimeConverter = new OptionalDateTimeConverter(this.stringConverter);
            this.intConverter = new OptionalIntegerConverter(this.stringConverter);

            this.mapping = new Dictionary<string, Action<LogEntry, string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "date", (e, v) => this.AddDate(e, v) },
                { "time", (e, v) => this.AddTime(e, v) },
                { "s-ip", (e, v) => e.sIp = this.stringConverter.Transform(v) },
                { "s-sitename", (e, v) => e.sSitename = this.stringConverter.Transform(v) },
                { "s-computername", (e, v) => e.sComputername = this.stringConverter.Transform(v) },
                { "s-port", (e, v) => e.sPort = this.intConverter.Transform(v) },
                { "cs-host", (e, v) => e.csHost = this.stringConverter.Transform(v) },
                { "cs-method", (e, v) => e.csMethod = this.stringConverter.Transform(v) },
                { "cs-uri-stem", (e, v) => e.csUriStem = this.stringConverter.Transform(v) },
                { "cs-uri-query", (e, v) => e.csUriQuery = this.stringConverter.Transform(v) },
                { "cs-username", (e, v) => e.csUsername = this.stringConverter.Transform(v) },
                { "cs-version", (e, v) => e.csVersion = this.stringConverter.Transform(v) },
                { "cs(User-Agent)", (e, v) => e.csUserAgent = this.stringConverter.Transform(v) },
                { "cs(Cookie)", (e, v) => e.csCookie = this.stringConverter.Transform(v) },
                { "cs(Referer)", (e, v) => e.csReferer = this.stringConverter.Transform(v) },
                { "c-ip", (e, v) => e.cIp = this.stringConverter.Transform(v) },
                { "sc-bytes", (e, v) => e.scBytes = this.intConverter.Transform(v) },
                { "sc-status", (e, v) => e.scStatus = this.intConverter.Transform(v) },
                { "sc-substatus", (e, v) => e.scSubStatus = this.intConverter.Transform(v) },
                { "sc-win32-status", (e, v) => e.scWin32Status = this.intConverter.Transform(v) },
                { "time-taken", (e, v) => e.TimeTaken = this.intConverter.Transform(v) }
            };
        }

        public IDictionary<int, string> DefaultFieldConfiguration
        {
            get { return new Dictionary<int, string>(); }
        }

        public bool IsComment(string line)
        {
            if (string.IsNullOrEmpty(line))
                return false;

            return line.StartsWith("#");
        }

        public bool IsFieldConfiguration(string line)
        {
            return this.IsComment(line)
                && line.StartsWith("#Fields:");
        }

        public IDictionary<int, string> ReadFieldConfiguration(string fieldsLine)
        {
            var result = new Dictionary<int, string>();

            if (string.IsNullOrWhiteSpace(fieldsLine))
                return result;

            string cleaned = fieldsLine
                .Replace("#", string.Empty)
                .Replace("Fields:", string.Empty)
                .Trim();

            string[] parts = cleaned.Split(' ');

            for (int index = 0; index < parts.Length; index++)
            {
                result.Add(index, parts[index]);
            }

            return result;
        }

        public bool SetValue(LogEntry entry, string field, string value)
        {
            if (!this.mapping.ContainsKey(field))
                return false;

            this.mapping[field].Invoke(entry, value);

            return true;
        }

        private void AddDate(LogEntry e, string v)
        {
            var d = this.dateTimeConverter.Transform(v);

            if (d.HasValue)
            {
                if (e.TimeStamp.HasValue)
                {
                    d = d.Value.Date.Add((e.TimeStamp.Value - e.TimeStamp.Value.Date));
                }

                e.TimeStamp = d;
            }
            else
            {
                e.TimeStamp = null;
            }                
        }

        private void AddTime(LogEntry e, string v)
        {
            var d = this.dateTimeConverter.Transform(v);

            if (d.HasValue)
            {
                if (e.TimeStamp.HasValue)
                {
                    d = e.TimeStamp.Value.Date.Add((d.Value - d.Value.Date));
                }

                e.TimeStamp = d;
            }
            else
            {
                e.TimeStamp = null;
            }
        }
    }
}
