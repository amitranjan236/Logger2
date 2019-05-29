
using PulseSolutions.Common.Objects.Config;
using PulseSolutions.Common.Objects.Enums;
using PulseSolutions.Common.Objects.RestClient;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace PulseSolutions.Common.Objects.Logging
{
    public class Logger
    {
        private static readonly object Locker = new object();
        private static Logger _instance;

        private Logger()
        {
        }

        public static Logger GetInstance()
        {
            if (_instance == null)
            {
                lock (Locker)
                {
                    if (_instance == null)
                        _instance = new Logger();
                }
            }
            return _instance;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Log(StringBuilder obj, LogTypes logType, int userId = 0, string userName = "", [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "")
        {
            Log(logType, (object)obj, memberName, fileName, userId, userName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Log(string obj, LogTypes logType, int userId = 0, string userName = "", [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "")
        {
            Log(logType, (object)obj, memberName, fileName, userId, userName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Log(Exception obj, int userId = 0, string userName = "", [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "")
        {
            Log(LogTypes.Exception, (object)obj, memberName, fileName, userId, userName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Log(LogTypes logType, object obj, string memberName, string fileName, int userId = 0, string userName = "")
        {
            ApplicationLog applicationLog = new ApplicationLog();
            StackFrame stackFrame = new StackFrame(2, true);
            try
            {
                bool isHosted = HostingEnvironment.IsHosted;
                applicationLog.UserId = userId;
                applicationLog.UserName = userName;
                applicationLog.InstanceName = ConfigurationSettingsHelper.InstanceName;
                applicationLog.LoggedOn = DateTime.UtcNow;
                if (isHosted)
                {
                    applicationLog.ProjectName = HostingEnvironment.ApplicationHost.GetSiteName();
                    applicationLog.FilePath = HttpContext.Current.Request.RawUrl;
                }
                else
                {
                    applicationLog.ProjectName = Assembly.GetCallingAssembly().GetName().Name;
                    applicationLog.FilePath = fileName;
                }
                if (logType == LogTypes.Exception)
                {
                    applicationLog.LogType = 1;
                    if (obj is Exception exception)
                    {
                        Type declaringType = exception.TargetSite.DeclaringType;
                        if (declaringType != (Type)null)
                        {
                            applicationLog.ClassName = declaringType.FullName;
                            applicationLog.MethodName = exception.TargetSite.Name;
                        }
                        applicationLog.LogDetails = exception.ToString();
                        applicationLog.Message = exception.Message;
                    }
                    else
                    {
                        applicationLog.ClassName = stackFrame.GetMethod().DeclaringType.AssemblyQualifiedName;
                        applicationLog.MethodName = memberName;
                        applicationLog.LogDetails = obj.ToString();
                        applicationLog.Message = "User defined custom exception";
                    }
                }
                else
                {
                    applicationLog.LogType = 2;
                    applicationLog.ClassName = stackFrame.GetMethod().DeclaringType.AssemblyQualifiedName;
                    applicationLog.MethodName = memberName;
                    applicationLog.LogDetails = obj.ToString();
                    applicationLog.Message = "User defined custom exception";
                }
            }
            catch (Exception ex)
            {
                try
                {
                    applicationLog = new ApplicationLog()
                    {
                        UserId = userId,
                        UserName = userName,
                        FilePath = new StackFrame(1).GetFileName(),
                        Message = ex.Message,
                        InstanceName = ConfigurationSettingsHelper.InstanceName,
                        ProjectName = Assembly.GetCallingAssembly().GetName().Name,
                        LoggedOn = DateTime.UtcNow,
                        LogType = 1,
                        LogDetails = ex.ToString()
                    };
                    if (ex.TargetSite.DeclaringType != (Type)null)
                        applicationLog.ClassName = ex.TargetSite.DeclaringType.FullName;
                    applicationLog.MethodName = ex.TargetSite.Name;
                }
                finally
                {
                    HttpWebMethods.PostAsync(ConfigurationSettingsHelper.LoggerApiBaseAddress, ConfigurationSettingsHelper.SaveLogEndpoint, applicationLog);
                }
            }
            finally
            {
                HttpWebMethods.PostAsync(ConfigurationSettingsHelper.LoggerApiBaseAddress, ConfigurationSettingsHelper.SaveLogEndpoint, applicationLog);
            }
        }
    }
}
