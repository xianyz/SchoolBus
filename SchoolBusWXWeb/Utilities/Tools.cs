using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using Newtonsoft.Json;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using SchoolBusWXWeb.Business;
using SchoolBusWXWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Utilities
{
    public static class Tools
    {
        private static IMqttClient _mqttClient;
        private static bool _isReconnect = true;
        private static ILogger _toollogger;
        private static SiteConfig _settings;
        private static readonly string[] ImageExtensions = { ".jpg", ".png", ".gif", ".jpeg", ".bmp" };

        public static IApplicationBuilder SetUtilsProviderConfiguration(this IApplicationBuilder serviceProvider, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _settings = configuration.GetSection("SiteConfig").Get<SiteConfig>();
            _toollogger = loggerFactory.CreateLogger("Tools");
            return serviceProvider;
        }

        /// <summary>
        ///  上传图片到七牛
        /// </summary>
        /// <param name="imgInfo"></param>
        /// <returns></returns>
        public static async Task<bool> UploadStream(ImgInfo imgInfo)
        {
            var res = false;
            try
            {
                if (imgInfo != null && !string.IsNullOrEmpty(imgInfo.FileName) && imgInfo.ImageId > 0)
                {
                    var fileName = imgInfo.FileName;
                    var extensionName = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                    if (ImageExtensions.Contains(extensionName.ToLower()))
                    {
                        // 上传策略，参见
                        // https://developer.qiniu.com/kodo/manual/put-policy
                        var mac = new Mac(_settings.QiniuOption.AccessKey, _settings.QiniuOption.SecretKey);
                        // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY" putPolicy.Scope = bucket + ":" + saveKey;
                        var putPolicy = new PutPolicy
                        {
                            Scope = _settings.QiniuOption.Bucketfirst + ":" + imgInfo.SaveKey
                        };
                        putPolicy.SetExpires(3600); // 上传策略有效期(对应于生成的凭证的有效期)

                        // putPolicy.DeleteAfterDays = 1;  // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除

                        // 生成上传凭证，参见
                        // https://developer.qiniu.com/kodo/manual/upload-token
                        var jstr = putPolicy.ToJsonString();
                        var token = Auth.CreateUploadToken(mac, jstr);
                        var fu = new FormUploader();
                        var result = await fu.UploadStreamAsync(imgInfo.FileStream, imgInfo.SaveKey, token);
                        if (result.Code == 200) res = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _toollogger.LogError("上传图片到云存储异常:异常信息:" + ex);
            }

            return res;
        }

        /// <summary>
        /// 获取七牛Token
        /// </summary>
        /// <returns></returns>
        public static Token GetUploadToken()
        {
            var mac = new Mac(_settings.QiniuOption.AccessKey, _settings.QiniuOption.SecretKey);
            var auth = new Auth(mac);
            var putPolicy = new PutPolicy
            {
                Scope = _settings.QiniuOption.Bucketfirst
            };
            // 上传策略有效期(对应于生成的凭证的有效期)
            putPolicy.SetExpires(3600);
            // putPolicy.DeleteAfterDays = 1; // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除 
            return new Token
            {
                uptoken = auth.CreateUploadToken(putPolicy.ToJsonString())
            };
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        public static byte[] CreateCheckCodeImage()
        {
            var bb = default(byte[]);
            try
            {
                var checkCode = VerficationCodeSrc(5);

                if (!string.IsNullOrEmpty(checkCode.Trim()))
                {
                    var image = new Bitmap(90, 28);
                    var g = Graphics.FromImage(image);
                    try
                    {
                        //生成随机生成器
                        var random = new Random();
                        //清空图片背景色
                        g.Clear(Color.White);
                        //画图片的背景噪音线
                        for (var i = 0; i < 2; i++)
                        {
                            var x1 = random.Next(image.Width);
                            var x2 = random.Next(image.Width);
                            var y1 = random.Next(image.Height);
                            var y2 = random.Next(image.Height);
                            g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
                        }

                        var font = new Font("Arial", 16, FontStyle.Bold);

                        var brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue,
                            Color.DarkRed, 1.2f, true);

                        g.DrawString(checkCode, font, brush, 2, 2);
                        //画图片的前景噪音点
                        for (var i = 0; i < 100; i++)
                        {
                            var x = random.Next(image.Width);
                            var y = random.Next(image.Height);
                            image.SetPixel(x, y, Color.FromArgb(random.Next()));
                        }

                        //画图片的边框线
                        g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                        using (var stream = new MemoryStream())
                        {
                            image.Save(stream, ImageFormat.Gif);
                            bb = stream.GetBuffer();
                        }
                    }
                    finally
                    {
                        g.Dispose();
                        image.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                _toollogger.LogError("使用System.Drawing.Common生成验证码失败: 异常信息:" + ex);
            }

            return bb;
        }

        /// <summary>
        /// 生成验证码字符串
        /// </summary>
        /// <param name="numberOfChars"></param>
        /// <returns></returns>
        private static string VerficationCodeSrc(int numberOfChars)
        {
            if (numberOfChars > 36)
                throw new InvalidOperationException("Random Word Charecters can not be greater than 36.");
            var columns = new char[36];
            //字母
            for (var charPos = 97; charPos < 97 + 26; charPos++)
                columns[charPos - 97] = (char)charPos;
            //数字
            for (var intPos = 48; intPos <= 57; intPos++)
                columns[intPos - 22] = (char)intPos;

            var randomBuilder = new StringBuilder();
            var randomSeed = new Random();
            for (var incr = 0; incr < numberOfChars; incr++)
                randomBuilder.Append(columns[randomSeed.Next(36)].ToString());

            return randomBuilder.ToString();
        }

        /// <summary>
        /// 上传图片到七牛
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="fileStream"></param>
        /// <param name="saveKey"></param>
        /// <returns></returns>
        private static async Task<bool> UploadStream(string bucket, Stream fileStream, string saveKey)
        {
            var res = false;
            try
            {
                var mac = new Mac(_settings.QiniuOption.AccessKey, _settings.QiniuOption.SecretKey);
                var putPolicy = new PutPolicy
                {
                    Scope = bucket + ":" + saveKey
                };
                putPolicy.SetExpires(3600); // 上传策略有效期(对应于生成的凭证的有效期)
                var jstr = putPolicy.ToJsonString();
                var token = Auth.CreateUploadToken(mac, jstr);
                var fu = new FormUploader();
                var result = await fu.UploadStreamAsync(fileStream, saveKey, token);
                if (result.Code == 200) res = true;
            }
            catch (Exception ex)
            {
                _toollogger.LogError("上传图片到云存储异常:异常信息:" + ex);
            }

            return res;
        }


        /// <summary>
        /// 批量下载图片上传到七牛 参考c#并发编程实例 2.6节
        /// </summary>
        /// <param name="srclist"></param>
        /// <returns></returns>
        public static async Task<bool> UploadImgAsync(List<SrcModel> srclist)
        {
            var b = false;
            try
            {
                var processingTasks = srclist.Select(async t =>
                {
                    // 对结果得处理
                    var stream = await HttpClientHelper.HttpGetStreamAsync(t.Oldsrc);
                    if (stream != null)
                    {
                        b = await UploadStream(_settings.QiniuOption.Bucketfirst, stream, t.Newsrc);
                        if (!b) _toollogger.LogError("[QN]上传图片:" + t.Newsrc + " 失败.原图地址:" + t.Oldsrc);
                    }
                    else
                    {
                        _toollogger.LogError("[QN]" + t.Oldsrc + ",获取该图片流失败");
                    }
                }).ToArray();
                await Task.WhenAll(processingTasks);
            }
            catch (Exception ex)
            {
                _toollogger.LogError("批量下载图片上传到QINIU异常:" + ex);
            }

            return b;
        }

        /// <summary>
        ///  SHA1加密方法
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns></returns>
        public static string GetSha1Str(string str)
        {
            var _byte = Encoding.Default.GetBytes(str);
            HashAlgorithm ha = new SHA1CryptoServiceProvider();
            _byte = ha.ComputeHash(_byte);
            var sha1Str = new StringBuilder();
            foreach (var b in _byte) sha1Str.AppendFormat("{0:x2}", b);
            return sha1Str.ToString();
        }

        public static async Task<bool> Down(string oldsrc, string newsrc)
        {
            var stream = await HttpClientHelper.HttpGetStreamAsync(oldsrc);
            var b = await UploadStream(_settings.QiniuOption.Bucketfirst, stream, newsrc);
            return b;
        }

        public static async Task<string> DownloadAllAsync(IEnumerable<string> urls)
        {
            var httpClient = new HttpClient();
            // 定义每一个url 的使用方法。
            var downloads = urls.Select(url => httpClient.GetStringAsync(url));
            // 注意，到这里，序列还没有求值，所以所有任务都还没真正启动。
            // 下面，所有的URL 下载同步开始。
            var downloadTasks = downloads.ToArray();
            // 到这里，所有的任务已经开始执行了。
            // 用异步方式等待所有下载完成。
            var htmlPages = await Task.WhenAll(downloadTasks);
            return string.Concat(htmlPages);
        }

        public static bool IsUrl(string str)
        {
            try
            {
                const string url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                return Regex.IsMatch(str, url);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static SiteConfig GetInitConst()
        {
            return AppSetting.GetConfig();
        }

        /// <summary>
        /// 替换Html标签 最快 https://www.cnblogs.com/jaxu/p/3682042.html
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripTagsCharArray(string source)
        {
            var array = new char[source.Length];
            var arrayIndex = 0;
            var inside = false;

            foreach (var @let in source)
            {
                switch (@let)
                {
                    case '<':
                        inside = true;
                        continue;
                    case '>':
                        inside = false;
                        continue;
                }

                if (inside) continue;
                array[arrayIndex] = @let;
                arrayIndex++;
            }

            return new string(array, 0, arrayIndex);
        }

        /// <summary>
        ///  Ajax重定向 ajax请求不能用Redirect,使用下面方法.如果想使用,表单提交前检查用onsubmit="return checkpm();"
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="url"></param>
        public static void AjaxRedireUrl(IHttpContextAccessor httpContextAccessor, string url)
        {
            var isAjax = httpContextAccessor.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (!isAjax) return;
            httpContextAccessor.HttpContext.Response.Headers.Add("Redirect", "true");
            httpContextAccessor.HttpContext.Response.Headers.Add("RedirectUrl", url);
        }


        #region List和datatable相互转换

        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                var t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name.Replace("_", "__"), t); // 支持列名带下划线
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];

                for (var i = 0; i < props.Length; i++) values[i] = props[i].GetValue(item, null);

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        private static Type GetCoreType(Type t)
        {
            if (t == null || !IsNullable(t)) return t;
            return !t.IsValueType ? t : Nullable.GetUnderlyingType(t);
        }

        /// <summary>
        /// List转Datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null) return null;

            var rows = new List<DataRow>();

            foreach (DataRow row in table.Rows) rows.Add(row);

            return ConvertTo<T>(rows);
        }

        private static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = rows.Select(CreateItem<T>).ToList();
            }

            return list;
        }

        private static T CreateItem<T>(DataRow row)
        {
            var obj = default(T);
            if (row == null) return obj;
            obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in row.Table.Columns)
            {
                var prop = obj.GetType().GetProperty(column.ColumnName);
                try
                {
                    var value = row[column.ColumnName];
                    prop.SetValue(obj, value, null);
                }
                catch
                {
                    //You can log something here     
                    //throw;    
                }
            }

            return obj;
        }

        #endregion

        #region 阿里云发信息

        /// <summary>
        /// 发短信
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="msg">内容</param>
        public static AliSmsModel SendSms(string phoneNumber, string msg)
        {
            IClientProfile profile = DefaultProfile.GetProfile(_settings.AliOption.RegionId, _settings.AliOption.AccessKeyId, _settings.AliOption.AccessKeySecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest
            {
                Method = MethodType.POST,
                Domain = _settings.AliOption.Domain,
                Version = _settings.AliOption.Version,
                Action = _settings.AliOption.Action
                //,Protocol = ProtocolType.HTTP
            };
            request.AddQueryParameters("PhoneNumbers", phoneNumber);
            request.AddQueryParameters("SignName", _settings.AliOption.SignName);
            request.AddQueryParameters("TemplateCode", _settings.AliOption.TemplateCode);
            request.AddQueryParameters("TemplateParam", "{\"code\":\"" + msg + "\"}");
            request.AddQueryParameters("OutId", _settings.AliOption.OutId);
            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                return JsonConvert.DeserializeObject<AliSmsModel>(Encoding.Default.GetString(response.HttpResponse.Content));
            }
            catch (ServerException)
            {
                return new AliSmsModel { Code = "scfaile", Message = "发短信服务器异常,请稍后重试" };
            }
            catch (ClientException)
            {
                return new AliSmsModel { Code = "scfaile", Message = "发短信客户端异常,请稍后重试" };
            }
        }
        #endregion

        /// <summary>
        /// 写TXT
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static async Task WriteTxt(string path, string msg)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                await sw.WriteLineAsync(msg);
            }

            //FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            //StreamWriter sw = new StreamWriter(fs);
            ////开始写入
            //await sw.WriteAsync(msg);
            ////清空缓冲区
            //await sw.FlushAsync();
            ////关闭流
            //sw.Close();
            //fs.Close();
        }

        #region Mqtt
        public static async Task ConnectMqttServerAsync(ISchoolBusBusines _schoolBusBusines)
        {
            IMqttClientOptions MqttOptions()
            {
                var options = new MqttClientOptionsBuilder()
                    .WithClientId(_settings.MqttOption.ClientID)
                    .WithTcpServer(_settings.MqttOption.HostIp, _settings.MqttOption.HostPort)
                    .WithCredentials(_settings.MqttOption.UserName, _settings.MqttOption.Password)
                    //.WithTls()//服务器端没有启用加密协议，这里用tls的会提示协议异常
                    .WithCleanSession()
                    .Build();
                return options;
            }

            // Create a new Mqtt client.
            try
            {
                if (_mqttClient == null)
                {
                    _mqttClient = new MqttFactory().CreateMqttClient();

                    // 接收到消息回调
                    _mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate( e =>
                    {
                        var received = new MqttMessageReceived
                        {
                            Topic = e.ApplicationMessage.Topic,
                            Payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload),
                            QoS = e.ApplicationMessage.QualityOfServiceLevel,
                            Retain = e.ApplicationMessage.Retain
                        };

                        Console.WriteLine($">> ### 接受消息 ###{Environment.NewLine}");
                        Console.WriteLine($">> Topic = {received.Topic}{Environment.NewLine}");
                        Console.WriteLine($">> Payload = {received.Payload}{Environment.NewLine}");
                        Console.WriteLine($">> QoS = {received.QoS}{Environment.NewLine}");
                        Console.WriteLine($">> Retain = {received.Retain}{Environment.NewLine}");
                    });

                    // 连接成功回调
                    _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(async e =>
                    {
                        Console.WriteLine("已连接到MQTT服务器！" + Environment.NewLine);
                        await Subscribe(_mqttClient, _settings.MqttOption.MqttTopic);
                    });
                    // 断开连接回调
                    _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(async e =>
                    {
                        var curTime = DateTime.UtcNow;
                        Console.WriteLine($">> [{curTime.ToLongTimeString()}]");
                        Console.WriteLine("已断开MQTT连接！" + Environment.NewLine);
                        //Reconnecting 重连
                        if (_isReconnect && !e.ClientWasConnected)
                        {
                            Console.WriteLine("正在尝试重新连接" + Environment.NewLine);
                            await Task.Delay(TimeSpan.FromSeconds(5));
                            try
                            {
                                await _mqttClient.ConnectAsync(MqttOptions());
                            }
                            catch
                            {
                                Console.WriteLine("### 重新连接 失败 ###" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            Console.WriteLine("已下线！" + Environment.NewLine);
                        }
                    });

                    try
                    {
                        await _mqttClient.ConnectAsync(MqttOptions());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("连接到MQTT服务器失败！" + Environment.NewLine + ex.Message + Environment.NewLine);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("连接到MQTT服务器未知异常！" + Environment.NewLine + e.Message + Environment.NewLine);
            }
        }
        private static async Task Subscribe(IMqttClient mqttClient, string topic)
        {
            if (mqttClient.IsConnected && !string.IsNullOrEmpty(topic))
            {
                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new TopicFilterBuilder()
                    .WithTopic(topic)
                    .WithAtMostOnceQoS()
                    .Build()
                );
                Console.WriteLine($"已订阅[{topic}]主题{Environment.NewLine}");
            }
        }
        #endregion
    }

}
