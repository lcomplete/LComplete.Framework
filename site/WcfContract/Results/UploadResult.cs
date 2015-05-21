using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfContract.Results
{
    [MessageContract]
    public class UploadResult 
    {
        [MessageHeader]
        public bool IsSuccess { get; set; }

        [MessageHeader]
        public string Message { get; set; }

        [MessageHeader]
        public int FileId { get; set; }

        [MessageHeader]
        public string UniqueKey { get; set; }

        [MessageHeader]
        public string HttpPath { get; set; }
    }
}
