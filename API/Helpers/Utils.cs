using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;

namespace API.Helpers
{
    public static class Utils
    {
        public static IQueryable<T> OrderByFromString<T>(this IQueryable<T> q, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }

        public static string sendFcm(string title, string body, string token, string gps = "")
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, "key=AAAANhfyuOc:APA91bGUFn70arLPRPBviHu_b8Vu6JTH8rCFEKP1bcGGKHbFZ0K6S26QvlTxlzLbb3qARKkRgyGhjTfo9b0ikTVibNO-WBPGNscdkvwwpwDHcuY9qw2pSP-EH27a6OaMQt5nJnEU9iMC");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string json = "{\"to\":\"" + token + "\",\"data\":{},\"notification\": {\"title\":\"" + title + "\",\"body\":\"" + body + "\"}}";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string result = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        public static string CP_SendFcm(string title, string body, string serverKey)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, "key=" + serverKey);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string json = "{\"to\":\"/topics/General\",\"data\":{},\"notification\": {\"title\":\"" + title + "\",\"body\":\"" + body + "\"}}";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string result = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        public static bool ifRoleExists(string UserRole)
        {
            switch (UserRole)
            {
                case "Admin": break;
                case "WebAdmin": break;
                case "Company": break;
                case "Customer": break;
                default:
                    return false;
            }
            return true;
        }

        public static string GenerateSlug(string text)
        {
            string str = RemoveAccent(text).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private static string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }


    }
}
