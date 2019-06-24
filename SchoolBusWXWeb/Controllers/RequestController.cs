using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Http;

// ReSharper disable RedundantToStringCallForValueType


namespace SchoolBusWXWeb.Controllers
{
    public class RequestController : Controller
    {
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
            var filePath= "E:/Download/";
            var fileName= Senparc.CO2NET.HttpUtility.Get.Download(url,filePath);
            var newFileName=fileName+".jpg";
            System.IO.File.Move(fileName,newFileName);

            // form表单上传本地文件
            var formData = new Dictionary<string, string> { ["file"] = newFileName };
            var uploadUrl = "https://localhost:5001/Request/UploadFile";
            var html = Senparc.CO2NET.HttpUtility.RequestUtility.HttpPost(uploadUrl,fileDictionary: formData);
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
    }
    public class TestModel
    {
        public object[] data { get; set; }
        public int status { get; set; }
        public string msg { get; set; }
    }
}