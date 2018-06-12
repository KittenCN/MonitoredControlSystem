using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleHelper
{
    public class ConsoleHelper
    {
        public static ConsoleColor ccDefColor = ConsoleColor.Green;
        public static ConsoleColor ccDefBackColer = ConsoleColor.Black;
        public static string GenLinkString = "Server=localhost;user id=root;password=;Database=chenkuserdb37;Port=3308;charset=utf8;";
        public static Boolean boolLog = true;
        public static void wl(string strValues)
        {
            Console.ForegroundColor = ccDefColor;
            Console.BackgroundColor = ccDefBackColer;
            Console.WriteLine(strValues);
            Log(strValues);
        }
        public static void wl(string strValues, Boolean boolLogFlag)
        {
            Console.ForegroundColor = ccDefColor;
            Console.BackgroundColor = ccDefBackColer;
            Console.WriteLine(strValues);
            if (boolLogFlag == true)
            {
                Log(strValues);
            }
        }
        public static void wl(string strValues, Boolean boolLogFlag, ConsoleColor cc, ConsoleColor bcc)
        {
            Console.ForegroundColor = cc;
            Console.BackgroundColor = bcc;
            Console.WriteLine(strValues);
            Console.ForegroundColor = ccDefColor;
            Console.BackgroundColor = ccDefBackColer;
            if (boolLogFlag == true)
            {
                Log(strValues);
            }
        }
        public static void wl_Thread(string strValues, Boolean boolLogFlag, ConsoleColor cc, ConsoleColor bcc)
        {
            Console.ForegroundColor = cc;
            Console.BackgroundColor = bcc;
            Console.WriteLine(strValues);
            if (boolLogFlag == true)
            {
                Log(strValues);
            }
        }
        public static void wl(string strValues, ConsoleColor cc)
        {
            Console.ForegroundColor = cc;
            Console.WriteLine(strValues);
            Console.ForegroundColor = ccDefColor;
            Log(strValues);
        }
        public static void wl(string strValues, ConsoleColor cc, ConsoleColor bcc)
        {
            Console.ForegroundColor = cc;
            Console.BackgroundColor = bcc;
            Console.WriteLine(strValues);
            Console.ForegroundColor = ccDefColor;
            Console.BackgroundColor = ccDefBackColer;
            Log(strValues);
        }
        public static void wrr(string strValues, Boolean boolFlag)
        {
            Console.ForegroundColor = ccDefColor;
            Console.BackgroundColor = ccDefBackColer;
            boolLog = boolFlag;
            Console.Write("\r" + strValues);
            Log(strValues);
            boolLog = true;
        }
        public static void wrr(string strValues, ConsoleColor cc, ConsoleColor bcc, Boolean boolFlag)
        {
            boolLog = boolFlag;
            Console.ForegroundColor = cc;
            Console.BackgroundColor = bcc;
            Console.Write("\r" + strValues);
            Console.ForegroundColor = ccDefColor;
            Console.BackgroundColor = ccDefBackColer;
            Log(strValues);
            boolLog = true;
        }

        public static void cInitiaze()
        {
            Console.ForegroundColor = ccDefColor;
            Console.BackgroundColor = ccDefBackColer;
            Console.WindowWidth = 120;
            Console.WindowHeight = 33;
            Console.Title = "MCS-Monitored Control System";
        }
        public static void Log(string LogBody)
        {
            try
            {
                if (boolLog == true && LogBody != null && LogBody != "")
                {
                    LogBody = LogBody.Replace("'", "#");
                    LogBody = LogBody.Replace("\"", "#");
                    string sql = "insert into Log(Log,LogDateTime) ";
                    sql = sql + " values('" + LogBody + "',#" + DateTime.Now.ToString() + "#) ";
                    int intInSql = MySqlHelper.MySqlHelper.ExecuteSql(sql, GenLinkString);
                }
            }
            catch
            {
                wl("Error!Record Log Fail!!", false, ConsoleColor.Red, ConsoleColor.Black);
                Console.Write("\r");
            }
        }
    }
}

