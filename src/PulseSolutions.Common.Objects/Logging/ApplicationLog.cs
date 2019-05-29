
using System;

namespace PulseSolutions.Common.Objects.Logging
{
    public class ApplicationLog
    {
        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public DateTime LoggedOn { get; set; }

        public string ProjectName { get; set; }

        public string InstanceName { get; set; }

        public int UserId { get; set; }

        public string LogDetails { get; set; }

        public string UserName { get; set; }

        public string FilePath { get; set; }

        public int LogType { get; set; }

        public string Message { get; set; }

        public int FilterDays { get; set; }

        public string SearchKey { get; set; }
    }
}
