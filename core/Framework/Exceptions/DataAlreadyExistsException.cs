using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LComplete.Framework.Exceptions
{
    /// <summary>
    /// 数据已存在异常
    /// </summary>
    [Serializable]
    public class DataAlreadyExistsException : VerifyException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public DataAlreadyExistsException()
        {
        }

        public DataAlreadyExistsException(string message) : base(message)
        {
        }

        public DataAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DataAlreadyExistsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
