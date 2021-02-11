using System;
using System.Collections;
using System.IO;

namespace CSharpConsoleDemo
{
    public class DirectoryCopy
    {

        public static bool Copy(string sourceDirName, string destDirName, out string error)
        {
            return Copy(sourceDirName, destDirName, out error, null, false);
        }

        /// <summary>
        /// 
        /// 替换文件
        /// 
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool Copy(string sourceDirName, string destDirName, out string error, ArrayList sqlScriptList, bool logger)
        {
            error = string.Empty;
            string filename = "";
            try
            {
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                    File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
                }

                if (destDirName[destDirName.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                {
                    destDirName = destDirName + System.IO.Path.DirectorySeparatorChar;
                }

                string[] files = Directory.GetFiles(sourceDirName);

                if (logger && files != null && files.Length > 0)
                {
                    Console.WriteLine("--> " + sourceDirName);
                }

                //替换文件
                foreach (string file in files)
                {
                    //排除更新使用的dll
                    FileInfo info = new FileInfo(file);
                    if (info.Name.ToLower().Equals(("ICSharpCode.SharpZipLib.dll").ToLower()) ||
                        info.Name.ToLower().Equals(("TaskScheduler.dll").ToLower()) ||
                        info.Name.ToLower().IndexOf(("Newtonsoft.Json.dll").ToLower()) >= 0 ||
                        info.Name.ToLower().Equals(("d3dcompiler_47.dll").ToLower()) ||
                        info.Name.ToLower().Equals(("New_SoftwareUpdater.exe").ToLower()) ||
                        //info.Name.ToLower().Equals(("SoftwareAutoUpdater.exe").ToLower())||
                        info.Name.Contains((".vshost")) || info.Name.Contains((".pdb"))
                        )
                    {
                        continue;
                    }
                    //替换文件
                    filename = file;
                    File.Copy(file, destDirName + System.IO.Path.GetFileName(file), true);
                    File.SetAttributes(destDirName + System.IO.Path.GetFileName(file), FileAttributes.Normal);
                    if (logger)
                    {
                        Console.WriteLine("    -> " + System.IO.Path.GetFileName(file));
                    }
                    if (file.EndsWith("sql") && sqlScriptList != null)
                    {
                        sqlScriptList.Add(destDirName + System.IO.Path.GetFileName(file));
                    }
                }

                //替换文件夹
                string[] dirs = Directory.GetDirectories(sourceDirName);
                foreach (string dir in dirs)
                {
                    //排除不备份的文件夹
                    DirectoryInfo info = new DirectoryInfo(dir);
                    if (info.Name.Equals("update") ||
                        info.Name.Equals("log") ||
                        info.Name.Equals("previousVersion") ||
                        info.Name.Equals("database") ||
                        info.Name.Equals("jre1.8") ||
                        string.IsNullOrEmpty(dir))
                    {
                        continue;
                    }

                    Copy(dir, destDirName + System.IO.Path.GetFileName(dir), out error, sqlScriptList, logger);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("替换文件失败: " + filename);
                error = ex.Message + "文件:" + filename;
                return false;
            }
        }
    }
}
