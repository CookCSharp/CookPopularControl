using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

#if NETFRAMEWORK

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using CookPopularCSharpToolkit.Windows.Interop;

#endif


/*
 * Description：AppInstanceManager 
 * Author： Chance.Zheng
 * Create Time: 2022-07-13 17:09:59
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */
namespace CookPopularCSharpToolkit.Windows
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class StartupNextInstanceEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of the StartupNextInstanceEventArgs.
        /// </summary>
        public StartupNextInstanceEventArgs(ReadOnlyCollection<string> args, bool bringToForegroundFlag)
        {
            if (args == null)
                args = new ReadOnlyCollection<string>(null);

            CommandLine = args;
            BringToForeground = bringToForegroundFlag;
        }

        /// <summary>
        /// Indicates whether we will bring the application to the foreground when processing the StartupNextInstance event.
        /// </summary>
        public bool BringToForeground { get; set; }

        /// <summary>
        /// Returns the command line sent to this application
        /// </summary>
        /// <remarks>
        /// I'm using Me.CommandLine so that it is consistent with my.net and to assure they always return the same values
        /// </remarks>
        public ReadOnlyCollection<string> CommandLine { get; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable]
    public class CantStartSingleInstanceException : Exception
    {
        public CantStartSingleInstanceException() : base("Unable to connect to the started instance.")
        {
        }

        public CantStartSingleInstanceException(string message) : base(message)
        {
        }

        public CantStartSingleInstanceException(string message, System.Exception inner) : base(message, inner)
        {
        }

        // Deserialization constructor must be defined since we are serializable
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected CantStartSingleInstanceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    /// Signature for the StartupNextInstance event handler
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public delegate void StartupNextInstanceEventHandler(object sender, StartupNextInstanceEventArgs e);

    /// <summary>
    /// Signature for the Shutdown event handler
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public delegate void ShutdownEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Signature for the UnhandledException event handler
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public delegate void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e);


    /// <summary>
    /// Application base for WPF application model.
    /// in order to implement singleton app
    /// </summary>
    /// <remarks>
    /// This class replaces <see cref="Application"/>.
    /// </remarks>
    public class ApplicationBase : Application
    {
        private class SingletonManagerHelper
        {
#if NETFRAMEWORK
        private const PipeOptions NamedPipeOptions = PipeOptions.Asynchronous;
#elif NETCOREAPP3_1_OR_GREATER
            private const PipeOptions NamedPipeOptions = PipeOptions.Asynchronous | PipeOptions.CurrentUserOnly;
#endif
            public static bool TryCreatePipeServer(string pipeName, out NamedPipeServerStream pipeServer)
            {
                try
                {
                    pipeServer = new NamedPipeServerStream(pipeName: pipeName,
                                                           direction: PipeDirection.In,
                                                           maxNumberOfServerInstances: 1,
                                                           transmissionMode: PipeTransmissionMode.Byte,
                                                           options: NamedPipeOptions);
                    return true;
                }
                catch (Exception)
                {
                    pipeServer = null;
                    return false;
                }
            }

            private async static Task<string[]> ReadArgsAsync(NamedPipeServerStream pipeServer, CancellationToken cancellationToken)
            {
                const int bufferLength = 1024;
                var buffer = new byte[bufferLength];
                using (MemoryStream stream = new MemoryStream())
                {
                    while (true)
                    {
#if NETFRAMEWORK
                    var bytesRead = await pipeServer.ReadAsync(buffer, 0, bufferLength, cancellationToken).ConfigureAwait(false);
#elif NETCOREAPP3_1_OR_GREATER
                        var bytesRead = await pipeServer.ReadAsync(buffer.AsMemory(0, bufferLength), cancellationToken).ConfigureAwait(false);
#endif
                        if (bytesRead == 0) break;
                        stream.Write(buffer, 0, bytesRead);
                    }
                    stream.Seek(0, SeekOrigin.Begin);
                    var serializer = new DataContractSerializer(typeof(string[]));
                    try
                    {
                        return (string[])serializer.ReadObject(stream);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }

            public async static Task WaitForClientConnectionsAsync(NamedPipeServerStream pipeServer, Action<string[]> callback, CancellationToken cancellationToken)
            {
                do
                {
                    await pipeServer.WaitForConnectionAsync(cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var args = await ReadArgsAsync(pipeServer, cancellationToken).ConfigureAwait(false);
                        if (args != null) callback(args);
                    }
                    finally
                    {
                        pipeServer.Disconnect();
                    }
                } while (!cancellationToken.IsCancellationRequested);
            }

            private async static Task WriteArgsAsync(NamedPipeClientStream pipeClient, string[] args, CancellationToken cancellationToken)
            {
                byte[] content;
                using (MemoryStream stream = new MemoryStream())
                {
                    var serializer = new DataContractSerializer(typeof(string[]));
                    serializer.WriteObject(stream, args);
                    content = stream.ToArray();
                }
#if NETFRAMEWORK
            await pipeClient.WriteAsync(content, 0, content.Length, cancellationToken).ConfigureAwait(false);
#elif NETCOREAPP3_1_OR_GREATER
                await pipeClient.WriteAsync(content.AsMemory(0, content.Length), cancellationToken).ConfigureAwait(false);
#endif
            }

            public async static Task SendSecondInstanceArgsAsync(string pipeName, string[] args, CancellationToken cancellationToken)
            {
                using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(serverName: ".",
                                                                                    pipeName: pipeName,
                                                                                    direction: PipeDirection.Out,
                                                                                    options: NamedPipeOptions))
                {
                    await pipeClient.ConnectAsync(cancellationToken).ConfigureAwait(false);
                    await WriteArgsAsync(pipeClient, args, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        public event StartupNextInstanceEventHandler StartupNextInstance;
        //How long a subsequent instance will wait for the original instance to get on its feet.
        private const int SECOND_INSTANCE_TIMEOUT = 2500; // milliseconds.
        private CancellationTokenSource _firstInstanceTokenSource;

        /// <summary>
        /// Indicates whether this application is singleton.
        /// </summary>
        public bool IsSingleInstance { get; set; }

        /// <summary>
        /// Generates the name for the remote singleton that we use to channel multiple instances to the same application model thread.
        ///  </summary>
        private static string GetApplicationInstanceID(Assembly Entry) => Entry.ManifestModule.ModuleVersionId.ToString();

        /// <summary>
        /// Extensibility point which raises the StartupNextInstance
        /// </summary>
        /// <param name="eventArgs"></param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            StartupNextInstance?.Invoke(this, eventArgs);
            if (eventArgs.BringToForeground && MainWindow != null)
            {
                if (MainWindow.Visibility != Visibility.Visible)
                    MainWindow.Show();
                if (MainWindow.WindowState == WindowState.Minimized)
                    MainWindow.WindowState = WindowState.Normal;

                MainWindow.Activate();
                MainWindow.SwitchToThisWindow();
            }
        }

        private void OnStartupNextInstanceMarshallingAdaptor(string[] args)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.InvokeAsync(() => OnStartupNextInstanceMarshallingAdaptor(args));
                return;
            }
            if (MainWindow == null)
                return;
            var invoked = false;
            try
            {
                invoked = true;
                OnStartupNextInstance(new StartupNextInstanceEventArgs(new ReadOnlyCollection<string>(args), bringToForegroundFlag: true));
            }
            catch (Exception) when (!invoked)
            {
            }
        }

        /// <summary>
        /// Entry point to kick off the App Startup/Shutdown Application model
        /// </summary>
        /// <param name="commandLine">The command line from Main()</param>
        /// <param name="continueStartup"></param>
        public void RunImpl(string[] commandLine, ref bool continueStartup)
        {
            if (!IsSingleInstance)
            {
                continueStartup = true;
                return;
            }
            string ApplicationInstanceID = GetApplicationInstanceID(Assembly.GetCallingAssembly()); // Note: Must pass the calling assembly from here so we can get the running app.  Otherwise, can break single instance.
            if (SingletonManagerHelper.TryCreatePipeServer(ApplicationInstanceID, out NamedPipeServerStream pipeServer))
            {
                // --- This is the first instance of a single-instance application to run.  
                var tokenSource = new CancellationTokenSource();
                _firstInstanceTokenSource = tokenSource;
                var tsk = SingletonManagerHelper.WaitForClientConnectionsAsync(pipeServer, OnStartupNextInstanceMarshallingAdaptor, cancellationToken: tokenSource.Token);
                continueStartup = true;
            }
            else
            {
                // --- This is the instance that subsequent instances will attach to.
                var tokenSource = new CancellationTokenSource();
                tokenSource.CancelAfter(SECOND_INSTANCE_TIMEOUT);
                try
                {
                    var awaitable = SingletonManagerHelper.SendSecondInstanceArgsAsync(ApplicationInstanceID, commandLine, cancellationToken: tokenSource.Token).ConfigureAwait(false);
                    awaitable.GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                    throw new CantStartSingleInstanceException();
                }
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            bool continueStartup = false;
            RunImpl(e.Args, ref continueStartup);
            if (continueStartup)
                base.OnStartup(e);
            else
                Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _firstInstanceTokenSource?.Cancel();
            base.OnExit(e);
        }
    }


#if NETFRAMEWORK

    /// <summary>
    /// <see cref="Application"/>实现这个接口
    /// </summary>
    public interface ISingleInstance
    {
        void SignalExternalCommandLineArgs(IList<string> args);
    }

    /// <summary>
    /// Application base for WPF application model.
    /// in order to implement singleton app
    /// </summary>
    /// <remarks>
    /// Apply to .NetFramework
    /// </remarks>
    public class AppSingleInstanceBase : Application, ISingleInstance
    {
        public virtual void SignalExternalCommandLineArgs(IList<string> args)
        {
            if (MainWindow.Visibility != Visibility.Visible)
                MainWindow.Show();
            if (MainWindow.WindowState == WindowState.Minimized)
                MainWindow.WindowState = WindowState.Normal;

            MainWindow.Activate();
            MainWindow.SwitchToThisWindow();
        }
    }

    /// <summary>
    /// This class checks to make sure that only one instance of this application is running at a time.
    /// </summary>
    /// <remarks>
    /// Note: this class should be used with some caution, because it does no
    /// security checking. For example, if one instance of an app that uses this class
    /// is running as Administrator, any other instance, even if it is not
    /// running as Administrator, can activate it with command line arguments.
    /// For most apps, this will not be much of an issue.
    /// </remarks>
    public class SingleInstance<TApplication> where TApplication : AppSingleInstanceBase
    {
        /// <summary>
        /// Remoting service class which is exposed by the server i.e the first instance and called by the second instance
        /// to pass on the command line arguments to the first instance and cause it to activate itself.
        /// </summary>
        private class IPCRemoteService : MarshalByRefObject
        {
            /// <summary>
            /// Activates the first instance of the application.
            /// </summary>
            /// <param name="args">List of arguments to pass to the first instance.</param>
            public void InvokeFirstInstance(IList<string> args)
            {
                if (Application.Current != null)
                {
                    // Do an asynchronous call to ActivateFirstInstance function
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(ActivateFirstInstanceCallback), args);
                }
            }

            /// <summary>
            /// Remoting Object's ease expires after every 5 minutes by default. We need to override the InitializeLifetimeService class
            /// to ensure that lease never expires.
            /// </summary>
            /// <returns>Always null.</returns>
            public override object InitializeLifetimeService()
            {
                return null;
            }
        }


        /// <summary>
        /// String delimiter used in channel names.
        /// </summary>
        private const string Delimiter = ":";

        /// <summary>
        /// Suffix to the channel name.
        /// </summary>
        private const string ChannelNameSuffix = "SingeInstanceIPCChannel";

        /// <summary>
        /// Remote service name.
        /// </summary>
        private const string RemoteServiceName = "SingleInstanceApplicationService";

        /// <summary>
        /// IPC protocol used (string).
        /// </summary>
        private const string IpcProtocol = "ipc://";

        /// <summary>
        /// Application mutex.
        /// </summary>
        private static Mutex singleInstanceMutex;

        /// <summary>
        /// IPC channel for communications.
        /// </summary>
        private static IpcServerChannel channel;


        /// <summary>
        /// List of command line arguments for the application.
        /// </summary>
        private static IList<string> commandLineArgs;

        /// <summary>
        /// Gets list of command line arguments for the application.
        /// </summary>
        public static IList<string> CommandLineArgs => commandLineArgs;

        /// <summary>
        /// Checks if the instance of the application attempting to start is the first instance. 
        /// If not, activates the first instance.
        /// </summary>
        /// <returns>True if this is the first instance of the application.</returns>
        public static bool InitializeAsFirstInstance(string uniqueName)
        {
            commandLineArgs = GetCommandLineArgs(uniqueName);

            // Build unique application Id and the IPC channel name.
            string applicationIdentifier = uniqueName + Environment.UserName;

            string channelName = String.Concat(applicationIdentifier, Delimiter, ChannelNameSuffix);

            // Create mutex based on unique application Id to check if this is the first instance of the application. 
            bool firstInstance;
            singleInstanceMutex = new Mutex(true, applicationIdentifier, out firstInstance);
            if (firstInstance)
            {
                CreateRemoteService(channelName);
            }
            else
            {
                SignalFirstInstance(channelName, commandLineArgs);
            }

            return firstInstance;
        }

        /// <summary>
        /// Cleans up single-instance code, clearing shared resources, mutexes, etc.
        /// </summary>
        public static void Cleanup()
        {
            if (singleInstanceMutex != null)
            {
                singleInstanceMutex.Close();
                singleInstanceMutex = null;
            }

            if (channel != null)
            {
                ChannelServices.UnregisterChannel(channel);
                channel = null;
            }
        }

        /// <summary>
        /// Gets command line args - for ClickOnce deployed applications, command line args may not be passed directly, they have to be retrieved.
        /// </summary>
        /// <returns>List of command line arg strings.</returns>
        private static IList<string> GetCommandLineArgs(string uniqueApplicationName)
        {
            string[] args = null;
            if (AppDomain.CurrentDomain.ActivationContext == null)
            {
                // The application was not clickonce deployed, get args from standard API's
                args = Environment.GetCommandLineArgs();
            }
            else
            {
                // The application was clickonce deployed
                // Clickonce deployed apps cannot recieve traditional commandline arguments
                // As a workaround commandline arguments can be written to a shared location before 
                // the app is launched and the app can obtain its commandline arguments from the 
                // shared location               
                string appFolderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), uniqueApplicationName);

                string cmdLinePath = Path.Combine(appFolderPath, "cmdline.txt");
                if (File.Exists(cmdLinePath))
                {
                    try
                    {
                        using (TextReader reader = new StreamReader(cmdLinePath, Encoding.Unicode))
                        {
                            args = CommandLineToArgvW(reader.ReadToEnd());
                        }

                        File.Delete(cmdLinePath);
                    }
                    catch (IOException)
                    {
                    }
                }
            }

            if (args == null)
            {
                args = new string[] { };
            }

            return new List<string>(args);
        }

        public static string[] CommandLineToArgvW(string cmdLine)
        {
            IntPtr argv = IntPtr.Zero;
            try
            {
                int numArgs = 0;

                argv = InteropMethods.CommandLineToArgvW(cmdLine, out numArgs);
                if (argv == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }
                var result = new string[numArgs];

                for (int i = 0; i < numArgs; i++)
                {
                    IntPtr currArg = Marshal.ReadIntPtr(argv, i * Marshal.SizeOf(typeof(IntPtr)));
                    result[i] = Marshal.PtrToStringUni(currArg);
                }

                return result;
            }
            finally
            {
                IntPtr p = InteropMethods.LocalFree(argv);
                // Otherwise LocalFree failed.
                // Assert.AreEqual(IntPtr.Zero, p);
            }
        }

        /// <summary>
        /// Creates a remote service for communication.
        /// </summary>
        /// <param name="channelName">Application's IPC channel name.</param>
        private static void CreateRemoteService(string channelName)
        {
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Dictionary<string, string>();

            props["name"] = channelName;
            props["portName"] = channelName;
            props["exclusiveAddressUse"] = "false";

            // Create the IPC Server channel with the channel properties
            channel = new IpcServerChannel(props, serverProvider);

            // Register the channel with the channel services
            ChannelServices.RegisterChannel(channel, true);

            // Expose the remote service with the REMOTE_SERVICE_NAME
            IPCRemoteService remoteService = new IPCRemoteService();
            RemotingServices.Marshal(remoteService, RemoteServiceName);
        }

        /// <summary>
        /// Creates a client channel and obtains a reference to the remoting service exposed by the server - 
        /// in this case, the remoting service exposed by the first instance. Calls a function of the remoting service 
        /// class to pass on command line arguments from the second instance to the first and cause it to activate itself.
        /// </summary>
        /// <param name="channelName">Application's IPC channel name.</param>
        /// <param name="args">
        /// Command line arguments for the second instance, passed to the first instance to take appropriate action.
        /// </param>
        private static void SignalFirstInstance(string channelName, IList<string> args)
        {
            IpcClientChannel secondInstanceChannel = new IpcClientChannel();
            ChannelServices.RegisterChannel(secondInstanceChannel, true);

            string remotingServiceUrl = IpcProtocol + channelName + "/" + RemoteServiceName;

            // Obtain a reference to the remoting service exposed by the server i.e the first instance of the application
            IPCRemoteService firstInstanceRemoteServiceReference = (IPCRemoteService)RemotingServices.Connect(typeof(IPCRemoteService), remotingServiceUrl);

            // Check that the remote service exists, in some cases the first instance may not yet have created one, in which case
            // the second instance should just exit
            if (firstInstanceRemoteServiceReference != null)
            {
                // Invoke a method of the remote service exposed by the first instance passing on the command line
                // arguments and causing the first instance to activate itself
                firstInstanceRemoteServiceReference.InvokeFirstInstance(args);
            }
        }

        /// <summary>
        /// Callback for activating first instance of the application.
        /// </summary>
        /// <param name="arg">Callback argument.</param>
        /// <returns>Always null.</returns>
        private static object ActivateFirstInstanceCallback(object arg)
        {
            // Get command line args to be passed to first instance
            IList<string> args = arg as IList<string>;
            ActivateFirstInstance(args);
            return null;
        }

        /// <summary>
        /// Activates the first instance of the application with arguments from a second instance.
        /// </summary>
        /// <param name="args">List of arguments to supply the first instance of the application.</param>
        private static void ActivateFirstInstance(IList<string> args)
        {
            // Set main window state and process command line args
            if (Application.Current == null)
            {
                return;
            }

            ((TApplication)Application.Current).SignalExternalCommandLineArgs(args);
        }
    }

#endif

}
