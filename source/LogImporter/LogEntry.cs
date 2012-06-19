using System;

namespace LogImporter
{
    public class LogEntry
    {
        public string LogFilename { get; set; }

        public int LogRow { get; set; }

        public DateTime? TimeStamp { get; set; }

        public string cIp { get; set; }

        public string csUsername { get; set; }

        public string sSitename { get; set; }

        public string sComputername { get; set; }

        public string sIp { get; set; }

        public int? sPort { get; set; }

        public string csMethod { get; set; }

        public string csUriStem { get; set; }

        public string csUriQuery { get; set; }

        public int? scStatus { get; set; }

        public int? scSubStatus { get; set; }

        public int? scWin32Status { get; set; }

        public int? scBytes { get; set; }

        public int? TimeTaken { get; set; }

        public string csVersion { get; set; }

        public string csHost { get; set; }

        public string csUserAgent { get; set; }

        public string csCookie { get; set; }
        
        public string csReferer { get; set; }
        
        public string sEvent { get; set; }
        
        public string sProcessType { get; set; }
        
        public float? sUserTime { get; set; }
        
        public float? sKernelTime { get; set; }
        
        public int? sPageFaults { get; set; }
        
        public int? sTotalProcs { get; set; }
        
        public int? sActiveProcs { get; set; }
        
        public int? sStoppedProcs { get; set; }
        
        public string CountryName { get; set; }
        
        public string CountryCode { get; set; }

        public string CleanUri { get; set; }
    }
}
