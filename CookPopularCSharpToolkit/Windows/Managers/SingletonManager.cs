using CookPopularCSharpToolkit.Windows.Interop;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.MemoryMappedFiles;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;


namespace CookPopularCSharpToolkit.Windows
{
    public sealed class SingletonManager : IDisposable
    {
        private sealed class Context
        {
            public IntPtr HWnd { get; }

            public Context(IntPtr hWnd) => HWnd = hWnd;
        }

        private static readonly string guid = GetGuid();
        private static readonly MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateOrOpen($"{guid}.mmf", 32 * 1024, MemoryMappedFileAccess.ReadWrite);

        private static Mutex mutex;

        /// <summary>
        /// 初始化 <see cref="SingletonManager" /> 类的新实例。
        /// </summary>
        public SingletonManager(string token = default)
        {
            mutex = (mutex == null) ? new Mutex(true, $"Global\\{token ?? guid}", out var isSingleton) : throw new InvalidOperationException("仅允许实例化一次。");

            if (isSingleton)
            {
                if (Application.Current is Application application)
                {
                    Application.Current.Activated += Application_Activated;
                }
                else
                {
                    throw new InvalidOperationException("应在 Application 类实例化之后创建此类。");
                }
            }
            else
            {
                if (GetContext() is Context context)
                {
                    InteropMethods.SwitchToThisWindow(context.HWnd, true);
                }

                Environment.Exit(0);
                Process.GetCurrentProcess().Kill();
            }
        }

        public static void Restart(int exitCode = default, string arguments = default)
        {
            try
            {
                mutex?.ReleaseMutex();
                mutex?.Dispose();

                if (Process.GetCurrentProcess() is Process current)
                {
                    if (current.MainModule is ProcessModule processModule)
                    {
                        Process.Start(processModule.FileName, arguments)?.Dispose();
                    }

                    current.Kill();
                    current.Dispose();
                }
            }
            catch (Exception)
            {
                ;
            }
            finally
            {
                Environment.Exit(exitCode);
            }
        }

        private void Application_Activated(object sender, EventArgs e)
        {
            try
            {
                if (sender is Window window)
                {
                    SetContext(new Context(window.EnsureHandle()));
                }
            }
            catch (Exception exc)
            {
                throw new Exception("纪录活动窗口句柄时产生异常：", exc);
            }
        }

        private static string GetGuid()
        {
            var assembly = Assembly.GetEntryAssembly();

            return (assembly.GetCustomAttribute<GuidAttribute>() is GuidAttribute guidAttribute) ? guidAttribute.Value : throw new ArgumentException($"未能在程序集“{assembly.FullName}”上找到“{typeof(GuidAttribute).FullName}”特性。");
        }

        private static Context GetContext()
        {
            try
            {
                using var accessor = memoryMappedFile.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read);
                var length = accessor.ReadInt32(0);
                var array = new char[length];

                accessor.ReadArray(4, array, 0, length);

                return JsonConvert.DeserializeObject<Context>(new string(array));
            }
            catch (Exception)
            {
                return default;
            }
        }

        private static void SetContext(Context context)
        {
            try
            {
                using var accessor = memoryMappedFile.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Write);
                var array = JsonConvert.SerializeObject(context).ToCharArray();

                accessor.Write(0, array.Length);
                accessor.WriteArray(4, array, 0, array.Length);
            }
            catch (Exception)
            {

            }
        }

        [SuppressMessage("Design", "CA1063:正确实现 IDisposable", Justification = "<挂起>")]
        void IDisposable.Dispose()
        {
            memoryMappedFile?.Dispose();

            mutex?.ReleaseMutex();
            mutex?.Dispose();
        }
    }
}
