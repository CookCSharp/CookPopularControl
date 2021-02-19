﻿using System.Diagnostics.CodeAnalysis;

namespace CookPopularControl.Tools.Interop.Handle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class CommonHandles
    {
        public static readonly int Icon = HandleCollector.RegisterType(nameof(Icon), 20, 500);

        public static readonly int HDC = HandleCollector.RegisterType(nameof(HDC), 100, 2);

        public static readonly int GDI = HandleCollector.RegisterType(nameof(GDI), 50, 500);

        public static readonly int Kernel = HandleCollector.RegisterType(nameof(Kernel), 0, 1000);
    }
}
