using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using static ConsoleHelper.ConsoleHelper;

namespace MainModel
{
    class MainModel
    {
        static void Main(string[] args)
        {
            //(ArrayList alOW = null, ArrayList alKW = null, int intDeepth = 5, int intThreadLimit = 30)
            
            ArrayList alWebadds = new ArrayList();
            ArrayList alKeyWords = new ArrayList();
            int intDeepth = 3;
            int intTDLimit = 100;
            alWebadds.Add("https://longint.org/");
            //alWebadds.Add("http://www.noi.cn/");
            alWebadds.Add("https://longint.org/");
            alWebadds.Add("https://longint.org/");
            alWebadds.Add("https://longint.org/");
            alWebadds.Add("https://longint.org/");
            alKeyWords.Add("奶牛");
            alKeyWords.Add("赛");
            Thread[] tdMain = new Thread[alWebadds.Count];
            int intTDindex = 0;
            Boolean boolDTfirst = true;
            foreach (string strWebAdd in alWebadds)
            {
                if(!boolDTfirst)
                {
                    intTDindex++;
                }
                else
                {
                    intTDindex = 0;
                    boolDTfirst = false;
                }
                ArrayList alWA = new ArrayList();
                alWA.Add(strWebAdd);
                using (var mProcess = new Process())
                {
                    //process1.StartInfo.FileName = @".\MonitoredControlSystem.exe " + strWebAdd + " 3 100";
                    //process1.Start();
                    wl("Create New Process:" + strWebAdd);
                    mProcess.StartInfo.FileName = @".\MonitoredControlSystem.exe ";//需要启动的程序名       
                    mProcess.StartInfo.Arguments = strWebAdd + " " + intDeepth + " " + intTDLimit;//启动参数       
                    mProcess.Start();//启动       
                }
                //tdMain[intTDindex] = new Thread(() => MonitoredControlSystem.MonitoredControlSystem.Init(alWA, alKeyWords, 3, 100));
                //tdMain[intTDindex].Start();
            }
            Console.ReadKey();
        }
    }
}
