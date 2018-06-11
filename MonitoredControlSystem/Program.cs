using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using static ConsoleHelper.ConsoleHelper;

namespace MonitoredControlSystem
{
    class Program
    {
        public static ArrayList alOriWebAdd;
        public static ArrayList alOriHtml;
        public static ArrayList alKeyWords;
        public static string[] strUnUseExName = { ".js", ".png", ".gif", ".jpeg", "jpg", ".css" };
        public static int intDeepth = 5;
        static void Main(string[] args)
        {
            alOriWebAdd = new ArrayList();
            alOriHtml = new ArrayList();
            alKeyWords = new ArrayList();
            alOriWebAdd.Add("http://club.tgfcer.com/");
            alKeyWords.Add("游戏");
            //alOriHtml.Add(ch.gethtml(alOriWebAdd[0].ToString(), "utf-8"));
            MainProcess(alOriWebAdd[0].ToString(), intDeepth, true);
            wl("all clear!", false);
            Console.ReadLine();
        }
        private static void MainProcess(string strOriAdd, int intCurrentDeepth = 0, Boolean boolFirstRun = false)
        {                       
            if(boolFirstRun)
            {
                ProcessWeb(strOriAdd);
            }
            CollectionHelper.CollectionHelper ch = new CollectionHelper.CollectionHelper();
            string strOri = ch.gethtml(strOriAdd);
            ArrayList alNextWebAdds = new ArrayList();
            int intAddNum = ch.GetHyperLinks(strOri).Count;
            if (intAddNum > 0)
            {
                foreach (string strWebadd in ch.GetHyperLinks(strOri))
                {
                    alNextWebAdds.Add(strWebadd.ToString());
                }
            }
            if (ch.getstr("\"/", "\"", strOri).Count > 0)
            {
                foreach (string strWebadd in ch.getstr("\"/", "\"", strOri))
                {
                    alNextWebAdds.Add(strOriAdd + strWebadd.ToString());
                }
            }
            if (alNextWebAdds.Count > 0)
            {
                string[] strNextWebAdds = new string[alNextWebAdds.Count];
                int intCurrentStrNWAIndex = 0;
                foreach (string strNextWebAdd in alNextWebAdds)
                {
                    char charCheck = strNextWebAdd.Substring(strNextWebAdd.Length - 1, 1).ToCharArray()[0];
                    if (char.IsLetterOrDigit(charCheck) || charCheck == '/')
                    {
                        string strExName = System.IO.Path.GetExtension(strNextWebAdd);
                        if (!CheckStringList(strUnUseExName, strExName))
                        {
                            Boolean boolWebCheck = false;
                            if (strNextWebAdds[0] != null)
                            {
                                foreach (string strNextWebAddCheck in strNextWebAdds)
                                {
                                    if (strNextWebAdd == strNextWebAddCheck)
                                    {
                                        boolWebCheck = true;
                                        break;
                                    }
                                }
                            }
                            if (!boolWebCheck)
                            {
                                if (intCurrentStrNWAIndex < strNextWebAdds.Count())
                                {
                                    strNextWebAdds[intCurrentStrNWAIndex] = strNextWebAdd;
                                    ProcessWeb(strNextWebAdd);
                                    intCurrentStrNWAIndex++;
                                    intCurrentDeepth++;
                                    if(intCurrentDeepth <= 5)
                                    {
                                        MainProcess(strNextWebAdd, intCurrentDeepth);
                                    }
                                    else
                                    {
                                        intCurrentDeepth = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private static Boolean CheckStringList(string[] OriStrs, string Oristr)
        {
            Boolean boolResult = false;
            foreach (string str in OriStrs)
            {
                if (str == Oristr)
                {
                    boolResult = true;
                    break;
                }
            }
            return boolResult;
        }
        private static void ProcessWeb(string strWebAdd)
        {
            CollectionHelper.CollectionHelper ch = new CollectionHelper.CollectionHelper();
            string strHtml = ch.gethtml(strWebAdd);
            int intKeyNum = strHtml.IndexOf(alKeyWords[0].ToString());
            if (intKeyNum > 0 || 1==1)
            {
                wl(strWebAdd + "||" + intKeyNum.ToString(), false);
            }
        }
    }
}
