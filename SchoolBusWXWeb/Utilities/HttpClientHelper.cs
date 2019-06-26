using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Utilities
{
    public static class HttpClientHelper
    {
        private static readonly HttpClient HttpClient = new HttpClient(
            new SocketsHttpHandler
            {
                UseProxy = false, // 不加这个会慢
                MaxConnectionsPerServer = 100,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            }
        )
        {
            Timeout = new TimeSpan(0, 10, 0)
        };

        /// <summary>
        ///     Http Get 异步步方法 在循环下会没有用HttpWebRequest稳定
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, Encoding encoding)
        {
            var data = await HttpClient.GetByteArrayAsync(url);
            var ret = encoding.GetString(data);
            return ret;
        }

        /// <summary>
        ///     Http Get 同步方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Encoding encoding)
        {
            var t = HttpClient.GetByteArrayAsync(url);
            t.Wait();
            var ret = encoding.GetString(t.Result);
            return ret;
        }

        /// <summary>
        ///     Http获取流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<Stream> HttpGetStreamAsync(string url)
        {
            try
            {
                return await HttpClient.GetStreamAsync(url);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     POST 异步
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formData"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static async Task<string> HttpPostAsync(string url, Dictionary<string, string> formData,
            Encoding encoding, int timeOut = 10000)
        {
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));
            //使用FormUrlEncodedContent做HttpContent
            var content = new FormUrlEncodedContent(formData);
            content.Headers.Add("UserAgent",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");
            content.Headers.Add("Timeout", timeOut.ToString());
            content.Headers.Add("KeepAlive", "true");
            var response = await HttpClient.PostAsync(url, content);
            var tmp = await response.Content.ReadAsByteArrayAsync();
            return encoding.GetString(tmp);
        }

        /// <summary>
        ///     POST 同步
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formData"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string HttpPost(string url, Encoding encoding, Dictionary<string, string> formData = null,
            int timeOut = 10000)
        {

            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));
            //使用FormUrlEncodedContent做HttpContent
            var content = new FormUrlEncodedContent(formData);
            content.Headers.Add("UserAgent",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");
            content.Headers.Add("Timeout", timeOut.ToString());
            content.Headers.Add("KeepAlive", "true");
            var response = HttpClient.PostAsync(url, content);
            response.Wait();
            var tmp = response.Result.Content.ReadAsByteArrayAsync();
            return encoding.GetString(tmp.Result);
        }

        /// <summary>
        ///     老版本POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> PostWebRequestNet3Async(string url, string data, Encoding encoding)
        {
            Stream receiveStream = null;
            WebResponse hwrs = null;
            try
            {
                //如果是发送HTTPS请求
                HttpWebRequest hwrq;
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                    hwrq = WebRequest.Create(url) as HttpWebRequest;
                    if (hwrq != null) hwrq.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    hwrq = WebRequest.Create(url) as HttpWebRequest;
                }

                if (hwrq != null)
                {
                    hwrq.ContentType = "application/x-www-form-urlencoded";
                    hwrq.Method = "POST";

                    var bytes = encoding.GetBytes(data);
                    var stream = await hwrq.GetRequestStreamAsync();
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();

                    hwrs = await hwrq.GetResponseAsync();
                }

                if (hwrs != null) receiveStream = hwrs.GetResponseStream();
                var sr = new StreamReader(receiveStream ?? throw new InvalidOperationException(), encoding);
                return sr.ReadToEnd();
            }
            finally
            {
                receiveStream?.Close();
                hwrs?.Close();
            }
        }

        /// <summary>
        ///     从url读取内容到内存MemoryStream流中
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static MemoryStream DownLoadFielToMemoryStream(string url)
        {
            if (!(WebRequest.Create(url) is HttpWebRequest wreq)) return null;
            if (!(wreq.GetResponse() is HttpWebResponse response)) return null;
            MemoryStream ms;
            using (var stream = response.GetResponseStream())
            {
                var buffer = new byte[response.ContentLength];
                int offset = 0, actuallyRead = 0;
                do
                {
                    if (stream != null) actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                    offset += actuallyRead;
                } while (actuallyRead > 0);
                ms = new MemoryStream(buffer);
            }
            response.Close();
            return ms;
        }
    }
}
