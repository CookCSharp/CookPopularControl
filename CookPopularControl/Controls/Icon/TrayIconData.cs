using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;



/*
 * Description：TrayIconData 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-01 21:11:16
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// Callback delegate which is used by the Windows API to submit window messages.
    /// </summary>
    public delegate IntPtr WindowProcedureHandler(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

    internal delegate InteropValues.POINT GetCustomPopupPosition();

    ///<summary>
    /// Supported icons for the tray's balloon messages.
    ///</summary>
    public enum BalloonIcon
    {
        /// <summary>
        /// The balloon message is displayed without an icon.
        /// </summary>
        None,

        /// <summary>
        /// An information is displayed.
        /// </summary>
        Info,

        /// <summary>
        /// A warning is displayed.
        /// </summary>
        Warning,

        /// <summary>
        /// An error is displayed.
        /// </summary>
        Error
    }

    /// <summary>
    /// This enum defines the windows messages we respond to.
    /// See more on Windows messages <a href="https://docs.microsoft.com/en-us/windows/win32/learnwin32/window-messages">here</a>
    /// </summary>
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public enum WindowsMessages : uint
    {
        /// <summary>
        /// Notifies a window that the user clicked the right mouse button (right-clicked) in the window.
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/menurc/wm-contextmenu">WM_CONTEXTMENU message</a>
        ///
        /// In case of a notify icon:
        /// If a user selects a notify icon's shortcut menu with the keyboard, the Shell now sends the associated application a WM_CONTEXTMENU message. Earlier versions send WM_RBUTTONDOWN and WM_RBUTTONUP messages.
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw">Shell_NotifyIcon function</a>
        /// </summary>
        WM_CONTEXTMENU = 0x007b,

        /// <summary>
        /// Posted to a window when the cursor moves.
        /// If the mouse is not captured, the message is posted to the window that contains the cursor.
        /// Otherwise, the message is posted to the window that has captured the mouse.
        ///
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mousemove">WM_MOUSEMOVE message</a>
        /// </summary>
        WM_MOUSEMOVE = 0x0200,

        /// <summary>
        /// Posted when the user presses the left mouse button while the cursor is in the client area of a window.
        /// If the mouse is not captured, the message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has captured the mouse.
        ///
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttondown">WM_LBUTTONDOWN message</a>
        /// </summary>
        WM_LBUTTONDOWN = 0x0201,

        /// <summary>
        /// Posted when the user releases the left mouse button while the cursor is in the client area of a window.
        /// If the mouse is not captured, the message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has captured the mouse.
        ///
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttonup">WM_LBUTTONUP message</a>
        /// </summary>
        WM_LBUTTONUP = 0x0202,

        /// <summary>
        /// Posted when the user double-clicks the left mouse button while the cursor is in the client area of a window.
        /// If the mouse is not captured, the message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has captured the mouse.
        ///
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttondblclk">WM_LBUTTONDBLCLK message</a>
        /// </summary>
        WM_LBUTTONDBLCLK = 0x0203,

        /// <summary>
        /// Posted when the user presses the right mouse button while the cursor is in the client area of a window.
        /// If the mouse is not captured, the message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has captured the mouse.
        ///
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-rbuttondown">WM_RBUTTONDOWN message</a>
        /// </summary>
        WM_RBUTTONDOWN = 0x0204,

        /// <summary>
        /// Posted when the user releases the right mouse button while the cursor is in the client area of a window.
        /// If the mouse is not captured, the message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has captured the mouse.
        ///
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-rbuttonup">WM_RBUTTONUP message</a>
        /// </summary>
        WM_RBUTTONUP = 0x0205,

        /// <summary>
        /// Posted when the user double-clicks the right mouse button while the cursor is in the client area of a window.
        /// If the mouse is not captured, the message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has captured the mouse.
        ///
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-rbuttondblclk">WM_RBUTTONDBLCLK message</a>
        /// </summary>
        WM_RBUTTONDBLCLK = 0x0206,

        /// <summary>
        /// Posted when the user presses the middle mouse button while the cursor is in the client area of a window.
        /// If the mouse is not captured, the message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has captured the mouse.
        ///
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mbuttondown">WM_MBUTTONDOWN message</a>
        /// </summary>
        WM_MBUTTONDOWN = 0x0207,

        /// <summary>
        /// Posted when the user releases the middle mouse button while the cursor is in the client area of a window.
        /// If the mouse is not captured, the message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has captured the mouse.
        ///
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mbuttonup">WM_MBUTTONUP message</a>
        /// </summary>
        WM_MBUTTONUP = 0x0208,

        /// <summary>
        /// Posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window.
        /// If the mouse is not captured, the message is posted to the window beneath the cursor.
        /// Otherwise, the message is posted to the window that has captured the mouse.
        ///
        /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mbuttondblclk">WM_MBUTTONDBLCLK message</a>
        /// </summary>
        WM_MBUTTONDBLCLK = 0x0209,

        /// <summary>
        /// Sent when the effective dots per inch (dpi) for a window has changed.
        /// The DPI is the scale factor for a window.
        /// There are multiple events that can cause the DPI to change.
        /// </summary>
        WM_DPICHANGED = 0x02e0,

        /// <summary>
        /// Used to define private messages for use by private window classes, usually of the form WM_USER+x, where x is an integer value.
        /// </summary>
        WM_USER = 0x0400,

        /// <summary>
        /// This message is only send when using NOTIFYICON_VERSION_4, the Shell now sends the associated application an NIN_SELECT notification.
        /// Send when a notify icon is activated with mouse or ENTER key.
        /// Earlier versions send WM_RBUTTONDOWN and WM_RBUTTONUP messages.
        /// </summary>
        NIN_SELECT = WM_USER,

        /// <summary>
        /// This message is only send when using NOTIFYICON_VERSION_4, the Shell now sends the associated application an NIN_SELECT notification.
        /// Send when a notify icon is activated with SPACEBAR or ENTER key.
        /// Earlier versions send WM_RBUTTONDOWN and WM_RBUTTONUP messages.
        /// </summary>
        NIN_KEYSELECT = WM_USER + 1,

        /// <summary>
        /// Sent when the balloon is shown (balloons are queued).
        /// </summary>
        NIN_BALLOONSHOW = WM_USER + 2,

        /// <summary>
        /// Sent when the balloon disappears. For example, when the icon is deleted.
        /// This message is not sent if the balloon is dismissed because of a timeout or if the user clicks the mouse.
        ///
        /// As of Windows 7, NIN_BALLOONHIDE is also sent when a notification with the NIIF_RESPECT_QUIET_TIME flag set attempts to display during quiet time (a user's first hour on a new computer).
        /// In that case, the balloon is never displayed at all.
        /// </summary>
        NIN_BALLOONHIDE = WM_USER + 3,

        /// <summary>
        /// Sent when the balloon is dismissed because of a timeout.
        /// </summary>
        NIN_BALLOONTIMEOUT = WM_USER + 4,

        /// <summary>
        /// Sent when the balloon is dismissed because the user clicked the mouse.
        /// </summary>
        NIN_BALLOONUSERCLICK = WM_USER + 5,

        /// <summary>
        /// Sent when the user hovers the cursor over an icon to indicate that the richer pop-up UI should be used in place of a standard textual tooltip.
        /// </summary>
        NIN_POPUPOPEN = WM_USER + 6,

        /// <summary>
        /// Sent when a cursor no longer hovers over an icon to indicate that the rich pop-up UI should be closed.
        /// </summary>
        NIN_POPUPCLOSE = WM_USER + 7
    }

    ///// <summary>
    ///// Win API WNDCLASS struct - represents a single window.
    ///// Used to receive window messages.
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential)]
    //public struct WindowClass
    //{
    //    public uint style;
    //    public WindowProcedureHandler lpfnWndProc;
    //    public int cbClsExtra;
    //    public int cbWndExtra;
    //    public IntPtr hInstance;
    //    public IntPtr hIcon;
    //    public IntPtr hCursor;
    //    public IntPtr hbrBackground;
    //    [MarshalAs(UnmanagedType.LPWStr)] public string lpszMenuName;
    //    [MarshalAs(UnmanagedType.LPWStr)] public string lpszClassName;
    //}

    /// <summary>
    /// Event flags for clicked events.
    /// </summary>
    public enum MouseEvent
    {
        /// <summary>
        /// The mouse was moved withing the
        /// taskbar icon's area.
        /// </summary>
        MouseMove,

        /// <summary>
        /// The right mouse button was clicked.
        /// </summary>
        IconRightMouseDown,

        /// <summary>
        /// The left mouse button was clicked.
        /// </summary>
        IconLeftMouseDown,

        /// <summary>
        /// The right mouse button was released.
        /// </summary>
        IconRightMouseUp,

        /// <summary>
        /// The left mouse button was released.
        /// </summary>
        IconLeftMouseUp,

        /// <summary>
        /// The middle mouse button was clicked.
        /// </summary>
        IconMiddleMouseDown,

        /// <summary>
        /// The middle mouse button was released.
        /// </summary>
        IconMiddleMouseUp,

        /// <summary>
        /// The taskbar icon was double clicked.
        /// </summary>
        IconDoubleClick,

        /// <summary>
        /// The balloon tip was clicked.
        /// </summary>
        BalloonToolTipClicked
    }

    /// <summary>
    /// Defines flags that define when a popup
    /// is being displyed.
    /// </summary>
    public enum PopupActivationMode
    {
        /// <summary>
        /// The item is displayed if the user clicks the
        /// tray icon with the left mouse button.
        /// </summary>
        LeftClick,

        /// <summary>
        /// The item is displayed if the user clicks the
        /// tray icon with the right mouse button.
        /// </summary>
        RightClick,

        /// <summary>
        /// The item is displayed if the user double-clicks the
        /// tray icon.
        /// </summary>
        DoubleClick,

        /// <summary>
        /// The item is displayed if the user clicks the
        /// tray icon with the left or the right mouse button.
        /// </summary>
        LeftOrRightClick,

        /// <summary>
        /// The item is displayed if the user clicks the
        /// tray icon with the left mouse button or if a
        /// double-click is being performed.
        /// </summary>
        LeftOrDoubleClick,

        /// <summary>
        /// The item is displayed if the user clicks the
        /// tray icon with the middle mouse button.
        /// </summary>
        MiddleClick,

        /// <summary>
        /// The item is displayed whenever a click occurs.
        /// </summary>
        All
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct TrayIconData
    {
        /// <summary>
        /// Size of this structure, in bytes.
        /// </summary>
        public uint CbSize;

        /// <summary>
        /// Handle to the window that receives notification messages associated with an icon in the
        /// taskbar status area. The Shell uses hWnd and uID to identify which icon to operate on
        /// when Shell_NotifyIcon is invoked.
        /// </summary>
        public IntPtr WindowHandle;

        /// <summary>
        /// Application-defined identifier of the taskbar icon. The Shell uses hWnd and uID to identify
        /// which icon to operate on when Shell_NotifyIcon is invoked. You can have multiple icons
        /// associated with a single hWnd by assigning each a different uID. This feature, however
        /// is currently not used.
        /// </summary>
        public uint TaskbarIconId;

        /// <summary>
        /// Flags that indicate which of the other members contain valid data. This member can be
        /// a combination of the NIF_XXX constants.
        /// </summary>
        public IconDataMembers ValidMembers;

        /// <summary>
        /// Application-defined message identifier. The system uses this identifier to send
        /// notifications to the window identified in hWnd.
        /// </summary>
        public uint CallbackMessageId;

        /// <summary>
        /// A handle to the icon that should be displayed. Just
        /// <c>Icon.Handle</c>.
        /// </summary>
        public IntPtr IconHandle;

        /// <summary>
        /// String with the text for a standard ToolTip. It can have a maximum of 64 characters including
        /// the terminating NULL. For Version 5.0 and later, szTip can have a maximum of
        /// 128 characters, including the terminating NULL.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string ToolTipText;

        /// <summary>
        /// State of the icon. Remember to also set the <see cref="StateMask"/>.
        /// </summary>
        public IconState IconState;

        /// <summary>
        /// A value that specifies which bits of the state member are retrieved or modified.
        /// For example, setting this member to <see cref="IconState.Hidden"/>
        /// causes only the item's hidden
        /// state to be retrieved.
        /// </summary>
        public IconState StateMask;

        /// <summary>
        /// String with the text for a balloon ToolTip. It can have a maximum of 255 characters.
        /// To remove the ToolTip, set the NIF_INFO flag in uFlags and set szInfo to an empty string.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string BalloonText;

        /// <summary>
        /// Mainly used to set the version when <see cref="TrayIcon.Shell_NotifyIcon"/> is invoked
        /// with <see cref="NotifyCommand.SetVersion"/>. However, for legacy operations,
        /// the same member is also used to set timeouts for balloon ToolTips.
        /// </summary>
        public uint VersionOrTimeout;

        /// <summary>
        /// String containing a title for a balloon ToolTip. This title appears in boldface
        /// above the text. It can have a maximum of 63 characters.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string BalloonTitle;

        /// <summary>
        /// Adds an icon to a balloon ToolTip, which is placed to the left of the title. If the
        /// <see cref="BalloonTitle"/> member is zero-length, the icon is not shown.
        /// </summary>
        public BalloonFlags BalloonFlags;

        /// <summary>
        /// Windows XP (Shell32.dll version 6.0) and later.<br/>
        /// - Windows 7 and later: A registered GUID that identifies the icon.
        ///   This value overrides uID and is the recommended method of identifying the icon.<br/>
        /// - Windows XP through Windows Vista: Reserved.
        /// </summary>
        public Guid TaskbarIconGuid;

        /// <summary>
        /// Windows Vista (Shell32.dll version 6.0.6) and later. The handle of a customized
        /// balloon icon provided by the application that should be used independently
        /// of the tray icon. If this member is non-NULL and the <see cref="BalloonFlags.User"/>
        /// flag is set, this icon is used as the balloon icon.<br/>
        /// If this member is NULL, the legacy behavior is carried out.
        /// </summary>
        public IntPtr CustomBalloonIconHandle;

        /// <summary>
        /// Creates a default data structure that provides
        /// a hidden taskbar icon without the icon being set.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns>NotifyIconData</returns>
        public static TrayIconData CreateDefault(IntPtr handle)
        {
            var data = new TrayIconData();

            if (Environment.OSVersion.Version.Major >= 6)
            {
                //use the current size
                data.CbSize = (uint)Marshal.SizeOf(data);
            }
            else
            {
                //we need to set another size on xp/2003- otherwise certain
                //features (e.g. balloon tooltips) don't work.
                data.CbSize = 952; // NOTIFYICONDATAW_V3_SIZE

                //set to fixed timeout
                data.VersionOrTimeout = 10;
            }

            data.WindowHandle = handle;
            data.TaskbarIconId = 0x0;
            data.CallbackMessageId = WindowMessageSink.CallbackMessageId;
            data.VersionOrTimeout = (uint)NotifyIconVersion.Win95;

            data.IconHandle = IntPtr.Zero;

            //hide initially
            data.IconState = IconState.Hidden;
            data.StateMask = IconState.Hidden;

            //set flags
            data.ValidMembers = IconDataMembers.Message | IconDataMembers.Icon | IconDataMembers.Tip;

            //reset strings
            data.ToolTipText = data.BalloonText = data.BalloonTitle = string.Empty;

            return data;
        }
    }

    /// <summary>
    /// A value that specifies an edge of the screen.
    /// </summary>
    public enum ScreenEdge
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = -1,
        /// <summary>
        /// Left edge.
        /// </summary>
        Left = 0,
        /// <summary>
        /// Top edge.
        /// </summary>
        Top = 1,
        /// <summary>
        /// Right edge.
        /// </summary>
        Right = 2,
        /// <summary>
        /// Bottom edge.
        /// </summary>
        Bottom = 3
    }

    /// <summary>
    /// Main operations performed on the <see cref="TrayIcon.Shell_NotifyIcon"/> function.
    /// </summary>
    public enum NotifyCommand
    {
        /// <summary>
        /// The taskbar icon is being created.
        /// </summary>
        Add = 0x00,

        /// <summary>
        /// The settings of the taskbar icon are being updated.
        /// </summary>
        Modify = 0x01,

        /// <summary>
        /// The taskbar icon is deleted.
        /// </summary>
        Delete = 0x02,

        /// <summary>
        /// Focus is returned to the taskbar icon. Currently not in use.
        /// </summary>
        SetFocus = 0x03,

        /// <summary>
        /// Shell32.dll version 5.0 and later only. Instructs the taskbar
        /// to behave according to the version number specified in the 
        /// uVersion member of the structure pointed to by lpdata.
        /// This message allows you to specify whether you want the version
        /// 5.0 behavior found on Microsoft Windows 2000 systems, or the
        /// behavior found on earlier Shell versions. The default value for
        /// uVersion is zero, indicating that the original Windows 95 notify
        /// icon behavior should be used.
        /// </summary>
        SetVersion = 0x04
    }

    /// <summary>
    /// Indicates which members of a <see cref="TrayIconData"/> structure
    /// were set, and thus contain valid data or provide additional information
    /// to the ToolTip as to how it should display.
    /// </summary>
    [Flags]
    public enum IconDataMembers
    {
        /// <summary>
        /// The message ID is set.
        /// </summary>
        Message = 0x01,

        /// <summary>
        /// The notification icon is set.
        /// </summary>
        Icon = 0x02,

        /// <summary>
        /// The tooltip is set.
        /// </summary>
        Tip = 0x04,

        /// <summary>
        /// State information (<see cref="IconState"/>) is set. This
        /// applies to both <see cref="IconState"/> and
        /// <see cref="TrayIconData.StateMask"/>.
        /// </summary>
        State = 0x08,

        /// <summary>
        /// The balloon ToolTip is set. Accordingly, the following
        /// members are set: <see cref="TrayIconData.BalloonText"/>,
        /// <see cref="TrayIconData.BalloonTitle"/>, <see cref="TrayIconData.BalloonFlags"/>,
        /// and <see cref="TrayIconData.VersionOrTimeout"/>.
        /// </summary>
        Info = 0x10,

        // Internal identifier is set. Reserved, thus commented out.
        //Guid = 0x20,

        /// <summary>
        /// Windows Vista (Shell32.dll version 6.0.6) and later. If the ToolTip
        /// cannot be displayed immediately, discard it.<br/>
        /// Use this flag for ToolTips that represent real-time information which
        /// would be meaningless or misleading if displayed at a later time.
        /// For example, a message that states "Your telephone is ringing."<br/>
        /// This modifies and must be combined with the <see cref="Info"/> flag.
        /// </summary>
        Realtime = 0x40,

        /// <summary>
        /// Windows Vista (Shell32.dll version 6.0.6) and later.
        /// Use the standard ToolTip. Normally, when uVersion is set
        /// to NOTIFYICON_VERSION_4, the standard ToolTip is replaced
        /// by the application-drawn pop-up user interface (UI).
        /// If the application wants to show the standard tooltip
        /// in that case, regardless of whether the on-hover UI is showing,
        /// it can specify NIF_SHOWTIP to indicate the standard tooltip
        /// should still be shown.<br/>
        /// Note that the NIF_SHOWTIP flag is effective until the next call 
        /// to Shell_NotifyIcon.
        /// </summary>
        UseLegacyToolTips = 0x80
    }

    /// <summary>
    /// The state of the icon - can be set to hide the icon.
    /// </summary>
    public enum IconState
    {
        /// <summary>
        /// The icon is visible.
        /// </summary>
        Visible = 0x00,

        /// <summary>
        /// Hide the icon.
        /// </summary>
        Hidden = 0x01,

        // The icon is shared - currently not supported, thus commented out.
        //Shared = 0x02
    }

    /// <summary>
    /// The notify icon version that is used. The higher the version, the more capabilities are available.
    /// </summary>
    public enum NotifyIconVersion
    {
        /// <summary>
        /// Default behavior (legacy Win95). Expects
        /// a <see cref="TrayIconData"/> size of 488.
        /// </summary>
        Win95 = 0x0,

        /// <summary>
        /// Behavior representing Win2000 an higher. Expects
        /// a <see cref="TrayIconData"/> size of 504.
        /// </summary>
        Win2000 = 0x3,

        /// <summary>
        /// Extended tooltip support, which is available for Vista and later.
        /// Detailed information about what the different versions do, can be found <a href="https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyicona">here</a>
        /// </summary>
        Vista = 0x4
    }

    /// <summary>
    /// Flags that define the icon that is shown on a balloon tooltip.
    /// </summary>
    public enum BalloonFlags
    {
        /// <summary>
        /// No icon is displayed.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// An information icon is displayed.
        /// </summary>
        Info = 0x01,

        /// <summary>
        /// A warning icon is displayed.
        /// </summary>
        Warning = 0x02,

        /// <summary>
        /// An error icon is displayed.
        /// </summary>
        Error = 0x03,

        /// <summary>
        /// Windows XP Service Pack 2 (SP2) and later.
        /// Use a custom icon as the title icon.
        /// </summary>
        User = 0x04,

        /// <summary>
        /// Windows XP (Shell32.dll version 6.0) and later.
        /// Do not play the associated sound. Applies only to balloon ToolTips.
        /// </summary>
        NoSound = 0x10,

        /// <summary>
        /// Windows Vista (Shell32.dll version 6.0.6) and later. The large version
        /// of the icon should be used as the balloon icon. This corresponds to the
        /// icon with dimensions SM_CXICON x SM_CYICON. If this flag is not set,
        /// the icon with dimensions XM_CXSMICON x SM_CYSMICON is used.<br/>
        /// - This flag can be used with all stock icons.<br/>
        /// - Applications that use older customized icons (NIIF_USER with hIcon) must
        ///   provide a new SM_CXICON x SM_CYICON version in the tray icon (hIcon). These
        ///   icons are scaled down when they are displayed in the System Tray or
        ///   System Control Area (SCA).<br/>
        /// - New customized icons (NIIF_USER with hBalloonIcon) must supply an
        ///   SM_CXICON x SM_CYICON version in the supplied icon (hBalloonIcon).
        /// </summary>
        LargeIcon = 0x20,

        /// <summary>
        /// Windows 7 and later.
        /// </summary>
        RespectQuietTime = 0x80
    }
}
