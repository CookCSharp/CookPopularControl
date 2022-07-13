using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SafeLocalMemoryHandle
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-27 11:17:32
 */
namespace CookPopularCSharpToolkit.Windows.Interop
{
    /// <summary>
    /// Represents a warpper class for a local memory pointer
    /// </summary>
    [SuppressUnmanagedCodeSecurity(),
    HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    public class SafeLocalMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeLocalMemoryHandle() : base(true)
        {

        }

        public SafeLocalMemoryHandle(IntPtr preexistingHandle, bool ownsHandle) : base(ownsHandle)
        {
            base.SetHandle(preexistingHandle);
        }



        protected override bool ReleaseHandle()
        {
            return InteropMethods.LocalFree(base.handle) == IntPtr.Zero;
        }
    }
}
