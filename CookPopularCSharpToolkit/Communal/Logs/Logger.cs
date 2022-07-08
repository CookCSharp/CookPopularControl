using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;

namespace CookPopularCSharpToolkit.Communal
{
    /// Copyright (c) 2020 All Rights Reserved.
    /// Description：Log4net日志 
    /// Author： Chance_写代码的厨子
    /// Create Time：2020/12/3 13:57:56
    /// 
    /// <summary>
    /// 控制级别，由低到高：ALL|DEBUG|INFO|WARN|ERROR|FATAL|OFF
    /// 比如定义级别为INFO，则INFO级别向下的级别，比如DEBUG日志将不会被记录
    /// 如果没有定义LEVEL的值，则缺省为DEBUG
    /// </summary>
    /// <remarks>基于log4net的日志方法</remarks>
    public sealed class Logger
    {
        private static readonly Assembly assembly = Assembly.GetEntryAssembly();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string DefaultConfigureFile = Assembly.GetExecutingAssembly().GetName().Name + ".log4net.config";


        public static void ConfigureDefault(string configureFile = null)
        {
            if (string.IsNullOrEmpty(configureFile))
                configureFile = DefaultConfigureFile;

            //读取配置文件
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(configureFile);
            XmlConfigurator.Configure(stream);
        }

        public static void SetLog4ConfigFile(string fileName)
        {
            XmlConfigurator.Configure(new FileInfo(fileName));
        }

        public static void SetLog4ConfigFile(Stream stream)
        {
            XmlConfigurator.Configure(stream);
        }

        public static void Fatal(object message)
        {
            log.Fatal(message);
        }

        public static void Fatal(object message, Exception ex = default)
        {
            log.Fatal(message, ex);
        }

        public static void Error(object message)
        {
            log.Error(message);
        }

        public static void Error(object message, Exception ex = default)
        {
            log.Error(message, ex);
        }

        public static void Warn(object message)
        {
            log.Warn(message);
        }

        public static void Warn(object message, Exception ex = default)
        {
            log.Warn(message, ex);
        }

        public static void Info(object message)
        {
            log.Info(message);
        }

        public static void Info(object message, Exception ex = default)
        {
            log.Info(message, ex);
        }

        public static void Debug(object message)
        {
            log.Debug(message);
        }

        public static void Debug(object message, Exception ex = default)
        {
            log.Debug(message, ex);
        }
    }
}
