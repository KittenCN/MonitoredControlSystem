using System.IO;
using System.Net;
using System.Text;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Threading;
using System.Web;
using System.Threading;
using static ConsoleHelper.ConsoleHelper;

namespace CollectionHelper
{
    public class CollectionHelper
    {
        public static void CHinit()
        {
            
        }
        #region 获取网页内容
        /// <summary>
        /// 获取网页内容
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="code">网页编码例如GB2312</param>
        /// <returns>网页源码</returns>
        public string gethtml(string url, string code = "utf-8", int intErrNum = 0)
        {
            string strResult;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //声明一个HttpWebRequest请求   
                request.Timeout = 30000;
                //设置连接超时时间   
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding(code);
                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                strResult = streamReader.ReadToEnd();
                return strResult;
            }
            catch (Exception ex)
            {
                //throw ex;
                //alCheckAdds.Add(url);
                //wl(url + "::" + ex.Message.ToString(), false, ConsoleColor.Red, ConsoleColor.Black);
                if(intErrNum <= 3)
                {
                    return "-2:" + intErrNum.ToString();
                }
                else
                {
                    return "-1";
                }                
            }
        }
        //url是要访问的网站地址，charSet是目标网页的编码，如果传入的是null或者""，那就自动分析网页的编码
        private string getHtmlPlus(string url, string charSet = "utf-8")
        {
            //创建WebClient实例myWebClient
            WebClient myWebClient = new WebClient();
            // 需要注意的：
            //有的网页可能下不下来，有种种原因比如需要cookie,编码问题等等
            //这是就要具体问题具体分析比如在头部加入cookie
            // webclient.Headers.Add("Cookie", cookie);
            //这样可能需要一些重载方法。根据需要写就可以了

            //获取或设置用于对向 Internet 资源的请求进行身份验证的网络凭据。
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            //如果服务器要验证用户名,密码
            //NetworkCredential mycred = new NetworkCredential(struser, strpassword);
            //myWebClient.Credentials = mycred;
            //从资源下载数据并返回字节数组。（加@是因为网址中间有"/"符号）
            byte[] myDataBuffer = myWebClient.DownloadData(url);
            string strWebData = Encoding.Default.GetString(myDataBuffer);

            //获取网页字符编码描述信息
            Match charSetMatch = Regex.Match(strWebData, "<meta([^<]*)charset=([^<]*)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string webCharSet = charSetMatch.Groups[2].Value;
            if (charSet == null || charSet == "")
                charSet = webCharSet;

            if (charSet != null && charSet != "" && Encoding.GetEncoding(charSet) != Encoding.Default)
                strWebData = Encoding.GetEncoding(charSet).GetString(myDataBuffer);
            return strWebData;
        }
        #endregion
        #region 替换换行符
        /// <summary>
        /// 替换掉网页源码里面的换行符，方便匹配
        /// </summary>
        /// <param name="HtmlCode">html代码</param>
        /// <returns>去除换行符后的字符串</returns>
        public string ReplaceEnter(string HtmlCode)
        {
            string s = "";
            if (HtmlCode == null || HtmlCode == "")
                s = "";
            else
                s = HtmlCode.Replace("\"", "");
            s = s.Replace("\r\n", "");
            return s;
        }
        #endregion
        #region 执行正则提取出值
        /// <summary>
        /// 执行正则提取出值
        /// </summary>
        /// <param name="RegexString">正则表达式</param>
        /// <param name="RemoteStr">HtmlCode源代码</param>
        /// <returns></returns>
        public MatchCollection GetRegValue(string RegexString, string RemoteStr)
        {
            Regex r = new Regex(RegexString, RegexOptions.Multiline);
            MatchCollection matches = r.Matches(RemoteStr);
            return matches;

        }
        #endregion
        #region 获取目标字符串
        /// <summary>
        /// 获取目标字符串
        /// </summary>
        /// <param name="fstr">目标字符串前面的字串</param>
        /// <param name="estr">目标字符串后面的字串</param>
        /// <param name="scstr">源字符串</param>
        /// <returns>匹配到的字符串数组</returns>
        public List<string> getstr(string fstr, string estr, string scstr)
        {
            //StringBuilder stb = new StringBuilder();
            string regstr = fstr + @".*?" + estr;
            List<string> rlist = new List<string>();
            MatchCollection match = GetRegValue(regstr, scstr);

            for (int i = 0; i < match.Count; i++)
            {
                string tpstr = match[i].ToString();
                tpstr = tpstr.Replace(fstr, "");
                tpstr = tpstr.Replace(estr, "");
                rlist.Add(tpstr);
            }
            return rlist;
        }
        #endregion
        #region 获取链接信息
        /// <summary>  
        /// 从HTML代码中分析出链接信息  
        /// </summary>  
        /// <returns>List<Link></returns>  
        public ArrayList GetHyperLinks(string htmlCode)
        {
            ArrayList al = new ArrayList();
            string strRegex = @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            Regex r = new Regex(strRegex, RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(htmlCode);
            for (int i = 0; i <= m.Count - 1; i++)
            {
                bool rep = false;
                string strNew = m[i].ToString();
                // 过滤重复的URL 
                foreach (string str in al)
                {
                    if (strNew == str)
                    {
                        rep = true;
                        break;
                    }
                }
                if (!rep) al.Add(strNew);
            }        
            al.Sort();
            return al;
        }
        #endregion
    }
}


