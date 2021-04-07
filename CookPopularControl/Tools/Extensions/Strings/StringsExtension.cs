using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：StringsExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-07 11:13:31
 */
namespace CookPopularControl.Tools.Extensions.Strings
{
    /// <summary>
    /// 提供<see cref="string"/>的扩展方法
    /// </summary>
    public static class StringsExtension
    {
        /// <summary>
        /// 打开链接
        /// </summary>
        /// <param name="url"></param>
        public static void ToOpenLink(this string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
        }

        /// <summary>
        /// 十六进制转<see cref="Brush"/>
        /// </summary>
        /// <param name="hexadecimal"></param>
        /// <returns></returns>
        public static Brush ToBrush(this string hexadecimal) => (Brush)new BrushConverter().ConvertFromString(hexadecimal);
    }
}
