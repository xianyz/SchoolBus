using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBusWXWeb.Models
{
    public class ImgInfo
    {
        public Stream FileStream { get; set; }
        public int ImageId { get; set; }
        public string FileName { get; set; }
        public string SaveKey { get; set; }
    }
    public class Token
    {
        public string uptoken { get; set; }
    }
    public class SrcModel
    {
        public string Oldsrc { get; set; }
        public string Newsrc { get; set; }
    }
}
