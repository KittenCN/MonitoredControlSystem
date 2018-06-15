using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace MainModel
{
    class MainModel
    {
        static void Main(string[] args)
        {
            //(ArrayList alOW = null, ArrayList alKW = null, int intDeepth = 5, int intThreadLimit = 30)
            ArrayList alWebadds = new ArrayList();
            ArrayList alKeyWords = new ArrayList();
            alWebadds.Add("https://longint.org/");
            alWebadds.Add("http://www.noi.cn/");
            alKeyWords.Add("奶牛");
            alKeyWords.Add("赛");
            MonitoredControlSystem.MonitoredControlSystem.Init(alWebadds, alKeyWords, 1, 30);
        }
    }
}
