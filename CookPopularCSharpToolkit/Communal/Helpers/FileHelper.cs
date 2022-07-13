using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：FileHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-07 11:13:31
 */
namespace CookPopularCSharpToolkit.Communal
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public sealed class FileHelper
    {
        //所有需要复制的原始子文件路径
        private static List<string> originFilesPath = new List<string>();
        //替换后所有目标子文件路径
        private static List<string> newFilesPath = new List<string>();
        //子目录路径
        private static List<string> allDirectories = new List<string>();

        /// <summary>
        /// 批量文件复制
        /// </summary>
        /// <param name="sourceFolder">源文件夹</param>
        /// <param name="desFolder">目标文件夹</param>
        /// <param name="overwrite">是否覆盖</param>
        /// <param name="exceptFolder">除开集合文件夹与文件</param>
        /// <remarks>注意：排除的文件信息是指要复制的原始文件而不是目标文件信息</remarks>
        public static void FileCopys(string sourceFolder, string desFolder, bool overwrite = true, params string[] exceptFolder)
        {
            if (originFilesPath != null) originFilesPath.Clear();
            if (newFilesPath != null) newFilesPath.Clear();
            if (allDirectories != null) allDirectories.Clear();
            if (!Directory.Exists(desFolder)) Directory.CreateDirectory(desFolder);

            GetAllFiles(sourceFolder, exceptFolder);
            //替换路径
            foreach (var origin in originFilesPath)
            {
                newFilesPath.Add(origin.Replace(sourceFolder, desFolder));
            }
            for (int i = 0; i < allDirectories.Count; i++)
            {
                allDirectories[i] = allDirectories[i].Replace(sourceFolder, desFolder);
                //创建目录
                if (!Directory.Exists(allDirectories[i]))
                    Directory.CreateDirectory(allDirectories[i]);
            }
            for (int i = 0; i < originFilesPath.Count; i++)
            {
                File.Copy(originFilesPath[i], newFilesPath[i], overwrite);
            }
        }

        /// <summary>
        /// 删除目标文件夹下所有子文件夹和子文件
        /// </summary>
        /// <param name="sourceFolder">删除的目标文件夹路径</param>
        /// <param name="isSourceFolderDelete">是否删除目标文件夹本身</param>
        /// <param name="exceptFolder">除开集合文件夹与文件</param>
        /// <remarks>目标文件夹也被删除</remarks>
        public static void FileDeletes(string sourceFolder, bool isSourceFolderDelete = true, params string[] exceptFolder)
        {
            if (originFilesPath != null) originFilesPath.Clear();
            if (allDirectories != null) allDirectories.Clear();
            GetAllFiles(sourceFolder, exceptFolder);
            foreach (var file in originFilesPath)
            {
                File.Delete(file);
            }

            DeleteEmptyDirectory(sourceFolder, exceptFolder);
            if (isSourceFolderDelete)
                Directory.Delete(sourceFolder);
        }

        /// <summary>
        /// 删除空文件夹目录
        /// </summary>
        /// <param name="sourceFolder">删除的目标文件夹路径</param>
        /// <param name="exceptFolder">除开集合文件夹与文件</param>
        private static void DeleteEmptyDirectory(string sourceFolder, params string[] exceptFolder)
        {
            foreach (var dir in Directory.GetDirectories(sourceFolder))
            {
                if (!exceptFolder.Contains(dir))
                {
                    if (Directory.GetDirectories(dir).Length > 0)
                        DeleteEmptyDirectory(dir);
                    Directory.Delete(dir);
                }
            }
        }

        /// <summary>
        /// 获取所有子文件
        /// </summary>
        /// <param name="sourceFolder">目标文件夹</param>
        /// <param name="exceptFolder">除开集合文件夹与文件</param>
        /// <returns>所有子文件</returns>
        public static List<string> GetAllFiles(string sourceFolder, params string[] exceptFolder)
        {
            var files = Directory.GetFiles(sourceFolder);
            var dirs = Directory.GetDirectories(sourceFolder);
            originFilesPath.AddRange(files);
            allDirectories.AddRange(dirs);

            var newDirs = dirs.ToList();
            //排除不需要操作的目录与文件
            foreach (var df in exceptFolder)
            {
                if (Directory.Exists(df) && newDirs.Contains(df))
                    newDirs.Remove(df);
                else if (File.Exists(df))
                    originFilesPath.Remove(df);

            }

            foreach (var dir in newDirs)
            {
                GetAllFiles(dir);
            }


            return originFilesPath;
        }
    }
}
