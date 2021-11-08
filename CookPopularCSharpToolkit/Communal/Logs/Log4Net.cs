using log4net;
using System;
using System.Reflection;

namespace CSharp.Communal.Logger
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
    public sealed class Log4Net
    {
        static Log4Net()
        {
            //要使用这个方法配置log4net，您必须指定log4net.config
            log4net.Config.XmlConfigurator.Configure();
            ////否则
            //log4net.Config.XmlConfigurator.Configure(new Uri(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
        }

        private static readonly Assembly assembly = Assembly.GetEntryAssembly();
        private static readonly ILog log = LogManager.GetLogger("LoggerObject");

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
