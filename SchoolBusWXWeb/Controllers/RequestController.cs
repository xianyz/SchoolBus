using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Http;
using SchoolBusWXWeb.CommServices.MessageHandlers.CustomMessageHandler;
using SchoolBusWXWeb.Models;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

// ReSharper disable RedundantToStringCallForValueType


namespace SchoolBusWXWeb.Controllers
{
    public class RequestController : Controller
    {
        private readonly string _appId = Config.SenparcWeixinSetting.WeixinAppId;
        public IActionResult Get(string url = "https://www.baidu.com")
        {
            var html = Senparc.CO2NET.HttpUtility.RequestUtility.HttpGet(url, encoding: Encoding.UTF8);
            return Content(html);
        }
        /// <summary>
        /// 模拟登陆
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IActionResult SimulateLogin(string url = "https://www.baidu.com")
        {
            var cookieContainer = new CookieContainer();
            var html = Senparc.CO2NET.HttpUtility.RequestUtility.HttpGet(url, cookieContainer, encoding: Encoding.UTF8, null, null, false);
            return Content(html);
        }

        public IActionResult Post(string url = "https://sdk.weixin.senparc.com/AsyncMethods/TemplateMessageTest", string code = "")
        {
            var formData = new Dictionary<string, string> { ["checkcode"] = code };
            var html = Senparc.CO2NET.HttpUtility.RequestUtility.HttpPost(url, null, formData);
            return Content(html);
        }

        public IActionResult GetJson(string url = "https://didi.360che.com/wxcms-api/api/pro/Product/GetPJSettingList")
        {
            var html = Senparc.CO2NET.HttpUtility.Get.GetJson<TestModel>(url);
            return Content(html.status.ToString());
        }
        /// <summary>
        /// 下载并且上传下载的文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IActionResult GetDowload(string url = "http://bing.360wll.cn/20190624.JPG")
        {
            var filePath = "E:/Download/";
            var fileName = Senparc.CO2NET.HttpUtility.Get.Download(url, filePath);
            var newFileName = fileName + ".jpg";
            System.IO.File.Move(fileName, newFileName);

            // form表单上传本地文件
            var formData = new Dictionary<string, string> { ["file"] = newFileName };
            var uploadUrl = "https://localhost:5001/Request/UploadFile";
            var html = Senparc.CO2NET.HttpUtility.RequestUtility.HttpPost(uploadUrl, fileDictionary: formData);
            return Content(html);
        }

        /// <summary>
        /// 下载上传用流方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IActionResult GetAndUP(string url = "http://bing.360wll.cn/20190624.JPG")
        {
            using (var ms = new MemoryStream())
            {
                Senparc.CO2NET.HttpUtility.Get.Download(url, ms);
                ms.Seek(0, SeekOrigin.Begin); // 最好建议操作
                var uploadUrl = "https://localhost:5001/Request/UploadImage";
                var html = Senparc.CO2NET.HttpUtility.RequestUtility.HttpPost(uploadUrl, null, ms);
            }
            return Content("");
        }

        public IActionResult UploadImage()
        {
            var stream = Request.Body;
            var fileName = "E:/Download/" + DateTime.Now.Ticks.ToString() + ".jpg";
            using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                stream.CopyTo(fileStream);
            }
            return Content("文件下载到:" + fileName);
        }

        public IActionResult UploadFile()
        {
            var stream = Request.Body;
            var fileName = "E:/Download/" + DateTime.Now.Ticks.ToString() + ".jpg";
            using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                stream.CopyTo(fileStream);
            }
            return Content("form上传的文件下载到:" + fileName);
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public IActionResult SendTemplateMessage( string openid = "ovzSu1Ux_R10fGTWCEawfdVADSy8")
        {
            const string templateId = "6RzeJPMnzoPv5AyWTx2ezAhDEyKmbidf4JspdBNT4Io";
            const string url = "https://www.baidu.com"; // 模板消息下面的链接
            //var testData = new //TestTemplateData()
            //{
            //    first = new TemplateDataItem("【异步模板消息测试】"),
            //    keyword1 = new TemplateDataItem(openid),
            //    keyword2 = new TemplateDataItem("网页测试"),
            //    keyword3 = new TemplateDataItem("内容"),
            //    keyword4 = new TemplateDataItem("内容2"),
            //    keyword5 = new TemplateDataItem(SystemTime.Now.ToString("O")),
            //    remark = new TemplateDataItem("更详细信息，请到Senparc.Weixin SDK官方网站（http://sdk.weixin.senparc.com）查看！")
            //};
            
            //var miniProgram = new TempleteModel_MiniProgram()
            //{
            //    appid = "wxfcb0a0031394a51c",//【盛派互动（BookHelper）】小程序
            //    pagepath = "pages/index/index"
            //};
            //var resule = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(_appId, openid, templateId, url, testData, null);
            
            var data=new TemplateMessageCustomNotice("【异步模板消息测试】", openid, "网页测试", "内容",
                "内容2", SystemTime.Now.ToString("O"), "更详细信息，请到Senparc.Weixin SDK官方网站（http://sdk.weixin.senparc.com）查看！", url);
            
            var resule1 = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(_appId, openid, data);
            return Content(resule1.ToJson());
        }
    }
    public class TestModel
    {
        public object[] data { get; set; }
        public int status { get; set; }
        public string msg { get; set; }
    }
}