using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using static ConsoleHelper.ConsoleHelper;
using static CollectionHelper.CollectionHelper;

namespace MonitoredControlSystem
{
    class Program
    {
        public static ArrayList alCheckAdds;
        public static ArrayList alOriWebAdd;
        public static ArrayList alOriHtml;
        public static ArrayList alKeyWords;
        public static ArrayList alAllWebAdds;
        public static ArrayList alAllWebAddsDeepth;
        public static ArrayList alAllShowAdds;
        public static ArrayList alShowAdds;
        public static string[] strUnUseExName = { ".js", ".png", ".gif", ".jpeg", ".jpg", ".css", ".ico", ".pdf", ".zip", ".rar", ".iso", ".wma", ".wmv", ".mp3", ".flac", ".ape", ".mp4", ".mp3", ".apk", ".exe", ".bin", ".swf", ".avi", ".flv", ".doc", ".docx" };
        //public static int intDeepth = 3;
        public static Boolean boolCrossWeb = false;
        public static string strOriWebAdd;
        public static int intAllAddress = 0;
        public static DateTime dtBegin = DateTime.Now;
        public static Thread tdBase;
        public static Thread tdChild;
        public static Thread tdPW;
        public static int intAllValidNode = 0;
        public static int intThreadNum = 0;
        static void Main(string[] args)
        {
            //CHinit();
            alOriWebAdd = new ArrayList();
            alOriHtml = new ArrayList();
            alKeyWords = new ArrayList();
            alOriWebAdd.Add("https://longint.org/");
            alKeyWords.Add("奶牛");
            //alOriHtml.Add(ch.gethtml(alOriWebAdd[0].ToString(), "utf-8"));
            MainProcess(alOriWebAdd[0].ToString(), 0, true);
            tdBase = new Thread(() => MainProcess(alOriWebAdd[0].ToString(), 0, true));
            tdBase.Start();
            //Thread tdProtect = new Thread(() => ProtectProcess());
            //tdProtect.Start();
            while (tdBase.ThreadState != ThreadState.Stopped || tdChild.ThreadState != ThreadState.Stopped || tdPW.ThreadState != ThreadState.Stopped)
            {
                //Thread.Sleep(500);
            }
            ShowProcess(alAllShowAdds);
            wl("all clear! Total Time is :" + (DateTime.Now - dtBegin).ToString() + " Total Node Num is :" + intAllAddress.ToString() + " Total Valid Node Num is " + intAllValidNode.ToString(), false);
            Console.ReadLine();
        }
        private static void ProtectProcess()
        {
            while (tdBase.ThreadState != ThreadState.Running && tdChild.ThreadState != ThreadState.Running)
            {
                ShowProcess(alAllShowAdds);
                wl("all clear! Total Time is :" + (DateTime.Now - dtBegin).ToString() + " Total Node Num is :" + intAllAddress.ToString() + " Total Valid Node Num is " + intAllValidNode.ToString(), false);
                Console.ReadLine();
                break;
            }
        }
        private static void MainProcess(string strOriAdd, int intIndex = 0, Boolean boolFirstRun = false, Boolean boolChildThreadFlag = false, int intDeepth = 5, int intThreadLimit = 30)
        {
            if (boolFirstRun)
            {
                //wl("Init..", false, ConsoleColor.Yellow, ConsoleColor.Black);
                alAllWebAdds = new ArrayList();
                alShowAdds = new ArrayList();
                alCheckAdds = new ArrayList();
                alAllWebAddsDeepth = new ArrayList();
                alAllShowAdds = new ArrayList();
                alAllWebAdds.Add(strOriAdd);
                alAllWebAddsDeepth.Add(intIndex.ToString());
                //ProcessWeb(strOriAdd);
                boolFirstRun = false;
                strOriWebAdd = strOriAdd;
            }
            else
            {
                try
                {
                    if (alAllWebAdds.IndexOf(strOriAdd) >= 0)
                    {
                        intIndex = int.Parse(alAllWebAddsDeepth[alAllWebAdds.IndexOf(strOriAdd)].ToString());
                    }
                }
                catch
                { }
                intIndex++;
            }
            //wl("Process Node:" + strOriAdd + "||Deepth is " + intIndex.ToString(), false, ConsoleColor.Yellow, ConsoleColor.Black);
            CollectionHelper.CollectionHelper ch = new CollectionHelper.CollectionHelper();
            string strBaseWeb = ch.getstr("://", "/", strOriAdd)[0].ToString();
            int intErrNum = 0;
            REBACK:
            string strOri = ch.gethtml(strOriAdd, "utf-8", intErrNum);
            tdPW = new Thread(() => ProcessWeb(strOriAdd, intDeepth));
            tdPW.Start();
            if (strOri.Substring(0, 2) != "-1" && strOri.Substring(0, 2) != "-2")
            {
                ArrayList alNextWebAdds = new ArrayList();
                int intAddNum = ch.GetHyperLinks(strOri).Count;
                if (intAddNum > 0)
                {
                    //wl("Child Node Process 1", false);
                    foreach (string strWebadd in ch.GetHyperLinks(strOri))
                    {
                        string strSeed = strWebadd;
                        if (!boolCrossWeb)
                        {
                            if (strSeed.Substring(strSeed.Length - 1, 1) != "/")
                            {
                                strSeed = strWebadd + "/";
                            }
                            string strCroseeWebSeed = ch.getstr("://", "/", strSeed)[0].ToString();
                            if (strBaseWeb == strCroseeWebSeed)
                            {
                                if (alAllWebAdds.IndexOf(strSeed.ToString()) < 0)
                                {
                                    alNextWebAdds.Add(strSeed.ToString());
                                    alAllWebAdds.Add(strSeed.ToString());
                                    alAllWebAddsDeepth.Add(intIndex);
                                    intAllAddress++;
                                }
                            }
                        }
                        else
                        {
                            if (alAllWebAdds.IndexOf(strSeed.ToString()) < 0)
                            {
                                alNextWebAdds.Add(strSeed.ToString());
                                alAllWebAdds.Add(strSeed.ToString());
                                alAllWebAddsDeepth.Add(intIndex);
                                intAllAddress++;
                            }
                        }
                    }
                }
                //wl("Child Node Process 2", false);
                if (ch.getstr("\"/", "\"", strOri).Count > 0)
                {
                    foreach (string strWebadd in ch.getstr("\"/", "\"", strOri))
                    {
                        if (alAllWebAdds.IndexOf(strOriWebAdd + strWebadd.ToString()) < 0)
                        {
                            alNextWebAdds.Add(strOriWebAdd + strWebadd.ToString());
                            alAllWebAdds.Add(strOriWebAdd + strWebadd.ToString());
                            alAllWebAddsDeepth.Add(intIndex);
                            intAllAddress++;
                        }
                    }
                }
                //wl("Current Node have " + alNextWebAdds.Count.ToString() + " Child Nodes, Deepth is " + (intIndex + 1).ToString(), false);
                if (alNextWebAdds.Count > 0)
                {
                    foreach (string strNextWebAdd in alNextWebAdds)
                    {
                        char charCheck = strNextWebAdd.Substring(strNextWebAdd.Length - 1, 1).ToCharArray()[0];
                        if ((char.IsLetterOrDigit(charCheck) || charCheck == '/') && (charCheck != ':'))
                        {
                            try
                            {
                                string strExName = System.IO.Path.GetExtension(strNextWebAdd);
                                if (!CheckStringList(strUnUseExName, strExName))
                                {
                                    if (intIndex <= intDeepth)
                                    {
                                        wl_Thread("Add New Node:" + strNextWebAdd + "||Deepth is " + intIndex.ToString(), false, ConsoleColor.Yellow, ConsoleColor.Black);
                                        alAllShowAdds.Add(strNextWebAdd);
                                        //MainProcess(strNextWebAdd, intIndex, false);
                                        if (intThreadNum >= intThreadLimit)
                                        {
                                            //tdChild.Join();
                                            MainProcess(strNextWebAdd, intIndex, false);
                                        }
                                        if (intThreadNum < intThreadLimit && intIndex <= intDeepth)
                                        {
                                            try
                                            {
                                                tdChild = new Thread(() => MainProcess(strNextWebAdd, intIndex, false, true));                                                
                                                intThreadNum++;
                                                wl("Create New Thread:" + intThreadNum, false, ConsoleColor.Yellow, ConsoleColor.Black);
                                                tdChild.Start();
                                                //tdChild.Join();
                                                //Thread.Sleep(500);
                                            }
                                            catch
                                            {
                                                intThreadNum--;
                                                wl("Destroy Old Thread:" + intThreadNum, false, ConsoleColor.Yellow, ConsoleColor.Black);
                                            }
                                        }
                                    }
                                    else if (intIndex > intDeepth)
                                    {
                                        return;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //intThreadNum--;
                                //alCheckAdds.Add(strNextWebAdd);
                                //wl(strNextWebAdd + "::" + ex.Message.ToString(), false, ConsoleColor.Red, ConsoleColor.Black);                             
                            }
                        }
                    }
                }
            }
            else if (strOri.Substring(0, 2) == "-2")
            {
                //wl("Current Node has no data", false);
                intErrNum = int.Parse(strOri.Substring(strOri.Length - 1, 1));
                goto REBACK;
            }
            if(boolChildThreadFlag)
            {
                intThreadNum--;
                wl("Destroy Old Thread:" + intThreadNum, false, ConsoleColor.Yellow, ConsoleColor.Black);
            }
        }
        private static void ShowProcess(ArrayList alAdds)
        {
            if (alAdds != null && alAdds.Count > 0)
            {
                for (int i = 0; i < alAdds.Count; i++)
                {
                    string strSeed = alAdds[i].ToString();
                    Boolean boolSeedCheck = false;
                    if (alCheckAdds.IndexOf(strSeed) >= 0 && alShowAdds.IndexOf(strSeed) >= 0)
                    {
                        boolSeedCheck = true;
                        break;
                    }
                    if (!boolSeedCheck)
                    {
                        int intIndex = 0;
                        if (alAllWebAdds.IndexOf(strSeed) >= 0)
                        {
                            intIndex = int.Parse(alAllWebAddsDeepth[alAllWebAdds.IndexOf(strSeed)].ToString());
                        }
                        ProcessWeb(strSeed.ToString(), intIndex);
                    }
                }
            }
        }
        private static Boolean CheckStringList(string[] OriStrs, string Oristr)
        {
            Boolean boolResult = false;
            foreach (string str in OriStrs)
            {
                if (str.ToLower() == Oristr.ToLower())
                {
                    boolResult = true;
                    break;
                }
            }
            return boolResult;
        }
        private static void ProcessWeb(string strWebAdd, int intIndex)
        {
            CollectionHelper.CollectionHelper ch = new CollectionHelper.CollectionHelper();
            string strHtml = ch.gethtml(strWebAdd);
            int intKeyNum = strHtml.IndexOf(alKeyWords[0].ToString());
            if (intKeyNum > 0)
            {
                alShowAdds.Add(strWebAdd);
                alCheckAdds.Add(strWebAdd);
                intAllValidNode++;
                wl(strWebAdd + "||Deepth :" + intIndex.ToString() + " First Position : " + intKeyNum.ToString(), false);
            }
            else
            {
                alCheckAdds.Add(strWebAdd);
                //wl(strWebAdd + "||" + intKeyNum.ToString(), false, ConsoleColor.Yellow, ConsoleColor.Black);
            }
        }
    }
}
