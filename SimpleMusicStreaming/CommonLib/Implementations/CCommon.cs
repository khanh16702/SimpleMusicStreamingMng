
using CommonLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Implementations
{
    public class CCommon : CErrorHandler
    {
        public CCommon(ISerilogProvider serilogProvider)
            : base(serilogProvider)
        {}
        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public string GetDeepCaller()
        {
            string strCallerName = "";
            for (int i = 3; i >= 2; i--)
                strCallerName += GetCaller(i) + "=>";

            //returns a composite of the namespace, class and method name.
            return strCallerName;
        }

        /// <summary>
        /// get caller function name
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public string GetCaller(int level = 2)
        {
            var m = new StackTrace().GetFrame(level).GetMethod();

            if (m.DeclaringType == null) return ""; //9:33 AM 6/18/2014 Exception Details: System.NullReferenceException: Object reference not set to an instance of an object.

            // .Name is the name only, .FullName includes the namespace
            var className = m.DeclaringType.FullName;

            //the method/function name you are looking for.
            var methodName = m.Name;

            //returns a composite of the namespace, class and method name.
            return className + "->" + methodName;
        }

        public string FormatDate(string date)
        {
            if (date == null || date == "")
                return null;
            else
            {
                string[] s = new string[3];
                for (int i = 0; i < 3; i++)
                    s[i] = "";
                string str = "";
                int n = 0;
                for (int i = 0; i < date.Length; i++)
                {
                    if (date[i] != '/')
                        s[n] += date[i];
                    else
                        n++;
                }
                str = s[2] + '/' + s[1] + '/' + s[0];
                return str;
            }
        }

        public DateTime ConvertStringFromNewsToDateTime(string strDateTime)
        {
            DateTimeFormatInfo usDtfi = new CultureInfo("vi-VN", false).DateTimeFormat;
            return Convert.ToDateTime(strDateTime, usDtfi);
        }
    }
}
