using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        #region FileOperation

        private static readonly string[] suffixes = new string[] { " B", " KB", " MB", " GB", " TB", " PB" };
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="decimalPlaces">保留几位小数</param>
        /// <returns>返回文件的实际大小及单位</returns>
        public static string ToFileMemorySize(this string filePath, int decimalPlaces = 2)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException($"{filePath} is not exist");

            try
            {
                using var fs = File.OpenRead(filePath);
                var size = fs.Length;
                double last = 1;
                for (int i = 0; i < suffixes.Length; i++)
                {
                    var current = Math.Pow(1024, i + 1);
                    var temp = size / current;
                    if (temp < 1)
                        return (size / last).ToString($"N{decimalPlaces}") + suffixes[i];

                    last = current;
                }

                return fs.Length.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 安全读取文件(一次性读取)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回文件流的字节大小</returns>
        /// <remarks>适用于确定文件流的长度时，可以保障文件已经完全读完</remarks>
        public static byte[] ReadFileInSafe(this string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException($"{filePath} is not exist");

            try
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                int offset = 0; long remainData = fs.Length;
                var buffer = new byte[fs.Length];
                //只要有剩余的字节就不停的读
                while (remainData>0)
                {
                    int read = fs.Read(buffer, offset, (int)remainData);
                    if (read <= 0)
                        throw new EndOfStreamException($"文件读取到{read}时失败");

                    //剩余的字节数
                    remainData -= read;
                    //偏移量
                    offset += read;
                }

                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 使用内存缓存循环读取文件(高效)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回文件流的字节大小</returns>
        /// <remarks>
        /// 适用于不确定流的实际长度时，例如网络流，
        /// 可以先初始化一段缓存，再将流读出来的流信息写到内存流里面
        /// </remarks>
        public static byte[] ReadFileInBufferMemory(this string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException($"{filePath} is not exist");

            try
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                //初始化一个48KB的缓存
                int bufferSize = 1024 * 48;
                var buffer = new byte[bufferSize];
                using (var ms = new MemoryStream())
                {
                    while (true)
                    {
                        int read = fs.Read(buffer, 0, bufferSize);
                        //如果值为0,则已到达流结尾,返回结果
                        if (read <= 0)
                            return ms.ToArray();
                        ms.Write(buffer, 0, read);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 使用指定缓存长度的方式读取文件(高效)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="bufferLen">缓存长度</param>
        /// <returns>返回文件流的字节大小</returns>
        /// <remarks>
        /// 虽然在很多情况下可以直接使用Stream.Length得到流的长度，但不是所有的流都可以得到
        /// </remarks>
        public static byte[] ReadFileInBufferLength(this string filePath, int bufferLen = 0x8000)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException($"{filePath} is not exist");

            //如果指定的无效长度的缓冲区，则指定一个默认的长度作为缓存大小
            if (bufferLen < 1)
                bufferLen = 0x8000;

            try
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                var buffer = new byte[bufferLen];
                int offset = 0, block;
                while ((block = fs.Read(buffer, offset, buffer.Length - offset)) > 0)
                {
                    //重新设定读取位置
                    offset += block;

                    //检查是否到达了缓存的边界，检查是否还有可以读取的信息
                    if (offset == buffer.Length)
                    {
                        int nextByte = fs.ReadByte();
                        //如果值为-1，则已到达流的末尾，可以返回
                        if (nextByte.Equals(-1))
                            return buffer;

                        //调整数组大小准备继续读取
                        var newBuffer = new byte[buffer.Length * 2];
                        Array.Copy(buffer, newBuffer, buffer.Length);
                        newBuffer[offset] = (byte)nextByte;
                        buffer = newBuffer; //buffer是一个引用(指针)，这里意在重新设定buffer指针指向一个更大的内存

                        offset++;
                    }
                }

                //如果缓存太大,则使用ret来收缩前面while读取的buffer，然后直接返回
                var ret = new byte[offset];
                Array.Copy(buffer, ret, offset);

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SimpleStringExtension

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

        #endregion

        #region Brush

        /// <summary>
        /// 十六进制转<see cref="Brush"/>
        /// </summary>
        /// <param name="hexadecimalString"></param>
        /// <returns></returns>
        public static Brush ToBrush(this string hexadecimalString) => (Brush)new BrushConverter().ConvertFromString(hexadecimalString);

        /// <summary>
        /// 十六进制转<see cref="Color"/>
        /// </summary>
        /// <param name="hexadecimalString"></param>
        /// <returns></returns>
        public static Color ToColor(this string hexadecimalString) => (Color)(ColorConverter.ConvertFromString(hexadecimalString));

        #endregion
    }
}
