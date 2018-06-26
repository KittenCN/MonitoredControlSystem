using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using static ConsoleHelper.ConsoleHelper;
using static CollectionHelper.CollectionHelper;
using static MySqlHelper.MySqlHelper;
using System.Timers;

namespace MonitoredControlSystem
{
    public class MonitoredControlSystem
    {
        public static ArrayList alCheckAdds;
        public static ArrayList alOriWebAdds;
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
        public static Thread[] tdChild;
        public static Thread[] tdPW;
        public static int intAllValidNode = 0;
        public static int intThreadNum = 0;
        public static int intAllThreadNum = 0;
        public static System.Timers.Timer aTimer;
        public static int intTDMax = 0;
        static void Main(string[] args)
        {
            alOriWebAdds = new ArrayList();
            alOriHtml = new ArrayList();
            alKeyWords = new ArrayList();
            //---------------------------------
            alKeyWords.Add("奶牛");
            alKeyWords.Add("赛");
            //---------------------------------
            alOriWebAdds.Add(args[0].ToString());
            try
            {
                Init(alOriWebAdds, alKeyWords, int.Parse(args[1]), int.Parse(args[2]));
            }
            catch
            {
                wl("Test Mode");
                Init();
            }
        }
        public static void Init(ArrayList alOW = null, ArrayList alKW = null, int intDeepth = 3, int intThreadLimit = 5)
        {
            //CHinit();
            if (alOW == null || alOW.Count <= 0 ) { alOW = new ArrayList(); alOW.Add("https://longint.org/"); }
            if (alKW == null || alKW.Count <= 0) { alKW = new ArrayList(); alKW.Add("奶牛"); }
            //alOriHtml.Add(ch.gethtml(alOriWebAdd[0].ToString(), "utf-8"));
            if (alOW.Count > 0 && alKW.Count > 0)
            {
                alOriWebAdds = alOW;
                alKeyWords = alKW;
                aTimer = new System.Timers.Timer(30000);
                aTimer.Elapsed += new ElapsedEventHandler(ShowTD);
                aTimer.Enabled = true;
                foreach (string strWebadd in alOriWebAdds)
                {
                    //MainProcess(strWebadd.ToString(), 0, true, false, intDeepth, intThreadLimit);
                    while(tdBase != null)
                    {
                        bool booltdCheck = false;
                        while(tdBase.ThreadState != ThreadState.Stopped)
                        {
                            booltdCheck = true;
                        }
                        if(booltdCheck) { break; }
                    }
                    tdBase = new Thread(() =>  MainProcess(strWebadd.ToString(), 0, true, false, intDeepth, intThreadLimit,true));
                    tdBase.Start();
                    //tdBase.Join();
                    //Thread.Sleep(500);
                }
            }
            tdBase.Join();
            //while (tdBase.ThreadState != ThreadState.Stopped || tdChild.ThreadState != ThreadState.Stopped || tdPW.ThreadState != ThreadState.Stopped)
            while (tdBase.ThreadState != ThreadState.Stopped || !CheckTD(tdChild) || !CheckTD(tdPW))

            {
                //Thread.Sleep(500);
            }
            ShowProcess(alAllShowAdds);
            wl("all clear! Total Time is :" + (DateTime.Now - dtBegin).ToString() + " Total Node Num is :" + intAllAddress.ToString() + " Total Valid Node Num is " + intAllValidNode.ToString(), true);
            Console.ReadLine();
            //Thread.Sleep(5000);
        }
        private static Boolean CheckTD(Thread[] tdClass)
        {
            Boolean boolResult = true;
            if(tdClass.Count() > 0)
            {
                foreach(Thread tdIn in tdClass)
                {
                    if(tdIn.ThreadState != ThreadState.Stopped)
                    {
                        boolResult = false;
                        break;
                    }
                    else
                    {
                        boolResult = true;
                    }
                }
            }
            return boolResult;
        }
        private static void ProtectProcess()
        {
            while (tdBase.ThreadState != ThreadState.Running)
            {
                ShowProcess(alAllShowAdds);
                wl("all clear! Total Time is :" + (DateTime.Now - dtBegin).ToString() + " Total Node Num is :" + intAllAddress.ToString() + " Total Valid Node Num is " + intAllValidNode.ToString(), true);
                Console.ReadLine();
                //Thread.Sleep(5000);
                break;
            }
        }
        private static void MainProcess(string strOriAdd, int intIndex = 0, Boolean boolFirstRun = false, Boolean boolChildThreadFlag = false, int intDeepth = 3, int intThreadLimit = 5, Boolean boolTDin = false)
        {
            if(boolTDin)
            {
                intAllThreadNum++;
            }
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
            if(!boolFirstRun)
            {
                try
                {
                    if (intThreadNum < intThreadLimit)
                    {
                        List<Thread> tdPW_p = new List<Thread>();
                        if (tdPW != null)
                        {
                            tdPW_p = tdPW.ToList();
                        }                      
                        tdPW_p.Add(new Thread(() => ProcessWeb(strOriAdd, intDeepth, true)));
                        tdPW = tdPW_p.ToArray();
                        intThreadNum++;
                        wl("Create New Thread for PW:" + intThreadNum, false, ConsoleColor.Cyan, ConsoleColor.Black);
                        tdPW[tdPW.Count() - 1].Start();
                    }
                    else
                    {
                        ProcessWeb(strOriAdd, intDeepth);
                    }
                }
                catch(Exception ex)
                {
                    wrr(ex.Message.ToString());
                    //if(intThreadNum > 0)
                    //{
                    //    intThreadNum--;
                    //    wl("Destroy Old Thread for PW:" + intThreadNum, false, ConsoleColor.Cyan, ConsoleColor.Black);
                    //}
                    ProcessWeb(strOriAdd, intDeepth);
                }
            }
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
                                        wl_Thread("Add New Node:" + strNextWebAdd + "||Deepth is " + intIndex.ToString(), true, ConsoleColor.Yellow, ConsoleColor.Black);
                                        alAllShowAdds.Add(strNextWebAdd);
                                        //MainProcess(strNextWebAdd, intIndex, false);
                                        if (intThreadNum >= intThreadLimit)
                                        {
                                            //tdChild.Join();
                                            //, 0, true, false, intDeepth, intThreadLimit
                                            MainProcess(strNextWebAdd, intIndex, false, false, intDeepth, intThreadLimit);
                                        }
                                        if (intThreadNum < intThreadLimit && intIndex <= intDeepth)
                                        {
                                            try
                                            {
                                                //tdPW = new Thread[intTDMax];
                                                List<Thread> tdChild_p = new List<Thread>();
                                                if(tdChild != null)
                                                {
                                                    tdChild_p = tdChild.ToList();
                                                }
                                                tdChild_p.Add(new Thread(() => MainProcess(strNextWebAdd, intIndex, false, false, intDeepth, intThreadLimit, true)));
                                                tdChild = tdChild_p.ToArray();
                                                //tdChild[intTDMax - 1] = new Thread(() => MainProcess(strNextWebAdd, intIndex, false, false, intDeepth, intThreadLimit, true));
                                                intThreadNum++;
                                                wl("Create New Thread for Main:" + intThreadNum, false, ConsoleColor.Cyan, ConsoleColor.Black);
                                                tdChild[tdChild.Count() - 1].Start();
                                                //tdChild.Join();
                                                //Thread.Sleep(500);
                                            }
                                            catch(Exception ex)
                                            {
                                                wrr(ex.Message.ToString());
                                                //if (intThreadNum > 0)
                                                //{
                                                //    intThreadNum--;
                                                //    wl("Destroy Old Thread for Main:" + intThreadNum, false, ConsoleColor.Cyan, ConsoleColor.Black);
                                                //}
                                                //intThreadNum--;
                                                //wl("Destroy Old Thread for Main:" + intThreadNum, false, ConsoleColor.Cyan, ConsoleColor.Black);
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
            if (boolChildThreadFlag)
            {
                intThreadNum--;
                wl("Destroy Old Thread for Main:" + intThreadNum, false, ConsoleColor.Cyan, ConsoleColor.Black);
            }
            if(boolTDin)
            {
                intAllThreadNum--;
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
        private static void ProcessWeb(string strWebAdd, int intIndex, Boolean boolTDin = false)
        {
            if(boolTDin)
            {
                intAllThreadNum++;
            }
            CollectionHelper.CollectionHelper ch = new CollectionHelper.CollectionHelper();
            string strHtml = ch.gethtml(strWebAdd);
            if(alKeyWords != null && alKeyWords.Count > 0)
            {
                foreach(string strKW in alKeyWords)
                {
                    int intKeyNum = strHtml.IndexOf(strKW.ToString());
                    if (intKeyNum > 0)
                    {
                        alShowAdds.Add(strWebAdd);
                        alCheckAdds.Add(strWebAdd);
                        intAllValidNode++;
                        wl(strWebAdd + "||Deepth :" + intIndex.ToString() + " First Position : " + intKeyNum.ToString(), true);
                        ProcessDB(strWebAdd, strKW.ToString(), intIndex, intKeyNum, 0);
                    }
                    else
                    {
                        alCheckAdds.Add(strWebAdd);
                        //wl(strWebAdd + "||" + intKeyNum.ToString(), false, ConsoleColor.Yellow, ConsoleColor.Black);
                    }
                }
            }
            if(boolTDin)
            {
                intAllThreadNum--;
                intThreadNum--;
                wl("Destroy Old Thread for PW:" + intThreadNum, false, ConsoleColor.Cyan, ConsoleColor.Black);
            }
        }
        private static void ProcessDB( string strAddress, string strKeyWord, int intDeepth, int intFirstPosition, int intAddNo = 0)
        {
            string strDT = DateTime.Now.ToString();
            string strSQL = "insert into MonitorResults(AddNo,Address,KeyWord,Deepth,FirstPosition,Datetime) ";
            strSQL = strSQL + " values(" + intAddNo + ",'" + strAddress + "','" + strKeyWord + "'," + intDeepth + "," + intFirstPosition + ",'" + strDT + "'); ";
            int intInSql = MySqlHelper.MySqlHelper.ExecuteSql(strSQL, MySqlHelper.MySqlHelper.GenLinkString);
        }
        private static void ShowTD(object source, ElapsedEventArgs e)
        {
            wl("Current Thread Num :" + intAllThreadNum.ToString(), false, ConsoleColor.Magenta, ConsoleColor.Black);
        }
    }
}
