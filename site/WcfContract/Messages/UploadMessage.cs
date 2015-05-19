using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfContract.Messages
{
    [MessageContract]
    public class UploadMessage
    {
        [MessageHeader]
        public string FileName { get; set; }

        [MessageHeader]
        public string ContentType { get; set; }

        [MessageHeader]
        public int? UploadAdminId { get; set; }

        [MessageHeader]
        public int? UploadUserId { get; set; }

        [MessageHeader]
        public bool IsImage { get; set; }

        [MessageHeader]
        public ThumbSize[] ThumbSizes { get; set; }

        [MessageBodyMember]
        public Stream FileStream { get; set; }
    }

    [MessageContract]
    public class ThumbSize
    {
        /// <summary>
        /// 缩略图限制宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 缩略图限制高度（为null时 表示不限制高度）
        /// </summary>
        public int? Height { get; set; }
    }
}
