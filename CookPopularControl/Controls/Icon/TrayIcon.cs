using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;



/*
 * Description：TrayIcon 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-01 14:40:25
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    /***************
     * 可参考 https://github.com/hardcodet/wpf-notifyicon
     * */
    /// <summary>
    /// 托盘图标
    /// </summary>
    public partial class TrayIcon : FrameworkElement, IDisposable
    {
        private static readonly object LockObject = new object();
        private const int ABM_GETTASKBARPOS = 0x00000005;

        /// <summary>
        /// Creates, updates or deletes the taskbar icon.
        /// </summary>
        [DllImport("shell32.Dll", CharSet = CharSet.Unicode)]
        internal static extern bool Shell_NotifyIcon(NotifyCommand cmd, [In] ref TrayIconData data);

        #region Fields and Properties

        private TrayIconData _trayIconData;
        private readonly WindowMessageSink _messageSink;
        private InteropValues.APPBARDATA m_data;
        /// <summary>
        /// An action that is being invoked if the <see cref="_singleClickTimer"/> fires.
        /// </summary>
        private Action _singleClickTimerAction;

        /// <summary>
        /// A timer that is used to differentiate between single and double clicks.
        /// </summary>
        private readonly Timer _singleClickTimer;

        /// <summary>
        /// A timer that is used to close open balloon tooltips.
        /// </summary>
        private readonly Timer _balloonCloseTimer;

        public static bool IsDesignMode => (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;

        /// <summary>
        /// Indicates whether the taskbar icon has been created or not.
        /// </summary>
        public bool IsTaskbarIconCreated { get; private set; }

        /// <summary>
        /// The time we should wait for a double click.
        /// </summary>
        private int DoubleClickWaitTime => NoLeftClickDelay ? 0 : InteropMethods.GetDoubleClickTime();

        /// <summary>
        /// Indicates whether custom tooltips are supported, which depends
        /// on the OS. Windows Vista or higher is required in order to
        /// support this feature.
        /// </summary>
        public bool SupportsCustomToolTips => _messageSink.Version == NotifyIconVersion.Vista;

        /// <summary>
        /// Checks whether a non-tooltip popup is currently opened.
        /// </summary>
        private bool IsPopupOpen
        {
            get
            {
                var popup = TrayPopupResolved;
                var menu = ContextMenu;
                var balloon = CustomBalloon;

                return popup != null && popup.IsOpen ||
                       menu != null && menu.IsOpen ||
                       balloon != null && balloon.IsOpen;
            }
        }

        /// <summary>
        /// Specify a custom popup position
        /// </summary>
        internal GetCustomPopupPosition CustomPopupPosition { get; set; }

        #endregion

        public TrayIcon()
        {
            _messageSink = IsDesignMode ? WindowMessageSink.CreateEmpty() : new WindowMessageSink(NotifyIconVersion.Win95);
            _trayIconData = TrayIconData.CreateDefault(_messageSink.MessageWindowHandle);

            CreateTrayIcon();

            _messageSink.MouseEventReceived += OnMouseEvent;
            _messageSink.TaskbarCreated += OnTaskbarCreated;
            _messageSink.ChangeToolTipStateRequest += OnToolTipChange;
            _messageSink.BalloonToolTipChanged += OnBalloonToolTipChanged;

            // init single click / balloon timers
            _singleClickTimer = new Timer(DoSingleClickAction);
            _balloonCloseTimer = new Timer(CloseBalloonCallback);

            // register listener in order to get notified when the application closes
            if (Application.Current != null)
            {
                Application.Current.Exit += OnExit;
            }
        }

        /// <summary>
        /// Processes mouse events, which are bubbled through the class' routed events, trigger certain actions (e.g. show a popup), or both.
        /// </summary>
        /// <param name="me">Event flag</param>
        private void OnMouseEvent(MouseEvent me)
        {
            if (IsDisposed) return;

            switch (me)
            {
                case MouseEvent.MouseMove:
                    RaiseTrayMouseMoveEvent();
                    // immediately return - there's nothing left to evaluate
                    return;
                case MouseEvent.IconRightMouseDown:
                    RaiseTrayRightMouseDownEvent();
                    break;
                case MouseEvent.IconLeftMouseDown:
                    RaiseTrayLeftMouseDownEvent();
                    break;
                case MouseEvent.IconRightMouseUp:
                    RaiseTrayRightMouseUpEvent();
                    break;
                case MouseEvent.IconLeftMouseUp:
                    RaiseTrayLeftMouseUpEvent();
                    break;
                case MouseEvent.IconMiddleMouseDown:
                    RaiseTrayMiddleMouseDownEvent();
                    break;
                case MouseEvent.IconMiddleMouseUp:
                    RaiseTrayMiddleMouseUpEvent();
                    break;
                case MouseEvent.IconDoubleClick:
                    // cancel single click timer
                    _singleClickTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    // bubble event
                    RaiseTrayMouseDoubleClickEvent();
                    break;
                case MouseEvent.BalloonToolTipClicked:
                    RaiseTrayBalloonTipClickedEvent();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(me), "Missing handler for mouse event flag: " + me);
            }

            // get mouse coordinates
            InteropValues.POINT cursorPosition = new InteropValues.POINT();
            if (_messageSink.Version == NotifyIconVersion.Vista)
            {
                // physical cursor position is supported for Vista and above
                InteropMethods.GetPhysicalCursorPos(ref cursorPosition);
            }
            else
            {
                InteropMethods.GetCursorPos(out cursorPosition);
            }

            cursorPosition = GetDeviceCoordinates(cursorPosition);

            bool isLeftClickCommandInvoked = false;

            // show popup, if requested
            if (IsMatch(me, PopupActivation))
            {
                if (me == MouseEvent.IconLeftMouseUp)
                {
                    // show popup once we are sure it's not a double click
                    _singleClickTimerAction = () =>
                    {
                        ExecuteIfEnabled(LeftClickCommand, LeftClickCommandParameter, LeftClickCommandTarget ?? this);
                        ShowTrayPopup(cursorPosition);
                    };
                    _singleClickTimer.Change(DoubleClickWaitTime, Timeout.Infinite);
                    isLeftClickCommandInvoked = true;
                }
                else
                {
                    // show popup immediately
                    ShowTrayPopup(cursorPosition);
                }
            }

            // show context menu, if requested
            if (IsMatch(me, MenuActivation))
            {
                if (me == MouseEvent.IconLeftMouseUp)
                {
                    // show context menu once we are sure it's not a double click
                    _singleClickTimerAction = () =>
                    {
                        ExecuteIfEnabled(LeftClickCommand, LeftClickCommandParameter, LeftClickCommandTarget ?? this);
                        ShowContextMenu(cursorPosition);
                    };
                    _singleClickTimer.Change(DoubleClickWaitTime, Timeout.Infinite);
                    isLeftClickCommandInvoked = true;
                }
                else
                {
                    // show context menu immediately
                    ShowContextMenu(cursorPosition);
                }
            }

            // make sure the left click command is invoked on mouse clicks
            if (me == MouseEvent.IconLeftMouseUp && !isLeftClickCommandInvoked)
            {
                // show context menu once we are sure it's not a double click
                _singleClickTimerAction = () =>
                {
                    ExecuteIfEnabled(LeftClickCommand, LeftClickCommandParameter, LeftClickCommandTarget ?? this);
                };
                _singleClickTimer.Change(DoubleClickWaitTime, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Displays a custom tooltip, if available. This method is only
        /// invoked for Windows Vista and above.
        /// </summary>
        /// <param name="visible">Whether to show or hide the tooltip.</param>
        private void OnToolTipChange(bool visible)
        {
            // if we don't have a tooltip, there's nothing to do here...
            if (TrayToolTipResolved == null) return;

            if (visible)
            {
                if (IsPopupOpen)
                {
                    // ignore if we are already displaying something down there
                    return;
                }

                var args = RaisePreviewTrayToolTipOpenEvent();
                if (args.Handled) return;

                TrayToolTipResolved.IsOpen = true;

                // raise attached event first
                if (TrayToolTip != null) RaiseToolTipOpenedEvent(TrayToolTip);

                // bubble routed event
                RaiseTrayToolTipOpenEvent();
            }
            else
            {
                var args = RaisePreviewTrayToolTipCloseEvent();
                if (args.Handled) return;

                // raise attached event first
                if (TrayToolTip != null) RaiseToolTipCloseEvent(TrayToolTip);

                TrayToolTipResolved.IsOpen = false;

                // bubble event
                RaiseTrayToolTipCloseEvent();
            }
        }

        /// <summary>
        /// Bubbles events if a balloon ToolTip was displayed
        /// or removed.
        /// </summary>
        /// <param name="visible">Whether the ToolTip was just displayed
        /// or removed.</param>
        private void OnBalloonToolTipChanged(bool visible)
        {
            if (visible)
            {
                RaiseTrayBalloonTipShownEvent();
            }
            else
            {
                RaiseTrayBalloonTipClosedEvent();
            }
        }

        /// <summary>
        /// Performs a delayed action if the user requested an action
        /// based on a single click of the left mouse.<br/>
        /// This method is invoked by the <see cref="_singleClickTimer"/>.
        /// </summary>
        private void DoSingleClickAction(object state)
        {
            if (IsDisposed) return;

            // run action
            Action action = _singleClickTimerAction;
            if (action != null)
            {
                // cleanup action
                _singleClickTimerAction = null;

                // switch to UI thread
                GetDispatcher(this).Invoke(action);
            }
        }

        /// <summary>
        /// Timer-invoke event which closes the currently open balloon and
        /// resets the <see cref="CustomBalloon"/> dependency property.
        /// </summary>
        private void CloseBalloonCallback(object state)
        {
            if (IsDisposed) return;

            // switch to UI thread
            Action action = CloseBalloon;
            GetDispatcher(this).Invoke(action);
        }

        /// <summary>
        /// Displays the <see cref="TrayPopup"/> control if it was set.
        /// </summary>
        private void ShowTrayPopup(InteropValues.POINT cursorPosition)
        {
            if (IsDisposed) return;

            // raise preview event no matter whether popup is currently set
            // or not (enables client to set it on demand)
            var args = RaisePreviewTrayPopupOpenEvent();
            if (args.Handled) return;

            if (TrayPopup == null)
            {
                return;
            }

            // use absolute position, but place the popup centered above the icon
            TrayPopupResolved.Placement = PlacementMode.AbsolutePoint;
            TrayPopupResolved.HorizontalOffset = cursorPosition.X;
            TrayPopupResolved.VerticalOffset = cursorPosition.Y;

            // open popup
            TrayPopupResolved.IsOpen = true;

            IntPtr handle = IntPtr.Zero;
            if (TrayPopupResolved.Child != null)
            {
                // try to get a handle on the popup itself (via its child)
                HwndSource source = (HwndSource)PresentationSource.FromVisual(TrayPopupResolved.Child);
                if (source != null) handle = source.Handle;
            }

            // if we don't have a handle for the popup, fall back to the message sink
            if (handle == IntPtr.Zero) handle = _messageSink.MessageWindowHandle;

            // activate either popup or message sink to track deactivation.
            // otherwise, the popup does not close if the user clicks somewhere else
            InteropMethods.SetForegroundWindow(handle);

            // raise attached event - item should never be null unless developers
            // changed the CustomPopup directly...
            if (TrayPopup != null) RaisePopupOpenedEvent(TrayPopup);

            // bubble routed event
            RaiseTrayPopupOpenEvent();
        }

        /// <summary>
        /// Displays the <see cref="ContextMenu"/> if it was set.
        /// </summary>
        private void ShowContextMenu(InteropValues.POINT cursorPosition)
        {
            if (IsDisposed) return;

            // raise preview event no matter whether context menu is currently set
            // or not (enables client to set it on demand)
            var args = RaisePreviewTrayContextMenuOpenEvent();
            if (args.Handled) return;

            if (ContextMenu == null)
            {
                return;
            }

            // use absolute positioning. We need to set the coordinates, or a delayed opening
            // (e.g. when left-clicked) opens the context menu at the wrong place if the mouse
            // is moved!
            ContextMenu.Placement = PlacementMode.AbsolutePoint;
            ContextMenu.HorizontalOffset = cursorPosition.X;
            ContextMenu.VerticalOffset = cursorPosition.Y;
            ContextMenu.IsOpen = true;

            IntPtr handle = IntPtr.Zero;

            // try to get a handle on the context itself
            HwndSource source = (HwndSource)PresentationSource.FromVisual(ContextMenu);
            if (source != null)
            {
                handle = source.Handle;
            }

            // if we don't have a handle for the popup, fall back to the message sink
            if (handle == IntPtr.Zero) handle = _messageSink.MessageWindowHandle;

            // activate the context menu or the message window to track deactivation - otherwise, the context menu
            // does not close if the user clicks somewhere else. With the message window
            // fallback, the context menu can't receive keyboard events - should not happen though
            InteropMethods.SetForegroundWindow(handle);

            // bubble event
            RaiseTrayContextMenuOpenEvent();
        }

        private void ExecuteIfEnabled(ICommand command, object commandParameter, IInputElement target)
        {
            if (command == null) return;

            RoutedCommand? rc = command as RoutedCommand;
            if (rc != null)
            {
                //routed commands work on a target
                if (rc.CanExecute(commandParameter, target)) rc.Execute(commandParameter, target);
            }
            else if (command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }

        private bool IsMatch(MouseEvent me, PopupActivationMode activationMode)
        {
            switch (activationMode)
            {
                case PopupActivationMode.LeftClick:
                    return me == MouseEvent.IconLeftMouseUp;
                case PopupActivationMode.RightClick:
                    return me == MouseEvent.IconRightMouseUp;
                case PopupActivationMode.LeftOrRightClick:
                    return Is(MouseEvent.IconLeftMouseUp, MouseEvent.IconRightMouseUp);
                case PopupActivationMode.LeftOrDoubleClick:
                    return Is(MouseEvent.IconLeftMouseUp, MouseEvent.IconDoubleClick);
                case PopupActivationMode.DoubleClick:
                    return Is(MouseEvent.IconDoubleClick);
                case PopupActivationMode.MiddleClick:
                    return me == MouseEvent.IconMiddleMouseUp;
                case PopupActivationMode.All:
                    //return true for everything except mouse movements
                    return me != MouseEvent.MouseMove;
                default:
                    throw new ArgumentOutOfRangeException("activationMode");
            }
        }

        /// <summary>
        /// Checks a list of candidates for equality to a given
        /// reference value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The evaluated value.</param>
        /// <param name="candidates">A liste of possible values that are
        /// regarded valid.</param>
        /// <returns>True if one of the submitted <paramref name="candidates"/>
        /// matches the evaluated value. If the <paramref name="candidates"/>
        /// parameter itself is null, too, the method returns false as well,
        /// which allows to check with null values, too.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="candidates"/>
        /// is a null reference.</exception>
        private bool Is<T>(T value, params T[] candidates)
        {
            if (candidates == null) return false;
            
            foreach (var t in candidates)
            {
                if (value.Equals(t)) return true;
            }

            return false;
        }

        /// <summary>
        /// Closes the current <see cref="CustomBalloon"/>, if the
        /// property is set.
        /// </summary>
        public void CloseBalloon()
        {
            if (IsDisposed) return;

            Dispatcher dispatcher = GetDispatcher(this);
            if (!dispatcher.CheckAccess())
            {
                Action action = CloseBalloon;
                dispatcher.Invoke(DispatcherPriority.Normal, action);
                return;
            }

            lock (LockObject)
            {
                // reset timer in any case
                _balloonCloseTimer.Change(Timeout.Infinite, Timeout.Infinite);

                // reset old popup, if we still have one
                Popup popup = CustomBalloon;
                if (popup == null)
                {
                    return;
                }

                UIElement element = popup.Child;

                // announce closing
                RoutedEventArgs eventArgs = RaiseBalloonClosingEvent(element, this);
                if (!eventArgs.Handled)
                {
                    // if the event was handled, clear the reference to the popup,
                    // but don't close it - the handling code has to manage this stuff now

                    // close the popup
                    popup.IsOpen = false;

                    // remove the reference of the popup to the balloon in case we want to reuse
                    // the balloon (then added to a new popup)
                    popup.Child = null;

                    // reset attached property
                    if (element != null) SetParentTrayIcon(element, null);
                }

                // remove custom balloon anyway
                SetCustomBalloon(null);
            }
        }

        private InteropValues.POINT GetDeviceCoordinates(InteropValues.POINT point)
        {
            return new InteropValues.POINT
            {
                X = (int)(point.X / DpiHelper.GetScaleX()),
                Y = (int)(point.Y / DpiHelper.GetScaleY())
            };
        }

        /// <summary>
        /// Returns a dispatcher for multi-threaded scenarios
        /// </summary>
        /// <returns>Dispatcher</returns>
        private Dispatcher GetDispatcher(DispatcherObject source)
        {
            //use the application's dispatcher by default
            if (Application.Current != null) return Application.Current.Dispatcher;

            //fallback for WinForms environments
            if (source.Dispatcher != null) return source.Dispatcher;

            // ultimately use the thread's dispatcher
            return Dispatcher.CurrentDispatcher;
        }

        #region WriteIconData

        /// <summary>
        /// Updates the taskbar icons with data provided by a given
        /// <see cref="TrayIconData"/> instance.
        /// </summary>
        /// <param name="data">Configuration settings for the NotifyIcon.</param>
        /// <param name="command">Operation on the icon (e.g. delete the icon).</param>
        /// <returns>True if the data was successfully written.</returns>
        /// <remarks>See Shell_NotifyIcon documentation on MSDN for details.</remarks>
        private bool WriteIconData(ref TrayIconData data, NotifyCommand command)
        {
            return WriteIconData(ref data, command, data.ValidMembers);
        }

        /// <summary>
        /// Updates the taskbar icons with data provided by a given
        /// <see cref="TrayIconData"/> instance.
        /// </summary>
        /// <param name="data">Configuration settings for the NotifyIcon.</param>
        /// <param name="command">Operation on the icon (e.g. delete the icon).</param>
        /// <param name="flags">Defines which members of the <paramref name="data"/>
        /// structure are set.</param>
        /// <returns>True if the data was successfully written.</returns>
        /// <remarks>See Shell_NotifyIcon documentation on MSDN for details.</remarks>
        private bool WriteIconData(ref TrayIconData data, NotifyCommand command, IconDataMembers flags)
        {
            if (IsDesignMode) return true;

            data.ValidMembers = flags;
            lock (LockObject)
            {
                return Shell_NotifyIcon(command, ref data);
            }
        }

        #endregion

        #region Create / Remove Taskbar Icon

        /// <summary>
        /// Recreates the taskbar icon if the whole taskbar was recreated (e.g. because Explorer was shut down).
        /// </summary>
        private void OnTaskbarCreated()
        {
            RemoveTrayIcon();
            CreateTrayIcon();
        }

        /// <summary>
        /// Creates the taskbar icon. This message is invoked during initialization,
        /// if the taskbar is restarted, and whenever the icon is displayed.
        /// </summary>
        private void CreateTrayIcon()
        {
            lock (LockObject)
            {
                if (IsTaskbarIconCreated)
                {
                    return;
                }

                const IconDataMembers members = IconDataMembers.Message | IconDataMembers.Icon | IconDataMembers.Tip;

                //write initial configuration
                var status = WriteIconData(ref _trayIconData, NotifyCommand.Add, members);
                if (!status)
                {
                    // couldn't create the icon - we can assume this is because explorer is not running (yet!)
                    // -> try a bit later again rather than throwing an exception. Typically, if the windows
                    // shell is being loaded later, this method is being re-invoked from OnTaskbarCreated
                    // (we could also retry after a delay, but that's currently YAGNI)
                    return;
                }

                //set to most recent version
                SetVersion();
                _messageSink.Version = (NotifyIconVersion)_trayIconData.VersionOrTimeout;

                IsTaskbarIconCreated = true;
            }
        }

        /// <summary>
        /// Closes the taskbar icon if required.
        /// </summary>
        private void RemoveTrayIcon()
        {
            lock (LockObject)
            {
                // make sure we didn't schedule a creation

                if (!IsTaskbarIconCreated)
                {
                    return;
                }

                WriteIconData(ref _trayIconData, NotifyCommand.Delete, IconDataMembers.Message);
                IsTaskbarIconCreated = false;
            }
        }

        private void SetVersion()
        {
            _trayIconData.VersionOrTimeout = (uint)NotifyIconVersion.Vista;
            bool status = Shell_NotifyIcon(NotifyCommand.SetVersion, ref _trayIconData);

            if (!status)
            {
                _trayIconData.VersionOrTimeout = (uint)NotifyIconVersion.Win2000;
                status = WriteIconData(ref _trayIconData, NotifyCommand.SetVersion);
            }

            if (!status)
            {
                _trayIconData.VersionOrTimeout = (uint)NotifyIconVersion.Win95;
                status = WriteIconData(ref _trayIconData, NotifyCommand.SetVersion);
            }

            if (!status)
            {
                Debug.Fail("Could not set version");
            }
        }

        #endregion

        #region Dispose / Exit

        /// <summary>
        /// Set to true as soon as <c>Dispose</c> has been invoked.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Checks if the object has been disposed and
        /// raises a <see cref="ObjectDisposedException"/> in case
        /// the <see cref="IsDisposed"/> flag is true.
        /// </summary>
        private void EnsureNotDisposed()
        {
            if (IsDisposed) throw new ObjectDisposedException(Name ?? GetType().FullName);
        }

        /// <summary>
        /// Disposes the class if the application exits.
        /// </summary>
        private void OnExit(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// This destructor will run only if the <see cref="Dispose()"/>
        /// method does not get called. This gives this base class the
        /// opportunity to finalize.
        /// <para>
        /// Important: Do not provide destructor in types derived from this class.
        /// </para>
        /// </summary>
        ~TrayIcon()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <remarks>This method is not virtual by design. Derived classes
        /// should override <see cref="Dispose(bool)"/>.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Closes the tray and releases all resources.
        /// </summary>
        /// <summary>
        /// <c>Dispose(bool disposing)</c> executes in two distinct scenarios.
        /// If disposing equals <c>true</c>, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// </summary>
        /// <param name="disposing">If disposing equals <c>false</c>, the method
        /// has been called by the runtime from inside the finalizer and you
        /// should not reference other objects. Only unmanaged resources can
        /// be disposed.</param>
        /// <remarks>Check the <see cref="IsDisposed"/> property to determine whether
        /// the method has already been called.</remarks>
        private void Dispose(bool disposing)
        {
            // don't do anything if the component is already disposed
            if (IsDisposed || !disposing) return;

            lock (LockObject)
            {
                IsDisposed = true;

                // de-register application event listener
                if (Application.Current != null)
                {
                    Application.Current.Exit -= OnExit;
                }

                // stop timers
                _singleClickTimer.Dispose();
                _balloonCloseTimer.Dispose();

                // dispose message sink
                _messageSink.Dispose();

                // remove icon
                RemoveTrayIcon();
            }
        }

        #endregion

        /// <summary>
        /// Shows a custom control as a tooltip in the tray location.
        /// </summary>
        /// <param name="balloon"></param>
        /// <param name="animation">An optional animation for the popup.</param>
        /// <param name="timeout">The time after which the popup is being closed.
        /// Submit null in order to keep the balloon open indefinitely
        /// </param>
        /// <exception cref="ArgumentNullException">If <paramref name="balloon"/>
        /// is a null reference.</exception>
        public void ShowCustomBalloon(UIElement balloon, PopupAnimation animation, int? timeout)
        {
            var dispatcher = GetDispatcher(this);
            if (!dispatcher.CheckAccess())
            {
                var action = new Action(() => ShowCustomBalloon(balloon, animation, timeout));
                dispatcher.Invoke(DispatcherPriority.Normal, action);
                return;
            }

            if (balloon == null) throw new ArgumentNullException(nameof(balloon));
            if (timeout.HasValue && timeout < 500)
            {
                string msg = "Invalid timeout of {0} milliseconds. Timeout must be at least 500 ms";
                msg = string.Format(msg, timeout);
                throw new ArgumentOutOfRangeException(nameof(timeout), msg);
            }

            EnsureNotDisposed();

            // make sure we don't have an open balloon
            lock (LockObject)
            {
                CloseBalloon();
            }

            // create an invisible popup that hosts the UIElement
            Popup popup = new Popup
            {
                AllowsTransparency = true
            };

            // provide the popup with the taskbar icon's data context
            UpdateDataContext(popup, null, DataContext);

            // don't animate by default - developers can use attached events or override
            popup.PopupAnimation = animation;

            // in case the balloon is cleaned up through routed events, the
            // control didn't remove the balloon from its parent popup when
            // if was closed the last time - just make sure it doesn't have
            // a parent that is a popup
            var parent = LogicalTreeHelper.GetParent(balloon) as Popup;
            if (parent != null) parent.Child = null;

            if (parent != null)
            {
                string msg = "Cannot display control [{0}] in a new balloon popup - that control already has a parent. You may consider creating new balloons every time you want to show one.";
                msg = string.Format(msg, balloon);
                throw new InvalidOperationException(msg);
            }

            popup.Child = balloon;

            //don't set the PlacementTarget as it causes the popup to become hidden if the
            //TaskbarIcon's parent is hidden, too...
            //popup.PlacementTarget = this;

            popup.Placement = PlacementMode.AbsolutePoint;
            popup.StaysOpen = true;


            InteropValues.POINT position = CustomPopupPosition != null ? CustomPopupPosition() : GetTrayLocation();
            popup.HorizontalOffset = position.X - 1;
            popup.VerticalOffset = position.Y - 1;

            //store reference
            lock (LockObject)
            {
                SetCustomBalloon(popup);
            }

            // assign this instance as an attached property
            SetParentTrayIcon(balloon, this);

            // fire attached event
            RaiseBalloonShowingEvent(balloon, this);

            // display item
            popup.IsOpen = true;

            if (timeout.HasValue)
            {
                // register timer to close the popup
                _balloonCloseTimer.Change(timeout.Value, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Gets the position of the system tray.
        /// </summary>
        /// <returns>Tray coordinates.</returns>
        private InteropValues.POINT GetTrayLocation()
        {
            int space = 2;
            GetSystemTaskBarPosition();

            Rectangle rcWorkArea = new Rectangle(m_data.rc.Left, m_data.rc.Top, m_data.rc.Right - m_data.rc.Left, m_data.rc.Bottom - m_data.rc.Top);

            int x = 0, y = 0;
            switch ((ScreenEdge)m_data.uEdge)
            {
                case ScreenEdge.Left:
                    x = rcWorkArea.Right + space;
                    y = rcWorkArea.Bottom;
                    break;
                case ScreenEdge.Bottom:
                    x = rcWorkArea.Right;
                    y = rcWorkArea.Bottom - rcWorkArea.Height - space;
                    break;
                case ScreenEdge.Top:
                    x = rcWorkArea.Right;
                    y = rcWorkArea.Top + rcWorkArea.Height + space;
                    break;
                case ScreenEdge.Right:
                    x = rcWorkArea.Right - rcWorkArea.Width - space;
                    y = rcWorkArea.Bottom;
                    break;
            }

            return GetDeviceCoordinates(new InteropValues.POINT { X = x, Y = y });
        }

        /// <summary>
        /// Returns the location of the system tray
        /// </summary>
        /// <returns>Point</returns>
        public System.Windows.Point GetPopupTrayPosition()
        {
            var point = GetTrayLocation();
            return new System.Windows.Point(point.X, point.Y);
        }

        /// <summary>
        /// Updates the system taskbar position
        /// </summary>
        public void GetSystemTaskBarPosition()
        {
            GetPosition("Shell_TrayWnd", null);

            // Update the location of the appbar with the specified classname and window name.
            void GetPosition(string strClassName, string strWindowName)
            {
                m_data = new InteropValues.APPBARDATA();
                m_data.cbSize = (int)Marshal.SizeOf(m_data.GetType());

                IntPtr hWnd = InteropMethods.FindWindow(strClassName, strWindowName);

                if (hWnd != IntPtr.Zero)
                {
                    uint uResult = InteropMethods.SHAppBarMessage(ABM_GETTASKBARPOS, ref m_data);

                    if (uResult != 1)
                    {
                        throw new Exception("Failed to communicate with the given AppBar");
                    }
                }
                else
                {
                    throw new Exception("Failed to find an AppBar that matched the given criteria");
                }
            }
        }

        /// <summary>
        /// Resets the closing timeout, which effectively
        /// keeps a displayed balloon message open until
        /// it is either closed programmatically through
        /// <see cref="CloseBalloon"/> or due to a new
        /// message being displayed.
        /// </summary>
        public void ResetBalloonCloseTimer()
        {
            if (IsDisposed) return;

            lock (LockObject)
            {
                //reset timer in any case
                _balloonCloseTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Displays a balloon tip with the specified title,
        /// text, and icon in the taskbar for the specified time period.
        /// </summary>
        /// <param name="title">The title to display on the balloon tip.</param>
        /// <param name="message">The text to display on the balloon tip.</param>
        /// <param name="symbol">A symbol that indicates the severity.</param>
        public void ShowBalloonTip(string title, string message, BalloonIcon symbol)
        {
            lock (LockObject)
            {
                ShowBalloonTip(title, message, GetBalloonFlag(symbol), IntPtr.Zero);
            }

            BalloonFlags GetBalloonFlag(BalloonIcon icon)
            {
                switch (icon)
                {
                    case BalloonIcon.None:
                        return BalloonFlags.None;
                    case BalloonIcon.Info:
                        return BalloonFlags.Info;
                    case BalloonIcon.Warning:
                        return BalloonFlags.Warning;
                    case BalloonIcon.Error:
                        return BalloonFlags.Error;
                    default:
                        throw new ArgumentOutOfRangeException("icon");
                }
            }
        }

        /// <summary>
        /// Displays a balloon tip with the specified title,
        /// text, and a custom icon in the taskbar for the specified time period.
        /// </summary>
        /// <param name="title">The title to display on the balloon tip.</param>
        /// <param name="message">The text to display on the balloon tip.</param>
        /// <param name="customIcon">A custom icon.</param>
        /// <param name="largeIcon">True to allow large icons (Windows Vista and later).</param>
        /// <exception cref="ArgumentNullException">If <paramref name="customIcon"/>
        /// is a null reference.</exception>
        public void ShowBalloonTip(string title, string message, Icon customIcon, bool largeIcon = false)
        {
            if (customIcon == null) throw new ArgumentNullException(nameof(customIcon));

            lock (LockObject)
            {
                var flags = BalloonFlags.User;

                if (largeIcon)
                {
                    // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                    flags |= BalloonFlags.LargeIcon;
                }

                ShowBalloonTip(title, message, flags, customIcon.Handle);
            }
        }

        /// <summary>
        /// Hides a balloon ToolTip, if any is displayed.
        /// </summary>
        public void HideBalloonTip()
        {
            EnsureNotDisposed();

            // reset balloon by just setting the info to an empty string
            _trayIconData.BalloonText = _trayIconData.BalloonTitle = string.Empty;
            WriteIconData(ref _trayIconData, NotifyCommand.Modify, IconDataMembers.Info);
        }

        /// <summary>
        /// Invokes <see cref="Shell_NotifyIcon"/> in order to display
        /// a given balloon ToolTip.
        /// </summary>
        /// <param name="title">The title to display on the balloon tip.</param>
        /// <param name="message">The text to display on the balloon tip.</param>
        /// <param name="flags">Indicates what icon to use.</param>
        /// <param name="balloonIconHandle">A handle to a custom icon, if any, or
        /// <see cref="IntPtr.Zero"/>.</param>
        private void ShowBalloonTip(string title, string message, BalloonFlags flags, IntPtr balloonIconHandle)
        {
            EnsureNotDisposed();

            _trayIconData.BalloonText = message ?? string.Empty;
            _trayIconData.BalloonTitle = title ?? string.Empty;

            _trayIconData.BalloonFlags = flags;
            _trayIconData.CustomBalloonIconHandle = balloonIconHandle;
            WriteIconData(ref _trayIconData, NotifyCommand.Modify, IconDataMembers.Info | IconDataMembers.Icon);
        }
    }

    /// <summary>
    /// Receives messages from the taskbar icon through
    /// window messages of an underlying helper window.
    /// </summary>
    public class WindowMessageSink : IDisposable
    {
        /// <summary>
        /// The ID of messages that are received from the the
        /// taskbar icon.
        /// </summary>
        public const int CallbackMessageId = 0x400;

        /// <summary>
        /// The ID of the message that is being received if the
        /// taskbar is (re)started.
        /// </summary>
        private uint _taskbarRestartMessageId;

        /// <summary>
        /// Used to track whether a mouse-up event is just
        /// the aftermath of a double-click and therefore needs
        /// to be suppressed.
        /// </summary>
        private bool _isDoubleClick;

        /// <summary>
        /// A delegate that processes messages of the hidden
        /// native window that receives window messages. Storing
        /// this reference makes sure we don't loose our reference
        /// to the message window.
        /// </summary>
        private WindowProcedureHandler _messageHandler;

        /// <summary>
        /// Window class ID.
        /// </summary>
        internal string WindowId { get; private set; }

        /// <summary>
        /// Handle for the message window.
        /// </summary>
        internal IntPtr MessageWindowHandle { get; private set; }

        /// <summary>
        /// The version of the underlying icon. Defines how
        /// incoming messages are interpreted.
        /// </summary>
        public NotifyIconVersion Version { get; set; }

        /// <summary>
        /// The custom tooltip should be closed or hidden.
        /// </summary>
        public event Action<bool> ChangeToolTipStateRequest;

        /// <summary>
        /// Fired in case the user clicked or moved within
        /// the taskbar icon area.
        /// </summary>
        public event Action<MouseEvent> MouseEventReceived;

        /// <summary>
        /// Fired if a balloon ToolTip was either displayed
        /// or closed (indicated by the boolean flag).
        /// </summary>
        public event Action<bool> BalloonToolTipChanged;

        /// <summary>
        /// Fired if the taskbar was created or restarted. Requires the taskbar
        /// icon to be reset.
        /// </summary>
        public event Action TaskbarCreated;


        /// <summary>
        /// Creates a new message sink that receives message from
        /// a given taskbar icon.
        /// </summary>
        /// <param name="version"></param>
        public WindowMessageSink(NotifyIconVersion version)
        {
            Version = version;
            CreateMessageWindow();
        }


        private WindowMessageSink()
        {
        }

        /// <summary>
        /// Creates a dummy instance that provides an empty
        /// pointer rather than a real window handler.<br/>
        /// Used at design time.
        /// </summary>
        /// <returns>WindowMessageSink</returns>
        internal static WindowMessageSink CreateEmpty()
        {
            return new WindowMessageSink
            {
                MessageWindowHandle = IntPtr.Zero,
                Version = NotifyIconVersion.Vista
            };
        }

        /// <summary>
        /// Creates the helper message window that is used
        /// to receive messages from the taskbar icon.
        /// </summary>
        private void CreateMessageWindow()
        {
            //generate a unique ID for the window
            WindowId = "WPFTaskbarIcon_" + Guid.NewGuid();

            //register window message handler
            _messageHandler = OnWindowMessageReceived;

            // Create a simple window class which is reference through
            //the messageHandler delegate
            InteropValues.WNDCLASS wc;

            wc.style = 0;
            wc.lpfnWndProc = _messageHandler;
            wc.cbClsExtra = 0;
            wc.cbWndExtra = 0;
            wc.hInstance = IntPtr.Zero;
            wc.hIcon = IntPtr.Zero;
            wc.hCursor = IntPtr.Zero;
            wc.hbrBackground = IntPtr.Zero;
            wc.lpszMenuName = string.Empty;
            wc.lpszClassName = WindowId;

            // Register the window class
            InteropMethods.RegisterClass(ref wc);

            // Get the message used to indicate the taskbar has been restarted
            // This is used to re-add icons when the taskbar restarts
            _taskbarRestartMessageId = InteropMethods.RegisterWindowMessage("TaskbarCreated");

            // Create the message window
            MessageWindowHandle = InteropMethods.CreateWindowEx(0, WindowId, "", 0, 0, 0, 1, 1, IntPtr.Zero, IntPtr.Zero,
                IntPtr.Zero, IntPtr.Zero);

            if (MessageWindowHandle == IntPtr.Zero)
            {
                throw new Win32Exception("Message window handle was not a valid pointer");
            }
        }

        #region Handle Window Messages

        /// <summary>
        /// Callback method that receives messages from the taskbar area.
        /// </summary>
        private IntPtr OnWindowMessageReceived(IntPtr hWnd, uint messageId, IntPtr wParam, IntPtr lParam)
        {
            if (messageId == _taskbarRestartMessageId)
            {
                //recreate the icon if the taskbar was restarted (e.g. due to Win Explorer shutdown)
                var listener = TaskbarCreated;
                listener?.Invoke();
            }

            //forward message
            ProcessWindowMessage(messageId, wParam, lParam);

            // Pass the message to the default window procedure
            return InteropMethods.DefWindowProc(hWnd, messageId, wParam, lParam);
        }

        /// <summary>
        /// Processes incoming system messages.
        /// </summary>
        /// <param name="msg">Callback ID.</param>
        /// <param name="wParam">If the version is <see cref="NotifyIconVersion.Vista"/>
        /// or higher, this parameter can be used to resolve mouse coordinates.
        /// Currently not in use.</param>
        /// <param name="lParam">Provides information about the event.</param>
        private void ProcessWindowMessage(uint msg, IntPtr wParam, IntPtr lParam)
        {
            // Check if it was a callback message
            if (msg != CallbackMessageId)
            {
                // It was not a callback message, but make sure it's not something else we need to process
                switch ((WindowsMessages)msg)
                {
                    case WindowsMessages.WM_DPICHANGED:
                        Debug.WriteLine("DPI Change");
                        //SystemInfo.UpdateDpiFactors();
                        break;
                }
                return;
            }

            var message = (WindowsMessages)lParam.ToInt32();
            Debug.WriteLine("Got message " + message);
            switch (message)
            {
                case WindowsMessages.WM_CONTEXTMENU:
                    // TODO: Handle WM_CONTEXTMENU, see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw
                    Debug.WriteLine("Unhandled WM_CONTEXTMENU");
                    break;
                case WindowsMessages.WM_MOUSEMOVE:
                    MouseEventReceived?.Invoke(MouseEvent.MouseMove);
                    break;
                case WindowsMessages.WM_LBUTTONDOWN:
                    MouseEventReceived?.Invoke(MouseEvent.IconLeftMouseDown);
                    break;
                case WindowsMessages.WM_LBUTTONUP:
                    if (!_isDoubleClick)
                    {
                        MouseEventReceived?.Invoke(MouseEvent.IconLeftMouseUp);
                    }
                    _isDoubleClick = false;
                    break;
                case WindowsMessages.WM_LBUTTONDBLCLK:
                    _isDoubleClick = true;
                    MouseEventReceived?.Invoke(MouseEvent.IconDoubleClick);
                    break;
                case WindowsMessages.WM_RBUTTONDOWN:
                    MouseEventReceived?.Invoke(MouseEvent.IconRightMouseDown);
                    break;
                case WindowsMessages.WM_RBUTTONUP:
                    MouseEventReceived?.Invoke(MouseEvent.IconRightMouseUp);
                    break;
                case WindowsMessages.WM_RBUTTONDBLCLK:
                    //double click with right mouse button - do not trigger event
                    break;
                case WindowsMessages.WM_MBUTTONDOWN:
                    MouseEventReceived?.Invoke(MouseEvent.IconMiddleMouseDown);
                    break;
                case WindowsMessages.WM_MBUTTONUP:
                    MouseEventReceived?.Invoke(MouseEvent.IconMiddleMouseUp);
                    break;
                case WindowsMessages.WM_MBUTTONDBLCLK:
                    //double click with middle mouse button - do not trigger event
                    break;
                case WindowsMessages.NIN_BALLOONSHOW:
                    BalloonToolTipChanged?.Invoke(true);
                    break;
                case WindowsMessages.NIN_BALLOONHIDE:
                case WindowsMessages.NIN_BALLOONTIMEOUT:
                    BalloonToolTipChanged?.Invoke(false);
                    break;
                case WindowsMessages.NIN_BALLOONUSERCLICK:
                    MouseEventReceived?.Invoke(MouseEvent.BalloonToolTipClicked);
                    break;
                case WindowsMessages.NIN_POPUPOPEN:
                    ChangeToolTipStateRequest?.Invoke(true);
                    break;
                case WindowsMessages.NIN_POPUPCLOSE:
                    ChangeToolTipStateRequest?.Invoke(false);
                    break;
                case WindowsMessages.NIN_SELECT:
                    // TODO: Handle NIN_SELECT see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw
                    Debug.WriteLine("Unhandled NIN_SELECT");
                    break;
                case WindowsMessages.NIN_KEYSELECT:
                    // TODO: Handle NIN_KEYSELECT see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw
                    Debug.WriteLine("Unhandled NIN_KEYSELECT");
                    break;
                default:
                    Debug.WriteLine("Unhandled NotifyIcon message ID: " + lParam);
                    break;
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Set to true as soon as <c>Dispose</c> has been invoked.
        /// </summary>
        public bool IsDisposed { get; private set; }


        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <remarks>This method is not virtual by design. Derived classes
        /// should override <see cref="Dispose(bool)"/>.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This destructor will run only if the <see cref="Dispose()"/>
        /// method does not get called. This gives this base class the
        /// opportunity to finalize.
        /// <para>
        /// Important: Do not provide destructor in types derived from
        /// this class.
        /// </para>
        /// </summary>
        ~WindowMessageSink()
        {
            Dispose(false);
        }

        /// <summary>
        /// Removes the windows hook that receives window
        /// messages and closes the underlying helper window.
        /// </summary>
        private void Dispose(bool disposing)
        {
            //don't do anything if the component is already disposed
            if (IsDisposed) return;
            IsDisposed = true;

            //always destroy the unmanaged handle (even if called from the GC)
            InteropMethods.DestroyWindow(MessageWindowHandle);
            _messageHandler = null;
        }

        #endregion
    }
}
