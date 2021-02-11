using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace CSharpConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
        

            
        }

        /// <summary>
        /// 模拟POS更新文件复制
        /// </summary>
        private static void copyFiles() {

            string srcPath = "D:\\pack_new\\setup\\part_update_dir";
            string dstPath = "D:\\tmp";
            string outError = null;
            ArrayList sqlScriptList = new ArrayList();
            DirectoryCopy.Copy(srcPath, dstPath, out outError, sqlScriptList, true);
            if (!string.IsNullOrEmpty(outError))
            {
                Console.WriteLine("error = " + outError);
            }
            else
            {
                if (sqlScriptList.Count > 0)
                {
                    Console.WriteLine("success sqlScript = " + (string)sqlScriptList[0]);
                }
            }
            Console.WriteLine("--------------------");
            return;
        }


        //文件是否存在
        protected static void fileExists()
        {
            Console.WriteLine("Hello World!");
            string Fi = @"D:\idea\hydee\pos-client\Bin\log\POS_2020-07-302.Log";
            //这里可能是影藏文件
            if (System.IO.File.Exists(@"D:\idea\hydee\pos-client\Bin\log\POS_2020-07-30.Log"))
            {
                Console.WriteLine("存在!");
            }
            else
            {
                Console.WriteLine("不存在!");
            }
        }

        //检查端口是否
        protected static void checkPortNum()
        {
            if (checkPortNum(1088))
            {
                Console.WriteLine("1088端口已占用!");
            }
            else
            {
                Console.WriteLine("1088端口未占用!");
            }
        }

        protected static bool checkPortNum(int portNum)
        {
            /// 将IP和端口替换成为你要检测的
            string ipAddress = "127.0.0.1";
            IPAddress ip = IPAddress.Parse(ipAddress);
            try
            {
                IPEndPoint point = new IPEndPoint(ip, portNum);
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    sock.Connect(point);
                    sock.Close();
                    return true;
                }
            }
            catch (SocketException e)
            {
            }
            return false;
        }
    }
}
