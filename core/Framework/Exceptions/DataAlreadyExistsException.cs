using System;
using System.Runtime.Serialization;

namespace LComplete.Framework.Exceptions
{
    /// <summary>
    /// 数据重复异常
    /// </summary>
    [Serializable]
    public class DataAlreadyExistsException : System.Exception
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

        public DataAlreadyExistsException(string message, System.Exception inner) : base(message, inner)
        {
        }

        protected DataAlreadyExistsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
