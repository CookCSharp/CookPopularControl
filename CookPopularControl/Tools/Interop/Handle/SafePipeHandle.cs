using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SafePipeHandle
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-27 11:04:43
 */
namespace CookPopularControl.Tools.Interop.Handle
{
    /// <summary>
    /// Represents a warpper class for pipe handle
    /// </summary>
    [SecurityCritical(),
     HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true),
     SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    internal sealed class SafePipeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafePipeHandle(bool ownsHandle) : base(true)
        {
        }

        public SafePipeHandle(IntPtr preexistingHandle, bool ownsHandle) : base(ownsHandle)
        {
            base.SetHandle(preexistingHandle);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success),
         DllImport("Kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool CloseHandle(IntPtr handle);

        protected override bool ReleaseHandle()
        {
            return CloseHandle(base.handle);
        }
    }
}
