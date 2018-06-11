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
        static void Main(string[] args)
        {
            ArrayList alNextWebAdds = new ArrayList();
            CollectionHelper.CollectionHelper ch = new CollectionHelper.CollectionHelper();
            alOriWebAdd = new ArrayList();
            alOriHtml = new ArrayList();
            alKeyWords = new ArrayList();
            alOriWebAdd.Add("https://longint.org");
            alKeyWords.Add("赛");
            alOriHtml.Add(ch.gethtml(alOriWebAdd[0].ToString(), "utf-8"));
            int intAddNum = ch.GetHyperLinks(alOriHtml[0].ToString()).Count;
            if (intAddNum > 0)
            {
                foreach (string strWebadd in ch.GetHyperLinks(alOriHtml[0].ToString()))
                {
                    alNextWebAdds.Add(strWebadd.ToString());
                }
            }
            if (ch.getstr("\"/", "\"", alOriHtml[0].ToString()).Count > 0)
            {
                foreach (string strWebadd in ch.getstr("\"/", "\"", alOriHtml[0].ToString()))
                {
                    alNextWebAdds.Add("https://longint.org/" + strWebadd.ToString());
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
                                    string strHtml = ch.gethtml(strNextWebAdd);
                                    int intKeyNum = strHtml.IndexOf(alKeyWords[0].ToString());
                                    if (intKeyNum > 0)
                                    {
                                        wl(strNextWebAdd + "||" + intKeyNum.ToString(), false);
                                    }
                                    intCurrentStrNWAIndex++;
                                }
                            }
                        }
                    }
                }
            }
            Console.ReadLine();
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
    }
}
