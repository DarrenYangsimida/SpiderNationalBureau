using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SpiderNationalBureau.BLL
{
    public static class CommonBLL
    {
        #region 静态方法

        /// <summary>
        /// 通过 URL 获取 html 内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtmlByUrl(string url)
        {
            ServicePointManager.Expect100Continue = false;
            try
            {
                //发起 Http 请求
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
                WebProxy proxyObject = new WebProxy("10.173.0.26", 80)
                {
                    Credentials = new NetworkCredential("gdcdevcontractor6-c", "Xl18580749091")
                };
                Request.Proxy = proxyObject;
                Request.Method = "GET";
                Request.Accept = "text/plain, */*; q=0.01";
                Request.ContentType = "application/x-www-form-urlencoded; charset=gb2312";
                Request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)";
                Request.KeepAlive = true;
                Request.Headers.Set(HttpRequestHeader.CacheControl, "no-cache");
                HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
                //读取响应内容
                Stream responeStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
                string wkStrReader = sr.ReadToEnd();
                sr.Close();
                response.Close();
                return wkStrReader;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return "";
            }
        }

        /// <summary>
        /// 正则表达式筛选字符串中的中文字符
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetChineseWord(string text)
        {
            try
            {
                string pattern = @"[\u4E00-\u9FFF]+";
                MatchCollection Matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
                StringBuilder builder = new StringBuilder();
                foreach (Match NextMatch in Matches)
                {
                    builder.Append(NextMatch.Value);
                }
                return builder.ToString();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return "";
            }
        }

        /// <summary>
        /// 正则表达式筛选字符串中的数字
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetCodeNum(string text)
        {
            try
            {
                string pattern = @"[\d]+";
                MatchCollection Matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
                StringBuilder builder = new StringBuilder();
                foreach (Match NextMatch in Matches)
                {
                    builder.Append(NextMatch.Value);
                }
                return builder.ToString();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return "";
            }
        }

        /// <summary>
        /// 判断字符串是否为数字字符串
        /// </summary>
        /// <param name="numberString"></param>
        /// <returns></returns>
        public static bool IsNumber(string numberString)
        {
            try
            {
                _ = double.Parse(numberString);
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// List<T>转DataTbale
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                tb.Rows.Add(values);
            }
            return tb;
        }

        /// <summary>
        /// 获取基础类型参数
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        /// <summary>
        /// 是否可Null的类型定义
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        #endregion

        #region 扩展

        /// <summary>
        /// 数字字符串向右补齐到指定位数
        /// </summary>
        /// <param name="org"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string AddedDigitsRight(this string org, int length)
        {
            try
            {
                return org.PadRight(length, '0');
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return org;
            }
        }

        /// <summary>
        /// 用原 Url 前缀拼接新的 Url 后缀
        /// </summary>
        /// <param name="org"></param>
        /// <param name="newUrlSuffix"></param>
        /// <returns></returns>
        public static string GetUrlPrefix(this string org, string newUrlSuffix)
        {
            try
            {
                return $"{org.Substring(0, org.LastIndexOf("/") + 1)}{newUrlSuffix}";
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return org;
            }
        }

        #endregion

    }
}
