using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfContract.Messages;
using WcfContract.Results;

namespace WcfContract.Services
{
    [ServiceContract]
    public interface IFileService
    {
        [OperationContract]
        UploadResult Upload(UploadMessage uploadMessage);
    }
}
